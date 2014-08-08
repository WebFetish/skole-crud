<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OprerBruger.aspx.cs" Inherits="Admin_OprerBruger" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-lg-6">

            <div class="form-group" id="eName" runat="server">
                <label for="TextBox_Name" class="control-label">Navn</label>
                <asp:TextBox ID="TextBox_Name" runat="server" placeholder="Brugerens navn" CssClass="form-control" MaxLength="32" required="required"></asp:TextBox>
            </div>
            <div class="form-group" id="eEmail" runat="server">
                <label for="TextBox_Email" class="control-label">Email</label>
                <asp:TextBox ID="TextBox_Email" runat="server" placeholder="Brugernes Email" CssClass="form-control" MaxLength="128" required="required" TextMode="Email"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="TextBox_Password">Kodeord</label>
                <asp:TextBox ID="TextBox_Password" runat="server" placeholder="Kodeord" CssClass="form-control" MaxLength="40" TextMode="Password"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator_Passwords" runat="server" Display="Dynamic" ErrorMessage="<p class='alert alert-danger'>De to kodeord er ikke ens</p>" ControlToValidate="TextBox_Password" ControlToCompare="TextBox_Password_Repeat"></asp:CompareValidator>
            </div>

            <div class="form-group">
                <label for="TextBox_Password_Repeat">Gentag Kodeord</label>
                <asp:TextBox ID="TextBox_Password_Repeat" runat="server" placeholder="Gentag Kodeord" CssClass="form-control" MaxLength="40" TextMode="Password"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="TextBox_Role">Rolle</label>
                <asp:DropDownList ID="DropDownList_Role" runat="server" CssClass="form-control"></asp:DropDownList>
                <asp:RangeValidator ID="RangeValidator_Role" Display="Dynamic" runat="server" ErrorMessage="<p class='alert alert-danger'>Vælg en rolle</p>" ControlToValidate="DropDownList_Role" MinimumValue="1" MaximumValue="100"></asp:RangeValidator>
            </div>
            <asp:Button ID="Button_Save" runat="server" CssClass="btn btn-success" Text="Gem" OnClick="Button_Save_Click" />
        </div>
    <script src="Scripts/jquery-2.0.3.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</asp:Content>

