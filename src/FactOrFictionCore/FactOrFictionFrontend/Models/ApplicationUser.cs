using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FactOrFictionFrontend.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<TextEntry> TextEntries { get; set; }
    }
}
