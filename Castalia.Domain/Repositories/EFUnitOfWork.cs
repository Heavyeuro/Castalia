using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castalia.Domain.EF;
using Castalia.Domain.Entities;
using Castalia.Domain.Interfaces;

namespace Castalia.Domain.Repositories
{
    /// <summary>
    /// Represents sequence of repositorie as a full entity to work with database
    /// </summary>
    public class EFUnitOfWork : IUnitOfWork
    {
        private FacultyContext db;
        private TopicRepository topicRepository;
        private TeacherRepository teacherRepository;
        private LogRepository logRepository;
        private LearnerRepository learnerRepository;
        private CourseRepository courseRepository;
        private NicknameNameRepository nicknameNameRepository;

        public EFUnitOfWork(string connectionString)
        {
            db = new FacultyContext(connectionString);

        }

        public IRepository<NicknameName> NickName
        {
            get
            {
                if (nicknameNameRepository == null)
                    nicknameNameRepository = new NicknameNameRepository(db);
                return nicknameNameRepository;
            }
        }

        public IRepository<Topic> Topics
        {
            get
            {
                if (topicRepository == null)
                    topicRepository = new TopicRepository(db);
                return topicRepository;
            }
        }

        public IRepository<Teacher> Teachers
        {
            get
            {
                if (teacherRepository == null)
                    teacherRepository = new TeacherRepository(db);
                return teacherRepository;
            }
        }

        public IRepository<Log> Logs
        {
            get
            {
                if (logRepository == null)
                    logRepository = new LogRepository(db);
                return logRepository;
            }
        }

        public IRepository<Learner> Learners
        {
            get
            {
                if (learnerRepository == null)
                    learnerRepository = new LearnerRepository(db);
                return learnerRepository;
            }
        }

        public IRepository<Course> Courses
        {
            get
            {
                if (courseRepository == null)
                    courseRepository = new CourseRepository(db);
                return courseRepository;
            }
        }


        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
