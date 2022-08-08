﻿using System;

#if NETCOREAPP

#else
#endif

namespace YuzuDelivery.Umbraco.Members
{
    public class YuzuDeliveryMembersConfiguration : IYuzuDeliveryMembersConfig
    {
        public YuzuDeliveryMembersConfiguration()
        {
            ForgottenPasswordLabel = "Forgotten Password?";

            EmailNotFoundErrorMessage = "Email address not found";
            MemberNotFoundErrorMessage = "Member not found";

            ForgottenPasswordEmailAction = (string email, string name, string changePasswordLink) => { };
        }

        public string HomeUrl { get; set; }
        public string ForgottenPasswordUrl { get; set; }
        public string ChangePasswordUrl { get; set; }

        public string ForgottenPasswordLabel { get; set; }

        public string EmailNotFoundErrorMessage { get; set; }
        public string MemberNotFoundErrorMessage { get; set; }

        public Action<string, string, string> ForgottenPasswordEmailAction { get; set; }
    }
}
