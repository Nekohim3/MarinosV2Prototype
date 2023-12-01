using MarinosV2PrototypeShared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MarinosV2Prototype.Controllers;

[ApiController]
[Route("[controller]")]
public class SmsPartitionController : TreeTController<SmsPartition>
{
    public SmsPartitionController(MarinosContext ctx) : base(ctx)
    {

    }

    [HttpGet("ByParent/{id}")]
    public virtual async Task<IActionResult> GetChildsByParentId(Guid id)
    {
        if (Error is not OkResult)
            return Error;

        try
        {
            return Ok(await Context.Set<SmsPartition>().Where(_ => _.IdParent== id).ToListAsync());
        }
        catch (Exception e)
        {
            return GetProblemFromException(e);
        }
    }
}