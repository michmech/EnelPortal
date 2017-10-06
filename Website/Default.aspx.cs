using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Xsl;

namespace Website
{
	public partial class _Default : System.Web.UI.Page
	{
		protected string uilang="en";
		protected Metadata metadata;
		protected string pageMode="home"; //home|catalog|permalink, later: home|catalogHome|catalogListing|permalink

		protected string objLang="x";
		protected string metaLang="x";
		protected string dicType="x";
		protected string txt="";

		public List<Hierarchy> hierarchies=new List<Hierarchy>();
		public List<XmlDocument> dictionaries=new List<XmlDocument>();
		public int dictionariesCount=0;

		public List<string> objLangs=new List<string>();
		public List<string> searchableObjLangs=new List<string>();
		public List<string> metaLangs=new List<string>();

		protected string permalinkTitle="";

		protected void Page_Load(object sender, EventArgs e)
		{
			this.pageMode=(string)Context.Items["pageMode"];
			this.uilang=(string)Context.Items["uilang"];
			this.metadata=new Metadata(this.uilang, Server);

			if(Request.QueryString["objLang"]!=null) this.objLang=Request.QueryString["objLang"];
			if(Request.QueryString["dicType"]!=null) this.dicType=Request.QueryString["dicType"];
			if(Request.QueryString["metaLang"]!=null) this.metaLang=Request.QueryString["metaLang"];
			if(Request.QueryString["txt"]!=null) this.txt=Request.QueryString["txt"];

			if(pageMode=="catalog") {
				if(this.objLang=="x" && this.metaLang=="x" && this.dicType=="x") {
					this.pageMode="catalogHome";
				} else {
					this.pageMode="catalogListing";
				}
			}

			SqlConnection conn=new SqlConnection(Connfigger.GetConnectionString());
			conn.Open();

			SqlCommand command; SqlParameter param;
			command=new SqlCommand("catalog_list", conn);
			command.CommandType=CommandType.StoredProcedure;

			param=new SqlParameter();
			param.ParameterName="@pageMode";
			param.SqlDbType=SqlDbType.NVarChar;
			param.Value=this.pageMode;
			param.Direction=ParameterDirection.Input;
			command.Parameters.Add(param);

			param=new SqlParameter();
			param.ParameterName="@objLang";
			param.SqlDbType=SqlDbType.NVarChar;
			param.Value=this.objLang;
			param.Direction=ParameterDirection.Input;
			command.Parameters.Add(param);

			param=new SqlParameter();
			param.ParameterName="@dicType";
			param.SqlDbType=SqlDbType.NVarChar;
			param.Value=this.dicType;
			param.Direction=ParameterDirection.Input;
			command.Parameters.Add(param);

			param=new SqlParameter();
			param.ParameterName="@metaLang";
			param.SqlDbType=SqlDbType.NVarChar;
			param.Value=this.metaLang;
			param.Direction=ParameterDirection.Input;
			command.Parameters.Add(param);

			if(this.pageMode=="permalink") {
				param=new SqlParameter();
				param.ParameterName="@dictID";
				param.SqlDbType=SqlDbType.Int;
				param.Value=(int)Context.Items["dictID"];
				param.Direction=ParameterDirection.Input;
				command.Parameters.Add(param);
			}

			SqlDataReader reader=command.ExecuteReader();
			while(reader.Read()) { //read objLangs:
				Hierarchy h=new Hierarchy();
				h.objLang=(string)reader["ObjLang"];
				h.count=(int)reader["DictCount"];
				this.hierarchies.Add(h);
				this.objLangs.Add(h.objLang);
			}
			reader.NextResult();
			while(reader.Read()) { //read searchable objLangs:
				string code=(string)reader["SearchableObjLang"];
				this.searchableObjLangs.Add(code);
			}
			reader.NextResult();
			while(reader.Read()) { //read metaLangs:
				string code=(string)reader["MetaLang"];
				this.metaLangs.Add(code);
			}
			reader.NextResult();
			while(reader.Read()) { //read objLangs-dicType combos:
				string objLang=(string)reader["ObjLang"];
				foreach(Hierarchy h in this.hierarchies) {
					if(h.objLang==objLang) {
						h.dicTypes.Add((string)reader["DicType"], (int)reader["DictCount"]);
						break;
					}
				}
			}
			reader.NextResult();
			while(reader.Read()) { //read objLangs-metaLang combos:
				string objLang=(string)reader["ObjLang"];
				foreach(Hierarchy h in this.hierarchies) {
					if(h.objLang==objLang) {
						h.metaLangs.Add((string)reader["MetaLang"], (int)reader["DictCount"]);
						break;
					}
				}
			}
			reader.NextResult();
			List<int> ids=new List<int>();
			while(reader.Read()) { //Read catalog items:
				XmlDocument doc=new XmlDocument(); doc.LoadXml((string)reader["Xml"]);
				doc.DocumentElement.SetAttribute("id", ((int)reader["ID"]).ToString());
				if(reader["ParentID"]!=DBNull.Value) doc.DocumentElement.SetAttribute("parentID", ((int)reader["ParentID"]).ToString());
				this.dictionaries.Add(doc);
				ids.Add((int)reader["ID"]);
			}
			reader.Close();
			conn.Close();

			this.dictionariesCount = this.dictionaries.Count;
			//if(this.pageMode == "catalogListing") {
			//	//Remove dictionaries whose parent is also here:
			//	this.dictionariesCount = this.dictionaries.Count;
			//	List<XmlDocument> temp = new List<XmlDocument>();
			//	foreach(XmlDocument xmlDic in this.dictionaries) {
			//		int parentID = int.Parse(this.getXmlValue(xmlDic, "/dictionary/@parentID", "0"));
			//		if(!ids.Contains(parentID)) temp.Add(xmlDic);
			//	}
			//	this.dictionaries = temp;
			//}

			if(this.pageMode=="permalink" && this.dictionaries.Count==0) {
				Response.Redirect("/"+this.uilang+"/");
			}
			if(this.pageMode=="permalink" && this.dictionaries.Count>0) {
				this.permalinkTitle=this.getInnerXml(this.dictionaries[0], "/dictionary/title", "");
				this.permalinkTitle=System.Text.RegularExpressions.Regex.Replace(this.permalinkTitle, @"\</[^\>]+\>", ")");
				this.permalinkTitle=System.Text.RegularExpressions.Regex.Replace(this.permalinkTitle, @"\<[^\>]+\>", "(");
			}
		}

