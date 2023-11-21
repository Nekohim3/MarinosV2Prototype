using MarinosV2Prototype.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarinosV2Prototype.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestEntityController : TController<TestEntity>
    {
        public TestEntityController(MarinosContext ctx) : base(ctx)
        {
        }
    }
}
