using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RegistrarApp.Objects
{
    public class Student
    {
        private int _id;
        private int _student_id;
        private int _course_id;
        private int _department_id;
        private bool _pass;
        private bool _complete;

        public Student(bool pass, bool complete, int student_id = 0, int course_id = 0, int department_id = 0, int id = 0,)
        {
            _id = id;
            _student_id = student_id;
            _course_id = course_id;
            _department_id = department_id;
            _pass = pass;
            _complete = complete;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetStudentId()
        {
            return _student_id;
        }

        public int GetCourseId()
        {
            return _course_id;
        }

        public int GetDeptId()
        {
            return _department_id;
        }

        public bool GetPass()
        {
            return _pass;
        }

        public bool GetComp()
        {
            return _complete
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

            SqlCommand cmd = new SqlCommand("INSERT INTO pass_fail (student_id, course_id, department_id, pass, complete) OUTPUT INSERTED.id VALUES (@StudentId, @CourseId, @DeptId, @Pass, @Comp);", conn);

            SqlParameter studentIdParameter = new SqlParameter("@StudentId", this.GetStudentId());
            SqlParameter courseIdParameter = new SqlParameter("@CourseId", this.GetCourseId());
            SqlParameter departmentIdParameter = new SqlParameter("@DeptId", this.GetDeptId());
            SqlParameter passParameter = new SqlParameter("@Pass", this.GetPass());
            SqlParameter completeParameter = new SqlParameter("@Comp", this.GetComp());

            cmd.Parameters.Add(studentIdParameter);
            cmd.Parameters.Add(courseIdParameter);
            cmd.Parameters.Add(departmentIdParameter);
            cmd.Parameters.Add(passParameter);
            cmd.Parameters.Add(completeParameter);

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

            SqlCommand cmd = new SqlCommand("SELECT * FROM pass_fail;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                int student_id = rdr.GetInt32(1);
                int course_id = rdr.GetInt32(2);
                int department_id = rdr.GetInt32(3);
                bool pass = rdr.Get(4);
                bool complete = rdr.Get(5);
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
