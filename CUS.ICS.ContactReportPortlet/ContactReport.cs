using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jenzabar.Portal.Framework.Web.UI;
using Jenzabar.Portal.Framework.Security.Authorization;

namespace CUS.ICS.ContactReport
{
    [PortletOperation(
        "CANADMIN",
        "Can Admin Portlet",
        "Whether a user can admin this portlet or not",
        PortletOperationScope.Global)]

    public class ContactReport : SecuredPortletBase
    {
        public Jenzabar.Portal.Framework.Facade.IPortalUserFacade MyPortalUserFacade { get; set; }

        public ContactReport(Jenzabar.Portal.Framework.Facade.IPortalUserFacade facade)
        {
            MyPortalUserFacade = facade;
        }

        protected override PortletViewBase GetCurrentScreen()
        {
            PortletViewBase screen = null;

            try
            {
                screen = this.LoadPortletView("ICS/ContactReportPortlet/" + this.CurrentPortletScreenName.Trim() + ".ascx");
            }
            catch
            {
                screen = this.LoadPortletView("ICS/ContactReportPortlet/Default_View.ascx");
            }

            return screen;
        }

    }
}
