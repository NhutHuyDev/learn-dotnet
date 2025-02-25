namespace DependencyInjectionExample.Services
{
    public interface ITransientService
    {
        Guid GetOperationId();
    }

    public class TransientService : ITransientService
    {
        private readonly Guid _operationId;

        public TransientService()
        {
            _operationId = Guid.NewGuid();
        }

        public Guid GetOperationId() => _operationId;
    }

}
