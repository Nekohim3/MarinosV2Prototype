using MarinosV2PrototypeShared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarinosV2Prototype.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SmsDocumentFileController : TController<SmsDocumentFile>
    {
        public SmsDocumentFileController(MarinosContext ctx) : base(ctx)
        {
        }

        [HttpGet("ByDocument/{id}")]
        public virtual async Task<IActionResult> GetDocumentFilesByDocument(Guid id)
        {
            if (Error is not OkResult)
                return Error;

            try
            {
                return Ok(await Context.Set<SmsDocumentFile>().Where(_ => _.IdDocument == id).ToListAsync());
            }
            catch (Exception e)
            {
                return GetProblemFromException(e);
            }
        }
    }
}
