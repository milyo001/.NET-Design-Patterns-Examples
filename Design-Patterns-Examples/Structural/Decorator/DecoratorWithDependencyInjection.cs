using Autofac;

namespace AutofacDemos
{
    public interface IReportingService
    {
        void Report();
    }

    // Let's say we have a service that reports the current time
    public class ReportingService : IReportingService
    {
        public void Report()
        {
            Console.WriteLine("Here is your report");
        }
    }

    // And we need to alter the functionality of the reporting service, withoud chaging the service
    public class ReportingServiceWithLogging : IReportingService
    {
        private IReportingService decorated;

        // Injected from the constrcutor
        public ReportingServiceWithLogging(IReportingService decorated)
        {
            if (decorated == null)
            {
                throw new ArgumentNullException(paramName: nameof(decorated));
            }
            this.decorated = decorated;
        }

        public void Report()
        {
            Console.WriteLine("Commencing log...");
            // Here we reuse the functionality of the reporting service which is injected from constructor
            decorated.Report();
            Console.WriteLine("Ending log...");
        }
    }

    public class Decorators
    {
        static void Main_(string[] args)
        {
            // Dependency injection with autofac
            
            var b = new ContainerBuilder();
            b.RegisterType<ReportingService>().Named<IReportingService>("reporting");
            b.RegisterDecorator<IReportingService>(
                (context, service) => new ReportingServiceWithLogging(service),
              "reporting");

            // open generic decorators also supported
            // b.RegisterGenericDecorator()

            using (var c = b.Build())
            {
                var r = c.Resolve<IReportingService>();
                r.Report();
            }
        }
    }
}