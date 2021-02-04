using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Polly;
using Publisher.App.Crosscutting.DomainUtils;
using Publisher.App.Persistence;

namespace Publisher.App.Application.Infrastructure
{
    public sealed class RetryDecoratorNotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : INotification
    {
        private readonly INotificationHandler<TNotification> intercepted;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<RetryDecoratorNotificationHandler<TNotification>> logger;

        public RetryDecoratorNotificationHandler(
            INotificationHandler<TNotification> intercepted,
            IUnitOfWork unitOfWork,
            ILogger<RetryDecoratorNotificationHandler<TNotification>> logger)
        {
            this.intercepted = intercepted ?? throw new ArgumentNullException(nameof(intercepted));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"{nameof(RetryDecoratorNotificationHandler<TNotification>)} activated");

            var policy = Policy
                .Handle<UoWException>()
                .Or<MongoCommandException>()
                .WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromMilliseconds(500), (ex, timeSpan, count, context) =>
                {
                    this.logger.LogWarning($"Error in {typeof(RetryDecoratorNotificationHandler<TNotification>).Name} " +
                                      $"-> {ex.Message} {Environment.NewLine} ... retry: {count} in count: {timeSpan.Seconds} seconds ");
                });

            await policy.ExecuteAsync(async () =>
            {
                await this.unitOfWork.StartAsync().ConfigureAwait(false);
                await this.intercepted.Handle(notification, cancellationToken).ConfigureAwait(false);
                await this.unitOfWork.CommitAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            this.logger.LogInformation($"{nameof(RetryDecoratorNotificationHandler<TNotification>)} finished");
        }
    }
}