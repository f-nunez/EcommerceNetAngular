using Fnunez.Ena.API.Errors;
using Fnunez.Ena.Infrasctructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Fnunez.Ena.API.Controllers;

public class BuggyController : BaseApiController
{
    private readonly StoreDbContext _dbContext;

    public BuggyController(StoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("notfound")]
    public ActionResult GetNotFoundRequest()
    {
        var thing = _dbContext.Products.Find(42);

        if (thing == null) return NotFound(new ApiResponse(404));

        return Ok();
    }

    [HttpGet("servererror")]
    public ActionResult GetServerError()
    {
        var thing = _dbContext.Products.Find(42);

        var thingToReturn = thing.ToString();

        return Ok();
    }

    [HttpGet("badrequest")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new ApiResponse(400));
    }

    [HttpGet("badrequest/{id}")]
    public ActionResult GetNotFoundRequest(int id)
    {
        return Ok();
    }
}