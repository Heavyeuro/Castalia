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
    class LearnerRepository : IRepository<Learner>
    {

        private FacultyContext db;

        public LearnerRepository(FacultyContext context)
        {
            this.db = context;
        }

        public IEnumerable<Learner> GetAll()
        {
            return db.Learners.ToList();
        }

        public Learner Get(int id)
        {
            return db.Learners.Find(id);
        }

        public void Create(Learner learner)
        {
            db.Learners.Add(learner);
        }

        public void Update(Learner learner)
        {
            db.Entry(learner).State = EntityState.Modified;
        }

        public IEnumerable<Learner> Find(Func<Learner, Boolean> predicate)
        {
            return db.Learners.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Learner learner = db.Learners.Find(id);
            if (learner != null)
                db.Learners.Remove(learner);
        }
    }
}
