using MarinosV2Prototype.Models;
using MarinosV2Prototype.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarinosV2Prototype.Controllers
{
    public abstract class TController<T> : ControllerBase where T : IdEntity
    {
        protected readonly MarinosContext Context;
        private readonly   IActionResult  _error;
        protected TController(MarinosContext ctx)
        {
            Context = ctx;
            _error  = CheckConnection();
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            if (_error is not OkResult)
                return _error;

            try
            {

                var lst = await Context.Set<T>().AsNoTracking().ToListAsync();
                return Ok(lst);
            }
            catch (Exception e)
            {
                return GetProblemFromException(e);
            }
        }

        [HttpGet]
        [Route("{guid}")]
        public virtual async Task<IActionResult> Get(Guid guid)
        {
            if (_error is not OkResult)
                return _error;

            try
            {
                var t = await Context.Set<T>().AsNoTracking().SingleOrDefaultAsync(_ => _.Id == guid);
                if (t == null)
                    return NotFound();
                return Ok(t);
            }
            catch (Exception e)
            {
                return GetProblemFromException(e);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create(T t)
        {
            if (_error is not OkResult)
                return _error;

            await using var ts = await Context.Database.BeginTransactionAsync();
            try
            {
                await Context.AddAsync(t);
                if (await Context.SaveChangesAsync() > 0)
                {
                    await ts.CommitAsync();
                    return Ok(t);
                }

                await ts.RollbackAsync();
                return Problem("Not saved");
            }
            catch (Exception e)
            {
                await ts.RollbackAsync();
                return GetProblemFromException(e);
            }
        }

        [HttpPost]
        [Route("Bulk")]
        public virtual async Task<IActionResult> Create(List<T> tList)
        {
            if (_error is not OkResult)
                return _error;

            await using var ts = await Context.Database.BeginTransactionAsync();

            try
            {
                await Context.AddRangeAsync(tList);
                if (await Context.SaveChangesAsync() == tList.Count)
                {
                    await ts.CommitAsync();
                    return Ok(tList);
                }

                await ts.RollbackAsync();
                return Problem("Not saved");
            }
            catch (Exception e)
            {
                await ts.RollbackAsync();
                return GetProblemFromException(e);
            }
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(T t)
        {
            if (_error is not OkResult)
                return _error;

            await using var ts = await Context.Database.BeginTransactionAsync();

            try
            {

                Context.Update(t);
                if (await Context.SaveChangesAsync() > 0)
                {
                    await ts.CommitAsync();
                    return Ok(t);
                }

                await ts.RollbackAsync();
                return Problem("Not saved");
            }
            catch (DbUpdateConcurrencyException e)
            {
                await ts.RollbackAsync();
                return GetProblemFromException(e);
            }
            catch (Exception e)
            {
                await ts.RollbackAsync();
                return GetProblemFromException(e);
            }
        }

        [HttpPut]
        [Route("Bulk")]
        public virtual async Task<IActionResult> Update(List<T> tList)
        {
            if (_error is not OkResult)
                return _error;

            await using var ts = await Context.Database.BeginTransactionAsync();

            try
            {

                Context.UpdateRange(tList);
                if (await Context.SaveChangesAsync() > 0)
                {
                    await ts.CommitAsync();
                    return Ok(tList);
                }

                await ts.RollbackAsync();
                return Problem("Not saved");
            }
            catch (DbUpdateConcurrencyException e)
            {
                await ts.RollbackAsync();
                return GetProblemFromException(e);
            }
            catch (Exception e)
            {
                await ts.RollbackAsync();
                return GetProblemFromException(e);
            }
        }

        [HttpPatch]
        public virtual async Task<IActionResult> Save(T t)
        {
            if (_error is not OkResult)
                return _error;

            await using var ts = await Context.Database.BeginTransactionAsync();

            try
            {

                if (t.Id == Guid.Empty)
                    Context.Add(t);
                else
                    Context.Update(t);

                if (await Context.SaveChangesAsync() > 0)
                {
                    await ts.CommitAsync();
                    return Ok(t);
                }

                await ts.RollbackAsync();
                return Problem("Not saved");
            }
            catch (DbUpdateConcurrencyException e)
            {
                await ts.RollbackAsync();
                return GetConflictFromDbUpdateConcurrencyException(e);
            }
            catch (Exception e)
            {
                await ts.RollbackAsync();
                return GetProblemFromException(e);
            }
        }

        [HttpPatch]
        [Route("Bulk")]
        public virtual async Task<IActionResult> Save(List<T> tList)
        {
            if (_error is not OkResult)
                return _error;

            await using var ts      = await Context.Database.BeginTransactionAsync();

            try
            {
                var forAdd  = tList.Where(_ => _.Id == Guid.Empty).ToList();
                var forSave = tList.Where(_ => _.Id != Guid.Empty).ToList();
                Context.AddRange(forAdd);
                Context.UpdateRange(forSave);
                if (await Context.SaveChangesAsync() == tList.Count)
                {
                    await ts.CommitAsync();
                    return Ok(tList);
                }

                await ts.RollbackAsync();
                return Problem("Not saved");
            }
            catch (DbUpdateConcurrencyException e)
            {
                await ts.RollbackAsync();
                return GetConflictFromDbUpdateConcurrencyException(e);
            }
            catch (Exception e)
            {
                await ts.RollbackAsync();
                return GetProblemFromException(e);
            }
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(T t)
        {
            if (_error is not OkResult)
                return _error;

            await using var ts = await Context.Database.BeginTransactionAsync();

            try
            {
                Context.Remove(t);
                if (await Context.SaveChangesAsync() > 0)
                {
                    await ts.CommitAsync();
                    return Ok();
                }

                await ts.RollbackAsync();
                return Problem("Not saved");
            }
            catch (DbUpdateConcurrencyException e)
            {
                await ts.RollbackAsync();
                return GetConflictFromDbUpdateConcurrencyException(e);
            }
            catch (Exception e)
            {
                await ts.RollbackAsync();
                return GetProblemFromException(e);
            }

        }

        [HttpDelete]
        [Route("{guid}")]
        public virtual async Task<IActionResult> Delete(Guid guid)
        {
            if (_error is not OkResult)
                return _error;

            await using var ts = await Context.Database.BeginTransactionAsync();

            try
            {
                var t = await Context.Set<T>().FindAsync(guid);
                if (t == null)
                    return NotFound();
                Context.Remove(t);
                if (await Context.SaveChangesAsync() > 0)
                {
                    await ts.CommitAsync();
                    return Ok();
                }

                await ts.RollbackAsync();
                return Problem("Not saved");
            }
            catch (DbUpdateConcurrencyException e)
            {
                await ts.RollbackAsync();
                return GetConflictFromDbUpdateConcurrencyException(e);
            }
            catch (Exception e)
            {
                await ts.RollbackAsync();
                return GetProblemFromException(e);
            }
            
        }

        [HttpDelete]
        [Route("Bulk")]
        public virtual async Task<IActionResult> Delete(List<T> tList)
        {
            if (_error is not OkResult)
                return _error;

            await using var ts = await Context.Database.BeginTransactionAsync();

            try
            {
                Context.RemoveRange(tList);
                if (await Context.SaveChangesAsync() == tList.Count)
                {
                    await ts.CommitAsync();
                    return Ok(true);
                }

                await ts.RollbackAsync();
                return Problem("Not saved");
            }
            catch (DbUpdateConcurrencyException e)
            {
                await ts.RollbackAsync();
                return GetConflictFromDbUpdateConcurrencyException(e);
            }
            catch (Exception e)
            {
                await ts.RollbackAsync();
                return GetProblemFromException(e);
            }
        }

        [HttpDelete]
        [Route("BulkGuid")]
        public virtual async Task<IActionResult> Delete(List<Guid> guidList)
        {
            if (_error is not OkResult)
                return _error;
            
            await using var ts = await Context.Database.BeginTransactionAsync();
            
            try
            {
                var t = await Context.Set<T>().Where(_ => guidList.Contains(_.Id)).ToListAsync();
                if (t.Count != guidList.Count)
                {
                    await ts.RollbackAsync();
                    return NotFound();
                }

                Context.RemoveRange(t);
                if (await Context.SaveChangesAsync() == guidList.Count)
                {
                    await ts.CommitAsync();
                    return Ok();
                }

                await ts.RollbackAsync();
                return Problem("Not saved");
            }
            catch (DbUpdateConcurrencyException e)
            {
                await ts.RollbackAsync();
                return GetConflictFromDbUpdateConcurrencyException(e);
            }
            catch (Exception e)
            {
                await ts.RollbackAsync();
                return GetProblemFromException(e);
            }
        }

        private IActionResult CheckConnection()
        {
            if (DatabaseConnection.DatabaseSettings == null)
                return ValidationProblem("Empty api config");
            if (!Context.IsValid)
                return ValidationProblem("Wrong api config");
            return Ok();
        }

        private IActionResult GetProblemFromException(Exception e)
        {
            return Problem(e.FromChain(_ => _.InnerException).Aggregate("", (current, x) => current + $"Message:\n{x.Message}\nStackTrace:\n{x.StackTrace}\n==========").TrimEnd('='));
        }

        private IActionResult GetConflictFromDbUpdateConcurrencyException(DbUpdateConcurrencyException e)
        {
            return Conflict($"Message:\n{e.Message}\nStackTrace:\n{e.StackTrace}");
        }
    }
}
