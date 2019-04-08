using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iGotUp.Api.Data;
using iGotUp.Api.Data.Repositories;
using iGotUp.Api.Model;
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
    }
}