		protected string getQueryString()
		{
			string ret="";
			if(Request.QueryString.Count>0) ret+="?"+Request.QueryString;
			return ret;
		}
		protected string getQueryString(string objLang, string metaLang, string dicType)
		{
			string ret="";
			if(this.txt!="") { if(ret!="") ret+="&"; ret+="txt="+Server.UrlEncode(this.txt); }
			if(objLang!="x") { if(ret!="") ret+="&"; ret+="objLang="+objLang; }
			if(metaLang!="x") { if(ret!="") ret+="&"; ret+="metaLang="+metaLang; }
			if(dicType!="x") { if(ret!="") ret+="&"; ret+="dicType="+dicType; }
			if(ret!="") ret="?"+ret;
			return ret;
		}
		protected string getInnerXml(XmlDocument doc, string xpath, string ifNull)
		{
			string ret=ifNull;
			if(doc.SelectSingleNode(xpath)!=null) {
				ret=doc.SelectSingleNode(xpath).InnerXml;
				ret=ret.Replace(" xml:space='preserve'", "");
				ret=ret.Replace(" xml:space=\"preserve\"", "");
			}
			return ret;
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

		protected string printDictionary(XmlDocument doc)
		{
			return printDictionary(doc, true);
		}
		protected string printDictionary(XmlDocument doc, bool isTopLevel)
		{
			string ret="";

			string id=this.getXmlValue(doc, "/dictionary/@id", "0");
			string url=this.getXmlValue(doc, "/dictionary/homepage/text()", "");
			string title=this.getInnerXml(doc, "/dictionary/title", "");
			string titleLang=this.getXmlValue(doc, "/dictionary/title/@lang", "");
			string year=this.getXmlValue(doc, "/dictionary/year/text()", "");
			string style=""; if(!isTopLevel) style="display: none";
			ret+="<div class='dictionary' style='"+style+"' data-sortkey='"+Server.UrlEncode(title)+"'>";
			if(Session["email"]!=null && (this.pageMode=="catalogListing" || this.pageMode=="permalink")) {
				ret+="<a class='cms editLink' href='javascript:openDicEditor("+id+")'><span class='dot'></span></a>";
			}
			if(isTopLevel) {
				ret+="<a class='screenshot' href='"+url+"' target='_blank'><img src='http://img.bitpixels.com/getthumbnail?code=78981&size=120&url="+Server.UrlEncode(url)+"' width='120' height='90'/></a>";
				ret+="<a class='stamp' href='/"+uilang+"/stamp/'><img src='/stamp-tiny.gif' width='120' height='20'/></a>";
			}
			ret+="<div class='permalink'><a href='/"+uilang+"/"+id+"/'>DICTIONARYPORTAL.EU/"+id+"</a></div>";
			ret+="<div class='titleContainer'><a class='title' target='_blank' href='"+url+"'>";
			ret+=title.Replace("<abbrev>", "<span class='abbrev'>(").Replace("</abbrev>", ")</span>");
			if(year!="") ret+=" <span class='year'>"+Server.HtmlEncode(year)+"</span>";
			ret+="</a></div> ";

			List<string> langs=new List<string>();
			foreach(XmlElement el in doc.SelectNodes("/dictionary/metaLang")) {
				string lang=el.GetAttribute("code");
				if(!langs.Contains(lang)) langs.Add(lang);
			}
			if(!langs.Contains(uilang)) langs.Add(uilang);
			foreach(string lang in langs) {
				string subtitle=this.getInnerXml(doc, "/dictionary/title[@lang='"+lang+"']", "");
				if(subtitle==title) subtitle="";
				if(subtitle!="") {
					ret+="<span class='subtitle'>";
					ret+=subtitle.Replace("<abbrev>", "<span class='abbrev'>(").Replace("</abbrev>", ")</span>");
					ret+="</span> ";
				}
			}

			if(this.getXmlValue(doc, "/dictionary/@loginRequired", "0")=="1") {
				ret+="<div class='loginRequired'>"+L("loginRequired")+"</div>";
			}

			bool canSearch=false; if(doc.SelectSingleNode("/dictionary/search")!=null) canSearch=true;
			if(canSearch) {
				ret+="<form class='searchform' target='_blank' method='get' action='/go/'>";
				ret+="<div class='inside'>";
				ret+="<input type='hidden' name='uilang' value='"+this.uilang+"'/>";
				ret+="<input type='hidden' name='id' value='"+doc.DocumentElement.GetAttribute("id")+"'/>";
				ret+="<div class='textboxContainer'><input class='textbox directSearch' name='txt' onchange='synchroSearch(this)' onkeyup='synchroSearch(this)' value='"+Server.HtmlEncode(this.txt)+"'/></div>";
				ret+="<input class='button' type='submit' value=''/>";
				ret+="</div>";
				if(doc.SelectNodes("/dictionary/search").Count>1) {
					string objLang=this.getXmlValue(doc, "/dictionary/objLang/@code", "");
					ret+="<div class='dirselect'>";
					ret+=L("searchIn");
					int count=0;
					foreach(XmlElement el in doc.SelectNodes("/dictionary/search")) {
						count++;
						string lang=el.GetAttribute("lang");
						string chk=""; if(lang==objLang) chk="checked='checked'";
						ret+="<label>";
						ret+="<input type='radio' name='index' value='"+count+"' "+chk+"/>";
						ret+=this.metadata.getLanguage(lang).name;
						ret+="</label>";
					}
					ret+="</div>";
				}
				ret+="</form>";
			}

			string s="";
			ret+="<span class='data'>";
			s=""; foreach(XmlElement el in doc.SelectNodes("/dictionary/objLang")) { if(s!="") s+=", "; s+=this.metadata.getLanguage(el.GetAttribute("code")).name; }
			if(s!="") { ret+="<span class='datum'><span class='label'>"+L("objLang")+"</span> <span class='value'>"+s+"</span></span> "; }
			s=""; foreach(XmlElement el in doc.SelectNodes("/dictionary/metaLang")) { if(s!="") s+=", "; s+=this.metadata.getLanguage(el.GetAttribute("code")).name; }
			if(s!="") { ret+="<span class='datum'><span class='label'>"+L("metaLang")+"</span> <span class='value'>"+s+"</span></span> "; }
			s=""; foreach(XmlElement el in doc.SelectNodes("/dictionary/dicType")) { if(s!="") s+=", "; s+=this.metadata.getDicType(el.GetAttribute("code")).name; }
			if(s!="") { ret+="<span class='datum'><span class='label'>"+L("dicType")+"</span> <span class='value'>"+s+"</span></span> "; }
			ret+="</span>";

			if(url!="") ret+="<span class='url'><a target='_blank' href='"+url+"'>"+url+"</a></span>";
			if(this.getXmlValue(doc, "/dictionary/@tcRequired", "0")=="1") {
				ret+="<div class='tcRequired'>"+L("tsRequired")+"</div>";
			}

			if(Session["email"]!=null && this.pageMode=="catalogListing" && doc.SelectNodes("/dictionary/history/historyItem").Count>0) {
				ret+="<div class='cms admininfo'>";
				ret+="<table>";
				foreach(XmlElement el in doc.SelectNodes("/dictionary/history/historyItem")) {
					string action=el.GetAttribute("action")+"d";
					string when=el.GetAttribute("when").Substring(0, 10);
					string email=el.GetAttribute("email");
					ret+="<tr><td><span class='dot small'></span></td><td>"+action+"</td><td>"+when+"</td><td>"+email+"</td></tr>";
				}
				ret+="</table>";
				ret+="</div>";
			}

			int subCount=doc.SelectNodes("/dictionary/dictionary").Count;
			if(subCount>0 && (this.pageMode=="catalogListing" || this.pageMode=="permalink")) {
				ret+="<div class='subdictionaries'>";
				ret+="<div class='revealerContainer'><a class='revealer collapsed' href='javascript:void(null)' onclick='toggleSubdictionaries(this)'>"+L("included")+" ("+subCount+")</a></div>";
				foreach(XmlElement el in doc.SelectNodes("/dictionary/dictionary")) {
					XmlDocument subdoc=new XmlDocument();
					subdoc.LoadXml(el.OuterXml);
					ret+=printDictionary(subdoc, false);
				}
				ret+="</div>";
			}

			ret+="<span class='clear'></span>";
			ret+="</div>";
			return ret;
		}

		protected bool hierarchyHasLang(string langCode)
		{
			bool ret=false;
			foreach(Hierarchy h in this.hierarchies) {
				if(h.objLang==langCode) {
					ret=true;
					break;
				}
			}
			return ret;
		}

		protected string L(string code) {
			return this.metadata.getString(code);
		}
	}

	public class Hierarchy
	{
		public string objLang="";
		public int count=0;
		public Dictionary<string, int> dicTypes=new Dictionary<string, int>(); //eg. <"gen", 4>
		public Dictionary<string, int> metaLangs=new Dictionary<string, int>(); //eg. <"de", 2>
	}
}
