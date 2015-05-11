using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Casper.Domain.Features.Files;
using Casper.Domain.Features.Files.Commands;
using Casper.Domain.Features.Files.Events;
using Casper.Domain.Infrastructure.Messaging;
using Casper.Domain.Specifications.Helpers;
using Casper.Domain.Specifications.Helpers.Dummies;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Casper.Domain.Specifications.Features.Files.Steps
{
    [Binding]
    public class UploadFileSteps
    {
        private readonly ActualData _actual;
        private readonly ICommandBus _commandBus;
        private readonly GivenData _given;
        private readonly InvocationRecorder _invocationRecorder;

        public UploadFileSteps(GivenData given, ActualData actual, ICommandBus commandBus, IEventBus eventBus, InvocationRecorder invocationRecorder)
        {
            _given = given;
            _actual = actual;
            _commandBus = commandBus;
            _invocationRecorder = invocationRecorder;

            eventBus.SubscribeTo<UploadedFileEvent>(HandleUploadedFileEvent);
        }

        [Given(@"UploadFile command is valid")]
        public void GivenUploadFileCommandIsValid()
        {
            var uploadedFile = new DummyHttpPostedFile();
            _given.Command = new UploadFile(uploadedFile, "dummy/relative/directory", "dummy-url-friendly-name.jpg", Dummy.DateTimeOffset(), Dummy.Author());
        }

        [Then(@"a UploadedFile event should be published")]
        public void ThenAUploadedFileEventShouldBePublished()
        {
            _actual.PublishedEvents.Count(e => e.GetType() == typeof(UploadedFileEvent)).Should().Be(1, "because the UploadedFileEvent event should have been published by the EventBus");
        }

        [Then(@"the file should be saved")]
        public void ThenTheFileShouldBeSaved()
        {
            var methods = _invocationRecorder.CallsTo<IFileRepository>();
            var method = methods.Single(m => m.Method.Name == "PublishAsync");
            var parameter = (UploadedFile)method.Arguments[0];
            var command = (UploadFile)_given.Command;

            parameter.RelativeUri.Should().Be(Path.Combine(command.RelativeDirectory, command.UrlFriendlyFileNameWithExtension));
        }

        [When(@"I send the UploadFile command")]
        public void WhenISendTheUploadFileCommand()
        {
            _commandBus.SendCommandAsync(_given.Command).Wait();
        }

        private Task HandleUploadedFileEvent(UploadedFileEvent @event)
        {
            _actual.PublishedEvents.Add(@event);

            return Task.FromResult(0);
        }
    }
}