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

    //public void Test()
    //{
    //    var q = Context.Set<SmsPartition>().Include(_ => _.Documents).ThenInclude(_ => _.DocumentChanges);
    //}
}