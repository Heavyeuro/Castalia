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
    class CourseRepository : IRepository<Course>
    {
        private FacultyContext db;

        public CourseRepository(FacultyContext context)
        {
            this.db = context;
        }
        /// <summary>
        /// Returns list of courses with joined topic and teacher tables
        /// </summary>
        public IEnumerable<Course> GetAll()
        {
            return db.Courses.Include(c=>c.Topic)
                .Include(c=>c.Teacher).ToList();//ToList() decline deferred execution
        }

        public Course Get(int id)
        {
            return db.Courses.Find(id);
        }
        /// <summary>
        /// Creating new record
        /// </summary>
        /// <param name="course"></param>
        public void Create(Course course)
        {
            db.Courses.Add(course);
            db.SaveChanges();
        }
        /// <summary>
        /// Creates new record if course don`t exist and updates data in table otherwise.
        /// </summary>
        /// <param name="course"></param>
        public void Update(Course course)
        {
            if (course.Id == 0)
                db.Courses.Add(course);
            else
            {
                Course dbEntry = db.Courses.Include(c => c.Topic)
                .Include(c => c.Teacher).Where(x => x.Id == course.Id).FirstOrDefault(); ;
                if (dbEntry != null)
                {
                    dbEntry.CourseName = course.CourseName;
                    dbEntry.DurationDays = course.DurationDays;
                    dbEntry.StartDate = course.StartDate;
                    if (dbEntry.Topic.TopicName != course.Topic.TopicName)
                        dbEntry.Topic= course.Topic;
                }
            }
            db.SaveChanges();
        }

        public IEnumerable<Course> Find(Func<Course, Boolean> predicate)
        {
            return db.Courses.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Course course = db.Courses.Find(id);
            if (course != null)
            {
                if (course != null)
                db.Courses.Remove(course);
                db.SaveChanges();
            }
        }
    }
}
