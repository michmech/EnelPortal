using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace Website
{
	public partial class Login : System.Web.UI.Page
	{
		protected string email="";
		protected string password="";
		protected string stage="start";
		
		protected void Page_Load(object sender, EventArgs e)
		{
			if(Request.Form.Count>0) {
				if(Request.Form["email"]!="") this.email=Request.Form["email"];
				if(Request.Form["password"]!="") this.password=Request.Form["password"];

				SqlConnection conn=new SqlConnection(Connfigger.GetConnectionString());
				conn.Open();

				SqlCommand command; SqlParameter param;
				command=new SqlCommand("user_login", conn);
				command.CommandType=CommandType.StoredProcedure;

				param=new SqlParameter();
				param.ParameterName="@email";
				param.SqlDbType=SqlDbType.NVarChar;
				param.Value=this.email;
				param.Direction=ParameterDirection.Input;
				command.Parameters.Add(param);

				param=new SqlParameter();
				param.ParameterName="@password";
				param.SqlDbType=SqlDbType.NVarChar;
				param.Value=this.password;
				param.Direction=ParameterDirection.Input;
				command.Parameters.Add(param);

				SqlDataReader reader=command.ExecuteReader();
				if(reader.Read()) {
					this.stage="success";
					Session.Add("email", (string)reader["Email"]);
					Session.Add("isAdmin", (bool)reader["IsAdmin"]);
				} else {
					this.stage="error";
				}

				reader.Close();
				conn.Close();
			}
		}
	}
}
