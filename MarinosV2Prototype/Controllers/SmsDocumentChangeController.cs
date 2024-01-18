using MarinosV2PrototypeShared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarinosV2Prototype.Controllers;

[ApiController]
[Route("[controller]")]
public class SmsDocumentChangeController : TController<SmsDocumentChange>
{
    public SmsDocumentChangeController(MarinosContext ctx) : base(ctx)
    {
    }

    [HttpGet("ByDocument/{id}")]
    public virtual async Task<IActionResult> GetDocumentChangesByDocument(Guid id)
    {
        if (Error is not OkResult)
            return Error;

        try
        {
            return Ok(await Context.Set<SmsDocumentChange>().Where(_ => _.IdDocument == id).ToListAsync());
        }
        catch (Exception e)
        {
            return GetProblemFromException(e);
        }
    }
}