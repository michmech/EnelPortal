using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace Website
{
	public partial class DicEditor : System.Web.UI.Page
	{
		protected int id=0;
		protected XmlDocument dic=new XmlDocument();

		protected void Page_Load(object sender, EventArgs e)
		{
			if(Session["email"]==null) Response.Redirect("NoSession.aspx");
			
			if(Request.QueryString["id"]!=null) this.id=int.Parse(Request.QueryString["id"]);
			if(this.id==0) {
				string objLang=""; if(Request.QueryString["objLang"]!=null) objLang=Request.QueryString["objLang"];
				this.dic.LoadXml("<dictionary prominence='5' loginRequired='0' tcRequired='0'><objLang code='"+objLang+"'/><dicType code=''/><metaLang code='"+objLang+"'/><title lang='"+objLang+"'/><homepage/></dictionary>");
			} else {
				SqlConnection conn=new SqlConnection(Connfigger.GetConnectionString());
				conn.Open();

				SqlCommand command; SqlParameter param;
				command=new SqlCommand("catalog_item_get", conn);
				command.CommandType=CommandType.StoredProcedure;

				param=new SqlParameter();
				param.ParameterName="@id";
				param.SqlDbType=SqlDbType.Int;
				param.Value=this.id;
				param.Direction=ParameterDirection.Input;
				command.Parameters.Add(param);

				SqlDataReader reader=command.ExecuteReader();
				while(reader.Read()) {
					this.dic.LoadXml((string)reader["Xml"]);
				}
				reader.Close();
				conn.Close();
			}
		}
	}
}
