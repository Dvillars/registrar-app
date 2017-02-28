
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

        public void Dispose()
        {
            Student.DeleteAll();
        }
    }
}
