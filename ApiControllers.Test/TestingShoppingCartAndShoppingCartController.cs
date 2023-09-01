using HrevertCRM.Data.ViewModels;
using HrevertCRM.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ApiControllers.Test
{
    public class TestingShoppingCartAndShoppingCartController
    {

        private readonly ApiConfigurationService _service = new ApiConfigurationService();

        [Theory]
        [InlineData(1)]
        public async void GetAllShoppingCart(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/shoppingcart/getallShoppingCarts?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllActiveShoppingCart(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/ShoppingCart/getactiveShoppingCarts?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void GetAllDeletedShoppingCart(int test)
        {
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/shoppingcart/getdeletedShoppingCarts?pageNumber=1&pageSize=10");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count >= 0);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteShoppingCartById(int test)
        {
            int id = 2;
            var response = await Setup.GetClient(_service).DeleteAsync(_service.ServiceEndPoint + "/api/shoppingcart/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content == "");
        }


        [Theory]
        [InlineData(1)]
        public async void GetCartById(int test)
        {
            int id = 1;
            int customerId = 1;

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/shoppingcart/getcart/" + id + "/" + customerId);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            var content = response.Content.ReadAsStringAsync().Result;

            Assert.True(content.ToList().Count > 0);


        }

        [Theory]
        [InlineData(1)]
        public async void GetShoppingCartById(int test)
        {
            int id = 1;
            int customerId = 1;
            Guid guid = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/shoppingcart/getshoppingcart/" + id+"/"+customerId+"/"+ guid);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
        
            var content = response.Content.ReadAsStringAsync().Result;

           Assert.True(content.ToList().Count>0);
            

        }

        [Theory]
        [InlineData(1)]
        public async void ActivateShoppingCart(int test)
        {
            int id = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/shoppingcart/activateShoppingCart/" + id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedShoopingCart = JsonConvert.DeserializeObject<ShoppingCartViewModel>(content);
            Assert.True(returnedShoopingCart.Id > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void CreateShoppingCartShouldReturnId(int test)
        {
            List<ShoppingCartDetailViewModel> shoppingDetails = new List<ShoppingCartDetailViewModel>();
            shoppingDetails.Clear();

            var level = new ShoppingCartViewModel
            {
                HostIp = "192.168.2.100",
                Amount = 1200,
                PaymentTermId = 1,
                DeliveryMethodId = 1,
                WebActive = false,
                Active = true,
                CompanyId = 1,
                BillingAddressId = 1,
                ShippingAddressId = 1,
                CustomerId = 1,
                ShoppingCartDetails = shoppingDetails
            };

            var json = JsonConvert.SerializeObject(level);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/shoppingcart/createshoppingcart", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedShoopingCarts = JsonConvert.DeserializeObject<ShoppingCartViewModel>(content);
            Assert.True(returnedShoopingCarts.Id > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateShoppingCartShouldReturnId(int test)
        {
            int id = 7;
            int customerId = 1;
            Guid guid = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/shoppingcart/getshoppingcart/" + id+"/"+customerId+"/"+guid);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;

            var shoppingCart = JsonConvert.DeserializeObject<ShoppingCartViewModel>(content);

            List<ShoppingCartDetailViewModel> shoppingCartDetails = new List<ShoppingCartDetailViewModel>();

            shoppingCartDetails.Clear();
            
            shoppingCart.HostIp = "192.177.1.1";
            shoppingCart.Active = true;
            shoppingCart.CustomerId = 1;
            shoppingCart.IsCheckedOut = true;
            shoppingCart.BillingAddressId = 1;
            shoppingCart.CompanyId = 1;
            shoppingCart.PaymentTermId = 1;
            shoppingCart.DeliveryMethodId = 1;
            shoppingCart.WebActive = true;
            shoppingCart.Amount = 1000;
            shoppingCart.ShoppingCartDetails = shoppingCartDetails;

            var json = JsonConvert.SerializeObject(shoppingCart);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await Setup.GetClient(_service).PutAsync(_service.ServiceEndPoint + "/api/shoppingcart/updateshoppingcart", stringContent);
            if (!responses.IsSuccessStatusCode)
            {
                Console.WriteLine(responses.StatusCode);
            }
            var contents = responses.Content.ReadAsStringAsync().Result;
            var returnedShoopingCarts = JsonConvert.DeserializeObject<ShoppingCartViewModel>(contents);
            Assert.True(returnedShoopingCarts.Id > 0);
        }

        [Theory]
        [InlineData(1)]
        public async void AddToCartShouldReturnId(int test)
        {
            List<string> imageUrls = new List<string>();
            imageUrls.Clear();

            var level = new ShoppingCartDetailViewModel
            {
                ShoppingCartId = 1,
                ProductId = 1,
                ProductCost = 200,
                Quantity = 10,
                Discount = 10,
                TaxAmount = 230,
                ShoppingDateTime = DateTime.Parse("2017-1-5"),
                ProductName = "Samsung X01",
                Active = true,
                WebActive = false,
                CompanyId = 1,
                ProductImageUrls = imageUrls
            };

            var json = JsonConvert.SerializeObject(level);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/shoppingcart/addtocart", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            var returnedShoopingCartDetails = JsonConvert.DeserializeObject<ShoppingCartDetailViewModel>(content);
            Assert.True(returnedShoopingCartDetails.Id > 0);

        }

        [Theory]
        [InlineData(1)]
        public async void ConvertCartToOrderShouldReturnId(int test)
        {
            var address = new AddressViewModel
            {
                Title = Hrevert.Common.Enums.TitleType.Mr,
                FirstName = "Ramesh Suresh",
                MiddleName = "Prasad",
                LastName = "Kushwaha",
                Suffix = Hrevert.Common.Enums.SuffixType.Businessman,
                AddressLine1 = "Birgunj-19",
                AddressLine2 = "Birgunj-3",
                City = "Birgunj",
                State = "Central",
                ZipCode = "44013",
                CountryId = 1,
                Telephone = "05152230",
                MobilePhone = "9845886831",
                Fax = "test121",
                Email = "shippingaddress@gmail.com",
                Website = "http://wwww.abc.com",
                AddressType = Hrevert.Common.Enums.AddressType.Shipping,
                CustomerId = 5,
                IsDefault = true,
                Active = true,
                CompanyId = 1
            };

            var level = new ConvertCartToOrderViewModel()
            {
               CartId = 1,
               CustomerId = 14,
               BillingAddressId = 1,
               PaymentTermId = 1,
               PaymentMethodId = 1,
               DeliveryMethodId = 1,
               ShippingAddressViewModel = address,
               //FileBase64 = "abscrdeee"
            };

            var json = JsonConvert.SerializeObject(level);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/shoppingcart/ConvertCartToOrder", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            //var returnedShoopingCartDetails = JsonConvert.DeserializeObject<ConvertCartToOrderViewModel>(content);
            Assert.True(content.ToList().Count> 0);
        }

        [Theory]
        [InlineData(1)]
        public async void SendEmailToCustomer(int test)
        {
            var sendOrder = new SendOrderToCustomerViaEmailViewModel()
            {
                BillingAddressId = 1,
                FileBase64 = "dssjndskjh hrjeh rhrjkkhjh kjvbkbjh"
            };
            var json = JsonConvert.SerializeObject(sendOrder);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Setup.GetClient(_service).PostAsync(_service.ServiceEndPoint + "/api/shoppingcart/SendEmailToCustomer", stringContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            var content = response.Content.ReadAsStringAsync().Result;
            Assert.True(content=="");
        }


        [Theory]
        [InlineData(1)]
        public async void RemoveProdcutFromCart(int test)
        {
            int id = 1;

            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/shoppingcart/removeproductfromcart/" + id );

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            var content = response.Content.ReadAsStringAsync().Result;
            var status = JObject.Parse(content);
            Assert.True(status.HasValues.Equals(true));

        }

        [Theory]
        [InlineData(1)]
        public async void UpdateShoppingCart(int test)
        {
            int cartId = 1;
            int customerId = 1;
            var response = await Setup.GetClient(_service).GetAsync(_service.ServiceEndPoint + "/api/shoppingcart/updatecartwithcustomerdetails/" + cartId+"/"+customerId);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            var content = response.Content.ReadAsStringAsync().Result;
            var status = JObject.Parse(content);
            Assert.True(status.HasValues.Equals(true));

        }


    }
}
