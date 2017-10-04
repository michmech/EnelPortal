using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace Website
{
	public partial class DicSave : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if(Session["email"]==null) Response.Redirect("NoSession.aspx");

			XmlDocument dic=new XmlDocument(); dic.LoadXml((string)Request.Form["xml"]);
			int parentID=0; int.TryParse(dic.DocumentElement.GetAttribute("parentID"), out parentID);

			SqlConnection conn=new SqlConnection(Connfigger.GetConnectionString());
			conn.Open();

			SqlCommand command; SqlParameter param;
			command=new SqlCommand("catalog_item_save", conn);
			command.CommandType=CommandType.StoredProcedure;

			param=new SqlParameter();
			param.ParameterName="@xml";
			param.SqlDbType=SqlDbType.Xml;
			param.Value=dic.DocumentElement.OuterXml;
			param.Direction=ParameterDirection.Input;
			command.Parameters.Add(param);

			if(parentID>0) {
				param=new SqlParameter();
				param.ParameterName="@parentID";
				param.SqlDbType=SqlDbType.Int;
				param.Value=parentID;
				param.Direction=ParameterDirection.Input;
				command.Parameters.Add(param);
			}

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
