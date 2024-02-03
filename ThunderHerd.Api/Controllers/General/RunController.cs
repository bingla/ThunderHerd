using Microsoft.AspNetCore.Mvc;
using ThunderHerd.Core.Models.Dtos;
using ThunderHerd.Core.Models.Requests;
using ThunderHerd.Core.Models.Responses;
using ThunderHerd.Domain.Interfaces;

namespace ThunderHerd.Api.Controllers.General
{
    [ApiController]
    [Route("[controller]")]
    public class RunController : ControllerBase
    {
        private readonly IRunService _runService;

        public RunController(IRunService runService)
        {
            _runService = runService;
        }

        [HttpPatch]
        public async Task<IActionResult> Get(RunRequest request, CancellationToken cancellationToken)
        {
            var result = await _runService.RunAsync(Run.Map(request), cancellationToken);
            return Ok(RunResponse.Map(result));
        }
    }
}
