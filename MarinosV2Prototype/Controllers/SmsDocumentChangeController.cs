using MarinosV2Prototype.Models;
using MarinosV2PrototypeShared.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarinosV2Prototype.Controllers;

[ApiController]
[Route("[controller]")]
public class SmsDocumentChangeController : TController<SmsDocumentChange>
{
    public SmsDocumentChangeController(MarinosContext ctx) : base(ctx)
    {
    }
}