using System.Collections.ObjectModel;
using MarinosV2Prototype.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarinosV2Prototype.Models.BaseModels;

namespace MarinosV2Prototype.Controllers;

public abstract class TController<T> : ControllerBase where T : Entity
{
    protected readonly MarinosContext       Context;
    protected readonly IActionResult        Error;
    protected          Func<Task<List<T>?>> GetAllFunc;
    protected          Func<Guid, Task<T?>> GetByIdFunc;
    protected TController(MarinosContext ctx)
    {
        Context     = ctx;
        Error       = CheckConnection();
        GetAllFunc  = async () => await Context.Set<T>().ToListAsync();
        GetByIdFunc = async id => await Context.Set<T>().SingleOrDefaultAsync(_ => _.Id == id);
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAll()
    {
        if (Error is not OkResult)
            return Error;

        try
        {
            return Ok(await GetAllFunc());
        }
        catch (Exception e)
        {
            return GetProblemFromException(e);
        }
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetById(Guid id)
    {
        if (Error is not OkResult)
            return Error;

        try
        {
            var t = await GetByIdFunc(id);
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
        if (Error is not OkResult)
            return Error;

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
        if (Error is not OkResult)
            return Error;

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
        if (Error is not OkResult)
            return Error;

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
            return GetConflictFromDbUpdateConcurrencyException(e);
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
        if (Error is not OkResult)
            return Error;

        await using var ts = await Context.Database.BeginTransactionAsync();

        try
        {

            Context.UpdateRange(tList);
            if (await Context.SaveChangesAsync() >= tList.Count)
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

    [HttpPatch]
    public virtual async Task<IActionResult> Save(T t)
    {
        if (Error is not OkResult)
            return Error;

        await using var ts = await Context.Database.BeginTransactionAsync();

        try
        {

            if (t.Version == 0)
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
        if (Error is not OkResult)
            return Error;

        await using var ts = await Context.Database.BeginTransactionAsync();

        try
        {
            var forAdd  = tList.Where(_ => _.Version == 0).ToList();
            var forSave = tList.Where(_ => _.Version != 0).ToList();
            Context.AddRange(forAdd);
            Context.UpdateRange(forSave);
            if (await Context.SaveChangesAsync() >= tList.Count)
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
        if (Error is not OkResult)
            return Error;

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
    [Route("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        if (Error is not OkResult)
            return Error;

        await using var ts = await Context.Database.BeginTransactionAsync();

        try
        {
            var t = await Context.Set<T>().FindAsync(id);
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
        if (Error is not OkResult)
            return Error;

        await using var ts = await Context.Database.BeginTransactionAsync();

        try
        {
            Context.RemoveRange(tList);
            if (await Context.SaveChangesAsync() >= tList.Count)
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
    [Route("BulkGuid")]
    public virtual async Task<IActionResult> Delete(List<Guid> guidList)
    {
        if (Error is not OkResult)
            return Error;
            
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
            if (await Context.SaveChangesAsync() >= guidList.Count)
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

    protected IActionResult CheckConnection()
    {
        if (DatabaseConnection.DatabaseSettings == null)
            return Problem("Empty api config");
        if (!Context.IsValid)
            return Problem("Wrong api config");
        return Ok();
    }

    protected IActionResult GetProblemFromException(Exception e)
    {
        return Problem(e.FromChain(_ => _.InnerException).Aggregate("", (current, x) => current + $"Message:\n{x.Message}\nStackTrace:\n{x.StackTrace}\n==========").TrimEnd('='));
    }

    protected IActionResult GetConflictFromDbUpdateConcurrencyException(DbUpdateConcurrencyException e)
    {
        return Conflict($"Message:\n{e.Message}\nStackTrace:\n{e.StackTrace}");
    }
}