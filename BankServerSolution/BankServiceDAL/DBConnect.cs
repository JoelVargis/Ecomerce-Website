using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace BankServiceDAL
{
    class DBConnect
    {
        private readonly string constr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        SqlConnection con;
        SqlCommand cmd;
        public DBConnect()
        {
            con = new SqlConnection(constr);

        }


        public int fun_executenonquery(string ins, SqlParameter[] parameters = null)
        {


            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            using (cmd = new SqlCommand(ins, con))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                con.Open();

                return cmd.ExecuteNonQuery();

            }



        }


        public string fun_executescalar(string ins, SqlParameter[] parameters = null)
        {


            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            using (cmd = new SqlCommand(ins, con))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);

                }
                con.Open();


                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    return "null";
                }
                else
                {
                    return result.ToString();
                }

            }

        }

        public SqlDataReader fun_executereader(string ins, SqlParameter[] parameters = null)
        {


            if (con.State == ConnectionState.Open)
            {
                con.Close();

            }


            using (cmd = new SqlCommand(ins, con))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                con.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }


        }

        public DataSet fun_dataadapterset(string ins, SqlParameter[] parameters = null)
        {


            if (con.State == ConnectionState.Open)
            {
                con.Close();

            }

            using (cmd = new SqlCommand(ins, con))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }



        }
        public DataTable fun_dataadaptertable(string ins, SqlParameter[] parameters = null)
        {


            if (con.State == ConnectionState.Open)
            {
                con.Close();

            }

            using (cmd = new SqlCommand(ins, con))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }



        }

    }
}

