using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace Website
{
	public partial class AllXml : System.Web.UI.Page
	{
		public List<XmlDocument> dictionaries=new List<XmlDocument>();
		
		protected void Page_Load(object sender, EventArgs e)
		{
			SqlConnection conn=new SqlConnection(Connfigger.GetConnectionString());
			conn.Open();

			SqlCommand command; SqlParameter param;
			command=new SqlCommand("catalog_all", conn);
			command.CommandType=CommandType.StoredProcedure;

			SqlDataReader reader=command.ExecuteReader();
			List<int> ids=new List<int>();
			while(reader.Read()) { //Read catalog items:
				XmlDocument doc=new XmlDocument(); doc.LoadXml((string)reader["Xml"]);
				doc.DocumentElement.SetAttribute("id", ((int)reader["ID"]).ToString());
				if(reader["ParentID"]!=DBNull.Value) doc.DocumentElement.SetAttribute("parentID", ((int)reader["ParentID"]).ToString());
				this.dictionaries.Add(doc);
				ids.Add((int)reader["ID"]);

				//remove history:
				XmlNodeList histNodes=doc.SelectNodes("//history");
				foreach(XmlNode n in histNodes) n.ParentNode.RemoveChild(n);
			}
			reader.Close();
			conn.Close();

			//Remove dictionaries whose parent is also here:
			List<XmlDocument> temp=new List<XmlDocument>();
			foreach(XmlDocument xmlDic in this.dictionaries) {
				int parentID=int.Parse(this.getXmlValue(xmlDic, "/dictionary/@parentID", "0"));
				if(!ids.Contains(parentID)) temp.Add(xmlDic);
			}
			this.dictionaries=temp;

			Response.ContentType="text/xml";
			//Response.AppendHeader("Content-Disposition", "attachment; filename=catalog.xml");
			Response.Write("<dictionaries>\r\n\r\n");
			Response.Write("<!--Exported from the European Dictionary Portal on "+DateTime.Now.ToString("yyyy-MM-dd")+"\r\n");
			Response.Write("Available under Open Database Licence, http://opendatacommons.org/licenses/odbl/summary/\r\n");
			Response.Write("If you use this, do not forget to attribute the European Dictionary Portal, http://www.dictionaryportal.eu/-->\r\n\r\n");
			foreach(XmlDocument xmlDic in this.dictionaries) {
				Response.Write(PrettyPrintXml(xmlDic.DocumentElement.OuterXml)+"\r\n\r\n");
			}
			Response.Write("</dictionaries>\r\n");
			Response.End();

		}
		public static string PrettyPrintXml(string doc)
		{
			XDocument xdoc=XDocument.Load(new StringReader(doc));
			return xdoc.ToString(SaveOptions.None);
		}
		protected string getXmlValue(XmlDocument doc, string xpath, string ifNull)
		{
			string ret=ifNull;
			if(doc.SelectSingleNode(xpath)!=null) ret=doc.SelectSingleNode(xpath).Value;
			return ret;
		}
	}

}
