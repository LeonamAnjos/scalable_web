using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScalableWeb.Domain.Models;
using ScalableWeb.Domain.Patterns;
using ScalableWeb.Repository;

namespace ScalableWeb.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IRepository<DataRecord> _repository;

        public ValuesController(IRepository<DataRecord> repository)
        {
            _repository = repository;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _repository.Queriable().Select(d => $"id={d.Id}, idDiff={d.DiffId}, side={d.Side}");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
