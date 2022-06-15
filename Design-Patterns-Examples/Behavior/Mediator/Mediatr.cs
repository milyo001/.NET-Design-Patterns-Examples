using Autofac;
using JetBrains.Annotations;
using MediatR; // Needs to be installed

namespace DotNetDesignPatternDemos.Behavioral.Mediator.Mediatr
{
    public class PongResponse
    {
        // When request was recieved
        public DateTime Timestamp;

        public PongResponse(DateTime timestamp)
        {
            Timestamp = timestamp;
        }
    }
    
    public class PingCommand : IRequest<PongResponse>
    {
        // Nothing here
    }

    // The command handler, configured in dependancy injection framework (AutoFac in our case)
    [UsedImplicitly] // Part of JetBrains.Annotaions
    public class PingCommandHandler : IRequestHandler<PingCommand, PongResponse>
    {
        public async Task<PongResponse> Handle(PingCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new PongResponse(DateTime.UtcNow))
              .ConfigureAwait(false); // Avoid deadlocks
        }
    }

    public class Demo
    {
        public static async Task Main()
        {
            // AutoFac
            var builder = new ContainerBuilder();
            
            builder.RegisterType<IMediator>() // or builder.RegisterType<Mediator>()
              .As<IMediator>()
              .InstancePerLifetimeScope(); // singleton

            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterAssemblyTypes(typeof(Demo).Assembly)
              .AsImplementedInterfaces();

            var container = builder.Build();
            var mediator = container.Resolve<IMediator>();
            var response = await mediator.Send(new PingCommand());
            Console.WriteLine($"We got a pong at {response.Timestamp}");
        }
    }
}