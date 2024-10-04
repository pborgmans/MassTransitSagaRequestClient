using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MassTransitSagaRequestClient.StateMachine.Extensions
{
    /// <summary>
    /// Extensions to be used in state machines
    /// </summary>
    public static class SagaStateMachineExtensions
    {
        #region Public Methods

        public static void LogWithSaga<TSaga>(this ILogger logger, string sagaKey, TSaga saga, Action log)
        {
            using (logger.BeginScope(new Dictionary<string, object>()
            {
                {"@" + sagaKey, saga! }
            }))
            {
                log.Invoke();
            }
        }

        #endregion Public Methods
    }
}
