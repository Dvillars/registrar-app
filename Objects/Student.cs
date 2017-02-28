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

            SqlParameter studentIdParameter = new SqlParameter("@StudentId", this.GetId());
            SqlParameter courseIdParameter = new SqlParameter("@CoursesId", newCourse.GetId());

            cmd.Parameters.Add(studentIdParameter);
            cmd.Parameters.Add(courseIdParameter);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
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
