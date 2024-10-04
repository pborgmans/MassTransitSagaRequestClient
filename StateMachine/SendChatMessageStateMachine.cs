using MassTransit;
using MassTransitSagaRequestClient.Events;
using MassTransitSagaRequestClient.Request;
using MassTransitSagaRequestClient.StateMachine.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitSagaRequestClient.StateMachine
{
    /// <summary>
    /// Extensions for the change state machine
    /// </summary>
    public static class SendChatMessageStateMachineExtensions
    {
        #region Public Methods

        public static EventActivityBinder<SendChatMessageState, SendChatRequestedDomainEvent> Initialize(this EventActivityBinder<SendChatMessageState, SendChatRequestedDomainEvent> binder,
        ILogger logger)
        {
            return binder.Then(context => {
                context.Saga.InteractionId = context.Message.InteractionId;
                context.Saga.SendingParticipantId = context.Message.SendingParticipantId;
                context.Saga.TimeStamp = context.Message.Timestamp;
                context.Saga.Message = context.Message.Message;
                context.Saga.SessionId = context.Message.SessionId;
                context.Saga.InteractionSessionId = context.Message.InteractionSessionId;
                context.Saga.DeviceId = context.Message.DeviceId;
            });
        }

        #endregion Public Methods
    }

    /// <summary>
    /// Agent change state machine
    /// </summary>
    public class SendChatMessageStateMachine :
                MassTransitStateMachine<SendChatMessageState>
    {
        #region Private Fields

        private const string SAGA_KEY = "Saga";

        #endregion Private Fields

        #region Public Properties

        public Request<SendChatMessageState, SendMessageRequest, SendMessageResponse> SendChatRequest { get; private set; } = null!;

        public State? SendChat { get; private set; }
        public State? Failed { get; private set; }

        public Event<SendChatRequestedDomainEvent>? OnSendChatRequested { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public SendChatMessageStateMachine(ILogger<SendChatMessageStateMachine> logger)
        {
            InstanceState(x => x.CurrentState);

            // start state machine when changestate requested is incoming
            During(Initial,
                When(OnSendChatRequested)
                    .Initialize(logger)
                    .Then(x =>
                        logger.LogWithSaga(SAGA_KEY, x.Saga, () => logger.LogInformation("Send chat message requested for interaction {id}", x.Saga.InteractionId)))
                    .TransitionTo(SendChat)
                    );

            WhenEnter(SendChat, x => x
                .Request(SendChatRequest, x => new SendMessageRequest(x.Saga.Message, x.Saga.InteractionId, x.Saga.SendingParticipantId, x.Saga.TimeStamp, x.Saga.SessionId, x.Saga.InteractionSessionId, x.Saga.DeviceId))
                .TransitionTo(SendChatRequest.Pending));

            During(SendChatRequest.Pending,
                When(SendChatRequest.Completed)
                    .Then(x => logger.LogWithSaga(SAGA_KEY, x.Saga, () => logger.LogInformation("Send chat succeeded for interaction {id}", x.Saga.InteractionId)))
                    .IfElse(x => x.Message.Result.IsSuccess,
                        x => x
                        .Finalize(),
                        x => x
                        .Then(x => logger.LogWithSaga(SAGA_KEY, x.Saga, () => logger.LogInformation("Send chat failed for interaction {id} with result {@result}", x.Saga.InteractionId, x.Message.Result)))
                        .TransitionTo(Failed))
                    ,

                When(SendChatRequest.TimeoutExpired)
                    .Then(x => logger.LogWithSaga(SAGA_KEY, x.Saga, () => logger.LogWarning("Send chat failed, retry to setup chat connection and try again.")))
                    .TransitionTo(Failed)
                    ,

                When(SendChatRequest.Faulted)
                    .Then(x => logger.LogWithSaga(SAGA_KEY, x.Saga, () => logger.LogWarning("Send chat failed, finalize and send event to client")))
                    .TransitionTo(Failed));

            // publish event when failed at 1 place
            WhenEnter(Failed, binder => binder
                .Finalize());

            SetCompletedWhenFinalized(); // remove saga when finalized to not in cache anymore
        }

        #endregion Public Constructors
    }
}
