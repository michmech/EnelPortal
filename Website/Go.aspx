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
    <body>
        <form action="<%=this.url%>" method="post">
            <input type="text" name="keyword" value="<%=this.txt%>"/>
            <input type="text" name="type" value="1"/>
            <input type="submit">
        </form>
    </body>
</html>