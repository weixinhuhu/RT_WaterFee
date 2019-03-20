<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenerateHtml.aspx.cs" Inherits="TestAsposeWordForWeb.GenerateHtml" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <table>
        <tr>
            <td>
                <asp:Button ID="btnGenerateString" runat="server" Text="生成字符串内容" 
                    onclick="btnGenerateString_Click" />
            </td>
            <td style="width: 100px">
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnGenerateFile" runat="server" Text="生成文件" 
                    onclick="btnGenerateFile_Click" Width="133px" />
            </td>
        </tr>
         <tr>
            <td>
                生成内容展示
            </td>
            <td style="width: 100px" colspan="3">
                <textarea id="txtCode" runat="server" style="height:300px;width:400px" rows="10" cols="20"></textarea>
            </td>
        </tr>
    </table>
</asp:Content>
