<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Default_View.ascx.cs" Inherits="CUS.ICS.ContactReport.Default_View" %>
<%@ Register 
    TagPrefix="common"
	assembly="Jenzabar.Common"
	Namespace="Jenzabar.Common.Web.UI.Controls"
%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<div id="divAdminLink" style="text-align: center;" runat="server" visible="false">
	<table class="GrayBordered" style="background-color: #EFEFEF; text-align: center; border: 1px solid #B2B2B2; padding: 10px; margin-left: auto; margin-right: auto;" width="50%"  cellpadding="3" border="0">
		<tr>
			<td align="center"><img title="" alt="*" src="UI\Common\Images\PortletImages\Icons\portlet_admin_icon.gif" />&nbsp;<common:globalizedlinkbutton OnClick="glnkAdmin_Click" id="glnkAdmin" runat="server" TextKey="TXT_CS_ADMIN_THIS_PORTLET"></common:globalizedlinkbutton></td>
		</tr>
	</table>
</div>

<table>
<tr>
    <td><asp:Label id="lblID" runat="server" Text="ID #:" /></td>
    <td><asp:TextBox ID="txtID" runat="server"></asp:TextBox></td>
</tr>
<tr>
    <td><asp:Label id="lblID0" runat="server" Text="Last Name (starts with):" /> </td>
    <td><asp:TextBox id="txtLastName" runat="server"></asp:TextBox> (or Business Name)</td>
</tr>
<tr>
    <td><asp:Label id="lblID1" runat="server" Text="First Name (starts with):" /> </td>
    <td><asp:TextBox id="txtFirstName" runat="server"></asp:TextBox></td>
</tr>
</table>
<br />
<asp:Button id="btnSearch" OnClick="btnSearch_Click" runat="server" Text="Search" />
<asp:Button id="btnSelect" OnClick="btnSelect_Click" runat="server" Text="Select" />

<br /> <asp:ListBox Visible = "false" Width="100%" Rows="20" SelectionMode="Single" ID="lbIDNameList" runat="server">
</asp:ListBox><br />
<asp:Panel Visible="false" ID="pnlReport" runat="server">
<asp:Label ID="hdrStaff" Width="45%" runat="server">Reported By</asp:Label>: <asp:TextBox ID="txtStaff" Width="45%" runat="server"></asp:TextBox>
    <br />
<asp:Label ID="hdrProspectName" Width="45%" runat="server">Full Name of Prospect</asp:Label>: <asp:TextBox ID="txtProspectName" Width="45%" runat="server"></asp:TextBox>
    <br />
<fieldset style="padding:0 0 0 0"><table width="100%"><tr><td style="width:45%" align="left"><asp:Label ID="hdrDateContact" Width="100%" runat="server">Date of Contact</asp:Label></td><td style="width:auto; align="left">: <common:DatePicker ID="txtDateContact" runat="server" TimeDisplayFormat="None" DateDisplayFormat="Short" /></td></tr></table></fieldset>
<asp:Label ID="hdrAssignedTo" Width="45%" runat="server">Assigned To</asp:Label>: <asp:TextBox ID="txtAssignedTo" Width="45%" runat="server"></asp:TextBox>
    <br />
<asp:Label ID="hdrTypeContact" Width="45%" runat="server">Type of Contact</asp:Label>: <asp:DropDownList ID="ddlTypeContact" Width="45%" runat="server"></asp:DropDownList>
    <br />
    <asp:Label ID="hdrObjContact" Width="45%" runat="server">Purpose of the Meeting</asp:Label>: <asp:DropDownList ID="ddlObjContact" Width="45%" runat="server"></asp:DropDownList>
    <br />
    <br />
<asp:TextBox ID="hdrSummary" Width="45%" runat="server">Summary of Contact</asp:TextBox>: <asp:TextBox ID="txtSummary" Rows="5" Width="100%" TextMode="MultiLine" runat="server"></asp:TextBox>
<br />
    <br />
<asp:TextBox ID="hdrActionItems" Width="45%" runat="server">Action Items</asp:TextBox>:
<asp:TextBox ID="txtActionItems" Rows="5" Width="100%" TextMode="MultiLine" runat="server">
What: 
When: 
By Whom: </asp:TextBox>
    <br />
    <br />
<asp:TextBox ID="hdrNextSteps" Width="45%" runat="server">Next Steps</asp:TextBox>:
<asp:TextBox ID="txtNextSteps" Rows="5" Width="100%" TextMode="MultiLine" runat="server"></asp:TextBox>
    <br />
    <br />
<asp:TextBox ID="hdrAdditionalInfo" Width="45%" runat="server">New, Additional or Change of Information</asp:TextBox>:
<asp:TextBox ID="txtAdditionalInfo" Rows="5" Width="100%" TextMode="MultiLine" runat="server"></asp:TextBox>
    <br />
    <br />
<asp:TextBox ID="hdrCC" Width="45%" runat="server">CC</asp:TextBox>: <asp:TextBox ID="txtCC" Width="45%" runat="server"></asp:TextBox>
<br />
<br />
<div align="center"><asp:Button id="btnSubmit" OnClick="btnSubmit_Click" OnClientClick="return confirm('Submit Contact Report?');" runat="server" Text="Submit" /></div>
</asp:Panel>
