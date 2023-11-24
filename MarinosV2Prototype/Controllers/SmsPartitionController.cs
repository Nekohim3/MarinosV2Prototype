using MarinosV2Prototype.Models;
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

    [HttpGet("{id}/{withDocs?}/{withChanges?}/{withFiles?}")]
    public Task<IActionResult> GetById(Guid id, bool? withDocs = false, bool? withChanges = false, bool? withFiles = false)
    {
        var q = Context.Set<SmsPartition>().AsQueryable();
        if (withDocs.GetValueOrDefault())
        {
            q = q.Include(_ => _.Documents);
            if (withChanges.GetValueOrDefault())
                q = ((IIncludableQueryable<SmsPartition, ICollection<SmsDocument>>)q).ThenInclude(_ => _.DocumentChanges);
            if (withFiles.GetValueOrDefault())
                q = ((IIncludableQueryable<SmsPartition, ICollection<SmsDocument>>)q).ThenInclude(_ => _.DocumentFiles);
        }

        GetByIdFunc = guid => q.SingleOrDefaultAsync(_ => _.Id == guid);
        return base.GetById(id);
    }
    //[HttpGet]
    //public virtual async Task<IActionResult> GetAllWith()
    //{
    //    if (Error is not OkResult)
    //        return Error;

    //    try
    //    {
    //        var lst = await Context.Set<SmsPartition>().AsNoTracking().ToListAsync();
    //        return Ok(lst);
    //    }
    //    catch (Exception e)
    //    {
    //        return GetProblemFromException(e);
    //    }
    //}
}