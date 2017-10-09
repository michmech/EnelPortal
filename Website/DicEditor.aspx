<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DicEditor.aspx.cs" Inherits="Website.DicEditor" %><!DOCTYPE HTML>
<html>
	<head>
		<meta charset="UTF-8"/>
		<script src="http://code.jquery.com/jquery-1.7.2.min.js"></script>
		<link type="text/css" rel="stylesheet" href="/xonomy/xonomy.css?2017-10-09" />
		<script type="text/javascript" src="/xonomy/xonomy.js?2017-10-09"></script>
		<link type="text/css" rel="stylesheet" href="/furniture/templateEditor.css?2017-10-09" />
		<script type="text/javascript" src="/furniture/DicEditor.js?2017-10-09"></script>
	</head>
	<body onload="start()">
		<%if(this.id!=0){%>
			<form class="inline" method="POST" action="DicDelete.aspx" onsubmit="return confirm('Are you sure you want to delete this dictionary? You will not be able to undo this.')">
				<input class="action" type="submit" value="Delete"/>
				<input type="hidden" name="id" value="<%=this.id%>"/>
			</form>
		<%}%>
		<form class="inline" method="POST" action="DicSave.aspx" onsubmit="beforeSave()">
			<input id="butSave" class="action" type="submit" value="Save"/>
			<textarea id="txtXml" name="xml" spellcheck="false" style="display: none"><%=this.dic.DocumentElement.OuterXml.Replace("&", "&amp;")%></textarea>
		</form>
		<div id="editor"></div>
	</body>
</html>