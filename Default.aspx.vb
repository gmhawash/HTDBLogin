Imports System
Imports System.Collections.Generic
Imports BlueFinity.mvNET.CoreObjects

Partial Class _Default
  Inherits System.Web.UI.Page


  Const LOGIN_PROFILE = "AsiPublic"
  Const DBMAIN_PAGE = "http://devweb.advantos.net/asipublic"

  Private gAcct As mvAccount = Nothing
  Public Property mvAcct() As mvAccount
    Get
      If (gAcct Is Nothing) Then gAcct = New mvAccount(LOGIN_PROFILE)
      Return gAcct
    End Get

    Set(ByVal value As mvAccount)
      If ((value Is Nothing) And (Not gAcct Is Nothing)) Then gAcct.Logout()
      gAcct = value
    End Set
  End Property

  
  Structure LoginInfo
    Public ID As String
    Public Accounts As List(Of String)
    Public DBUrl As String
  End Structure

  Structure LoginRequest
    Public User As String
    Public Password As String
  End Structure

  Sub Page_Load(ByVal Sender As System.Object, ByVal e As System.EventArgs)
    lerr.Visible = False
    lerr.Text = ""
    tUser.Focus()
    AccountPanel.Visible = False
  End Sub


  Protected Sub bLogin_Clear(ByVal sender As Object, ByVal e As System.EventArgs)
    ClearDWState()
    Response.Redirect(Request.Url.AbsolutePath)
  End Sub

  Protected Sub bLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    Try
      If bLogin.Text = "Login" Then
        CheckLogin()
        lstAccounts.Focus()
      Else
        AccountSelected()
        GoToAccount()
      End If
    Catch ex As Exception
      ShowError(ex.Message)
      tUser.Focus()
    End Try
  End Sub

  Private Sub ClearDWState()
    Dim err As String = ""
    ' we get 3 tries to make a connection
    For i = 0 To 2
      Try
        Dim ID As String = Session("AdvantosID").ToString()
        Dim item As mvItem = New mvItem()
        item(1) = "CLEAR"
        item(2) = ID
        Dim data As String = item.RawContents
        mvAcct.CallProg("DB.VALIDATELOGIN", data)
        Exit For
      Catch ex As Exception
        System.Threading.Thread.Sleep(100)
        err = ex.Message
      Finally
        mvAcct = Nothing
      End Try
    Next

    If (Not String.IsNullOrEmpty(err)) Then
      ShowError(err)
    End If
  End Sub

  Private Sub CheckLogin()
    Dim loginRequest As LoginRequest
    loginRequest.User = tUser.Text
    loginRequest.Password = tPsw.Text

    Dim info As LoginInfo = IfValid(loginRequest)

    If (String.IsNullOrEmpty(info.ID)) Then
      ShowError("Login unsuccessful. Please try again.")
      Return
    End If

    ' Login good!  Proceed...
    luser.Enabled = False
    lpsw.Enabled = False
    Dim rgx As New Regex(".")
    Dim shadow As String = rgx.Replace(tPsw.Text, "*")
    tPsw.Attributes.Add("value", shadow)
    tUser.Enabled = False
    tPsw.Enabled = False
    AccountPanel.Visible = True

    ' Set session variables from return
    Session("AdvantosID") = info.ID
    Session("DbUrl") = info.DBUrl
    If (String.IsNullOrEmpty(info.DBUrl)) Then
      Session("DbUrl") = DBMAIN_PAGE
    End If


    If (info.Accounts.Count = 1) Then
      GoToAccount() ' Just go to homepage.
    End If

    ' Setup list for user to select account
    lstAccounts.DataSource = info.Accounts
    lstAccounts.DataBind()
    lstAccounts.SelectedIndex = 0
    bLogin.Text = "Go to Acct"
  End Sub

  Private Sub AccountSelected()
    Dim err As String = ""
    ' we get 3 tries to make a connection
    For i = 0 To 2
      Try
        Dim ID As String = Session("AdvantosID").ToString()
        Dim item As mvItem = New mvItem()
        item(1) = "ACCOUNT"
        item(2) = ID
        item(3) = lstAccounts.SelectedValue
        Dim data As String = item.RawContents
        mvAcct.CallProg("DB.VALIDATELOGIN", data)
        Exit For
      Catch ex As Exception
        System.Threading.Thread.Sleep(100)
        err = ex.Message
      Finally
        mvAcct = Nothing
      End Try
    Next

    If (Not String.IsNullOrEmpty(err)) Then
      ShowError(err)
    End If
  End Sub

  Private Sub GoToAccount()
    Dim ID As String = Session("AdvantosID").ToString()
    Response.Redirect(Session("DbUrl").ToString() + "?ID=" + ID)
  End Sub

  Private Sub ShowError(ByVal Err As String)
    If (Not String.IsNullOrEmpty(lerr.Text)) Then Return

    lerr.Text = Err
    lerr.Visible = True
  End Sub

  Private Function IfValid(ByVal loginRequest As LoginRequest) As LoginInfo
    Dim info As LoginInfo = New LoginInfo With {.ID = "", .Accounts = Nothing}

    Dim Err As String = ""

    For i = 0 To 2
      Try
        Dim item As mvItem = New mvItem()

        item(1) = "LOGIN"
        item(2) = loginRequest.User
        item(3) = loginRequest.Password
        Dim data As String = item.RawContents
        mvAcct.CallProg("DB.VALIDATELOGIN", data)

        If (Not String.IsNullOrEmpty(data)) Then
          item = New mvItem(data)
          info.ID = item(1).ToString()
          info.Accounts = New List(Of String)(item(2).ToString().Split(New String() {DataBASIC.VM}, StringSplitOptions.None))
        End If
        Return info
      Catch ex As Exception
        System.Threading.Thread.Sleep(100)
        Err = ex.Message
      Finally
        mvAcct = Nothing
      End Try
    Next
    If (Not String.IsNullOrEmpty(Err)) Then
      ShowError(Err)
    End If

    Return info
  End Function

End Class
