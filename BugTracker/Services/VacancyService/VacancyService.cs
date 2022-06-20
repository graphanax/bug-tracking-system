#nullable enable
using System.Net.Http;
using System.Threading.Tasks;
using BugTracker.Models.Domain.Vacancy;
using Newtonsoft.Json;

namespace BugTracker.Services.VacancyService
{
    public class VacancyService : IVacancyService
    {
        private readonly HttpClient _httpClient;

        public VacancyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Vacancy?> GetVacancies()
        {
            var responseString = await _httpClient.GetStringAsync(_httpClient.BaseAddress);

            var vacancies = JsonConvert.DeserializeObject<Vacancy>(responseString);
            return vacancies;
        }
    }
}