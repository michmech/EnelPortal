<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminEditor.aspx.cs" Inherits="Website.AdminEditor" %><!DOCTYPE HTML>
<html>
	<head>
		<meta charset="UTF-8"/>
		<script src="http://code.jquery.com/jquery-1.7.2.min.js"></script>
		<link type="text/css" rel="stylesheet" href="/xonomy/xonomy.css?2016-04-07" />
		<script type="text/javascript" src="/xonomy/xonomy.js?2016-04-07"></script>
		<link type="text/css" rel="stylesheet" href="/furniture/templateEditor.css?2016-04-07" />
		<script type="text/javascript" src="/furniture/AdminEditor.js?2016-04-07"></script>
	</head>
	<body onload="start()">
		<form class="inline" method="POST" action="AdminSave.aspx" onsubmit="beforeSave()">
			<input id="butSave" class="action" type="submit" value="Save"/>
			<textarea id="txtXml" name="xml" spellcheck="false" style="display: none"><%=this.doc.DocumentElement.OuterXml.Replace("&", "&amp;")%></textarea>
		</form>
		<div id="editor"></div>
	</body>
</html>