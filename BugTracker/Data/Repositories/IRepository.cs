using System;
using System.Collections.Generic;

namespace BugTracker.Models
{
    public interface IRepository<T>
        where T : class
    {
        IEnumerable<T> GetAllObjects();
        T GetObjectById(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
}