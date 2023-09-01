using System.ComponentModel;

namespace Hrevert.Common.Enums
{
    public enum TitleStyle
    {
        Horizontal = 1,
        Inline = 2
    }

    public enum SlideBackground : byte
    {
        Parallax = 1,
        Fixed = 2
    }

    public enum BackgroundImageOrColor : byte
    {
        Image = 1,
        Color = 2
    }

    public enum ShowTrendingOrLastest : byte
    {
        ShowTrendingProducts = 1,
        ShowLatestProducts = 2
    }

    public enum SelectedTheme : byte
    {
        DefaultBlue = 1,
        Pink = 2
    }

    public enum FiscalYearFormat : byte
    {
        [DisplayName("FY-20Y1/Y2")] WithPrefix = 1,
        [DisplayName("20Y1/Y2")] WithoutPrefix = 2,
        [DisplayName("FY-20YY")] WithPrefixSingleYear = 3 
    }

    public enum ProductType : byte
    {
        Regular = 1,
        Assembled = 2,
        Kit = 3
    }

    public enum ShippingCalculationType : byte
    {
        Minimum = 1,
        Maximum = 2
    }

    public enum DiscountCalculationType : byte
    {
        Minimum = 1,
        Maximum = 2
    }
    public enum ImageSize : byte
    {
        Small = 1,
        Medium = 2,
        Large = 3
    }

    public enum EntryMethod : byte
    {
        Decimal = 1,
        Fractional = 2,
        Hour = 3,
        Min = 4,
        Day = 5
    }

    public enum ShippingStatus : byte
    {
        Shipped = 1,
        PartiallyShipped = 2, 
        Pending = 3
    }
    public enum DisplayQuantity : byte
    {
        InStockOutStock = 1,
        DisplayExactAvailable = 2
       
    }
    public enum ProductOrCategory : byte
    {
        Product = 1,
        ProductCategory = 2
    }
    public enum TitleType
    {
        None = 1,
        Mr = 2,
        Mrs = 3,
        Miss = 4,
        Shri = 5,
        Smt = 6,
        Shushri = 7,
        Other = 8
    }

    public enum SuffixType
    {
        None = 1,
        Doctor = 2,
        Engineer = 3,
        Enterprenuer = 4,
        Lawyer = 5,
        Businessman = 5,
        Businesswoman = 7,
        Politician = 8,
        Freelancer = 9,
        MR = 10,
        Other = 11
    }

    public enum MediaType : byte
    {
        Photo = 1,
        Video = 2,
        Pdf = 3,
        TextFile = 4,
        Others = 5
    }
    public enum JournalType : byte
    {
        General = 1,
        Purchase = 2,
        Payment = 3,
        Sales = 4,
        Receipt = 5
    }

    public enum DueDateType : byte
    {
        SpecifiedDays = 1,
        EndOfMonth = 2,
        EndofNextMonth = 3,
        None = 4
    }

    public enum DiscountType : byte
    {
        None = 1,
        Percent = 2,
        Fixed = 3,
    }

    public enum PaymentDiscountType : byte
    {
        SpecifiedDays = 1,
        EndOfMonth = 2,
        EndOfNextMonth = 3,
        SameDayNextMonth = 4,
        None = 0
    }

    public enum TermType : byte
    {
        OnAccount = 1,
        CashOnDelivery = 2,
        Prepay = 3
    }

    public enum DueType : byte
    {
        CashOnDelivery = 1,
        PrePaid = 2,
        OnAccount = 3,
    }

    public enum SalesOrderStatus : byte
    {
        SalesQuote = 1,
        SalesOrder = 2,
        SalesInvoice = 3,
        CreditQuote = 4,
        CreditOrder = 5,
        CreditMemo = 6
    }

    public enum AccountLevel : byte
    {
        General = 1,
        Detail = 2
    }
    public enum AccountType : byte
    {
        Asset = 1,
        Liability = 2,
        CapitalEquity = 3,
        Expense = 5,
        Revenue = 4
       
    }
    public enum AccountDetailType
    {
        Bank = 1,
        AccountRecievable = 2,
        AccountPayable = 3,
        Inventory = 4,
        AccruedPurchase = 5,
        OtherIncome = 6,
        OperatingExpenses = 7,
        CostOfGoods = 8,
        OtherExpenses = 9,
        ProvisionForIncomeTax = 10,
        CustomerAdvances = 11,
        Other = 12


    }
    public enum AccountCashFlowType : byte
    {
        Operating = 1,
        Investing = 2,
        Financing = 3,
        None = 4
    }

    public enum PurchaseOrderStatus : byte
    {
        PurchaseOrder = 1,
        PurchaseQuote = 2,
        PurchaseInvoice = 3,
        CreditQuote = 4,
        CreditOrder = 5,
        CreditMemo = 6
    }
    public enum DescriptionType : byte
    {
        ProductDefault = 1,
        Modified = 2
    }

    public enum SalesOrderType : byte
    {
        Order = 1,
        Quote = 2,
        Invoice = 3 
    }

    public enum PurchaseOrderType : byte
    {
        Order = 1,
        Quote = 2,
        Invoice = 3
    }
    public enum TaxCaculationType : byte
    {
        Percent = 1,
        Fixed = 2
    }

    public enum AddressType : byte
    {
        Billing = 1,
        Shipping = 2,
        Contact = 3
    }

    public enum LockType : byte
    {
        MultipleInvalidAttempts = 1,
        PaymentForgery = 2,
        None = 3
    }

    public enum EncryptionType : byte
    {
        None = 1,
        SSL = 2,
        TLS = 3
    }

    public enum ImageType : byte
    {
        FullWidthImage = 1,
        HalfWidthImage = 2,
        QuaterWidthImage = 3
    }
    public enum ThemeSettingImageType : byte
    {
        Logo = 1,
        Favicon = 2,
        FooterImage= 3,
        BrandImage = 4,
        TrendingItemsImage = 5,
        HotThisWeekImage = 6,
        LatestProductsImage = 7,
        BackgroundImage = 8,
        PersonnelImage = 9,
        SlideImage = 11,
        SlideBackgroundImage = 12,
        HotThisWeekSeparator = 13,
        TrendingItemsSeparator = 14,
        LatestProductsSeparator = 15
    }
}
