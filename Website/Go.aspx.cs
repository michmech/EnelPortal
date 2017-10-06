using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Xsl;

namespace Website
{
	public partial class Go : System.Web.UI.Page
	{
		protected string uilang="en";
		protected Metadata metadata=null;

		protected string txt="";
		protected int id=0;
		protected int index=1;
		protected XmlDocument dic=new XmlDocument();

		protected string url="";
		protected Dictionary<string, string> postFields=new Dictionary<string, string>();

		protected void Page_Load(object sender, EventArgs e)
		{
			if(Request.QueryString["uilang"]!=null) this.uilang=Request.QueryString["uilang"];
			if(Request.QueryString["txt"]!=null) this.txt=Request.QueryString["txt"];
			if(Request.QueryString["id"]!=null) this.id=int.Parse(Request.QueryString["id"]);
			if(Request.QueryString["index"]!=null) this.index=int.Parse(Request.QueryString["index"]);

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

			this.url=getXmlValue(this.dic, "/dictionary/homepage/text()", "");
			if(this.dic.SelectSingleNode("/dictionary/search["+index+"]")!=null) {
				XmlElement url=(XmlElement)this.dic.SelectSingleNode("/dictionary/search["+index+"]/searchUrl");
				XmlElement el=(XmlElement)url.SelectSingleNode("word");
				if(el!=null) {
					XmlText nd=this.dic.CreateTextNode(Server.UrlEncode(this.txt));
					el.ParentNode.ReplaceChild(nd, el);
				}
				this.url=url.InnerText;
				foreach(XmlElement pf in this.dic.SelectNodes("/dictionary/search["+index+"]/postField")) {
					string name=pf.GetAttribute("name");
					string value=pf.GetAttribute("value");
					if(!this.postFields.ContainsKey(name)) this.postFields.Add(name, value);
				}
			}
			if(this.postFields.Count==0) { //this will be a GET request
				Response.Redirect(url);
			} else { //this will be a POST request
				this.metadata=new Metadata(this.uilang, Server);

			}
		}

		protected string getXmlValue(XmlDocument doc, string xpath, string ifNull)
		{
			string ret=ifNull;
			if(doc.SelectSingleNode(xpath)!=null) ret=doc.SelectSingleNode(xpath).Value;
			return ret;
		}
		protected bool hasXmlValue(XmlDocument doc, string xpath)
		{
			return (doc.SelectSingleNode(xpath)!=null);
		}
		protected string L(string code) {
			return this.metadata.getString(code);
		}
	}

}
