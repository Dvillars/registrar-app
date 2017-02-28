
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

            SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (student_id, courses_id) VALUES (@StudentId, @CoursesId);", conn);

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
