
using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using RegistrarApp.Objects;

namespace RegistrarApp
{
    public class DepartmentTest : IDisposable
    {
        public DepartmentTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void TEST_SaveAddsDepartmentToDb()
        {
            Department tempDepartment = new Department("math");
            List<Department> allDepartments = new List<Department>{tempDepartment};
            tempDepartment.Save();

            Assert.Equal(allDepartments, Department.GetAll());
        }

        [Fact]
        public void TEST_FindReturnsDept()
        {
            Department tempDepartment = new Department("math");
            tempDepartment.Save();

            Assert.Equal(tempDepartment, Department.Find(tempDepartment.GetId()));
        }

        [Fact]
        public void TEST_AddDeptToStudent()
        {
            Department tempDepartment = new Department("math");
            tempDepartment.Save();
            int deptId = Department.GetAll()[0].GetId();

            Student tempStudent = new Student("Name", "1993-05-11");
            tempStudent.SetDeptId(deptId);
            tempStudent.Save();

            tempStudent.AddDepartment(deptId);
            List<Student> backendList = new List<Student>{tempStudent};
            Assert.Equal(backendList, tempStudent.GetStudentsInDept());
        }

        [Fact]
        public void TEST_AddDeptToCourse()
        {
            Department tempDepartment = new Department("math");
            tempDepartment.Save();
            int deptId = Department.GetAll()[0].GetId();

            Course tempCourse = new Course("Name", "1993-05-11");
            tempCourse.SetDeptId(deptId);
            tempCourse.Save();

            tempCourse.AddDepartment(deptId);
            List<Course> backendList = new List<Course>{tempCourse};
            Assert.Equal(backendList, tempCourse.GetCoursesInDept());
        }

        public void Dispose()
        {
            Department.DeleteAll();
            Student.DeleteAll();
        }
    }
}
