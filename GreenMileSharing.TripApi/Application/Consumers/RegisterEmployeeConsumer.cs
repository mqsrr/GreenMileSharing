using GreenMileSharing.Messages;
using GreenMileSharing.TripApi.Application.Mappers;
using GreenMileSharing.TripApi.Persistence;
using MassTransit;

namespace GreenMileSharing.TripApi.Application.Consumers;

internal sealed class RegisterEmployeeConsumer(TripDbContext dbContext) : IConsumer<RegisterEmployee>
{
    private readonly TripDbContext _dbContext = dbContext;

    public async Task Consume(ConsumeContext<RegisterEmployee> context)
    {
        await _dbContext.AddAsync(context.Message.ToEmployee(), context.CancellationToken);
        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}