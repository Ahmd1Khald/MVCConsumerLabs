using Microsoft.AspNetCore.Mvc;
using MVCConsumer.Models;
using Newtonsoft.Json;
using System.Text;

namespace MVCConsumer.Controllers
{
    public class InstructorController : Controller
    {
        private readonly HttpClient _httpClient;

        public InstructorController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5223/");
        }

        public async Task<IActionResult> Index()
        {
            var response = _httpClient.GetAsync("api/instructor");
            if (response.Result.IsSuccessStatusCode)
            {
                var data = await response.Result.Content.ReadAsStringAsync()  ;
                var instructors = JsonConvert.DeserializeObject<List<Instructor>>(data);
                return View(instructors);
            }
            return View(new List<Instructor>());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Instructor instructor)
        {
            var json = JsonConvert.SerializeObject(instructor);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Instructor", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(instructor);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/Instructor/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var instructor = JsonConvert.DeserializeObject<Instructor>(json);
            return View(instructor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Instructor instructor)
        {
            var json = JsonConvert.SerializeObject(instructor);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Instructor/{id}", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(instructor);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Instructor/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
