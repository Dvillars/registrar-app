
using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using RegistrarApp.Objects;

namespace RegistrarApp
{
    public class CourseTest : IDisposable
    {
        public CourseTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void TEST_Save_CheckCourseSaveToDb()
        {
            Course tempCourse = new Course("math", "20");
            List<Course> allCourses = new List<Course>{tempCourse};
            tempCourse.Save();

            Assert.Equal(allCourses, Course.GetAll());
        }

        [Fact]
        public void TEST_AddStudent_AddStudentToJoinTable()
        {
            Course tempCourse = new Course("Math", "30");
            tempCourse.Save();

            Student tempStudent = new Student("Melvin", "2010-12,30");
            tempStudent.Save();

            List<Student> allStudents = new List<Student>{tempStudent};
            tempCourse.AddStudent(tempStudent);
            Assert.Equal(allStudents, tempCourse.GetStudents());
        }

        public void Dispose()
        {
            Course.DeleteAll();
            Student.DeleteAll();
        }
    }
}
