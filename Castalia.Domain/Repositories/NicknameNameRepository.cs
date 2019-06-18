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
    class NicknameNameRepository : IRepository<NicknameName>
    {
        private FacultyContext db;

        public NicknameNameRepository(FacultyContext context)
        {
            this.db = context;
        }

        /// <summary>
        /// Returns list of NicknameNames with joined topic and teacher tables
        /// </summary>
        public IEnumerable<NicknameName> GetAll()
        {
            return db.Nicknames.Include(c=>c.Learner)
                .Include(c => c.Teacher).ToList();//ToList() decline deferred execution
        }

        public NicknameName Get(int id)
        {
            return db.Nicknames.Find(id);
        }
        public NicknameName GetName(string NickName)
        {
            return db.Nicknames.Where(x=>x.UserName==NickName).First();
        }

        /// <summary>
        /// Creating new record
        /// </summary>
        /// <param name="NicknameName"></param>
        public void Create(NicknameName NicknameName)
        {
            db.Nicknames.Add(NicknameName);
            db.SaveChanges();
        }

        /// <summary>
        /// Creates new record if NicknameName don`t exist and updates data in table otherwise.
        /// </summary>
        /// <param name="NicknameName"></param>
        public void Update(NicknameName NicknameName)
        {
            if (NicknameName.Id == 0)
                db.Nicknames.Add(NicknameName);
            else
            {
                NicknameName dbEntry = db.Nicknames.Include(c => c.Learner)
                .Include(c => c.Teacher).Where(x => x.Id == NicknameName.Id).FirstOrDefault(); ;
                //if (dbEntry != null)
                //{
                //    dbEntry. = NicknameName.NNicknameNames;
                //    dbEntry.DurationDays = NicknameName.DurationDays;
                //    dbEntry.StartDate = NicknameName.StartDate;
                //    if (dbEntry.Topic.TopicName != NicknameName.Topic.TopicName)
                //        dbEntry.Topic = NicknameName.Topic;
                //}
            }
            db.SaveChanges();
        }

        public IEnumerable<NicknameName> Find(Func<NicknameName, Boolean> predicate)
        {
            return db.Nicknames.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            NicknameName NicknameName = db.Nicknames.Find(id);
            if (NicknameName != null)
            {
                if (NicknameName != null)
                    db.Nicknames.Remove(NicknameName);
                db.SaveChanges();
            }
        }
    }
}
