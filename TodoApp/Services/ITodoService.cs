using TodoApp.Models;

namespace TodoApp.Services
{
    public interface ITodoService
    {
        List<Todo> GetAll();
        List<Todo> GetByCategoryId(int categoryId);
        Todo? GetById(int id);
        void Add(Todo todo);
        void Update(Todo todo);
        void Delete(int id);
    }
}
