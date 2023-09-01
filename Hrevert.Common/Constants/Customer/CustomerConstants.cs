namespace Hrevert.Common.Constants.Customer
{
    public class CustomerConstants
    {
        public static class CustomerControllerConstants
        {
            public const string CustomerAlreadyExists = "Customer with the same code already exists";
            public const string AllowGuestDisabled = "Customer allow guest is disabled";
            public const string CustomerAlreadyExistsWithThisEmail = "Customer with the same email already exists";
        }

        public static class CustomerQueryProcessorConstants
        {
            public const string CustomerNotFound = "Customer not found";
        }
        public static class CustomerLevelControllerConstants
        {
            public const string CustomerLevelAlreadyExists = "Customer level with the same name already exists";
        }
        public static class CustomerContactGroupControllerConstants
        {
            public const string ContactGroupAlreadyExists = "Contact group with the same name already exists";
        }
        public static class CustomerInContactGroupControllerConstants
        {
            public const string CustomerAlreadyExistsInContactGroup = "Customer already exists in this contact group";
        }
        public static class CustomerInContactGroupQueryProcessorConstants
        {
            public const string CustomerInContactGroupNotFound = "Customer in contact group not found";
        }

        public static class DeliveryMethodControllerConstants
        {
            public const string DeliveryMethodAlreadyExists = "Delivery method with the same code already exists";
        }

        public static class PaymentMethodControllerConstants
        {
            public const string PaymentMethodNameAlreadyExists = "Payment method with the same name already exists";
            public const string PaymentMethodCodeAlreadyExists = "Payment method with the same code already exists";
        }

        public static class PaymentTermControllerConstants
        {
            public const string PaymentTermAlreadyExists = "Payment term with the same code already exists";
        }


        public static class CustomerLevelQueryProcessorConstants
        {
            public const string CustomerLevelNotFound = "Customer level not found";

        }
        public static class CustomerContactGroupQueryProcessorConstants
        {
            public const string ContactGroupNotFound = "Contact group not found";
        }

        public static class DeliveryMethodQueryProcessorConstants
        {
            public const string DeliveryMethodNotFound = "Delivery method not found";
        }

        public static class PaymentMethodQueryProcessorConstants
        {
            public const string PaymentMethodNotFound = "Payment method not found";
        }

        public static class PaymentTermQueryProcessorConstants
        {
            public const string PaymentTermNotFound = "Payment term not found";
        }
        public static class CustomerLoginEventQueryProcessorConstants
        {
            public const string InvalidLoginAttempt = "Invalid Login Attempt, this email does not exist in our database";
            public const string AccountLocked = "Your account has been locked. Please try again after ";
            public const string CredentialsDidntMatch = "Username/Password did not match. Enter your credentials again!";
            public const string LoginSuccessful = "Login Successfull";
        }
    }
}
