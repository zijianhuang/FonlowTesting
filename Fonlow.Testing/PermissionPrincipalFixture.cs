using System;
using System.Security.Principal;

namespace Fonlow.Testing
{
    /// <summary>
    /// Create scurity principal based on role, to test service implementations decorated with PrincipalPermissionAttribute with Role. 
    /// Client codes will be allowed to run the service implementation without needing authentication while authorization with a proper role will be sufficient.
    /// </summary>
    public class PermissionPrincipalFixture
    {
        /// <summary>
        /// Used by derived class to be used by XUnit.IClassFixture which requires a default constructor.
        /// </summary>
        /// <param name="roleName"></param>
        protected PermissionPrincipalFixture(string roleName)
        {
            SetPrincipal(roleName);
        }

        /// <summary>
        /// Give permission in every test. Used in constructor.
        /// </summary>
        /// <param name="roleName"></param>
        public static void SetPrincipal(string roleName)
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            GenericPrincipal principal = new GenericPrincipal(System.Threading.Thread.CurrentPrincipal.Identity, new string[] { roleName });
            System.Threading.Thread.CurrentPrincipal = principal;
        }
    }
}
