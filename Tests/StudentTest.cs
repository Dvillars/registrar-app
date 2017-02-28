
using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using RegistrarApp.Objects;

namespace RegistrarApp
{
    public class StudentTest : IDisposable
    {
        public StudentTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_Save_CheckStudentSaveToDB()
        {
            List<Student> totalStudents = new List<Student>{};
            Student tempStudent = new Student("Name", "1993-05-11");
            totalStudents.Add(tempStudent);

            tempStudent.Save();
            Assert.Equal(totalStudents, Student.GetAll());
        }

        [Fact]
        public void TEST_AddStudent_AddStudentToJoinTable()
        {
            Course tempCourse = new Course("Math", "30");
            tempCourse.Save();

            Student tempStudent = new Student("Melvin", "2010-12,30");
            tempStudent.Save();

            List<Course> allCourses = new List<Course>{tempCourse};
            tempStudent.AddCourse(tempCourse);
            Assert.Equal(allCourses, tempStudent.GetCourses());
        }

        public void Dispose()
        {
            Student.DeleteAll();
            Course.DeleteAll();
        }
    }
}
