using Castalia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Castalia.Domain.EF
{

    public class FacultyContext: DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Learner> Learners { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<NicknameName> Nicknames { get; set; }

        static FacultyContext()
        {
            Database.SetInitializer<FacultyContext>(new FacultyDbInitializer());
        }

        public FacultyContext(string connectionString)
           : base(connectionString)
        {
        }
    }

    /// <summary>
    /// That class sets initial data to database
    /// </summary>
    public class FacultyDbInitializer : CreateDatabaseIfNotExists<FacultyContext>
    {
        protected override void Seed(FacultyContext db)
        {
            Topic topic1 = new Topic { TopicName = "Math" };
            Topic topic2 = new Topic { TopicName = "Programming" };
            Topic topic3 = new Topic { TopicName = "Natural" };

            Teacher teacher2 = new Teacher { TeacherName = "Nikolev Oleksei" };
            Teacher teacher1 = new Teacher { TeacherName = "Brisina Irina" };
            Teacher teacher3 = new Teacher { TeacherName = "Kartashov Oleksei" };

            List<Learner> learners = new List<Learner>();
                learners.Add(new Learner { LearnerName = "Ivanov Ivan", IsBlocked = false });
                learners.Add(new Learner { LearnerName = "Serheiev Serhei", IsBlocked = false });
                learners.Add(new Learner { LearnerName = "Nikolaiev Nikolay", IsBlocked = false });
                learners.Add(new Learner { LearnerName = "Antonov Anton", IsBlocked = false });


            List<Course> courses = new List<Course>();
            courses.Add( new Course { CourseName = "linear algebra",AmountOfStudents=3, DurationDays = 150, StartDate = new DateTime(2015, 7, 20),Topic=topic1,Teacher=teacher2});
            courses.Add(new Course { CourseName = "OOP and algoriths",AmountOfStudents=3, DurationDays = 170, StartDate = new DateTime(2015, 7, 20),Teacher=teacher1,Topic=topic2 });
            courses.Add(new Course { CourseName = "Chemistry", AmountOfStudents = 0, DurationDays = 10, StartDate = new DateTime(2015, 7, 20), Teacher = teacher3, Topic = topic3 });
            courses.Add(new Course { CourseName = "Biology", AmountOfStudents = 0, DurationDays = 30, StartDate = new DateTime(2015, 7, 20), Teacher = teacher3, Topic = topic3 });
            courses.Add(new Course { CourseName = "С#", AmountOfStudents = 0, DurationDays = 20, StartDate = new DateTime(2015, 7, 20), Teacher = teacher1, Topic = topic2 });

            List<Log> logs = new List<Log>();
            logs.Add(new Log { Course = courses[0], Lerner = learners[0], Mark = null, RegisterDate = new DateTime(2019, 7, 20) });
            logs.Add(new Log { Course = courses[0], Lerner = learners[1], Mark = null, RegisterDate = new DateTime(2019, 7, 20) });
            logs.Add(new Log { Course = courses[0], Lerner = learners[3], Mark = null, RegisterDate = new DateTime(2019, 7, 20) });
            logs.Add(new Log { Course = courses[1], Lerner = learners[0], Mark = 100, RegisterDate = new DateTime(2019, 7, 20) });
            logs.Add(new Log { Course = courses[1], Lerner = learners[1], Mark = 50, RegisterDate = new DateTime(2019, 7, 20) });
            logs.Add(new Log { Course = courses[1], Lerner = learners[2], Mark = 80, RegisterDate = new DateTime(2019, 7, 20) });


            db.Nicknames.Add(new NicknameName() { Learner=learners[0],UserName= "user@mail.ru" });
            db.Nicknames.Add(new NicknameName() { Teacher=teacher1, UserName = "teacher@mail.ru" });

            db.Topics.Add(topic1);
            db.Topics.Add(topic2);
            db.Topics.Add(topic3);

            db.Teachers.Add(teacher2);
            db.Teachers.Add(teacher1);
            db.Teachers.Add(teacher3);

            foreach(var learner in learners)
            db.Learners.Add(learner);

            foreach(var course in courses)
            db.Courses.Add(course);
            

            foreach(var log in logs)
            db.Log.Add(log);
            

            db.SaveChanges();
        }
    }
}
