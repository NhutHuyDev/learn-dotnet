using DependencyInjectionExample.Services;

namespace DependencyInjectionExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddTransient<ITransientService, TransientService>();
            builder.Services.AddSingleton<ISingletonService, SingletonService>();
            builder.Services.AddScoped<IScopedService, ScopedService>();

            var app = builder.Build();

            app.MapControllers();

            app.Run();
        }
    }
}
