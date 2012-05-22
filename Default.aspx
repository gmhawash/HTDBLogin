<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <title>Advantos ERP Login</title>
  <link href="DBLogin.css" rel="StyleSheet" type="text/css" />
  <link rel="icon" href="../images/favicon.ico" type="image/x-icon" />
  <script type='text/javascript' src="js/jquery-1.6.2.js"></script>
  <script type="text/javascript">
    $(function () {
      $('#lstAccounts').keypress(function (event) {
        if(event.which == 13)
          $('#bLogin').click();
      });
    });
  </script>
</head>

<body>
  <div id='header'>
    <img src="images/advwscb_4.gif" alt="Advantos Logo"/>
  </div>
  <div id="content">
    <form id="form2" runat="server">

    <div class='field'>
    <asp:Label ID="luser" CssClass="label" runat="server" Text="User Name:" />
    <asp:TextBox ID="tUser" runat="server" CssClass="text" />
    </div>
    <div class='field'>
    <asp:Label ID="lpsw" CssClass="label" runat="server" Text="Password:" />
    <asp:TextBox ID="tPsw" runat="server" CssClass="text"  TextMode="Password"/>
    </div>
    <asp:Panel runat="server" ID="AccountPanel" CssClass="field">
      <asp:Label ID="lacct" CssClass="label" runat="server" Text="Account:"  />
      <asp:DropDownList ID="lstAccounts"  runat="server" CssClass="normalinput" />
    </asp:Panel>
    
    <div id="buttons">
      <asp:LinkButton ID="bClear" Text="Clear" runat="server" CssClass="loginbutton" OnClick="bLogin_Clear" />
      <asp:Button ID="bLogin" Text="Login" runat="server" CssClass="loginbutton" OnClick="bLogin_Click" />
    </div>
    </form>
  </div>
   <div class="error">
    <asp:Label ID="lerr" CssClass="error" runat="server" Text="Message" />
   </div>
</body>
</html>
