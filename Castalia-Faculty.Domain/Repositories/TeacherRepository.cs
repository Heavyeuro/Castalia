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
    class TeacherRepository : IRepository<Teacher>
        {

            private FacultyContext db;

            public TeacherRepository(FacultyContext context)
            {
                this.db = context;
            }

            public IEnumerable<Teacher> GetAll()
            {
                return db.Teachers;
            }

            public Teacher Get(int id)
            {
                return db.Teachers.Find(id);
            }

            public void Create(Teacher teacher)
            {
                db.Teachers.Add(teacher);
            }

            public void Update(Teacher teacher)
            {
                db.Entry(teacher).State = EntityState.Modified;
            }

            public IEnumerable<Teacher> Find(Func<Teacher, Boolean> predicate)
            {
                return db.Teachers.Where(predicate).ToList();
            }

            public void Delete(int id)
            {
                Teacher teacher = db.Teachers.Find(id);
                if (teacher != null)
                    db.Teachers.Remove(teacher);
            }
    }
}

