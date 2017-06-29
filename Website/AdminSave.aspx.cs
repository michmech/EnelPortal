using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace Website
{
	public partial class AdminSave : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if(Session["email"]==null) Response.Redirect("NoSession.aspx");
			if(!(bool)Session["IsAdmin"]) Response.Redirect("NoSession.aspx");

			XmlDocument doc=new XmlDocument(); doc.LoadXml((string)Request.Form["xml"]);

			SqlConnection conn=new SqlConnection(Connfigger.GetConnectionString());
			conn.Open();

			SqlCommand command; SqlParameter param;
			command=new SqlCommand("admin_save", conn);
			command.CommandType=CommandType.StoredProcedure;

			param=new SqlParameter();
			param.ParameterName="@xml";
			param.SqlDbType=SqlDbType.Xml;
			param.Value=doc.DocumentElement.OuterXml;
			param.Direction=ParameterDirection.Input;
			command.Parameters.Add(param);

			param=new SqlParameter();
			param.ParameterName="@currentUser";
			param.SqlDbType=SqlDbType.NVarChar;
			param.Value=(string)Session["email"];
			param.Direction=ParameterDirection.Input;
			command.Parameters.Add(param);

			command.ExecuteNonQuery();
			conn.Close();
		}
	}
}
