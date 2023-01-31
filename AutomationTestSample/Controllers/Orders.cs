using System.Net;
using AutomationTestSample.Controllers.Abstract;
using AutomationTestSample.Dtos;
using AutomationTestSample.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutomationTestSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Orders : TestControllerBase
    {
        public Orders(ITestDbContext dbContext, ILogger<Orders> logger) : base(dbContext, logger) { }

        /// <summary>
        /// Returns a list of orders
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetOrder>>> GetList(CancellationToken cancellationToken)
        {
            var orders = await _dbContext.Orders.Include(p => p.Patient).ToListAsync(cancellationToken);
            var clients = ClientHelper.GetList();

            var orderDto = orders.Select(p => new GetOrder(
                Id: p.Id,
                AccessionNumber: p.AccessionNumber,
                OrgCode: p.OrgCode,
                SiteName: clients.FirstOrDefault(c => c.OrgCode == p.OrgCode)?.Sites.FirstOrDefault(s => s.Id == p.SiteId)?.Name ?? String.Empty,
                PatientMrn: p.Patient.Mrn,
                PatientName: $"{p.Patient.FirstName} {p.Patient.LastName}".Trim(),
                Modality: p.Modality,
                StudyDateTime: p.StudyDateTime,
                Status: p.Status.ToString(),
                CreatedAt: p.CreatedAt,
                UpdatedAt: p.UpdatedAt
            )).OrderBy(p => p.StudyDateTime);

            return Ok(orderDto);
        }

        /// <summary>
        /// Returns an order by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GetOrder>>> Get(int id, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders.Include(p => p.Patient).FirstOrDefaultAsync(p => p.Id == id);

            if (order is null)
            {
                return Problem(detail: $"No order found with Id: [{id}]", statusCode: (int)HttpStatusCode.NotFound);
            }

            var clients = ClientHelper.GetList();

            var orderDto = new GetOrder(
                Id: order.Id,
                AccessionNumber: order.AccessionNumber,
                OrgCode: order.OrgCode,
                SiteName: clients.FirstOrDefault(c => c.OrgCode == order.OrgCode)?.Sites.FirstOrDefault(s => s.Id == order.SiteId)?.Name ?? String.Empty,
                PatientMrn: order.Patient.Mrn,
                PatientName: $"{order.Patient.FirstName} {order.Patient.LastName}".Trim(),
                Modality: order.Modality,
                StudyDateTime: order.StudyDateTime,
                Status: order.Status.ToString(),
                CreatedAt: order.CreatedAt,
                UpdatedAt: order.UpdatedAt
            );

            return Ok(orderDto);
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(PostOrder request, CancellationToken cancellationToken)
        {
            var existingOrder = await _dbContext.Orders.FirstOrDefaultAsync(p => p.AccessionNumber == request.AccessionNumber);
            if (existingOrder is not null)
            {
                return Problem(detail: $"An order already exists with accession number [{request.AccessionNumber}]", statusCode: (int)HttpStatusCode.Conflict);
            }

            if (request.StudyDateTime > DateTime.Now)
            {
                return Problem(detail: $"StudyDateTime cannot be in the future: [{request.StudyDateTime}]", statusCode: (int)HttpStatusCode.PreconditionFailed);
            }


            var patient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.Mrn == request.PatientMrn);
            if (patient is null)
            {
                patient = new Domain.Patient(request.PatientMrn, request.PatientFirstName, request.PatientLastName) { CreatedAt = DateTimeOffset.Now, UpdatedAt = DateTimeOffset.Now };
                _dbContext.Patients.Add(patient);
            } else {
                // this is wrong, it shouldn't update the Patient just because the entity is being attached to a new order. It certainly shouldnt reset the CreatedAt field!
                patient.CreatedAt = DateTimeOffset.Now;
                patient.UpdatedAt = DateTimeOffset.Now;
                _dbContext.Patients.Update(patient);
            }

            // set the wrong SiteId, and also don't set a CreatedAt or UpdatedAt value.
            var buggySiteId = request.SiteId + 1;
            var order = new Domain.Order(patient.Id, request.AccessionNumber, request.OrgCode, buggySiteId, request.Modality, request.StudyDateTime, Domain.Status.SC);
            
            _dbContext.Orders.Add(order);

            await _dbContext.SaveChanges(cancellationToken);

            await RandomDelay();

            return CreatedAtAction(nameof(Get), new { id = order.Id }, null);
        }

        /// <summary>
        /// Deletes an order by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            
            if (order is not null)
            {
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChanges(cancellationToken);
            }

            await RandomDelay();

            return NoContent();
        }
    }
}
