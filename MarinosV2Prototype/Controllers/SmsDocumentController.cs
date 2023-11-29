using MarinosV2PrototypeShared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarinosV2Prototype.Controllers;

[ApiController]
[Route("[controller]")]
public class SmsDocumentController : TController<SmsDocument>
{
    public SmsDocumentController(MarinosContext ctx) : base(ctx)
    {
    }

    [HttpGet("ByPartition/{id}")]
    public virtual async Task<IActionResult> GetDocumentsByPartition(Guid id)
    {
        if (Error is not OkResult)
            return Error;

        try
        {
            return Ok(await Context.Set<SmsDocument>().Where(_ => _.IdPartition == id).ToListAsync());
        }
        catch (Exception e)
        {
            return GetProblemFromException(e);
        }
    }
}