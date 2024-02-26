using ThunderHerd.Core.Models.Dtos;
using ThunderHerd.DataAccess.Interfaces;
using ThunderHerd.Domain.Interfaces;

namespace ThunderHerd.Domain.Services
{
    public class TestResultService : ITestResultService
    {
        private readonly ITestResultRepository _testResultRepository;
        private readonly ITestResultItemRepository _testResultItemRepository;

        public TestResultService(ITestResultRepository testResultRepository,
            ITestResultItemRepository testResultItemRepository)
        {
            _testResultRepository = testResultRepository;
            _testResultItemRepository = testResultItemRepository;
        }

        public async Task<TestResult?> CreateTestResultAsync(TestResult testResult, CancellationToken cancellationToken = default)
        {
            var entity = await _testResultRepository.CreateAsync(Core.Entities.TestResult.Map(testResult), cancellationToken);
            return TestResult.Map(entity);
        }

        public async Task<TestResult?> UpdateTestResultAsync(TestResult testResult, CancellationToken cancellationToken = default)
        {
            var entity = await _testResultRepository.UpdateAsync(testResult.Id, Core.Entities.TestResult.Map(testResult), cancellationToken);
            return TestResult.Map(entity);
        }


        public async Task<TestResult?> FindTestResultAsync(Guid testResultId, CancellationToken cancellationToken = default)
        {
            var entity = await _testResultRepository.FindAsync(testResultId, cancellationToken);
            return TestResult.Map(entity);
        }

        public async IAsyncEnumerable<TestResult?> FindTestResultsByTestId(Guid testId)
        {
            await foreach (var entity in _testResultRepository.GetTestResultsByTestId(testId))
            {
                yield return TestResult.Map(entity);
            }
        }

        public async Task<IEnumerable<TestResultGroup>> GroupTestResultItemsByTime(Guid testResultId, TimeSpan timespan)
        {
            var items = await _testResultItemRepository.GetTestResultItemsAsync(testResultId, true);

            return items
                    .OrderBy(p => p.Ticks)
                    .GroupBy(p => p.Ticks / timespan.Ticks, TestResultItem.Map)
                    .Select(TestResultGroup.Map);
        }

        public async Task CreateTestResultItem(TestResultItem testResultItem, CancellationToken cancellationToken)
        {
            await _testResultItemRepository.CreateAsync(Core.Entities.TestResultItem.Map(testResultItem), cancellationToken);
        }

        public async Task CreateTestResultItems(IEnumerable<TestResultItem> items, CancellationToken cancellationToken)
        {
            await _testResultItemRepository.CreateAsync(items.Select(Core.Entities.TestResultItem.Map), cancellationToken);
        }
    }
}
