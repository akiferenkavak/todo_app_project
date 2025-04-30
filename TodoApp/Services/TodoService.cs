using TodoApp.Models;
using TodoApp.Data;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Services
{
    public class TodoService : ITodoService
    {
        private readonly AppDbContext _context;

        public TodoService(AppDbContext context)
        {
            _context = context;
        }

        public List<Todo> GetAll()
        {
            return _context.Todos.Include(t => t.Category).ToList();
        }


        public List<Todo> GetByCategoryId(int categoryId)
        {
            return _context.Todos.Where(t => t.CategoryId == categoryId).ToList();
        }

        public Todo? GetById(int id)
        {
            return _context.Todos.Find(id);
        }

        public void Add(Todo todo)
        {
            _context.Todos.Add(todo);
            _context.SaveChanges();
        }

        public void Update(Todo todo)
        {
            _context.Todos.Update(todo);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var todo = _context.Todos.Find(id);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                _context.SaveChanges();
            }
        }
    }
}
