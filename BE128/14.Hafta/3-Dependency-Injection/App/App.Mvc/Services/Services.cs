namespace App.Mvc.Services
{
    public class SingletonService : BaseService
    {
    }
    public class ScopedService : BaseService
    {
    }
    public class TransientService : BaseService
    {
    }
    public abstract class BaseService
    {
        private int _counter = 0;

        public int GetCounter()
        {
            return ++_counter;
        }
    }
}
