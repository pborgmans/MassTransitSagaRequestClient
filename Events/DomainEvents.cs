using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitSagaRequestClient.Events
{
    [EntityName("partner.domain.chat.send.requested")]
    public record SendChatRequestedDomainEvent(int InteractionId, string Message, int SendingParticipantId, string SessionId, int InteractionSessionId, string DeviceId, DateTime Timestamp, Guid CorrelationId) : CorrelatedBy<Guid>;

}
