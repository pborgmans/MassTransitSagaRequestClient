using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitSagaRequestClient.StateMachine
{
    public class SendChatMessageState :
       SagaStateMachineInstance, ISagaVersion
    {
        #region Public Properties

        public Guid CorrelationId { get; set; }

        public string? CurrentState { get; set; }

        public int Version { get; set; }

        public int InteractionId { get; set; }
        public int InteractionSessionId { get; set; }
        public int SendingParticipantId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; } = default!;
        public string DeviceId { get; set; } = default!;
        public string SessionId { get; set; } = default!;

        #endregion Public Properties
    }
}
