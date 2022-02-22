using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace LMS
{
    public class UsersRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            string GetUserInformationresponseString = null;
            string response = null;

            var PayrollNm = System.Web.HttpContext.Current.Session["PayrollNo"];

            string req = @"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                                <Body>
                                                    <GetEmployeeHomeData xmlns=""urn:microsoft-dynamics-schemas/codeunit/HRWebPortal"">
                                                        <employeeNo>" + PayrollNm + @"</employeeNo>
                                                    </GetEmployeeHomeData>
                                                </Body>
                                            </Envelope>";
            response = Assest.Utility.CallWebService(req);

            GetUserInformationresponseString = Assest.Utility.GetJSONResponse(response);

            dynamic json = JObject.Parse(GetUserInformationresponseString);

            string Title = json.Title;
            string PayrollNo = json.PayrollNo;
            object[] objarr = new object[] {Title,PayrollNo};
            string[] arr = objarr.Cast<string>().ToArray();

            return arr;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}