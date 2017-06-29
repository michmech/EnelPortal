<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Website.Login" validateRequest="false" %><!DOCTYPE HTML>
<html>
	<head>
		<meta charset="UTF-8"/>
		<link type="text/css" rel="stylesheet" href="/furniture/templateDialog.css?2016-04-07" />
		<script type="text/javascript">
		</script>
	</head>
	<body>
	
	<%if(this.stage=="start" || this.stage=="error") {%>
		<div class="dialog">
			<form method="POST" action="/Login.aspx">
				<div class="formline">
					<label>Email address</label>
					<input name="email" value="<%=this.email%>" />
				</div>
				<div class="formline">
					<label>Password</label>
					<input name="password" type="password" />
				</div>
				<input class="action" type="submit" value="Login"/>
				<%if(this.stage=="error") {%><div class="message bad">Incorrect e-mail address or password.</div><%}%>
			</form>
		</div>
	<%} else if(this.stage=="success") {%>
		<div class="dialog">
			<div class="message good">You are now logged in.</div>
			<button class="action" onclick="parent.closeEditor(true)">OK</button>
		</div>
	<%}%>
	
	</body>
</html>