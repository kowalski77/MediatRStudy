using System;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace Receiver.App
{
    public class StudyConsumer : IConsumer<ValueEntered>
    {
        private readonly ITestService testService;

        public StudyConsumer(ITestService testService)
        {
            this.testService = testService ?? throw new ArgumentNullException(nameof(testService));
        }

        public Task Consume(ConsumeContext<ValueEntered> context)
        {
            Console.WriteLine($"Consumer activated, Value: {context.Message.Value}, from testService: {this.testService.GetTest()}");

            return Task.CompletedTask;
        }
    }
}