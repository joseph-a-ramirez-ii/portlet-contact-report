using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jenzabar.Portal.Framework.Web.UI;
using Jenzabar.Portal.Framework.Facade;

namespace CUS.ICS.ContactReport
{
    public partial class Default_View : PortletViewBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            IPortalUserFacade facade = (this.ParentPortlet as ContactReport).MyPortalUserFacade;

            if (IsFirstLoad)
            {
                txtStaff.Text = Jenzabar.Portal.Framework.PortalUser.Current.DisplayName;
                String[] liList = new String[] {"Visit","Phone Call","E-Mail","Letter","Other(invitation)"};

                for (int i = 0; i < liList.Length; i++)
                {
                    ddlTypeContact.Items.Add(liList[i]);
                }

                String[] liList2 = new String[] { "Discovery", "Cultivation", "Solicitation", "Stewardship", "Reporting (Corp/Foundation only)" };

                for (int i = 0; i < liList2.Length; i++)
                {
                    ddlObjContact.Items.Add(liList2[i]);
                }
                
            }

            if (ParentPortlet.AccessCheck("CANADMIN"))
            {
                divAdminLink.Visible = true;
            }

        }

        // Find users
        protected void btnSearch_Click(Object sender, EventArgs e)
        {
            lbIDNameList.Items.Clear();

            if (txtID.Text == String.Empty && txtLastName.Text == String.Empty && txtFirstName.Text == String.Empty)
            {
                ParentPortlet.ShowFeedback(FeedbackType.Error, "Enter in search criteria in at least one field");
                return;
            }

            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JenzabarConnectionString"].ConnectionString))
            {
                conn.Open();
                System.Data.SqlClient.SqlCommand sqlcmdSelect = conn.CreateCommand();

                sqlcmdSelect.CommandText = "SELECT a.ID_NUM,Last_Name,First_Name, ' ' + Middle_name as 'Middle_name',convert(varchar,Birth_dte,101) as Birth_dte,ADDR_LINE_1, c.CITY, c.STATE, c.ZIP From Name_master a left outer join BIOGRAPH_MASTER b on a.ID_NUM=b.ID_NUM left outer join ADDRESS_MASTER c on a.ID_NUM=c.ID_NUM AND c.ADDR_CDE='*LHP' WHERE"
                    + " (LAST_NAME IS NULL OR LAST_NAME like" + " '" + txtLastName.Text.Replace("'", "''").Trim() + "%')"
                    + " AND (FIRST_NAME IS NULL OR First_Name like" + " '" + txtFirstName.Text.Replace("'", "''").Trim() + "%')"
                    + " AND a.ID_NUM like" + " '" + txtID.Text.Replace("'", "''").Trim() + "%'"
                    + " AND (FIRST_NAME IS NOT NULL OR LAST_NAME IS NOT NULL)"
                    + " ORDER BY LAST_NAME, FIRST_NAME";
                using (System.Data.SqlClient.SqlDataReader sqlReader = sqlcmdSelect.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        lbIDNameList.Visible = true;
                        while (sqlReader.Read())
                        {
                            String listItemText = sqlReader["ID_NUM"].ToString()
                                + " || " + sqlReader["Last_Name"].ToString().Trim() + ", "
                                + sqlReader["First_Name"].ToString().Trim()
                                + sqlReader["Middle_name"].ToString().TrimEnd()
                                + " || " + sqlReader["Birth_dte"].ToString()
                                + " || " + sqlReader["ADDR_LINE_1"].ToString()
                                + " " + sqlReader["CITY"].ToString()
                                + " " + sqlReader["STATE"].ToString()
                                + " " + sqlReader["ZIP"].ToString();
                            lbIDNameList.Items.Add(new ListItem(listItemText,sqlReader["ID_NUM"].ToString()));
                        }
                    }
                    else
                    {
                       ParentPortlet.ShowFeedback(FeedbackType.Error,"No results for the specified criteria <br />If prospect is not yet in Jenzabar, you must create him/her through the Jenzabar EX Client");
                    }
                }
            }
        }

        protected void btnSelect_Click(Object sender, EventArgs e)
        {
            if (lbIDNameList.SelectedItem != null)
            {
                if (btnSelect.Text.Equals("Select") && lbIDNameList.SelectedItem.Text.Length > 20)
                {
                    txtID.Text = lbIDNameList.SelectedValue;
                    txtLastName.Text = lbIDNameList.SelectedItem.Text.Substring(12, lbIDNameList.SelectedItem.Text.IndexOf(",") - 12);

                    int firstNameStartIndex = lbIDNameList.SelectedItem.Text.IndexOf(",") + 2;
                    txtFirstName.Text = lbIDNameList.SelectedItem.Text.Substring(firstNameStartIndex, lbIDNameList.SelectedItem.Text.IndexOf(" ||", lbIDNameList.SelectedItem.Text.IndexOf(",")) - firstNameStartIndex);
                    txtProspectName.Text = (txtFirstName.Text + " " + txtLastName.Text).Trim();
                    txtID.Enabled = false;
                    lbIDNameList.Visible = false;
                    btnSearch.Enabled = false;
                    txtLastName.Enabled = false;
                    txtFirstName.Enabled = false;
                    btnSelect.Text = "Unselect";
                    pnlReport.Visible = true;
                }
                else
                {
                    btnSelect.Text = "Select";
                    txtID.Enabled = true;
                    txtFirstName.Enabled = true;
                    txtLastName.Enabled = true;
                    lbIDNameList.Visible = true;
                    btnSearch.Enabled = true;
                    pnlReport.Visible = false;
                }
            }
            else
            {
                ParentPortlet.ShowFeedback(FeedbackType.Error, "No entry selected <br />If prospect is not yet in Jenzabar, you must create him/her through the Jenzabar EX Client");
            }
        }

        protected void glnkAdmin_Click(object sender, EventArgs e)
        {
            this.ParentPortlet.NextScreen("Admin_View");
        }

        //protected void Calendar1_Click(Object sender, EventArgs e)
        //{
        //    txtDateContact.Text = Calendar1.SelectedDate.ToShortDateString();
        //}

        protected void btnSubmit_Click(Object sender, EventArgs e)
        {

            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JenzabarConnectionString"].ConnectionString))
            {
                conn.Open();
                System.Data.SqlClient.SqlCommand sqlcmdInsert = conn.CreateCommand();
                System.Data.SqlTypes.SqlDateTime sql_txtDateContact;

                //DateTime.TryParse(txtDateContact.DateIsValid, out dtContactDateTime);
                if (txtDateContact.DateIsValid && !txtDateContact.DateIsEmpty)
                {
                    try
                    {
                        sql_txtDateContact = new System.Data.SqlTypes.SqlDateTime(txtDateContact.SelectedDate);
                    }
                    catch
                    {
                        ParentPortlet.ShowFeedback(FeedbackType.Error, "Invalid Date of Contact");
                        return;
                    }
                }
                else
                {
                    ParentPortlet.ShowFeedback(FeedbackType.Error, "Invalid Date of Contact");
                    return;
                }

                String shortUserName;
                String itemDesc =  hdrStaff.Text + " : " + txtStaff.Text + '\r' + '\n'
                    + hdrDateContact.Text + " : " + txtDateContact.SelectedDate.ToShortDateString().Replace("'", "''") + '\r' + '\n'
                    + "Development Action : Contact Report" + '\r' + '\n'
                    + hdrProspectName.Text + " : " + txtProspectName.Text + '\r' + '\n'
                    + hdrAssignedTo.Text + " : " + txtAssignedTo.Text + '\r' + '\n'
                    + hdrTypeContact.Text + " : " + ddlTypeContact.SelectedValue + '\r' + '\n'
                    + hdrObjContact.Text + " : " + ddlObjContact.SelectedValue + '\r' + '\n'
                    + hdrSummary.Text + " : " + '\r' + '\n' + txtSummary.Text + '\r' + '\n'
                    + hdrActionItems.Text + " : " + '\r' + '\n' + txtActionItems.Text + '\r' + '\n'
                    + hdrNextSteps.Text + " : " + '\r' + '\n' + txtNextSteps.Text + '\r' + '\n'
                    + hdrAdditionalInfo.Text + " : " + '\r' + '\n' + txtAdditionalInfo.Text + '\r' + '\n'
                    + hdrCC.Text + " : " + '\r' + '\n' + txtCC.Text;

                if (Jenzabar.Portal.Framework.PortalUser.Current.Username.Length >= 14)
                    shortUserName = Jenzabar.Portal.Framework.PortalUser.Current.Username.Substring(0, 14);
                else
                    shortUserName = Jenzabar.Portal.Framework.PortalUser.Current.Username;
                
                sqlcmdInsert.CommandText = "INSERT INTO ITEMS (ID_NUMBER, GROUP_NUMBER, SUBGROUP_NUMBER, GROUP_SEQUENCE"
                + ", ACTION_CODE, ITEM_TYPE, ITEM_DESCRIPTION"
                + ", ACTIVE_INACTIVE, COMPLETION_CODE, ITEM_DATE"
                + ", MODULE_CODE, DISPLAY_ON_WEB, USER_NAME, JOB_NAME, JOB_TIME) "
                + "VALUES ("
                    + txtID.Text
                    + ", [dbo].[CUS_fn_GetNextNotepadGroupNumber] (" + txtID.Text + ")"
                    + ", 1, 1"
                    + ", 'DCONRP'"
                    + ", 'ACTION'"
                    + ",  '" + itemDesc.Replace("'", "''") + "'"
                    + ", 'A'"
                    + ", 'C'"
                    + ", '" + sql_txtDateContact.ToString().Replace("'","''") + "'"
                    + ", 'DE'"
                    + ", 'Y'"
                    + ", '" + shortUserName + "'"
                    + ", 'JICS_ContactReportPortletTLU'"
                    + ", GETDATE())";

                if(sqlcmdInsert.ExecuteNonQuery() != 0)
                {
                    ParentPortlet.NextScreen("Default");
                    ParentPortlet.ShowFeedback(FeedbackType.Message, "Contact Report Submitted Successfully.");
                }
                else
                {
                    ParentPortlet.ShowFeedback(FeedbackType.Error, "Contact Report was not sent Successfully.");
                }

            }
        }
    }
}