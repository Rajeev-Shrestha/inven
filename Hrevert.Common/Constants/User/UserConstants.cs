namespace Hrevert.Common.Constants.User
{
    public class UserConstants
    {
        public static class UserControllerConstants
        {
            public const string UserNotFound = "Application user not found";
            public const string UserAlreadyExists = "Application user with this email already exists";
            public const string ProfileDoesNotBelongToLoggedInUser = "Profile does not belong to logged in application user";
        }

        public static class UserQueryProcessorConstants
        {
            public const string UserNotFound = "Application user not found";
        }
        public static class UserSettingQueryProcessorConstants
        {
            public const string UserSettingNotFound = "User setting not found";
        }
    }
}
