using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iGotUp.Api.Data;
using iGotUp.Api.Data.Entities;
using iGotUp.Api.Data.Repositories;
using iGotUp.Api.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace iGotUp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunController : ControllerBase
    {
        private readonly IRunRepository runRepository;
        private readonly ILogger<RunController> logger;
        private readonly IMapper mapper;
        private readonly GotUpContext ctx;

        public RunController(IRunRepository runRepository, ILogger<RunController> logger, IMapper mapper, GotUpContext ctx)
        {
            this.runRepository = runRepository;
            this.logger = logger;
            this.mapper = mapper;
            this.ctx = ctx;
        }

        [HttpGet]
        [Route("/api/run/get_location_results")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> get_location_results([FromBody]Search search)
        {
            try
            {
                return Ok(await this.runRepository.GetLocationResults(search.lat, search.lng, search.radius));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Route("/api/run/get_pickup_games")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> get_pickup_games()
        {
            try
            {
                return Ok(runRepository.get_pickup_games());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Route("/api/run/get_run_detail/{run_id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> get_run_details(int run_id)
        {
            try
            {
                return Ok(this.runRepository.get_run_detail(run_id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Route("/api/run/add_run")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> add_run([FromBody] Run run)
        {
            try
            {
                this.runRepository.addRun(run);
                if (this.ctx.SaveChanges() > 0)
                {
                    return this.Ok();
                }
                else
                {
                    return this.BadRequest($"Save of run wasn't successful");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [Route("/api/run/reserve_run")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> reserver_run([FromBody] RunPlayers reserve)
        {
            try
            {
                this.runRepository.reserveRun(reserve.user_id, reserve.run_id);
                if (this.ctx.SaveChanges() > 0)
                {
                    return this.Ok();
                }
                else
                {
                    return this.BadRequest($"Reserve run wasn't successful");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}