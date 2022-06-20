#nullable enable
using System.Threading.Tasks;
using BugTracker.Models.Domain.Vacancy;

namespace BugTracker.Services.VacancyService
{
    public interface IVacancyService
    {
        public Task<Vacancy?> GetVacancies();
    }
}