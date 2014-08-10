<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Content/StyleSheet.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row" style="padding-top: 70px;">
        <div class="col-5 form-horizontal">
            <div class="form-group">
                <label for="TextBox_Email" class="col-lg-2 control-label">Email</label>
                <div class="col-lg-4">
                    <asp:TextBox ID="TextBox_Email" runat="server" CssClass="form-control" TextMode="Email" required autofocus ValidationGroup="Login"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label for="TextBox_Password" class="col-lg-2 control-label">Kodeord</label>
                <div class="col-lg-4">
                    <asp:TextBox ID="TextBox_Password" runat="server" TextMode="Password" CssClass="form-control" required ValidationGroup="Login"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-lg-offset-2 col-lg-10">
                    <asp:Button ID="Button_Login" runat="server" CssClass="btn btn-default" Text="Login" OnClick="Button_Login_Click" />
                    <asp:Label ID="Label_Errors" runat="server" CssClass="alert alert-danger" Visible="false"></asp:Label>
                 </div>
            </div>
        </div>
    </div>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.0.3.min.js"></script>
</asp:Content>

