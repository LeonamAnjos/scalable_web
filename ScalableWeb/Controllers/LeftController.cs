﻿using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScalableWeb.Domain.Models;
using ScalableWeb.Domain.UseCases.AddData;
using ScalableWeb.Models;

namespace ScalableWeb.Controllers
{
    [Produces("application/json")]
    [Route("v1/diff/{id}/[controller]")]
    public class LeftController : Controller
    {
        private readonly IMediator _mediator;

        public LeftController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST v1/diff/5/left
        [HttpPost]
        public async Task<IActionResult> Post(int id, [FromBody] DiffDataViewModel content)
        {
            if (content == null)
                return BadRequest(new {Error = "Content should not be null."});

            var response = await _mediator.Send(new AddDataRequest
            {
                DiffId = id,
                Side = DataSide.Left,
                Data = Convert.FromBase64String(content.Data)
            });

            if (!response.Success)
            {
                return BadRequest(new {Error = response.ErrorMessage});
            }
                
            return Ok(new { DiffId = id, Side = "Left" });
        }
    }

}