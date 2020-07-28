using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Assignments1
{
    class Program2
    {
        public static void initializeData() // only run this once at first
        {
            using (SchoolContext dbContext = new SchoolContext())
            {

                dbContext.Database.ExecuteSqlCommand("Truncate table Grades");
                dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE CourseStudents");
                dbContext.Database.ExecuteSqlCommand("delete from Teachers");
                dbContext.Database.ExecuteSqlCommand("delete from  Courses");
                dbContext.Database.ExecuteSqlCommand("delete from  Students");

                List<StudentPoco> listStudent = new List<StudentPoco>()
                {
                    new StudentPoco(0, "Ray Kim")
                    , new StudentPoco(0, "Yubin Kim")
                    , new StudentPoco(0, "Yoonhue Kim")
                    , new StudentPoco(0, "Sumin Kim")
                    , new StudentPoco(0, "John Doe")
                };
                dbContext.Students.AddRange(listStudent);
                List<TeacherPoco> listTeacher = new List<TeacherPoco>()
                {
                    new TeacherPoco(0, "John Hinz")
                    , new TeacherPoco(0, "Donald Trump")
                    , new TeacherPoco(0, "Justin trudeau")
                };
                dbContext.Teachers.AddRange(listTeacher);
                dbContext.SaveChanges();

                List<CoursePoco> listCourse = new List<CoursePoco>
                 {
                     new CoursePoco(0, ".Net", ".Net Core", listTeacher[0], listStudent)
                     , new CoursePoco(0, "Advanced C#", "Entity Framework", listTeacher[1], listStudent)
                 };
                dbContext.Courses.AddRange(listCourse);
                dbContext.SaveChanges();

                IEnumerable<IEnumerable<GradesPoco>> listGrade =
                    listCourse.Select(course => course.Students.Select(item => new GradesPoco(100, item.Course, item.Student)));

                foreach (var list in listGrade)
                {
                    dbContext.Grades.AddRange(list);
                }
                dbContext.SaveChanges();
            }
        }
        static void Main(string[] args)
        {
            initializeData(); // only run this once at first. 

            var students = new StudentRepository().AllListWithAll();
            foreach (var student in students)
            {
                Console.WriteLine($"{student.Id}, {student.Name}");
                foreach (var item in student.Courses)
                {
                    Console.WriteLine($"\t{item.Course?.Id}, {item.Course?.Name}");
                }
                Console.WriteLine($"\tGrade Info &&&&&&&&&&&");
                foreach (var item in student.Grades)
                {
                    Console.WriteLine($"\t{item.CourseId}, {item.Course?.Name}, {item.Score}");
                }
            }

            Console.WriteLine($"Course Info %%%%%%%%%%%%%%%%%");
            var courses = new CourseRepository().AllListWithAll();
            foreach (var course in courses)
            {
                Console.WriteLine($"{course.Id}, {course.Name}, {course.Teacher?.Name}, {course.averageGrade}");
                foreach (var item in course.Students)
                {
                    Console.WriteLine($"\t{item.Student.Id}, {item.Student.Name}");
                }
                Console.WriteLine($"\tGrade Info &&&&&&&&&&&");
                foreach (var item in course.Grades)
                {
                    Console.WriteLine($"\t{item.Student.Id}, {item.Student.Name}, {item.Score}");
                }
            }

            Console.WriteLine($"Teacher Info %%%%%%%%%%%%%%%%%");
            var teachers = new TeacherRepository().AllListWithAll();
            foreach (var teacher in teachers)
            {
                Console.WriteLine($"{teacher.Id}, {teacher.Name}");
                foreach (var item in teacher.Courses)
                {
                    Console.WriteLine($"\t{item.Id}, {item.Name}");
                }
            }
        }
    }

    public class StudentRepository
    {
        public void Add(IEnumerable<StudentPoco> list)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                dbContext.Students.AddRange(list);
                dbContext.SaveChanges();
            }
        }
        public void Update(IEnumerable<StudentPoco> list)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                dbContext.Students.UpdateRange(list);
                dbContext.SaveChanges();
            }
        }
        public void Remove(IEnumerable<StudentPoco> list)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                dbContext.Students.RemoveRange(list);
                dbContext.SaveChanges();
            }
        }
        public IEnumerable<StudentPoco> AllList()
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                return dbContext.Students.ToList();
            }
        }
        public IEnumerable<StudentPoco> AllListWithAll()
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                return dbContext.Students.Include(s => s.Courses).ThenInclude(c => c.Course).Include(s => s.Grades).ToList();
            }
        }
        public IEnumerable<StudentPoco> GetList(Expression<Func<StudentPoco, bool>> where)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                return dbContext.Students.Include(s => s.Courses).ThenInclude(c => c.Course).Include(s => s.Grades).Where(where).ToList();
            }
        }
        public StudentPoco GetOne(Expression<Func<StudentPoco, bool>> where)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                return dbContext.Students.Include(s => s.Courses).ThenInclude(c => c.Course).Include(s => s.Grades).Where(where).SingleOrDefault();
            }
        }
    }
    public class CourseRepository
    {
        public void Add(IEnumerable<CoursePoco> list)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                dbContext.Courses.AddRange(list);
                dbContext.SaveChanges();
            }
        }
        public void Update(IEnumerable<CoursePoco> list)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                dbContext.Courses.UpdateRange(list);
                dbContext.SaveChanges();
            }
        }
        public void Remove(IEnumerable<CoursePoco> list)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                dbContext.Courses.RemoveRange(list);
                dbContext.SaveChanges();
            }
        }
        public IEnumerable<CoursePoco> AllList()
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                return dbContext.Courses.ToList();
            }
        }
        public IEnumerable<CoursePoco> AllListWithAll()
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                return dbContext.Courses.Include(s => s.Students).ThenInclude(c => c.Student).Include(s => s.Grades).Include(s => s.Teacher).ToList();
            }
        }
        public IEnumerable<CoursePoco> GetList(Expression<Func<CoursePoco, bool>> where)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                return dbContext.Courses.Include(s => s.Students).ThenInclude(c => c.Student).Include(s => s.Grades).Include(s => s.Teacher).Where(where).ToList();
            }
        }
        public CoursePoco GetOne(Expression<Func<CoursePoco, bool>> where)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                return dbContext.Courses.Include(s => s.Students).ThenInclude(c => c.Student).Include(s => s.Grades).Include(s => s.Teacher).Where(where).SingleOrDefault();
            }
        }
    }
    public class TeacherRepository
    {
        public void Add(IEnumerable<TeacherPoco> list)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                dbContext.Teachers.AddRange(list);
                dbContext.SaveChanges();
            }
        }
        public void Update(IEnumerable<TeacherPoco> list)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                dbContext.Teachers.UpdateRange(list);
                dbContext.SaveChanges();
            }
        }
        public void Remove(IEnumerable<TeacherPoco> list)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                dbContext.Teachers.RemoveRange(list);
                dbContext.SaveChanges();
            }
        }
        public IEnumerable<TeacherPoco> AllList()
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                return dbContext.Teachers.ToList();
            }
        }
        public IEnumerable<TeacherPoco> AllListWithAll()
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                return dbContext.Teachers.Include(s => s.Courses).ToList();
            }
        }
        public IEnumerable<TeacherPoco> GetList(Expression<Func<TeacherPoco, bool>> where)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                return dbContext.Teachers.Include(s => s.Courses).Where(where).ToList();
            }
        }
        public TeacherPoco GetOne(Expression<Func<TeacherPoco, bool>> where)
        {
            using (SchoolContext dbContext = new SchoolContext())
            {
                return dbContext.Teachers.Include(s => s.Courses).Where(where).SingleOrDefault();
            }
        }
    }
    public class SchoolContext : DbContext
    {
        public DbSet<StudentPoco> Students { get; set; }
        public DbSet<CoursePoco> Courses { get; set; }
        public DbSet<TeacherPoco> Teachers { get; set; }
        public DbSet<GradesPoco> Grades { get; set; }

        protected override void
            OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-NVEGO8O\HUMBERBRIDGING;Initial Catalog=TEST_DB;Integrated Security=True");
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GradesPoco>().HasKey(entity =>
                  new { entity.CourseId, entity.StudentId });

            modelBuilder.Entity<CourseStudentPoco>().HasKey(entity => new { entity.StudentId, entity.CourseId });

            base.OnModelCreating(modelBuilder);
        }
    }
    public class BasePoco
    {

    }
    public class StudentPoco : BasePoco
    {
        public StudentPoco()
        {
            Courses = new HashSet<CourseStudentPoco>();
            Grades = new HashSet<GradesPoco>();
        }
        public StudentPoco(int id, string name) : this()
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CourseStudentPoco> Courses { get; set; }
        public ICollection<GradesPoco> Grades { get; set; }
    }

    public class CoursePoco : BasePoco
    {
        public CoursePoco()
        {
            Students = new HashSet<CourseStudentPoco>();
            Grades = new HashSet<GradesPoco>();
        }
        public CoursePoco(int id, string name, string subject, TeacherPoco teacher, List<StudentPoco> students)
        {
            Id = id;
            Name = name;
            Subject = subject;
            Teacher = teacher;
            Students = students?.Select(c => new CourseStudentPoco(id, c.Id)).ToList();
            Grades = new HashSet<GradesPoco>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        [Required]
        public TeacherPoco Teacher { get; set; }

        public ICollection<CourseStudentPoco> Students { get; set; }
        public ICollection<GradesPoco> Grades { get; set; }
        [NotMapped]
        public double averageGrade
        {
            get
            {
                return Grades.Average(g => g.Score);
            }
        }
    }
    [Table("CourseStudents")]
    public class CourseStudentPoco : BasePoco
    {
        public CourseStudentPoco()
        {

        }
        public CourseStudentPoco(int courseId, int studentId)
        {
            CourseId = courseId;
            StudentId = studentId;
        }
        public int CourseId { get; set; }
        public CoursePoco Course { get; set; }
        public int StudentId { get; set; }
        public StudentPoco Student { get; set; }
    }
    public class TeacherPoco : BasePoco
    {
        public TeacherPoco() { }
        public TeacherPoco(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CoursePoco> Courses { get; set; }
    }

    public class GradesPoco : BasePoco
    {
        public GradesPoco() { }
        public GradesPoco(int score, CoursePoco course, StudentPoco student)
        {
            Score = score;
            Course = course;
            Student = student;
        }
        [Range(0, 100)]
        public int Score { get; set; }

        public int CourseId { get; set; }
        public int StudentId { get; set; }

        public CoursePoco Course { get; set; }
        public StudentPoco Student { get; set; }
    }


}

