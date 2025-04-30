using Microsoft.AspNetCore.Mvc;
using TodoApp.Services;
using TodoApp.Models;


namespace TodoApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoService _todoService;
        private readonly ICategoryService? _categoryService;

        public TodoController(ITodoService todoService, ICategoryService? categoryService = null)
        {
            _todoService = todoService;
            _categoryService = categoryService;
        }


        public IActionResult Index()
        {
            var todos = _todoService.GetAll();
            var categories = _categoryService != null ? _categoryService.GetAll() : new List<Category>();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = null; // No category selected by default

            return View(todos);
        }


        [HttpPost]
        public IActionResult Add(string Title, string Description, int CategoryId)
        {
            var newTodo = new Todo
            {
                Title = Title,
                Description = Description,
                IsCompleted = false,
                CategoryId = CategoryId
            };

            _todoService.Add(newTodo);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Complete(int id)
        {
            var todo = _todoService.GetById(id);
            if (todo == null)
                return NotFound();

            todo.IsCompleted = true;
            _todoService.Update(todo);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _todoService.Delete(id);
            return RedirectToAction("Index");
        }


        [HttpGet("Filter")]
        public IActionResult FilterByCategory(int categoryId)
        {
            var filteredTodos = _todoService.GetByCategoryId(categoryId);
            var categories = _categoryService != null ? _categoryService.GetAll() : new List<Category>();

            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = categoryId;

            return View("Index", filteredTodos);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var todos = _todoService.GetAll();
            var categories = _categoryService != null ? _categoryService.GetAll() : new List<Category>();
            var todoToEdit = _todoService.GetById(id);

            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = todoToEdit?.CategoryId;
            ViewBag.TodoToEdit = todoToEdit;

            return View("Index", todos);
        }

        [HttpPost]
        public IActionResult Update(int id, string Title, string Description, int CategoryId)
        {
            var todo = _todoService.GetById(id);
            if (todo is null) return NotFound();

            todo.Title = Title;
            todo.Description = Description;
            todo.CategoryId = CategoryId;

            _todoService.Update(todo);
            return RedirectToAction("Index");
        }



    }
}
