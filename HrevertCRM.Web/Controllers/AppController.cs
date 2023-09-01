using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HrevertCRM.Web.Controllers
{
    [Authorize]

    public class AppController : Controller
    {
        // GET: /<controller>/
       

        public IActionResult Index()
        {
            return View();
        }
     
        public IActionResult Account()
        {
            return View();
        }

        public IActionResult Product()
        {
            return View();
        }
        public IActionResult PricingRule()
        {
            return View();
        }
        public IActionResult User()
        {
            return View();
        }
        public IActionResult RoleAssign()
        {
            return View();
        }
        public IActionResult SecurityRight()
        {
            return View();
        }
        public IActionResult FiscalYear()
        {
            return View();
        }

        public IActionResult Customer()
        {
            return View();
        }

        public IActionResult CompanySetup()
        {
            return View();
        }
        public IActionResult SalesOrder()
        {
            return View();
        }
        public IActionResult ContactManager()
        {
            return View();
        }

        public IActionResult PurchaseOrder()
        {
            return View();
        }

        public IActionResult ProductCategory()
        {
            return View();
        }
        public IActionResult Vendor()
        {
            return View();
        }

        public IActionResult PaymentMethod()
        {
            return View();
        }
        public IActionResult DeliveryMethod()
        {
            return View();
        }

        public IActionResult PaymentTerm()
        {
            return View();
        }

        public IActionResult JournalMaster()
        {
            return View();
        }

        public IActionResult EmailSetting()
        {
            return View();
        }

        public IActionResult EcommerceSetting()
        {
            return View();
        }

        public IActionResult DiscountSetting()
        {
            return View();
        }
       
        public IActionResult ZoneSetting()
        {
            return View();
        }

        public IActionResult ItemMeasure()
        {
            return View();
        }

        public IActionResult MeasureUnit()
        {
            return View();
        }

        public IActionResult DeliveryRate()
        {
            return View();
        }

        public IActionResult CompanywebSetting()
        {
            return View();
        }

        public IActionResult FeaturedItem()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Sales()
        {
            return View();
        }
       
        public IActionResult Purchase()
        {
            return View();
        }

        public IActionResult Setup()
        {
            return View();
        }
        public IActionResult TaskManager()
        {
            return View();
        }

        public IActionResult Retailer()
        {
            return View();
        }

        public IActionResult DatePickerDemo()
        {
            return View();
        }
        public IActionResult SalesOpportunities()
        {
            return View();
        }
        public IActionResult SalesOpportunitySetting()
        {
            return View();
        }

        [Route("ResetPassword/{param?}")]
        public IActionResult ResetPassword(string param="")
        {
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        public IActionResult TaxSetting()
        {
            return View();
        }

        public IActionResult ThemeSetting()
        {
            return View();
        }
    }
}
