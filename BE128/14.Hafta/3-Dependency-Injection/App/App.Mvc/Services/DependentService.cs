namespace App.Mvc.Services
{
    public class DependentService<TService> where TService : BaseService
    {
        private readonly TService _service;

        public DependentService(TService service)
        {
            _service = service;
        }

        public int GetOtherServiceCounter()
        {
            return _service.GetCounter();
        }
    }
}
