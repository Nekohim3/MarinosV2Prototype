using MarinosV2Prototype.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarinosV2Prototype.Controllers
{
    public abstract class TController<T> : ControllerBase where T : GuidEntity
    {
        protected readonly MarinosContext Context;

        protected TController(MarinosContext ctx)
        {
            Context = ctx;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            if (DatabaseConnection.DatabaseSettings == null || !Context.IsValid)
                return Problem("Empty api config");
            var lst = await Context.Set<T>().AsNoTracking().ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("{guid}")]
        public virtual async Task<IActionResult> Get(Guid guid)
        {
            if (DatabaseConnection.DatabaseSettings == null || !Context.IsValid)
                return Problem("Empty api config");
            var t = await Context.Set<T>().AsNoTracking().SingleOrDefaultAsync(_ => _.Guid == guid);
            if (t == null)
                return NotFound();
            return Ok(t);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create(T t)
        {
            if (DatabaseConnection.DatabaseSettings == null || !Context.IsValid)
                return Problem("Empty api config");
            Context.Add(t);
            await Context.SaveChangesAsync();
            return Ok(t);
        }

        [HttpPost]
        [Route("Bulk")]
        public virtual async Task<IActionResult> Create(List<T> tList)
        {
            if (DatabaseConnection.DatabaseSettings == null || !Context.IsValid)
                return Problem("Empty api config");
            Context.AddRange(tList);
            await Context.SaveChangesAsync();
            return Ok(tList);
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(T t)
        {
            if (DatabaseConnection.DatabaseSettings == null || !Context.IsValid)
                return Problem("Empty api config");
            Context.Update(t);
            await Context.SaveChangesAsync();
            return Ok(t);
        }

        [HttpPut]
        [Route("Bulk")]
        public virtual async Task<IActionResult> Update(List<T> tList)
        {
            if (DatabaseConnection.DatabaseSettings == null || !Context.IsValid)
                return Problem("Empty api config");
            Context.UpdateRange(tList);
            await Context.SaveChangesAsync();
            return Ok(tList);
        }

        [HttpPatch]
        public virtual async Task<IActionResult> Save(T t)
        {
            if (DatabaseConnection.DatabaseSettings == null || !Context.IsValid)
                return Problem("Empty api config");
            if (t.Guid == Guid.Empty)
                Context.Add(t);
            else
                Context.Update(t);
            await Context.SaveChangesAsync();
            return Ok(t);
        }

        [HttpPatch]
        [Route("Bulk")]
        public virtual async Task<IActionResult> Save(List<T> tList)
        {
            if (DatabaseConnection.DatabaseSettings == null || !Context.IsValid)
                return Problem("Empty api config");
            var forAdd = tList.Where(_ => _.Guid == Guid.Empty).ToList();
            var forSave = tList.Where(_ => _.Guid != Guid.Empty).ToList();
            Context.AddRange(forAdd);
            Context.UpdateRange(forSave);
            await Context.SaveChangesAsync();
            return Ok(tList);
        }

        [HttpDelete]
        [Route("{guid}")]
        public virtual async Task<IActionResult> Delete(Guid guid)
        {
            if (DatabaseConnection.DatabaseSettings == null || !Context.IsValid)
                return Problem("Empty api config");
            var t = await Context.Set<T>().FindAsync(guid);
            if (t == null)
                return NotFound();
            Context.Remove(t);
            await Context.SaveChangesAsync();
            return Ok(true);
        }

        [HttpDelete]
        [Route("Bulk")]
        public virtual async Task<IActionResult> Delete(List<Guid> guidList)
        {
            if (DatabaseConnection.DatabaseSettings == null || !Context.IsValid)
                return Problem("Empty api config");
            var t = await Context.Set<T>().Where(_ => guidList.Contains(_.Guid)).ToListAsync();
            if (t.Count != guidList.Count)
                return NotFound();
            Context.RemoveRange(t);
            await Context.SaveChangesAsync();
            return Ok(true);
        }
    }
}
