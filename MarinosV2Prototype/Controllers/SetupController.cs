using MarinosV2Prototype.Utils;
using Microsoft.AspNetCore.Mvc;

namespace MarinosV2Prototype.Controllers;

[ApiController]
[Route("[controller]")]
public class SetupController : ControllerBase
{
    public SetupController()
    {
    }

    [HttpGet]
    [Route("{host}/{port}/{dbName}/{username}/{password}")]
    public virtual async Task<IActionResult> SetupContext(string host, string port, string dbName, string username, string password)
    {
        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            HttpContext.Response.ContentType = "text/plain";
            return BadRequest("Invalid database params (some param is empty)");
        }

        if (DatabaseConnection.DatabaseSettings != null)
        {
            if (DatabaseConnection.DatabaseSettings.DatabaseHost     == host     &&
                DatabaseConnection.DatabaseSettings.DatabasePort     == port     &&
                DatabaseConnection.DatabaseSettings.DatabaseName     == dbName   &&
                DatabaseConnection.DatabaseSettings.DatabaseUsername == username &&
                DatabaseConnection.DatabaseSettings.DatabasePassword == password)
                return Ok(true);
        }

        DatabaseConnection.DatabaseSettings = new DatabaseSettings(host, port, dbName, username, password);

        var ctx = new MarinosContext();
        if (ctx.IsValid)
        {
            await ctx.DisposeAsync();
            return Ok(true);
        }
        else
        {
            DatabaseConnection.DatabaseSettings = null;
            HttpContext.Response.ContentType    = "text/plain";
            return BadRequest($"Invalid database params\n{string.Join("\n", ctx.Exception.FromChain(_ => _.InnerException).Select(_ => _.Message))}");
        }
    }

    [HttpGet]
    [Route("EnsureDeleted")]
    public virtual void EnsureDeleted()
    {
        new MarinosContext(true);
    }
}