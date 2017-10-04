using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace Website
{
	public partial class DicDelete : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if(Session["email"]==null) Response.Redirect("NoSession.aspx");
			
			int id=int.Parse(Request.Form["id"]);

			SqlConnection conn=new SqlConnection(Connfigger.GetConnectionString());
			conn.Open();

			SqlCommand command; SqlParameter param;
			command=new SqlCommand("catalog_item_delete", conn);
			command.CommandType=CommandType.StoredProcedure;

			param=new SqlParameter();
			param.ParameterName="@id";
			param.SqlDbType=SqlDbType.Int;
			param.Value=id;
			param.Direction=ParameterDirection.Input;
			command.Parameters.Add(param);

			param=new SqlParameter();
			param.ParameterName="@email";
			param.SqlDbType=SqlDbType.NVarChar;
			param.Value=(string)Session["email"];
			param.Direction=ParameterDirection.Input;
			command.Parameters.Add(param);

			command.ExecuteNonQuery();
			conn.Close();
		}
	}
}
