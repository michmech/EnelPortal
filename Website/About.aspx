<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Website.About" %><!DOCTYPE HTML>
<html lang="<%=this.uilang%>">
	<head>
		<meta charset="UTF-8"/>	
		<meta name="viewport" content="width=device-width, user-scalable=yes, initial-scale=1.0" />
		<meta name="MobileOptimized" content="300" />
		<meta name="HandheldFriendly" content="true" />
		<title><%=L("edp")%>: <%=this.aboutPage.title%></title>
		<link rel="icon" href="/favicon.ico" />
		<link rel="image_src" href="http://www.dictionaryportal.eu/preview.gif" />
		<meta property="og:image" content="http://www.dictionaryportal.eu/preview.gif" />
		<script src="http://code.jquery.com/jquery-1.7.2.min.js"></script>
		<script type="text/javascript" src="/furniture/catalog.js?2017-07-12"></script>
		<link type="text/css" rel="stylesheet" href="/furniture/template.css?2017-07-12" />
		<link type="text/css" rel="stylesheet" href="/furniture/markdown.css?2017-07-12" />
		<script type="text/javascript">
		$(document).ready(function(){
			$("a[href='#']").click(function(){
				alert("This doesn't work yet, sorry.");
				return false;
			});
			$("ul#uilang li.current a").click(function(){
				$('#uilang li.more').slideToggle().css('display', 'block');
				return false;
			});
			$("ul#uilang li.more a").click(function(){
				$('#uilang li.more').hide();
			});

			$("a#hamburger").click(function(){
				$('#sitemenu').fadeToggle().css('display', 'block');
				$("body").click(function(){
					$('#uilang li.more').fadeOut();
					$('#sitemenu').fadeOut();
				});
				return false;
			});
			$("body").click(function(){
				$('#uilang li.more').slideUp();
			});
			$(".sortme").each(function(){
				var $this=$(this);
				var val=$this.val();
				//var opts_list=$this.find('option');
				var opts_list=$this.children();
                opts_list.sort(function(a, b) { if($(a).data("sortkey")=="ZZZ") return 1; if($(b).data("sortkey")=="ZZZ") return -1; return $(a).data("sortkey").localeCompare($(b).data("sortkey"), "<%=uilang%>"); });
				$this.append(opts_list);
				$this.val(val);
			});
		});
		</script>
	</head>
	<body>
		<div id="envelope">
			<div id="menubar">
				<a href="javascript:void(null)" id="hamburger"></a>
				<ul id="sitemenu">
					<li class=""><a href="/<%=this.uilang%>/"><%=L("home")%></a></li>
					<li class=""><a href="/<%=this.uilang%>/ctlg/"><%=L("find")%></a></li>
					<li class="<%if(this.pageMode=="info"){%> current<%}%>"><a href="/<%=this.uilang%>/info/"><%=L("about-info")%></a></li>
				</ul>
				<ul id="uilang">
					<li lang="<%=this.uilang%>" class="<%=this.uilang%> current"><a href="/<%=this.uilang%>/about/"><%=this.metadata.getNativeName(this.uilang)%> <span class="arrow">▼</span></a></li>
					<%foreach(Website.Language l in this.metadata.languages) { if(l.isUI && l.code!=this.uilang) {%>
						<li lang="<%=l.code%>" class="<%=l.code%> more"><a href="/<%=l.code%>/<%=this.pageMode%>/"><%=l.nativeName%></a></li>
					<%}}%>
				</ul>
			</div>
			
			<div id="logoContainer">
				<a href="/<%=this.uilang%>/"><img src="/furniture/logo.gif" alt="dictionaryportal.eu"></a>
			</div>
			
			<h1 class="tab directory">
				<span><%=this.aboutPage.title%></span>
				<div class="socials">
						<a target="_blank" href="https://plus.google.com/share?url=<%=Server.UrlEncode("http://www.dictionaryportal.eu/"+uilang+"/"+this.pageMode+"/")%>" class="gplus"></a>
						<a target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=<%=Server.UrlEncode("http://www.dictionaryportal.eu/"+uilang+"/"+this.pageMode+"/")%>" class="facebook"></a>
						<a target="_blank" href="https://twitter.com/intent/tweet?url=<%=Server.UrlEncode("http://www.dictionaryportal.eu/"+uilang+"/"+this.pageMode+"/")%>" class="twitter"></a>
				</div>
			</h1>

            <%if(this.noloc) {%>
                <div class="noloc"><%=L("anglo")%></div>
            <%}%>
			
			<div class="markdown-body">
				<%=this.html%>
                <%if(this.pageMode=="prop") {%>
                    <script type="text/javascript">
                        function prop_validate(){
                            var $el=$("input.name"); $el.val($.trim($el.val()));
                            var $el=$("input.url"); $el.val($.trim($el.val()));
                            var $el=$("input.email"); $el.val($.trim($el.val()));
                            var $el=$("textarea.comment"); $el.val($.trim($el.val()));

                            var valid=true;

                            if($("input.name").val()=="") {
                                $("input.name").closest(".datum").find(".error").show(); valid=false;
                            } else {
                                 $("input.name").closest(".datum").find(".error").hide();
                            }

                            if($("input.url").val()=="" || $("input.url").val().indexOf("http")!=0) {
                                $("input.url").closest(".datum").find(".error").show(); valid=false;
                            } else {
                                 $("input.url").closest(".datum").find(".error").hide();
                            }

                            if($("select.objLang").val()=="") {
                                $("select.objLang").closest(".datum").find(".error").show(); valid=false;
                            } else {
                                 $("select.objLang").closest(".datum").find(".error").hide();
                            }

                            if($("input.email").val()=="" || $("input.email").val().indexOf("@")==-1) {
                                $("input.email").closest(".datum").find(".error").show(); valid=false;
                            } else {
                                 $("input.email").closest(".datum").find(".error").hide();
                            }

                            return valid;
                        }
                    </script>
                    <form method="post" action="http://mailer.lexiconista.com/Send.aspx" onsubmit="return prop_validate()">
                        <input type="hidden" name="_to" value="Bob.Boelhouwer@ivdnt.org" />
                        <input type="hidden" name="_subject" value="European Dictionary Portal - suggestion" />
                        <%string url="http://www.dictionaryportal.eu"; if(Request.Url.Host=="localhost") url="http://localhost:49901";%>
                        <input type="hidden" name="_successUrl" value="<%=url%>/<%=uilang%>/prop-ok/" />
                        <input type="hidden" name="_failUrl" value="<%=url%>/<%=uilang%>/prop-ko/" />
                        <div class="datum">
                            <div class="title"><%=L("prop-name")%></div>
                            <input class="name" name="dictionary-name"/>
                            <div class="error" style="display: none"><%=L("prop-name-error")%></div>
                        </div>
                        <div class="datum">
                            <div class="title"><%=L("prop-url")%></div>
                            <input class="url" name="dictionary-url"/>
                            <div class="error" style="display: none"><%=L("prop-url-error")%></div>
                        </div>
                        <div class="datum">
                            <div class="title"><%=L("objLang")%></div>
                            <div class="legend"><%=L("objLangBlurb")%></div>
                            <select name="object-language" class="objLang sortme">
                                <option value="" data-sortkey=""><%=L("select")%></option>
							    <%foreach(Website.Language l in this.metadata.languages) {%>
                                    <%if(!l.isSign) {%><option value="<%=l.name%> (<%=l.code%>)" data-sortkey="<%=l.name%>"><%=l.name%></option><%}%>
                                <%}%>
                                <optgroup label="<%=L("signlangs")%>" class="sortme" data-sortkey="ZZZ">
							        <%foreach(Website.Language l in this.metadata.languages) {%>
                                        <%if(l.isSign) {%><option value="<%=l.name%> (<%=l.code%>)" data-sortkey="<%=l.name%>"><%=l.name%></option><%}%>
                                    <%}%>
                                </optgroup>
						    </select>
                            <div class="error" style="display: none"><%=L("objLang-error")%></div>
                        </div>
                        <div class="datum">
                            <div class="title"><%=L("prop-email")%></div>
                            <div class="legend"><%=L("prop-email-legend")%></div>
                            <input class="email" name="_from"/>
                            <div class="error" style="display: none"><%=L("prop-email-error")%></div>
                        </div>
                        <div class="datum">
                            <div class="title"><%=L("prop-comments")%></div>
                            <textarea class="comment" name="comments"></textarea>
                        </div>
                        <div class="checkum">
                            <%string s=L("prop-crit");%>
                            <%s=Regex.Replace(s, @"\[([^\[\]]+)\]", "<a href='/"+uilang+"/crit/'>$1</a>");%>
                            <%=s%>
                        </div>
                        <div><input class="sbm" type="submit" value="<%=L("submit")%>"/></div>
                    </form>
                <%}%>
                <%if(this.pageMode=="info") {%>
                    <div class="crossroads">
                        <h2><%=L("about-more")%></h2>
                        <ul>
                            <%foreach(Website.AboutPage ap in this.metadata.aboutPages) {%>
                                <%if(ap.code!="info") {%><li><a href="/<%=uilang%>/<%=ap.code%>/"><%=ap.title%></a></li><%}%>
                            <%}%>
                        </ul>
                    </div>
                <%}%>
			</div>

			<div class="clear"></div>
		</div>
		<div id="footer">
			<div class="block left">
				<div class="title"><%=L("toc")%></div>
				<ul>
					<li><a href="/<%=this.uilang%>/"><%=L("home")%></a></li>
					<li><a href="/<%=this.uilang%>/ctlg/"><%=L("find")%></a></li>
					<li><a href="/<%=this.uilang%>/info/"><%=L("about-info")%></a></li>
                    <li>
                        <ul>
                            <%foreach(Website.AboutPage ap in this.metadata.aboutPages) {%>
                                <%if(ap.code!="info") {%><li><a href="/<%=uilang%>/<%=ap.code%>/"><%=ap.title%></a></li><%}%>
                            <%}%>
                        </ul>
                    </li>
				</ul>
                <div class="github"><a href="https://github.com/michmech/EnelPortal" target="_blank"><img src="/furniture/github.png" alt="GitHub" /></a></div>
			</div>
			<div class="block right">
				<%string foot1=L("foot1");%>
                <%foot1=Regex.Replace(foot1, @"\[([^\]]+)\]", "<a target='_blank' href='http://www.elexicography.eu/'>$1</a>");%>
                <%=foot1%>
				<div class="logos">
					<a href="http://www.elexicography.eu/" target="_blank"><img src="/furniture/footer-enel.gif" alt="European Network of e-Lexicography"></a>
					<a href="http://www.cost.eu/" target="_blank"><img src="/furniture/footer-cost.gif" alt="COST"></a>
					<a href="http://ec.europa.eu/programmes/horizon2020/" target="_blank"><img src="/furniture/footer-eu.gif" alt="Horizon 2020"></a>
					<a href="http://www.ivdnt.org/" target="_blank"><img src="/furniture/footer-ivdnt.gif" alt="Instituut voor de Nederlandse Taal"></a>
				</div>
				<%string foot2=L("foot2");%>
                <%foot2=Regex.Replace(foot2, @"\[\[([^\]]+)\]\]", "<a href='http://ec.europa.eu/programmes/horizon2020/' target='_blank'>$1</a>");%>
                <%foot2=Regex.Replace(foot2, @"\[([^\]]+)\]", "<a href='http://www.cost.eu/' target='_blank'>$1</a>");%>
                <%=foot2%>
				<div class="credits">
				    <%=L("mbm").Replace(" ", "&nbsp;")%>&nbsp;<a target="_blank" href="http://www.lexiconista.com/">Michal Boleslav Měchura</a><br/>
				    <%=L("ivdnt").Replace(" ", "&nbsp;")%>&nbsp;<a target="_blank" href="http://www.ivdnt.org/">Instituut voor de Nederlandse Taal</a><br/>
					<%=L("shots").Replace(" ", "&nbsp;")%>&nbsp;<a href="http://www.bitpixels.com/" target="_blank">BitPixels</a><br/>
					<%=L("icons").Replace(" ", "&nbsp;")%>&nbsp;<a href="http://famfamfam.com/lab/icons/" target="_blank">FamFamFam</a>
				</div>
				<div class="stamp">
					<a href="/<%=uilang%>/stmp/"><img src="/stamp.gif" alt="Stamp of approval"/></a>
				    <%
                    string foot3=L("foot3");
                    if(foot3.EndsWith("]")) foot3=Regex.Replace(foot3, @"\[([^\]]+)\]", "<a href='/"+uilang+"/stmp/'>$1&nbsp;»</a>");
                    else foot3=Regex.Replace(foot3, @"\[([^\]]+)\]", "<a href='/"+uilang+"/stmp/'>$1</a>")+".";
                    %>
                    <div><%=foot3%></div>
					<div></div>
				</div>
				<div class="loginContainer">
					<%if(Session["email"]==null) {%>
						<a class="cms loginLink" href="javascript:openLogin()"><span class="dot"></span> <%=L("login")%> »</a>
					<%} else {%>
						<span class="cms loginLink">
							<span class="dot"></span>
							<%=Session["email"]%>
							<%if( (bool)Session["IsAdmin"] ){%>| <a href="javascript:openAdminEditor()">Administration »</a><%}%>
							| <a href="javascript:openLogout()">Log out »</a>
						</span>
					<%}%>
				</div>
			</div>
			<div class="clear"></div>
		</div>
		
		<div id="courtain" style="display: none;" onclick="closeEditor()"></div>
		<div id="incourtain" style="display: none;"><iframe id="editorFrame" name="editorFrame"></iframe></div>

		<%=Website.Connfigger.GetTrackingCode()%>
	</body>
</html>
