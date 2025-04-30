using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly IMemoryCache _cache;

        public TodosController(ITodoService todoService, IMemoryCache cache)
        {
            _todoService = todoService;
            _cache = cache;
        }




        [HttpGet]
        public IActionResult GetAll()
        {
            const string cacheKey = "todoList";

            List<Todo>? todos = null;
            if (_cache == null || !_cache.TryGetValue(cacheKey, out todos) || todos == null)
            {
                todos = _todoService.GetAll();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30)); // delete  after 30 seconds of inactivity

                _cache?.Set(cacheKey, todos, cacheOptions);
            }

            return Ok(todos);
        }


        [HttpGet("byCategory/{categoryId}")]
        public IActionResult GetByCategoryId(int categoryId)
        {
            var todos = _todoService.GetByCategoryId(categoryId);
            return Ok(todos);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var todo = _todoService.GetById(id);
            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        [HttpPost]
        public IActionResult Add(Todo todo)
        {
            _todoService.Add(todo);
            _cache?.Remove("todoList");
            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Todo updatedTodo)
        {
            if (id != updatedTodo.Id)
                return BadRequest();

            var existing = _todoService.GetById(id);
            if (existing == null)
                return NotFound();

            _todoService.Update(updatedTodo);
            _cache?.Remove("todoList");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _todoService.GetById(id);
            if (existing == null)
                return NotFound();

            _todoService.Delete(id);
            _cache?.Remove("todoList");
            return NoContent();
        }
    }
}
