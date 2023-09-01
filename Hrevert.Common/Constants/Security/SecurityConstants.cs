namespace Hrevert.Common.Constants.Security
{
    public class SecurityConstants
    {
        public static class SecurityControllerConstants
        {
            public static string SecurityExists = "Security with the same code already exists";
        }
        public static class SecurityGroupControllerConstants
        {
            public static string SecurityGroupExists = "Security group with the same name already exists";
        }
        public static class SecurityGroupMemberControllerConstants
        {
            public static string MemberAlreadyExists = "Member already exists in this security group";
        }
        public static class SecurityRightControllerConstants
        {
            public static string AlreadyHasRights = "Members of this security group already has these rights";
        }
        public static class SecurityQueryProcessorConstants
        {
            public static string SecurityNotFound = "Security not found";
        }
        public static class SecurityGroupQueryProcessorConstants
        {
            public static string SecurityGroupNotFound = "Security group not found";
        }
        public static class SecurityGroupMemberQueryProcessorConstants
        {
            public static string SecurityGroupMemberNotFound = "Security group member not found";
        }
        public static class SecurityRightQueryProcessorConstants
        {
            public static string SecurityRightNotFound = "Security right not found";
        }
    }
}
