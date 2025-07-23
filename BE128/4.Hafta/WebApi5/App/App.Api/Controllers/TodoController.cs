using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {

        //aynı liste üzerinde işlemler yapaabilmek için liste static olmalıdır
        // Çünkü TodoController'a gelen her http isteğinde TodoController new'lenir.
        private static List<TodoItem> _todoItems = new(); // yapılacaklar listesi

        [HttpGet]
        public IEnumerable<TodoItem> GetList() // bütün listeyi döndüren metot
        {
            return _todoItems;
        }


        [HttpGet("{id}")]
        public TodoItem? GetItem(int id) // tek bir todo elemanı döndüren metot
        {
            return _todoItems.Find(x => x.Id == id);
        }

        [HttpPost]
        public void AddItem(string description)
        {
            var item = new TodoItem
            {
                Id = _todoItems.Count + 1,
                Description = description
            };
            _todoItems.Add(item);
        }
    }
}
