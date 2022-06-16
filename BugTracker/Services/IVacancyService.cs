#nullable enable
using System.Threading.Tasks;
using BugTracker.Models.Domain.Vacancy;

namespace BugTracker.Services
{
    public interface IVacancyService
    {
        public Task<Vacancy?> GetVacancies();
    }
}