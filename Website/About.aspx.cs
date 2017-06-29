using System;
using System.Text.RegularExpressions;
using System.IO;

namespace Website
{
	public partial class About : System.Web.UI.Page
	{
		protected string uilang="en";
		protected Metadata metadata;
		protected string pageMode="about"; //about
		protected AboutPage aboutPage=new AboutPage("", "");
		protected string html="";
		protected bool noloc=false;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.pageMode=(string)Context.Items["pageMode"];
			this.uilang=(string)Context.Items["uilang"];
			this.metadata=new Metadata(this.uilang, Server);
			string pageModeBare=this.pageMode.Split('-')[0];
			foreach(AboutPage ap in this.metadata.aboutPages) {
				if(ap.code==pageModeBare) {
					this.aboutPage=ap;
					break;
				}
			}

			this.html="";
			if(this.pageMode=="info") {
				string para=L("about1");
				para=Regex.Replace(para, @"\[\[([^\[\]]+)\]\]", "<a href='/"+uilang+"/crit/'>$1</a>");
				para=Regex.Replace(para, @"\[([^\[\]]+)\]", "<a href='http://www.elexicography.eu/' target='_blank'>$1</a>");
				html+="<p>"+para+"</p>";
				string para1=L("prop1");
				para1=Regex.Replace(para1, @"\[([^\[\]]+)\]", "<a href='/"+uilang+"/prop/'>$1</a>");
				string para2=L("prop2");
				para2=Regex.Replace(para2, @"\[([^\[\]]+)\]", "<a href='/"+uilang+"/stmp/'>$1</a>");
				html+="<p>"+para1+" "+para2+"</p>";
			} else if(pageMode=="prop") {
				string para=L("prop2");
				para=Regex.Replace(para, @"\[([^\[\]]+)\]", "<a href='/"+uilang+"/stmp/'>$1</a>");
				html+="<p>"+L("prop1").Replace("[", "").Replace("]", "")+" "+para+"</p>";
			} else if(pageMode=="prop-ok") {
				this.html+="<h2>"+L("thanks")+"</h2>";
				string para=L("prop2");
				para=Regex.Replace(para, @"\[([^\[\]]+)\]", "<a href='/"+uilang+"/stmp/'>$1</a>");
				html+="<p>"+para+"</p>";
				html+="<p><a class='button' href='/"+uilang+"/'>"+L("backhome")+"&nbsp;»</a></p>";
			} else if(pageMode=="prop-ko") {
				this.html+="<h2>"+L("error1")+"</h2>";
				string para=L("error2");
				para=Regex.Replace(para, @"\[([^\[\]]+)\]", "<a href='mailto:valselob@gmail.com'>$1</a>");
				html+="<p>"+para+"</p>";
			} else {
				string path=Server.MapPath(@"/markdown/"+this.aboutPage.code+"."+this.uilang+".mkd");
				if(!File.Exists(path)) {
					path=Server.MapPath(@"/markdown/"+this.aboutPage.code+".en.mkd");
					this.noloc=true;
				}
				StreamReader reader=new StreamReader(path);
				var s=reader.ReadToEnd();
				reader.Close();
				Markdown markdown=new Markdown();
				this.html=markdown.Transform(s);
				this.html=this.html.Replace("<a ", "<a target='_blank' ");
				this.html=this.html.Replace("<a target='_blank' href=\"/", "<a href=\"/");
			}
		}

		protected string L(string code) {
			return this.metadata.getString(code);
		}
	}
}
