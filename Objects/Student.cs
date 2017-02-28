using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RegistrarApp.Objects
{
    public class Student
    {
        private int _id;
        private string _name;
        private string _regDate;
        private int _department_id;

        public Student(string name, string regDate, int id = 0)
        {
            _id = id;
            _name = name;
            _regDate = regDate;
        }

        public string GetName()
        {
            return _name;
        }

        public string GetRegDate()
        {
            return _regDate;
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

        public override bool Equals(System.Object otherStudent)
        {
            if (!(otherStudent is Student))
            {
                return false;
            }
            else
            {
                Student newStudent = (Student) otherStudent;
                bool idEquality = this.GetId() == newStudent.GetId();
                bool nameEquality = this.GetName() == newStudent.GetName();
                bool regDateEquality = this.GetRegDate() == newStudent.GetRegDate();

                return (idEquality && nameEquality && regDateEquality);
            }
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO students (name, registration_date) OUTPUT INSERTED.id VALUES (@Name, @RegDate);", conn);

            SqlParameter nameParameter = new SqlParameter("@Name", this.GetName());
            SqlParameter regDateParameter = new SqlParameter("@RegDate", this.GetRegDate());

            cmd.Parameters.Add(nameParameter);
            cmd.Parameters.Add(regDateParameter);

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

        public static List<Student> GetAll()
        {
            List<Student> StudentList = new List<Student> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                string regDate = rdr.GetString(2);
                Student newStudent = new Student(name, regDate, id);
                StudentList.Add(newStudent);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return StudentList;
        }

        public void AddCourse(Course newCourse)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (student_id, course_id) VALUES(@StudentsId, @CoursesId);",conn);

            SqlParameter studentIdParameter = new SqlParameter("@StudentsId", this.GetId());
            SqlParameter courseIdParameter = new SqlParameter("@CoursesId", newCourse.GetId());

            cmd.Parameters.Add(studentIdParameter);
            cmd.Parameters.Add(courseIdParameter);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<Course> GetCourses()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT courses.* FROM students JOIN students_courses ON (students.id = students_courses.student_id) JOIN courses ON (students_courses.course_id = courses.id) WHERE students.id = @CourseId;", conn);

            SqlParameter CourseId = new SqlParameter("@CourseId", this.GetId().ToString());


            cmd.Parameters.Add(CourseId);
            SqlDataReader rdr = cmd.ExecuteReader();

            List<Course> courseList = new List<Course>{};

            while(rdr.Read())
            {
                int courseId = rdr.GetInt32(0);
                string courseName = rdr.GetString(1);
                string courseNumber = rdr.GetString(2);

                Course tempCourse = new Course(courseName, courseNumber, courseId);
                courseList.Add(tempCourse);
            }

            if (rdr != null)
            {
              rdr.Close();
            }
            if (conn != null)
            {
              conn.Close();
            }
            return courseList;
        }

        public void AddDepartment(int deptid)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE students SET department_id = @DepartmentId WHERE id = @ThisStudent;", conn);

            SqlParameter department_id = new SqlParameter("@DepartmentId", deptid);
            SqlParameter student_id = new SqlParameter("@ThisStudent", this.GetId());

            cmd.Parameters.Add(department_id);
            cmd.Parameters.Add(student_id);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<Student> GetStudentsInDept()
        {
            List<Student> allStudents = new List<Student>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE department_id = @DeptId;", conn);
            SqlParameter department_id = new SqlParameter("@DeptId", this.GetDeptId());
            cmd.Parameters.Add(department_id);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                string date = rdr.GetString(2);
                int deptId = rdr.GetInt32(3);

                Student tempStudent = new Student(name, date, id);
                tempStudent.SetDeptId(deptId);

                allStudents.Add(tempStudent);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }

            return allStudents;
        }


        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

}
