namespace Hrevert.Common.Constants.Others
{
    public class OtherConstants
    {
        public static class VersionMismatchExceptions
        {
            public const string VersionMismatchException =
                "This entity has already changed it's state from previous version";
        }
        public static class AuthorizationValidationExceptions
        {
            public const string AuthorizationValidationException =
                "Attemt to access secure records which does not belong to this company/user";
        }

        public static class SecurityException
        {
            public const string UserDoesNotHaveRightException =
                "User does not have this right";
        }

    }
}
