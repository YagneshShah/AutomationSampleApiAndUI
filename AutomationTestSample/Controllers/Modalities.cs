using AutomationTestSample.Domain;
using AutomationTestSample.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutomationTestSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Modalities : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Client>> GetList(CancellationToken cancellationToken)
        {
            return Ok(ModalityHelper.GetList());
        }
    }
}
