using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Website
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{

		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			//Canonize the URL:
			string domain=Request.Url.Host;
			if(domain!="localhost" && domain!="www.dictionaryportal.eu") {
				Response.Redirect("http://www.dictionaryportal.eu"+HttpContext.Current.Request.Url.PathAndQuery, false);
				Response.StatusCode=301;
				Response.End();
			}

			//The requested path:
			string reqPath=Request.Url.AbsolutePath;

			//catalog.xml:
			Match match=Regex.Match(reqPath, @"^/catalog\.xml$", RegexOptions.IgnoreCase);
			if(match.Success) {
				HttpContext.Current.RewritePath("/AllXml.aspx");
				return;
			}

			//File with an extension (GIF, CSS etc)? Let it through immediately:
			match=Regex.Match(reqPath, @"\.[a-zA-Z0-9]+$");
			if(match.Success && !reqPath.ToLower().EndsWith(".aspx")) {
				return;
			}

			Metadata metadata=new Metadata("", Server);

			//UI Languagess:
			List<string> uilangs=new List<string>();
			foreach(Language l in metadata.languages) {
				if(l.isUI) uilangs.Add(l.code);
			}
			string uilangStamp="__"; foreach(string uilang in uilangs) { if(uilangStamp!="") uilangStamp+="|"; uilangStamp+=uilang; }

			//Set uilang cookie:
			match =Regex.Match(reqPath, @"^/("+uilangStamp+@")/", RegexOptions.IgnoreCase);
			if(match.Success) {
				string uilang=match.Groups[1].ToString().ToLower();
                Response.Cookies["uilang"].Value=Server.UrlEncode(uilang);
                Response.Cookies["uilang"].Expires=DateTime.Now.AddDays(30);
                if(Request.Url.Host!="localhost") Response.Cookies["uilang"].Domain=Request.Url.Host;
			}

			//Make sure URL always ends in slash:
			if(!Regex.IsMatch(reqPath, @"/$", RegexOptions.IgnoreCase) && !reqPath.ToLower().EndsWith(".aspx")) {
				Response.Redirect(reqPath+"/");
				return;
			}

			//UILang + An about page:
			match=Regex.Match(reqPath, @"^/("+uilangStamp+@")/(info|prop|prop-ok|prop-ko|crit|crtr|stmp|dnld)/?$", RegexOptions.IgnoreCase);
			if(match.Success) {
				string uilang=match.Groups[1].ToString().ToLower();
				string pageMode=match.Groups[2].ToString();
				HttpContext.Current.Items.Add("pageMode", pageMode);
				HttpContext.Current.Items.Add("uilang", uilang);
				HttpContext.Current.RewritePath("/About.aspx");
				return;
			}

			//Go:
			match=Regex.Match(reqPath, @"^/go/$", RegexOptions.IgnoreCase);
			if(match.Success) {
				HttpContext.Current.RewritePath("/Go.aspx");
				return;
			}

			//UILang + Catalog:
			match=Regex.Match(reqPath, @"^/("+uilangStamp+@")/ctlg/?$", RegexOptions.IgnoreCase);
			if(match.Success) {
				string uilang=match.Groups[1].ToString().ToLower();
				HttpContext.Current.Items.Add("pageMode", "catalog");
				HttpContext.Current.Items.Add("uilang", uilang);
				HttpContext.Current.RewritePath("/Default.aspx");
				return;
			}

			//UILang + DictID:
			match=Regex.Match(reqPath, @"^/("+uilangStamp+@")/([0-9]+)/?$", RegexOptions.IgnoreCase);
			if(match.Success) {
				string uilang=match.Groups[1].ToString().ToLower();
				int dictID=int.Parse(match.Groups[2].ToString());
				HttpContext.Current.Items.Add("pageMode", "permalink");
				HttpContext.Current.Items.Add("dictID", dictID);
				HttpContext.Current.Items.Add("uilang", uilang);
				HttpContext.Current.RewritePath("/Default.aspx");
				return;
			}

			//no UILang + DictID:
			match=Regex.Match(reqPath, @"^/([0-9]+)/?$", RegexOptions.IgnoreCase);
			if(match.Success) {
				int dictID=int.Parse(match.Groups[1].ToString());
				string uilang=Connfigger.DetectUILang(HttpContext.Current.Request, metadata);
				Response.Redirect("/"+uilang+"/"+dictID+"/");
				return;
			}

			//Home page + language code:
			match=Regex.Match(reqPath, @"^/("+uilangStamp+@")/?$", RegexOptions.IgnoreCase);
			if(match.Success) {
				string uilang=match.Groups[1].ToString().ToLower();
				HttpContext.Current.Items.Add("pageMode", "home");
				HttpContext.Current.Items.Add("uilang", uilang);
				HttpContext.Current.RewritePath("/Default.aspx");
				return;
			}

			//Home page + no language code:
			match=Regex.Match(reqPath, @"^/(default\.aspx)?/?$", RegexOptions.IgnoreCase);
			if(match.Success) {
                string uilang=Connfigger.DetectUILang(HttpContext.Current.Request, metadata);
				Response.Redirect("/"+uilang+"/");
				return;
			}

			//Redirect deprecated URLs:
			if(reqPath=="/en/catalog/" && Request.QueryString.Count>0) Response.Redirect("/en/ctlg/?"+Request.QueryString);
			if(reqPath=="/en/catalog/") Response.Redirect("/en/ctlg/");
			if(reqPath=="/en/about/") Response.Redirect("/en/info/");
			if(reqPath=="/en/stamp/") Response.Redirect("/en/stmp/");
			if(reqPath=="/en/curators/") Response.Redirect("/en/crtr/");
			if(reqPath=="/en/download/") Response.Redirect("/en/dnld/");
			if(reqPath=="/en/suggest/") Response.Redirect("/en/prop/");

			//Last chance saloon:
			//Response.Redirect("/");
		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{

		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}