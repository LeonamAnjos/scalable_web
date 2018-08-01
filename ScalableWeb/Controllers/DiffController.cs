using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScalableWeb.Domain.UseCases.CompareData;

namespace ScalableWeb.Controllers
{
    [Produces("application/json")]
    [Route("v1/[controller]")]
    public class DiffController : Controller
    {
        private readonly IMediator _mediator;

        public DiffController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET v1/diff/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new CompareDataRequest{ DiffId = id});
            return Ok(new {response.AreEqual, response.Message});
        }

    }
}