
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

        public void Dispose()
        {
            Course.DeleteAll();
        }
    }
}
