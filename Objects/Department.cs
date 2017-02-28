using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RegistrarApp.Objects
{
    public class Department
    {
        private int _id;
        private string _name;

        public Department(string name, int id = 0)
        {
            _id = id;
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }

        public override bool Equals(System.Object otherDepartment)
        {
            if (!(otherDepartment is Department))
            {
                return false;
            }
            else
            {
                Department newDepartment = (Department) otherDepartment;
                bool idEquality = this.GetId() == newDepartment.GetId();
                bool nameEquality = this.GetName() == newDepartment.GetName();

                return (idEquality && nameEquality);
            }
        }

        public static Department Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM departments WHERE id = @DepartmentId;", conn);
            SqlParameter departmentIdParameter = new SqlParameter();
            departmentIdParameter.ParameterName = "@DepartmentId";
            departmentIdParameter.Value = id.ToString();
            cmd.Parameters.Add(departmentIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundDepartmentId = 0;
            string foundDepartmentName = null;

            while (rdr.Read())
            {
                foundDepartmentId = rdr.GetInt32(0);
                foundDepartmentName = rdr.GetString(1);
            }
            Department foundDepartment = new Department(foundDepartmentName, foundDepartmentId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundDepartment;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO departments (name) OUTPUT INSERTED.id VALUES (@Name);", conn);

            SqlParameter nameParameter = new SqlParameter("@Name", this.GetName());

            cmd.Parameters.Add(nameParameter);

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

        public static List<Department> GetAll()
        {
            List<Department> DepartmentList = new List<Department> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM departments;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                Department newDepartment = new Department(name, id);
                DepartmentList.Add(newDepartment);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return DepartmentList;
        }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM departments;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

}
