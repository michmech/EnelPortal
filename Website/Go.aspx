<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Go.aspx.cs" Inherits="Website.Go" %><!DOCTYPE HTML>
<html lang="<%=this.uilang%>">
	<head>
		<meta charset="UTF-8"/>	
		<meta name="viewport" content="width=device-width, user-scalable=yes, initial-scale=1.0" />
		<meta name="MobileOptimized" content="300" />
		<meta name="HandheldFriendly" content="true" />
		<title><%=L("edp")%></title>
		<link rel="icon" href="/favicon.ico" />
    </head>
    <body onload="document.forms[0].submit()">
        <form action="<%=Server.HtmlEncode(this.url)%>" method="post" style="visibility: hidden">
            <%foreach(string name in this.postFields.Keys) {%>
                <%string value=this.postFields[name]; if(value=="") value=this.txt;%>
                <input type="text" name="<%=Server.HtmlEncode(name)%>" value="<%=Server.HtmlEncode(value)%>"/>
            <%}%>
            <input type="submit">
        </form>
    </body>
</html>