using DependencyInjectionExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace DependencyInjectionExample.Controllers
{
    public class FirstController : Controller
    {
        private readonly ITransientService _transientService1;
        private readonly ITransientService _transientService2;

        private readonly ISingletonService _singletonService1;
        private readonly ISingletonService _singletonService2;

        private readonly IScopedService _scopedService1;
        private readonly IScopedService _scopedService2;

        public FirstController(
            ITransientService transientService1, 
            ITransientService transientService2, 
            ISingletonService singletonService1, 
            ISingletonService singletonService2,
            IScopedService scopedService1,
            IScopedService scopedService2)
        {
            _transientService1 = transientService1;
            _transientService2 = transientService2;

            _singletonService1 = singletonService1;
            _singletonService2 = singletonService2;

            _scopedService1 = scopedService1;
            _scopedService2 = scopedService2;
        }

        [Route("/first/transient")]
        public ContentResult FirstTransient()
        {
            // Hai instance khác nhau
            var id1 = _transientService1.GetOperationId();
            var id2 = _transientService2.GetOperationId();

            return Content($"Transient1: {id1}, Transient2: {id2}");
        }

        [Route("/first/singleton")]
        public IActionResult FirstSingleton()
        {
            // Dùng chung một instance trong toàn bộ ứng dụng
            var id1 = _singletonService1.GetOperationId();
            var id2 = _singletonService2.GetOperationId();

            return Content($"Singleton1: {id1}, Singleton2: {id2}");
        }

        [Route("/first/scoped")]
        public IActionResult FirstScoped()
        {
            // Cùng request (tức cùng một scope), cả hai service sẽ có chung ID
            var id1 = _scopedService1.GetOperationId();
            var id2 = _scopedService2.GetOperationId();

            return Content($"Scoped1: {id1}, Scoped2: {id2}");
        }
    }
}
