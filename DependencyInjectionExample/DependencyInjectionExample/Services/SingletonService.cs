namespace DependencyInjectionExample.Services
{
    public interface ISingletonService
    {
        Guid GetOperationId();
    }

    public class SingletonService : ISingletonService
    {
        private readonly Guid _operationId;

        public SingletonService()
        {
            _operationId = Guid.NewGuid();
        }

        public Guid GetOperationId() => _operationId;
    }

}
