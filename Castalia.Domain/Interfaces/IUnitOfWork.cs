using Castalia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castalia.Domain.Interfaces
{
    /// <summary>
    /// Exposes repositories for existing entities
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Topic> Topics { get; }
        IRepository<Teacher> Teachers { get; }
        IRepository<Log> Logs { get; }
        IRepository<Learner> Learners { get; }
        IRepository<Course> Courses { get; }
        IRepository<NicknameName> NickName { get; }

        void Save();
    }
}
