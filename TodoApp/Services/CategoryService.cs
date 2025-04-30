using TodoApp.Models;
using TodoApp.Data;

namespace TodoApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public List<Category> GetAll() => _context.Categories.ToList();

        public Category? GetById(int id) => _context.Categories.Find(id);

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category is not null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}
