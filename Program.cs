// See https://aka.ms/new-console-template for more information
using MassTransit;
using MassTransitSagaRequestClient.Request;
using MassTransitSagaRequestClient.StateMachine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => 
    {
        services.AddMassTransit(x => {
            x.UsingInMemory((ctx, cfg) => {
                cfg.ConfigureEndpoints(ctx);
            });
            x.AddRequestClient<SendMessageRequest>();

            x.AddSagaStateMachine<SendChatMessageStateMachine, SendChatMessageState>();
        });
    })
    .Build();

host.Start();
