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
    public class Patients : TestControllerBase
    {
        public Patients(ITestDbContext dbContext, ILogger<Orders> logger) : base(dbContext, logger) { }

        /// <summary>
        /// Returns a list of orders
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetPatient>>> GetList(CancellationToken cancellationToken)
        {
            var patients = await _dbContext.Patients.ToListAsync(cancellationToken);
            
            var patientDto = patients.Select(p => new GetPatient(
                Id: p.Id,
                Mrn: p.Mrn,
                FirstName: p.FirstName,
                LastName: p.LastName,
                UpdatedAt: p.UpdatedAt,
                CreatedAt: p.CreatedAt
            )).OrderBy(p => p.Id);

            return Ok(patientDto);
        }

        /// <summary>
        /// Returns a patient by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GetPatient>>> Get(int id, CancellationToken cancellationToken)
        {
            var patient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.Id == id);

            if (patient is null)
            {
                return Problem(detail: $"No patient found with id: [{id}]", statusCode: (int)HttpStatusCode.NotFound);
            }

            var patientDto = new GetPatient(
                Id: patient.Id,
                Mrn: patient.Mrn,
                FirstName: patient.FirstName,
                LastName: patient.LastName,
                CreatedAt: patient.CreatedAt,
                UpdatedAt: patient.UpdatedAt
            );

            return Ok(patientDto);
        }

        /// <summary>
        /// Creates a new patient
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(PostPatient request, CancellationToken cancellationToken)
        {
            var existingPatient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.Mrn == request.Mrn);
            if (existingPatient is not null)
            {
                return Problem(detail: $"A patient already exists with MRN [{request.Mrn}]", statusCode: (int)HttpStatusCode.Conflict);
            }

            var patient = new Domain.Patient(request.Mrn, request.FirstName, request.LastName) { CreatedAt = DateTimeOffset.Now, UpdatedAt = DateTimeOffset.Now };

            _dbContext.Patients.Add(patient);

            await _dbContext.SaveChanges(cancellationToken);

            await RandomDelay();

            return CreatedAtAction(nameof(Get), new { id = patient.Id }, null);
        }

        /// <summary>
        /// Deletes a patient by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var patient = await _dbContext.Patients.FindAsync(id);
            
            if (patient is not null)
            {
                _dbContext.Patients.Remove(patient);
                await _dbContext.SaveChanges(cancellationToken);
            }

            await RandomDelay();

            return NoContent();
        }
    }
}
