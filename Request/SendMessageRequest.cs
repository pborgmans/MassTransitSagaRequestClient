using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitSagaRequestClient.Request
{
    [EntityName("partner.requests.chatmessage.send")]
    public class SendMessageRequest
    {
        #region Public Properties

        public string Message { get; set; }
        public int InteractionId { get; set; }

        /// <summary>
        /// Participant id for the sender of the message
        /// </summary>
        public int SendingParticipantId { get; set; }

        public DateTime Timestamp { get; }
        public int InteractionSessionId { get; }
        public string DeviceId { get; }
        public string SessionId { get; set; }
        public Guid CorrelationId { get; }

        #endregion Public Properties

        #region Public Constructors + Destructors

        #region Public Constructors

        /// <summary>
        /// ctor
        /// </summary>
        public SendMessageRequest(string message, int interactionId, int sendingParticipantId, DateTime timestamp, string sessionId, int interactionSessionId, string deviceId)
        {
            Message = message;
            InteractionId = interactionId;
            SendingParticipantId = sendingParticipantId;
            Timestamp = timestamp;
            SessionId = sessionId;
            InteractionSessionId = interactionSessionId;
            DeviceId = deviceId;
            CorrelationId = Guid.NewGuid();
        }

        #endregion Public Constructors

        #endregion Public Constructors + Destructors
    }
}
