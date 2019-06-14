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
    public class TopicRepository : IRepository<Topic>
    {
            private FacultyContext db;

            public TopicRepository(FacultyContext context)
            {
                this.db = context;
            }

            public IEnumerable<Topic> GetAll()
            {
                return db.Topics;
            }

            public Topic Get(int id)
            {
                return db.Topics.Find(id);
            }

            public void Create(Topic topic)
            {
                db.Topics.Add(topic);
            }

            public void Update(Topic topic)
            {
                db.Entry(topic).State = EntityState.Modified;
            }

            public IEnumerable<Topic> Find(Func<Topic, Boolean> predicate)
            {
                return db.Topics.Where(predicate).ToList();
            }

            public void Delete(int id)
            {
                Topic topic = db.Topics.Find(id);
                if (topic != null)
                    db.Topics.Remove(topic);
            }
        }
    }

