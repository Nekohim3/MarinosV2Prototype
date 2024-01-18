using MarinosV2PrototypeShared.BaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarinosV2Prototype.Controllers
{
    public abstract class TreeTController<T> : TController<T> where T : TreeEntity<T>
    {
        protected TreeTController(MarinosContext ctx) : base(ctx)
        {
        }



        [HttpGet]
        [Route("Tree")]
        public virtual async Task<IActionResult> GetRootList()
        {
            if (Error is not OkResult)
                return Error;

            try
            {
                var lst  = await Context.Set<T>().Where(_ => _.IdParent == null).ToListAsync();
                Console.WriteLine($"GetRootList(); Count:{lst.Count}");
                return Ok(lst);
            }
            catch (Exception e)
            {
                return GetProblemFromException(e);
            }
        }

        //[HttpGet]
        //[Route("Branch/{id}")]
        //public virtual async Task<IActionResult> GetBranch(Guid id)
        //{
        //    if (Error is not OkResult)
        //        return Error;

        //    try
        //    {
        //        await GetBranchRecur(id);
        //        var item = await Context.Set<T>().Where(_ => _.Id == id).SingleOrDefaultAsync();
        //        return Ok(item);
        //    }
        //    catch (Exception e)
        //    {
        //        return GetProblemFromException(e);
        //    }
        //}

        //private async Task GetBranchRecur(Guid parentId)
        //{
        //    var lst   = await Context.Set<T>().Where(_ => _.IdParent == parentId).ToListAsync();
        //    foreach (var x in lst)
        //        await GetBranchRecur(x.Id);
        //}
    }
}
