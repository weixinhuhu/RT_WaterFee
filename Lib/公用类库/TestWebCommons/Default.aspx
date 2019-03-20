<%@ Page Title="主页" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="TestAsposeWordForWeb._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <br />
    <table>
        <tr>
            <td>
                <asp:Button ID="btnGenWord" runat="server" OnClick="btnGenWord_Click" Text="生成Word文档" />
            </td>
            <td style="width: 100px">
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnGenExcel" runat="server" Text="生成Excel文档" OnClick="btnGenExcel_Click" />
            </td>
        </tr>
         <tr>
            <td>
                <asp:Button ID="btnGenWord2" runat="server"  Text="生成Word文档(辅助类操作）" 
                    onclick="btnGenWord2_Click" />
            </td>
            <td style="width: 100px">
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnGenExcel2" runat="server" Text="生成Excel文档(辅助类操作）" 
                    onclick="btnGenExcel2_Click"  />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnConvertWordToHtml" runat="server"  Text="转换Word文档为HTML内容" 
                    onclick="btnConvertWordToHtml_Click" />
            </td>
            <td style="width: 100px">
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnConvertExcelToHtml" runat="server" Text="转换Excel文档为HTML内容" 
                    onclick="btnConvertExcelToHtml_Click"  />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
</asp:Content>
