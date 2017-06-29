<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Website._Default" %><!DOCTYPE HTML>
<html lang="<%=this.uilang%>">
	<head>
		<meta charset="UTF-8"/>	
		<meta name="viewport" content="width=device-width, user-scalable=yes, initial-scale=1.0" />
		<meta name="MobileOptimized" content="300" />
		<meta name="HandheldFriendly" content="true" />
		<title><%=L("edp")%></title>
		<link rel="icon" href="/favicon.ico" />
		<link rel="image_src" href="http://www.dictionaryportal.eu/preview.gif" />
		<meta property="og:image" content="http://www.dictionaryportal.eu/preview.gif" />
		<script src="http://code.jquery.com/jquery-1.7.2.min.js"></script>
		<script src="http://code.jquery.com/ui/1.8.19/jquery-ui.min.js"></script>
 		<script type="text/javascript" src="/furniture/jquery.pulse.js"></script>
		<script type="text/javascript" src="/furniture/catalog.js?2017-06-22"></script>
		<link type="text/css" rel="stylesheet" href="/furniture/template.css?2017-06-22" />
		<script type="text/javascript">
		$(document).ready(function(){
			$(document).keyup(function(e){
				if(e.keyCode==27) parent.closeEditor();
			});
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
				var opts_list=$this.children();
                opts_list.sort(function(a, b) { return $(a).data("sortkey").localeCompare($(b).data("sortkey"), "<%=uilang%>"); });
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
					<li class="<%if(this.pageMode=="home"){%> current<%}%>"><a href="/<%=this.uilang%>/"><%=L("home")%></a></li>
					<li class="<%if(this.pageMode=="catalogHome" || this.pageMode=="catalogListing"){%> current<%}%>"><a href="/<%=this.uilang%>/ctlg/"><%=L("find")%></a></li>
					<li class=""><a href="/<%=this.uilang%>/info/"><%=L("about-info")%></a></li>
				</ul>
				<ul id="uilang">
					<li lang="<%=this.uilang%>" class="<%=this.uilang%> current"><a href="/<%=this.uilang%>/<%=this.getQueryString()%>"><%=this.metadata.getNativeName(this.uilang)%> <span class="arrow">▼</span></a></li>
					<%foreach(Website.Language l in this.metadata.languages) { if(l.isUI && l.code!=this.uilang) {%>
						<li lang="<%=l.code%>" class="<%=l.code%> more"><a href="/<%=l.code%>/<%if(this.pageMode=="catalogHome" || this.pageMode=="catalogListing"){%>ctlg/<%}%><%=this.getQueryString()%>"><%=l.nativeName%></a></li>
					<%}}%>
				</ul>
			</div>
			
			<div id="logoContainer">
				<a href="/<%=this.uilang%>/"><img src="/furniture/logo.gif" alt="dictionaryportal.eu"></a>
			</div>
			
			<%if(this.pageMode=="home") {%>
				<form name="searchForm" class="search" method="GET" action="/<%=uilang%>/ctlg/" onsubmit="return submitSearch('<%=uilang%>')">
					<input placeholder="<%=L("searchbox")%>" class="txt directSearch" type="text" name="txt" onchange='synchroSearch(this)' onkeyup='synchroSearch(this)'/>
					<select class="lng sortme" name="objLang">
						<option value="x" data-sortkey="" class="title"><%=L("language")%></option>
						<%foreach(Website.Language l in this.metadata.languages) {%>
							<%if(this.searchableObjLangs.Contains(l.code)) {%>
								<option value="<%=l.code%>" data-sortkey="<%=l.name%>"><%=l.name%></option>
							<%}%>	
						<%}%>
					</select>	
					<input class="sbm" type="submit" value=""/>
				</form>
				<div class="greeting">
                    <%string blurb=L("blurb");%>
                    <%blurb=Regex.Replace(blurb, @"\[\[([^\]]+)\]\]", "<a href='/"+this.uilang+"/info/'>$1</a>");%>
                    <%blurb=Regex.Replace(blurb, @"\[([^\]]+)\]", "<a target='_blank' href='http://www.elexicography.eu/'>$1</a>");%>
                    <%=blurb%>
					<div class="socials">
						<a target="_blank" href="https://plus.google.com/share?url=<%=Server.UrlEncode("http://www.dictionaryportal.eu/"+uilang+"/")%>" class="gplus"></a>
						<a target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=<%=Server.UrlEncode("http://www.dictionaryportal.eu/"+uilang+"/")%>" class="facebook"></a>
						<a target="_blank" href="https://twitter.com/intent/tweet?url=<%=Server.UrlEncode("http://www.dictionaryportal.eu/"+uilang+"/")%>" class="twitter"></a>
					</div>
				</div>
			<%}%>
			
			<%if(this.pageMode=="home") {%>
				<h1 class="tab directory">
					<span><%=L("moment")%></span>
				</h1>
				<div class="dow">
					<%
						System.Xml.XmlDocument dow=this.dictionaries[0];
						string dowObjLang=this.getXmlValue(dow, "/dictionary/objLang/@code", "");
					%>
					<div class="filter">
						<a class="more" href="/<%=uilang%>/ctlg/<%=getQueryString(dowObjLang, "x", "x")%>"><%=L("moreDicts")%>&nbsp;(<%=this.metadata.getLanguage(dowObjLang).name%>)&nbsp;»</a>
					</div>
					<%=this.printDictionary(dow)%>
				</div>
			<%}%>
			
			<h1 class="tab directory">
				<span><%=L("find")%></span>
				<%if(this.pageMode!="home") {%>
					<div class="socials">
						<a target="_blank" href="https://plus.google.com/share?url=<%=Server.UrlEncode("http://www.dictionaryportal.eu/"+uilang+"/ctlg/"+getQueryString(objLang, metaLang, dicType))%>" class="gplus"></a>
						<a target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=<%=Server.UrlEncode("http://www.dictionaryportal.eu/"+uilang+"/ctlg/"+getQueryString(objLang, metaLang, dicType))%>" class="facebook"></a>
						<a target="_blank" href="https://twitter.com/intent/tweet?url=<%=Server.UrlEncode("http://www.dictionaryportal.eu/"+uilang+"/ctlg/"+getQueryString(objLang, metaLang, dicType))%>" class="twitter"></a>
					</div>
				<%}%>
			</h1>
			<%if(this.pageMode=="catalogHome" || this.pageMode=="catalogListing") {%>
				<form name="catalogFilter" class="filter" action="/<%=uilang%>/ctlg/" method="GET" onsubmit="return submitFilter()">
					<div class="title"><%=L("filter")%></div>
					<div class="datum">
						<div class="title"><%=L("objLang")%></div>
						<div class="field"><select name="objLang" class="sortme">
							<option value="x" data-sortkey="" <%if("x"==this.objLang){%>selected="selected"<%}%>>(<%=L("anyLang")%>)</option>
							<%foreach(Website.Language l in this.metadata.languages) {%>
								<%if(this.objLangs.Contains(l.code)) {%>
									<option value="<%=l.code%>" data-sortkey="<%=l.name%>" <%if(l.code==this.objLang){%>selected="selected"<%}%>><%=l.name%></option>
								<%}%>
							<%}%>
						</select></div>
						<div class="legend">
							<%=L("objLangBlurb")%>
						</div>
					</div>
					<div class="datum">
						<div class="title"><%=L("metaLang")%></div>
						<div class="field"><select name="metaLang" class="sortme">
							<option value="x" data-sortkey="" <%if("x"==this.metaLang){%>selected="selected"<%}%>>(<%=L("anyLang")%>)</option>
							<%foreach(Website.Language l in this.metadata.languages) {%>
								<%if(this.metaLangs.Contains(l.code)) {%>
									<option value="<%=l.code%>" data-sortkey="<%=l.name%>" <%if(l.code==this.metaLang){%>selected="selected"<%}%>><%=l.name%></option>
								<%}%>
							<%}%>
						</select></div>
						<div class="legend">
							<%=L("metaLangBlurb")%>
						</div>
					</div>
					<div class="datum">
						<div class="title"><%=L("dicType")%></div>
						<div class="field"><select name="dicType">
							<option value="x" <%if("x"==this.dicType){%>selected="selected"<%}%>>(<%=L("anyDicType")%>)</option>
							<%foreach(Website.DicType l in this.metadata.dicTypes) {%>
								<option value="<%=l.code%>" <%if(l.code==this.dicType){%>selected="selected"<%}%>><%=l.name%></option>
							<%}%>
						</select></div>
						<div class="legend">
							<%=L("dicTypeBlurb")%>
						</div>
					</div>
					<div class="datum">
						<input class="sbm" type="submit" value="<%=L("search")%>"/>
					</div>
				</form>
			<%}%>
							
			<%if(this.pageMode=="home" || this.pageMode=="catalogHome") {%>
				<div class="hierarchies <%=this.pageMode%> sortme">
					<%foreach(Website.Hierarchy h in this.hierarchies){%>
						<div class="hierarchy" data-sortkey="<%=this.metadata.getLanguage(h.objLang).name%>">
							<div class="title"><a href="/<%=uilang%>/ctlg/<%=getQueryString(h.objLang, "x", "x")%>"><%=this.metadata.getLanguage(h.objLang).name%><%if(h.dicTypes.Count<=1) {%>&nbsp;<span class="count">(<%=h.count%>)</span><%}%></a></div>
							<%if(h.dicTypes.Count>1) {%>
								<div class="body">
									<%bool first=true;%>
									<%foreach(string dicType in h.dicTypes.Keys) {%>
										<%if(!first){%>&middot;<%}%>
										<a href="/<%=uilang%>/ctlg/<%=getQueryString(h.objLang, "x", dicType)%>"><%=this.metadata.getDicType(dicType).name%> <span class="count">(<%=h.dicTypes[dicType]%>)</span></a>
										<%first=false;%>
									<%}%>
								</div>
							<%}%>
						</div>
					<%}%>
				</div>
				<div class="orphans">
					<span class="title"><%=L("notyet")%></span>
					<span class="sortme">
						<%foreach(Website.Language l in this.metadata.languages){%>
							<%if(!this.hierarchyHasLang(l.code) && !l.isSign){%>
								<span class="language" data-sortkey="<%=l.name%>">
									<a href="/<%=uilang%>/ctlg/<%=getQueryString(l.code, "x", "x")%>"><%=l.name%></a>
								</span>
							<%}%>
						<%}%>
					</span>
					<span class="subtitle"><%=L("signlangs")%></span>
					<span class="sortme">
						<%foreach(Website.Language l in this.metadata.languages){%>
							<%if(!this.hierarchyHasLang(l.code) && l.isSign){%>
								<span class="language" data-sortkey="<%=l.name%>">
									<a href="/<%=uilang%>/ctlg/<%=getQueryString(l.code, "x", "x")%>"><%=l.name%></a>
								</span>
							<%}%>
						<%}%>
					</span>
				</div>
			<%}%>
				
			<%if(this.pageMode=="catalogListing") {%>
				<div class="breadcrumbs">
					<a href="/<%=uilang%>/ctlg/"><%=L("top")%></a>
					<%if(this.objLang!="x" || this.metaLang!="x") {%>
						»
						<%if(this.objLang!="x" && this.metaLang=="x") {%>
							<a href="/<%=uilang%>/ctlg/<%=getQueryString(this.objLang, "x", "x")%>"><%=this.metadata.getLanguage(this.objLang).name%></a>
						<%} else if(this.objLang=="x" && this.metaLang!="x") {%>
							<a href="/<%=uilang%>/ctlg/<%=getQueryString("x", "x", "x")%>">(<%=L("anyLang")%>)</a> » <a href="/<%=uilang%>/ctlg/<%=getQueryString("x", this.metaLang, "x")%>"><%=this.metadata.getLanguage(this.metaLang).name%></a>
						<%} else {%>
							<a href="/<%=uilang%>/ctlg/<%=getQueryString(this.objLang, "x", "x")%>"><%=this.metadata.getLanguage(this.objLang).name%></a> » <a href="/<%=uilang%>/ctlg/?<%=getQueryString(this.objLang, this.metaLang, "x")%>"><%=this.metadata.getLanguage(this.metaLang).name%></a>
						<%}%>
					<%}%>
					<%if(this.dicType!="x") {%>
						» <a href="/<%=uilang%>/ctlg/<%=getQueryString(this.objLang, this.metaLang, this.dicType)%>"><%=this.metadata.getDicType(this.dicType).name%></a>
					<%}%>
					<span class="count">(<%=(this.dictionariesCount<=50 ? this.dictionariesCount.ToString() : "50+")%>)</span>
				</div>
				<%if(this.objLang!="x" && this.metaLang=="x" && this.dicType=="x") {%>
					<%foreach(Website.Hierarchy h in this.hierarchies){%>
						<%if(h.objLang==this.objLang) {%>
							<%if(h.dicTypes.Count>1 || h.metaLangs.Count>1) {%>
								<div class="minihierarchy">
									<%if(h.dicTypes.Count>1) {%>
										<div class="block">
											<%foreach(string dicType in h.dicTypes.Keys) {%>
												<div class="item">&middot; <a href="/<%=uilang%>/ctlg/<%=getQueryString(h.objLang, "x", dicType)%>"><%=this.metadata.getDicType(dicType).name%> <span class="count">(<%=h.dicTypes[dicType]%>)</span></a></div>
											<%}%>
										</div>
									<%}%>
									<%if(h.metaLangs.Count>1) {%>
										<div class="block">
											<%foreach(string metaLang in h.metaLangs.Keys) {%>
												<div class="item">&middot; <a href="/<%=uilang%>/ctlg/<%=getQueryString(h.objLang, metaLang, "x")%>"><%=this.metadata.getLanguage(h.objLang).name%> » <%=this.metadata.getLanguage(metaLang).name%> <span class="count">(<%=h.metaLangs[metaLang]%>)</span></a></div>
											<%}%>
										</div>
									<%}%>
									<div class="clear"></div>
								</div>
							<%}%>
						<%}%>
					<%}%>
				<%}%>
				<%if(this.dicType!="x" && this.dictionaries.Count>0) {%>
					<div class="dicTypeLegend">
						<%=this.metadata.getDicType(this.dicType).legend.Replace("[", "<strong>").Replace("]", "</strong>")%>
					</div>
				<%}%>
				<div>
					<%for(int i=0; i<50; i++) { if(this.dictionaries.Count>i) {%>
						<%System.Xml.XmlDocument doc=this.dictionaries[i];%>
						<%=this.printDictionary(doc)%>
					<%}}%>
				</div>
				<%if(this.dictionaries.Count>50) {%>
					<div class="nojoy"><%=L("toomany")%></div>
				<%}%>
				<%if(this.dictionaries.Count==0){%>
					<div class="nojoy"><%=L("nojoy")%></div>
				<%}%>
				<div class="suggestContainer">
					<%if(Session["email"]!=null){%><a class="cms addLink" href="javascript:openDicEditor(0, '<%=this.objLang%>')">Add a dictionary <span class="dot"></span></a><%}%>
					<a class="button" href="/<%=uilang%>/prop/"><%=L("about-prop")%>&nbsp;»</a>
				</div>
			<%}%>
			
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
				    <%string foot3=L("foot3");%>
                    <%foot3=Regex.Replace(foot3, @"\[([^\]]+)\]", "<a href='/"+uilang+"/stmp/'>$1&nbsp;»</a>");%>
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
