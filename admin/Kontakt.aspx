<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="Kontakt.aspx.cs" Inherits="Admin_Kontakt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 runat="server" id="sideOverskrift"></h2>
    <asp:Panel ID="Panel_List" runat="server" Visible="false">
        <asp:Literal ID="LiteralItems" runat="server"></asp:Literal>
    </asp:Panel>

    <asp:Panel ID="Panel_Form" runat="server" Visible="false">
        <div class="col-lg-6">
            <div class="form-group" id="eName" runat="server">
                <label for="TextBox_Name" class="control-label">Navn</label>
                <asp:TextBox ID="TextBox_Name" runat="server" placeholder="Navn" CssClass="form-control" MaxLength="32" required="required"></asp:TextBox>
            </div>
            <div class="form-group" id="eEmail" runat="server">
                <label for="TextBox_Email" class="control-label">Email</label>
                <asp:TextBox ID="TextBox_Email" runat="server" placeholder="Email" CssClass="form-control" MaxLength="128" required="required" TextMode="Email"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="DropDownList_Emne">Emne</label>
                <asp:DropDownList ID="DropDownList_Emne" runat="server" CssClass="form-control"></asp:DropDownList>
                <asp:RangeValidator ID="RangeValidator_Emne" runat="server" ErrorMessage="<p class='alert alert-danger'>Vælg et emne</p>" Display="Dynamic" ControlToValidate="DropDownList_Emne" MinimumValue="1" MaximumValue="100"></asp:RangeValidator>
            </div>
            <div class="form-group" id="eBesked" runat="server">
                <label for="TextBox_Besked" class="control-label">Besked</label>
                <asp:TextBox ID="TextBox_Besked" runat="server" CssClass="form-control" required="required" TextMode="MultiLine" Rows="5"></asp:TextBox>
            </div>
            <asp:Button ID="Button_Save" runat="server" CssClass="btn btn-success" Text="Gem" OnClick="Button_Save_Click" />
            <a runat="server" id="sideLink" href="" class="btn btn-default" onclick="return confirm('Er du sikker på du vil annullere?')">Annuller</a>
        </div>
        <script>
            $("#TextBox_Name").change(function () {
                if ($("#TextBox_Name").val().length > 0) {
                    $("#eName").attr("class", "form-group has-success");
                }
                else {
                    $("#eName").attr("class", "form-group has-error");
                }
            } );
            
            $("#TextBox_Email").change(function () {
                if ($("#TextBox_Email").val().length > 0) {
                    $("#eEmail").attr("class", "form-group has-success");
                }
                else {
                    $("#eEmail").attr("class", "form-group has-error");
                }
            });

            $("#TextBox_Besked").change(function () {
                if ($("#TextBox_Besked").val().length > 0) {
                    $("#eBesked").attr("class", "form-group has-success");
                }
                else {
                    $("#eBesked").attr("class", "form-group has-error");
                }
            });
        </script>
    </asp:Panel>

    <div class="clearfix"></div>

    <asp:Panel ID="Panel_Error" Visible="false" runat="server">
        <div class="alert alert-danger alert-dismissable">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <p>
                <asp:Label ID="Label_Error" runat="server" Text=""></asp:Label>
            </p>
        </div>
    </asp:Panel>
</asp:Content>

