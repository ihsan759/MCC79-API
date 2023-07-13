using API.DTOs.Employees;
using Client.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IEmployeeRepository repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            var result = await repository.Get();
            var ListEmployee = new List<GetEmployeeDto>();

            if (result.Data != null)
            {
                ListEmployee = result.Data.ToList();
            }
            return View(ListEmployee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GetEmployeeDto newEmploye)
        {

            var result = await repository.Post(newEmploye);
            if (result.Status == "200")
            {
                TempData["Success"] = "Data berhasil masuk";
                return RedirectToAction(nameof(Index));
            }
            else if (result.Status == "409")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid guid)
        {
            var result = await repository.Delete(guid);
            if (result.Status == "200")
            {
                TempData["Success"] = "Data berhasil dihapus";
                return RedirectToAction(nameof(Index));
            }
            else if (result.Status == "404")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(GetEmployeeDto newEmploye)
        {

            var result = await repository.Put(newEmploye);
            if (result.Status == "200")
            {
                TempData["Success"] = "Data berhasil diupdate";
                return RedirectToAction(nameof(Index));
            }
            else if (result.Status == "409")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid guid)
        {
            var result = await repository.Get(guid);
            var Employee = new GetEmployeeDto();

            if (result.Data != null)
            {
                Employee = result.Data;
            }
            return View(Employee);
        }

        public IActionResult Chart()
        {
            return View();
        }
    }
}