using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace Website
{
	public partial class AdminEditor : System.Web.UI.Page
	{
		protected int id=0;
		protected XmlDocument doc=new XmlDocument();

		protected void Page_Load(object sender, EventArgs e)
		{
			if(Session["email"]==null) Response.Redirect("NoSession.aspx");
			if(!(bool)Session["IsAdmin"]) Response.Redirect("NoSession.aspx");
			
			SqlConnection conn=new SqlConnection(Connfigger.GetConnectionString());
			conn.Open();

			SqlCommand command; SqlParameter param;
			command=new SqlCommand("admin_get", conn);
			command.CommandType=CommandType.StoredProcedure;

			param=new SqlParameter();
			param.ParameterName="@currentUser";
			param.SqlDbType=SqlDbType.NVarChar;
			param.Value=(string)Session["email"];
			param.Direction=ParameterDirection.Input;
			command.Parameters.Add(param);

			SqlDataReader reader=command.ExecuteReader();
			while(reader.Read()) {
				this.doc.LoadXml((string)reader["Xml"]);
			}
			reader.Close();
			conn.Close();
		}
	}
}
