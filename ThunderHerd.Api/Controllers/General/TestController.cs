using Microsoft.AspNetCore.Mvc;
using ThunderHerd.Core.Models.Dtos;
using ThunderHerd.Core.Models.Requests;
using ThunderHerd.Core.Models.Responses;
using ThunderHerd.Domain.Interfaces;

namespace ThunderHerd.Api.Controllers.General
{
    [ApiController]
    [Route("v1/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;
        private readonly ITestResultService _testResultService;
        private readonly IScheduleService _scheduleService;

        public TestController(ITestService testService,
            ITestResultService testResultService,
            IScheduleService scheduleService)
        {
            _testService = testService;
            _testResultService = testResultService;
            _scheduleService = scheduleService;
        }

        /// <summary>
        /// Create a new test
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTest(TestCreateRequest request, [FromQuery] bool start, CancellationToken cancellationToken)
        {
            var result = await _testService.CreateTestAsync(Test.Map(request), cancellationToken);

            if (start)
            {
                await _scheduleService.ScheduleTestImmediatelyAsync(result.Id, cancellationToken);
            }

            return Ok(TestCreateResponse.Map(result));
        }

        /// <summary>
        /// Start a test
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch("{testId}/start")]
        public async Task<IActionResult> StartTest(Guid testId, CancellationToken cancellationToken)
        {
            await _scheduleService.ScheduleTestImmediatelyAsync(testId, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Create a new test schedule
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("{testId}/schedule")]
        public async Task<IActionResult> ScheduleTest(Guid testId, ScheduleRequest request, CancellationToken cancellationToken)
        {
            var result = await _scheduleService.CreateScheduleAsync(Schedule.Map(testId, request), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Stream all TestResults
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        [HttpGet("{testId}/results")]
        public async IAsyncEnumerable<TestResultResponse?> GetTestResultsByTestId(Guid testId)
        {
            await foreach (var entity in _testResultService.FindTestResultsByTestId(testId))
            {
                yield return TestResultResponse.Map(entity);
            }
        }

        /// <summary>
        /// Stream all TestResultItems
        /// </summary>
        /// <param name="testResultId"></param>
        /// <returns></returns>
        [HttpGet("result/{testResultId}/results")]
        public async Task<IActionResult> GetTestResultItemsByTestResultId(Guid testResultId,
            [FromQuery] int timeSpan = 1)
        {
            var timespan = TimeSpan.FromSeconds(timeSpan < 1 ? 1 : timeSpan);
            var result = await _testResultService.GroupTestResultItemsByTime(testResultId, timespan);
            return Ok(result.Select(TestResultGroupResponse.Map));
        }
    }
}
