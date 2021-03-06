﻿using Trackwane.Framework.Infrastructure.Requests;

namespace Trackwane.AccessControl.Engine.Commands.Users
{
    public class RegisterUser : UserCommand
    {
        public string Email { get; set; }

        public string UserKey { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public string OrganizationKey { get; set; }

        public RegisterUser(string applicationKey, string requesterKey, string organizationKey, string userKey, string displayName, string email, string password) : base(applicationKey, requesterKey)
        {
            OrganizationKey = organizationKey;
            UserKey = userKey;
            DisplayName = displayName;
            Email = email;
            Password = password;
            Role = Role.Standard;
        }
    }
}
