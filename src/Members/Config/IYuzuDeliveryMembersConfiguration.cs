using System;

#if NETCOREAPP

#else
#endif

namespace YuzuDelivery.Umbraco.Members
{
    public interface IYuzuDeliveryMembersConfig
    {
        string HomeUrl { get; set; }
        string ForgottenPasswordUrl { get; set; }
        string ChangePasswordUrl { get; set; }

        string ForgottenPasswordLabel { get; set; }

        string EmailNotFoundErrorMessage { get; set; }
        string MemberNotFoundErrorMessage { get; set; }

        Action<string, string, string> ForgottenPasswordEmailAction { get; set; }
    }
}