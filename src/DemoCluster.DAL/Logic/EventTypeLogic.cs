using DemoCluster.DAL.Database.Configuration;
using DemoCluster.Models;
using DemoCluster.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DemoCluster.DAL.Logic
{
    public class EventTypeLogic
    {
        private readonly ILogger logger;
        private readonly IRepository<EventType, ConfigurationContext> events;

        public EventTypeLogic(ILogger<EventTypeLogic> logger, 
            IRepository<EventType, ConfigurationContext> events)
        {
            this.logger = logger;
            this.events = events;
        }

        public async Task<List<EventTypeViewModel>> GetEventListAsync(CancellationToken token = default(CancellationToken))
        {
            IEnumerable<EventType> listResults = await events.AllAsync(token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<EventTypeViewModel>> GetEventListAsync(Expression<Func<EventType, bool>> filter,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<EventType> listResults = await events.FindByAsync(filter);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<EventTypeViewModel>> GetEventListAsync(Expression<Func<EventType, bool>> filter,
            Func<IQueryable<EventType>, IOrderedQueryable<EventType>> orderBy,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<EventType> listResults = await events.FindByAsync(filter, orderBy);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<PaginatedList<EventTypeViewModel>> GetEventPageAsync(string filter, 
            int pageIndex, 
            int pageSize,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<EventType> listResults = await events.AllAsync(token);

            return listResults
                .Where(s => string.IsNullOrEmpty(filter) || s.Name.Contains(filter))
                .Select(o => o.ToViewModel())
                .ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<EventTypeViewModel> GetEventAsync(int eventId,
            CancellationToken token = default(CancellationToken))
        {
            EventType result = await events.FindByKeyAsync(eventId);

            return result?.ToViewModel();
        }

        public async Task<EventTypeViewModel> GetEventAsync(string eventName,
            CancellationToken token = default(CancellationToken))
        {
            EventType result = await events.FindByKeyAsync(eventName);

            return result?.ToViewModel();
        }

        public async Task<EventTypeViewModel> SaveEventAsync(EventTypeViewModel model,
            CancellationToken token = default(CancellationToken))
        {
            EventType eventItem = null;

            try
            {
                RepositoryResult result = null;

                if (!model.EventId.HasValue)
                {
                    result = await events.CreateAsync(model.ToModel());
                }
                else
                {
                    EventType original = await events.FindByKeyAsync(model.EventId.Value);
                    result = await events.UpdateAsync(original, model.ToModel());
                }

                if (result.Succeeded)
                {
                    logger.LogInformation($"Created event type {model.Name}");

                    eventItem = await events.FindByKeyAsync(model.Name);
                    if (eventItem == null)
                    {
                        logger.LogError($"Unable to find event type {model.Name} as result.");
                    }
                }
                else
                {
                    LogErrors(result.Errors);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error saving event type {model.Name}");
                throw;
            }

            return eventItem?.ToViewModel();
        }

        public async Task RemoveEventAsync(EventTypeViewModel model,
            CancellationToken token = default(CancellationToken))
        {
            try
            {
                RepositoryResult result = await events.DeleteAsync(model.ToModel());

                if (result.Succeeded)
                {
                    logger.LogInformation($"Removed event type {model.Name} successfully.");
                }
                else
                {
                    LogErrors(result.Errors);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error removing event type {model.Name}");
                throw;
            }
        }

        private void LogErrors(IEnumerable<RepositoryError> errors)
        {
            foreach (var error in errors)
            {
                logger.LogError($"{error.Code} - {error.Description}");
            }
        }
    }
}