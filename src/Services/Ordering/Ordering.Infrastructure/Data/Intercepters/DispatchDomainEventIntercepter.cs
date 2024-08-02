using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Intercepters;

public class DispatchDomainEventIntercepter(IMediator mediator):SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public async Task DispatchDomainEvents(DbContext? context)
    {
        if(context == null) return;

        var aggregator=context.ChangeTracker.Entries<IAggregate>()
            .Where(a=>a.Entity.DomainEvents.Any())
            .Select(a=>a.Entity).ToList();
        var domainevents=aggregator.SelectMany(a=>a.DomainEvents).ToList();
        aggregator.ToList().ForEach(a=>a.ClearDomainEvents());
        foreach(var domainEvent in domainevents)
        {
           await mediator.Publish(domainEvent);
        }
    }
}

