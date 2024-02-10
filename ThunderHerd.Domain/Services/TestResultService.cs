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

        public async IAsyncEnumerable<TestResultItem?> FindTestResultItemsByTestResultId(Guid testResultId)
        {
            await foreach (var entity in _testResultItemRepository.GetTestResultItems(testResultId))
            {
                yield return TestResultItem.Map(entity);
            }
        }
    }
}
