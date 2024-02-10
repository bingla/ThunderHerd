using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderHerd.Core.Entities;
using ThunderHerd.Core.Models.Dtos;
using ThunderHerd.DataAccess.Interfaces;
using ThunderHerd.Domain.Interfaces;

namespace ThunderHerd.Domain.Services
{
    /// <summary>
    /// Run scheduler
    /// </summary>
    public class ScheduleService : IScheduleService
    {
        private readonly IRunService _runService;
        private readonly IRunRepository _runRepository;

        public ScheduleService(IRunService runService,
            IRunRepository runRepository)
        {
            _runService = runService;
            _runRepository = runRepository;
        }

        public async Task ScheduleRunAsync(Core.Models.Dtos.Run run, CancellationToken cancellationToken)
        {
            // TODO: Map run item to entity and save
            var entity = await _runRepository.CreateAsync(Core.Entities.Run.Map(run), cancellationToken);

            // TODO: Schedule with HangFire
            if (!entity.Recurring)
            {
                // Schedule to run immediately
                BackgroundJob.Enqueue(() => _runService.RunAsync(entity.Id, CancellationToken.None));
            }
            else
            {
                // TODO: Schedule to run on cronjon
            }
        }
    }
}
