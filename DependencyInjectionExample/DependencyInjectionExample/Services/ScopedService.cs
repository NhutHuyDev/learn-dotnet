namespace DependencyInjectionExample.Services
{
    public interface IScopedService
    {
        Guid GetOperationId();
    }

    public class ScopedService : IScopedService
    {
        private readonly Guid _operationId;

        public ScopedService()
        {
            _operationId = Guid.NewGuid();
        }

        public Guid GetOperationId() => _operationId;
    }

}
