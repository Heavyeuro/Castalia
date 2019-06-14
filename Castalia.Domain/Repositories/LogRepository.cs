using Castalia.Domain.EF;
using Castalia.Domain.Entities;
using Castalia.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castalia.Domain.Repositories
{
    /// <summary>
    /// Allows to separate access to database directly from busies logic
    /// </summary>
    class LogRepository : IRepository<Log>
    {

        private FacultyContext db;

        public LogRepository(FacultyContext context)
        {
            this.db = context;
        }

        /// <summary>
        /// Returns list of logs with joined learner and course tables
        /// </summary>
        public IEnumerable<Log> GetAll()
        {
            return db.Log.Include(c=>c.Lerner).Include(c=>c.Course).ToList();//ToList() decline deferred execution
        }

        public Log Get(int id)
        {
            return db.Log.Find(id);
        }

        public void Create(Log log)
        {
            db.Log.Add(log);
        }

        public void Update(Log log)
        {
            db.Entry(log).State = EntityState.Modified;
        }

        public IEnumerable<Log> Find(Func<Log, Boolean> predicate)
        {
            return db.Log.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Log log = db.Log.Find(id);
            if (log != null)
                db.Log.Remove(log);
        }
    }
}
