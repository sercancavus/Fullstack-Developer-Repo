using Microsoft.AspNetCore.Mvc;
using TeamTaskTracker.Models;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace TeamTaskTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private static ConcurrentBag<TaskItem> tasks = new();

        [HttpGet]
        public IActionResult GetAll() => Ok(tasks);

        [HttpPost]
        public IActionResult Add([FromBody] TaskItem task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            task.Id = tasks.Count + 1;
            tasks.Add(task);
            return CreatedAtAction(nameof(GetAll), new { id = task.Id }, task);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = tasks.FirstOrDefault(t => t.Id == id);
            if (item == null) return NotFound();
            tasks = new ConcurrentBag<TaskItem>(tasks.Where(t => t.Id != id));
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TaskItem updatedTask)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var item = tasks.FirstOrDefault(t => t.Id == id);
            if (item == null) return NotFound();

            item.Title = updatedTask.Title;
            item.IsCompleted = updatedTask.IsCompleted;
            return NoContent();
        }

        [HttpGet("filter")]
        public IActionResult FilterByCompletion([FromQuery] bool isCompleted)
        {
            var filtered = tasks.Where(t => t.IsCompleted == isCompleted);
            return Ok(filtered);
        }

        [HttpGet("paged")]
        public IActionResult GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool includeDetails = true,
            [FromQuery] string sortBy = "Id",
            [FromQuery] string sortOrder = "asc")
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Sayfa ve sayfa boyutu 1'den küçük olamaz.");
            IEnumerable<TaskItem> query = tasks;
            query = sortBy.ToLower() switch
            {
                "title" => sortOrder == "desc" ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
                "duedate" => sortOrder == "desc" ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate),
                "priority" => sortOrder == "desc" ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority),
                _ => sortOrder == "desc" ? query.OrderByDescending(t => t.Id) : query.OrderBy(t => t.Id)
            };
            var paged = query.Skip((page - 1) * pageSize).Take(pageSize);
            if (!includeDetails)
            {
                var simple = paged.Select(t => new {
                    t.Id,
                    t.Title,
                    t.IsCompleted
                });
                return Ok(simple);
            }
            return Ok(paged);
        }
    }
}