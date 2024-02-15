using ThunderHerd.Core.Models.Dtos;
using ThunderHerd.DataAccess.Interfaces;
using ThunderHerd.Domain.Interfaces;

namespace ThunderHerd.Domain.Services
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;

        public TestService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public async Task<Test?> GetTestAsync(Guid testId, CancellationToken cancellationToken = default)
        {
            return Test.Map(await _testRepository.FindAsync(testId, cancellationToken));
        }

        public async Task<Test> CreateTestAsync(Test test, CancellationToken cancellationToken = default)
        {
            var entity = await _testRepository.CreateAsync(Core.Entities.Test.Map(test), cancellationToken);
            return Test.Map(entity);
        }
    }
}
