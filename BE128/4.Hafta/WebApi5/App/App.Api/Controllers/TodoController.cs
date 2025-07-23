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

        // listenin static olmasının sebebi, uygulama çalıştığı sürece bu listenin bellekte kalmasıdır, yoksa her http isteğinde yeni bir liste oluşturulurdu.

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

        [HttpPut("{id}")]
        public void UpdateItem(int id, string description) // todo elemanını güncelleyen metot
        {
            var index = _todoItems.FindIndex(x => x.Id == id);
            if (index == -1) // parametreden gelen id'li eleman yoksa
            {
                return; // early return, item bulunamadıysa metottan çık
            }
            _todoItems[index].Description = description;
        }

      [HttpDelete("{id}")]
        public void DeleteItem(int id) // todo elemanını silen metot
        {
            var index = _todoItems.FindIndex(x => x.Id == id);
            if (index == -1) // parametreden gelen id'li eleman yoksa
            {
                return; // early return, item bulunamadıysa metottan çık
            }
            _todoItems.RemoveAt(index); // index'teki elemanı sil
        }
    }
}
