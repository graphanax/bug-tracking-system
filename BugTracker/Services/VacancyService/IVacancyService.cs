#nullable enable
using System.Threading.Tasks;
using BugTracker.Models.Vacancy;

namespace BugTracker.Services.VacancyService
{
    public interface IVacancyService
    {
        public Task<Vacancy?> GetVacancies();
    }
}