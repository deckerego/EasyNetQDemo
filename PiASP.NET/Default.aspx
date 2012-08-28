<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PiASP.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Calculate Pi - Poorly!</title>
	<link rel="stylesheet" type="text/css" href="css/clippy.css" media="all"/>
</head>
<body>
    <form id="sendmessage" runat="server">
    <div>
    	Calculate Pi Using <i>X</i> Iterations:<br />
		<asp:TextBox ID="MessageText" runat="server" Width="192px"></asp:TextBox>
		<br />
		Pi is not: <asp:Label ID="MessageResponse" runat="server" />
		<br />
		<asp:Button ID="MessageSend" runat="server" onclick="SendMessage" Text="Send" />
    </div>
    </form>

	<script src="Scripts/jquery-1.8.0.min.js" type="text/javascript"></script>
	<script src="Scripts/clippy.min.js" type="text/javascript"></script>
	<script type="text/javascript">
		clippy.load('Clippy', function (agent) {
			agent.show();
			agent.moveTo(200, 10);
			agent.gestureAt(0, 10);
		});
	</script>
</body>
</html>