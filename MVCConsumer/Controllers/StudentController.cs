using Microsoft.AspNetCore.Mvc;
using MVCConsumer.Models;
using Newtonsoft.Json;
using System.Text;

namespace MVCConsumer.Controllers
{
    public class StudentController : Controller
    {
        private readonly HttpClient _httpClient;

        public StudentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5223/");
        }


        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/student");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var students = JsonConvert.DeserializeObject<List<Student>>(data);
                return View(students);
            }
            return View(new List<Student>());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            var json = JsonConvert.SerializeObject(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/student", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(student);
        }
    }
}
