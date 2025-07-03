using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCConsumer.DTO;
using MVCConsumer.Models;
using Newtonsoft.Json;
using System.Text;

namespace MVCConsumer.Controllers
{
    public class CourseController : Controller
    {

        private readonly HttpClient _httpClient;

        public CourseController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5223/");
        }
        public async Task<IActionResult> Index()
        {
            var response = _httpClient.GetAsync("api/course");
            if (response.Result.IsSuccessStatusCode)
            {
                var data = await response.Result.Content.ReadAsStringAsync();
                var course = JsonConvert.DeserializeObject<List<CourseDTO>>(data);

                foreach (var item in course)
                {
                    var instructor = await GetInstructorById(item.InstID);
                    if (instructor != null)
                    {
                        item.InstName = instructor.Name;
                    }

                }
                return View(course);
            }
            return View(new List<CourseDTO>());
        }
        public async Task<IActionResult> Create()
        {
            var instructors = await GetInstructorsList();
            ViewBag.Instructors = instructors;
            return View(new CourseDTO());
        }

        private async Task<List<Instructor>> GetInstructorsList()
        {
            var response = await _httpClient.GetAsync("api/Instructor");
            var json = await response.Content.ReadAsStringAsync();
            var instructors = JsonConvert.DeserializeObject<List<Instructor>>(json);

            return instructors;
        }

        private async Task<Instructor> GetInstructorById(int id)
        {
            var response = await _httpClient.GetAsync("api/Instructor");
            var json = await response.Content.ReadAsStringAsync();
            var instructors = JsonConvert.DeserializeObject<List<Instructor>>(json);

            return instructors.FirstOrDefault(i=>i.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseDTO dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Course", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Instructors = await GetInstructorsList();
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/Course/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var course = JsonConvert.DeserializeObject<CourseDTO>(json);

            ViewBag.Instructors = await GetInstructorsList();
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CourseDTO dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Course/{id}", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Instructors = await GetInstructorsList();
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Course/{id}");
            return RedirectToAction("Index");
        }
    }
}
