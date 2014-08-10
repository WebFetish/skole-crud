<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="Nyheder.aspx.cs" Inherits="Nyheder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 runat="server" id="sideOverskrift"></h2>
    <asp:Panel ID="Panel_List" runat="server" Visible="false">
        <asp:Literal ID="LiteralItems" runat="server"></asp:Literal>
    </asp:Panel>

    <asp:Panel ID="Panel_Form" runat="server" Visible="false">
        
        <div class="col-lg-6">
            <%-- Denne del skal du kopirer for at lave flere textboxe --%>
            <div class="form-group">
                <label for="TextBoxOverkskrift">Overkskrift</label>
                <asp:TextBox ID="TextBoxOverkskrift" runat="server" placeholder="Overkskrift" CssClass="form-control" required></asp:TextBox>
            </div>
            <%-- stop med copy her --%>
            <div class="form-group">
                <label for="TextBoxTekst">Tekst</label>
                <asp:TextBox ID="TextBoxTekst" runat="server" TextMode="MultiLine" CssClass="form-control" required></asp:TextBox>
            </div>
            <asp:Button ID="Button_Save" runat="server" CssClass="btn btn-success" Text="Gem" OnClick="Button_Save_Click" />
            <a runat="server" id="sideLink" href="" class="btn btn-default" onclick="return confirm('Er du sikker på du vil annullere?')">Annuller</a>
        </div>
    </asp:Panel>

    <div class="clearfix"></div>
    <asp:Panel ID="Panel_Error" Visible="false" runat="server">
        <div class="alert alert-danger alert-dismissable">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <p><asp:Label ID="Label_Error" runat="server" Text=""></asp:Label></p>
        </div>
    </asp:Panel>
</asp:Content>

