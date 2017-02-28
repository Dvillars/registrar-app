
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RegistrarApp.Objects
{
    public class Course
    {
        private int  _id;
        private string _name;
        private string _number;
        private int _department_id;

        public Course(string newName, string newNumber, int id = 0)
        {
            _name = newName;
            _number = newNumber;
            _id = id;
        }

        public string GetName()
        {
            return _name;
        }

        public string GetNumber()
        {
            return _number;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetDeptId()
        {
            return _department_id;
        }

        public void SetDeptId(int id)
        {
            _department_id = id;
        }

        public override bool Equals(System.Object otherCourse)
        {
            if (!(otherCourse is Course))
            {
                return false;
            }
            else
            {
                Course newCourse = (Course) otherCourse;
                bool idEquality = this.GetId() == newCourse.GetId();
                bool nameEquality = this.GetName() == newCourse.GetName();
                bool numberEquality = this.GetNumber() == newCourse.GetNumber();

                return (idEquality && nameEquality && numberEquality);
            }
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO courses (name, number) OUTPUT INSERTED.id VALUES (@Name, @Number);", conn);

            SqlParameter nameParameter = new SqlParameter("@Name", this.GetName());
            SqlParameter numberParameter = new SqlParameter("@Number", this.GetNumber());

            cmd.Parameters.Add(nameParameter);
            cmd.Parameters.Add(numberParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }

        public static List<Course> GetAll()
        {
            List<Course> CourseList = new List<Course> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                string number = rdr.GetString(2);
                Course newCourse = new Course(name, number, id);
                CourseList.Add(newCourse);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return CourseList;
        }

        public void AddStudent(Student newStudent)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (student_id, course_id) VALUES (@StudentId, @CoursesId);", conn);

            SqlParameter studentIdParameter = new SqlParameter("@StudentId", newStudent.GetId());
            SqlParameter courseIdParameter = new SqlParameter("@CoursesId", this.GetId());

            cmd.Parameters.Add(studentIdParameter);
            cmd.Parameters.Add(courseIdParameter);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<Student> GetStudents()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT students.* FROM courses JOIN students_courses ON (courses.id = students_courses.course_id) JOIN students ON (students_courses.student_id = students.id) WHERE courses.id = @StudentId;", conn);

            SqlParameter StudentId = new SqlParameter("@StudentId", this.GetId().ToString());

            cmd.Parameters.Add(StudentId);
            SqlDataReader rdr = cmd.ExecuteReader();
            List<Student> studentList = new List<Student>{};

            while(rdr.Read())
            {
                int studentId = rdr.GetInt32(0);
                string studentName = rdr.GetString(1);
                string studentNumber = rdr.GetString(2);

                Student tempStudent = new Student(studentName, studentNumber, studentId);
                studentList.Add(tempStudent);
            }

            if (rdr != null)
            {
              rdr.Close();
            }
            if (conn != null)
            {
              conn.Close();
            }
            return studentList;
        }

        public void AddDepartment(int deptid)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE courses SET department_id = @DepartmentId WHERE id = @ThisCourse", conn);

            SqlParameter department_id = new SqlParameter("@DepartmentId", deptid);
            SqlParameter course_id = new SqlParameter("@ThisCourse", this.GetId());

            cmd.Parameters.Add(department_id);
            cmd.Parameters.Add(course_id);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<Course> GetCoursesInDept()
        {
            List<Course> allCourses = new List<Course>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE department_id = @DeptId;", conn);
            SqlParameter department_id = new SqlParameter("@DeptId", this.GetDeptId());
            cmd.Parameters.Add(department_id);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                string number = rdr.GetString(2);
                int deptId = rdr.GetInt32(3);

                Course tempCourse = new Course(name, number, id);
                tempCourse.SetDeptId(deptId);

                allCourses.Add(tempCourse);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }

            return allCourses;
        }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

    }
}
