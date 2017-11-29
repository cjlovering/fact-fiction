using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FactOrFictionCommon.Models
{
    public class ApplicationRole : IdentityRole
    {
        public const string ADMINISTRATOR = "Administrator";
        public const string USER = "Register User";
    }
}
