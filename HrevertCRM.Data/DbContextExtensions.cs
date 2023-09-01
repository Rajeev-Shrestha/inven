using System;
using System.Collections.Generic;
using HrevertCRM.Data.QueryProcessors;
using HrevertCRM.Entities;

namespace HrevertCRM.Data
{
    //TODO: Need To Be Deleted, It has no use now since Arjun Dai has modified the seeding process
    public class DbContextExtensions
    {
        public void SeedingTrivialDatas(IServiceProvider serviceProvider, 
            ApplicationUser adminUser, Company newCompany, ApplicationUser sUser, ApplicationUser pUser)
        {
            //Here We are initializing all the query processors that are required in this Seeding method
            var productcategoriesQueryProcessor = (ProductCategoryQueryProcessor)serviceProvider.GetService(typeof(IProductCategoryQueryProcessor));
            var productQueryProcessor = (ProductQueryProcessor)serviceProvider.GetService(typeof(IProductQueryProcessor));
            var productInCategoryprocesor = (ProductInCategoryQueryProcessor)serviceProvider.GetService(typeof(IProductInCategoryQueryProcessor));
            var securityGroupQueryProcessor = (SecurityGroupQueryProcessor)serviceProvider.GetService(typeof(ISecurityGroupQueryProcessor));
            var securityGroupMemberQueryProcessor = (SecurityGroupMemberQueryProcessor)serviceProvider.GetService(typeof(ISecurityGroupMemberQueryProcessor));
            //Initialization End


            ProductCategory category1 = new ProductCategory()
            {
                Name = "Laptops"
            };
            category1 = productcategoriesQueryProcessor.CheckIfCategoryExistsOrSave(category1);

            ProductCategory category2 = new ProductCategory()
            {
                Name = "Home Appliances",
                ParentId = category1.Id
            };
            category2 = productcategoriesQueryProcessor.CheckIfCategoryExistsOrSave(category2);

            List<ProductCategory> productCategories = new List<ProductCategory>()
            {
                new ProductCategory() {Name = "Books", ParentId = category2.Id},
                new ProductCategory() {Name = "Accesories", ParentId = category1.Id}
            };
            productcategoriesQueryProcessor.CheckIfCategoryExistsOrSave(productCategories[0]);
            productcategoriesQueryProcessor.CheckIfCategoryExistsOrSave(productCategories[1]);

            Product newProduct = new Product()
            {
                Code = "DELL-201",
                Name = "DELL Inspiron 16 9000 series",
                Active = true,
                CompanyId = newCompany.Id,
                UnitPrice = 58000,
                QuantityOnHand = 10,
                QuantityOnOrder = 15,
                ShortDescription = "N/A",
                LongDescription = "N/A",
                CreatedBy = adminUser.Id,
                LastUpdatedBy = adminUser.Id
            };
            newProduct = productQueryProcessor.CheckIfProductExistsOrSave(newProduct);

            List<ProductInCategory> productInCategories = new List<ProductInCategory>()
            {
                new ProductInCategory() {CategoryId = category2.Id, ProductId = newProduct.Id},
                new ProductInCategory() {CategoryId = category1.Id, ProductId = newProduct.Id}
            };
            productInCategoryprocesor.CheckIfProductInCategoryExistsOrSave(productInCategories[0]);
            productInCategoryprocesor.CheckIfProductInCategoryExistsOrSave(productInCategories[1]);


            List <SecurityGroup> moreGroups= new List<SecurityGroup>
            {
                new SecurityGroup() {GroupName = "Admins", GroupDescription = "This is the admins group"},
                new SecurityGroup() {GroupName = "Sales", GroupDescription = "This is the group of salespersons"},
                new SecurityGroup() {GroupName = "Purchases", GroupDescription = "This is the group of Purchase Managers"}
            };
            var securityGroup1 = securityGroupQueryProcessor.CheckIfSecurityGroupExistsOrSave(moreGroups[0]);
            var securityGroup2 = securityGroupQueryProcessor.CheckIfSecurityGroupExistsOrSave(moreGroups[1]);
            var securityGroup3 = securityGroupQueryProcessor.CheckIfSecurityGroupExistsOrSave(moreGroups[2]);


            List<SecurityGroupMember> moreGroupMembers = new List<SecurityGroupMember>()
            {
                new SecurityGroupMember() {MemberId = adminUser.Id, SecurityGroupId = securityGroup1.Id},
                new SecurityGroupMember() {MemberId = sUser.Id, SecurityGroupId = securityGroup2.Id},
                new SecurityGroupMember() {MemberId = pUser.Id, SecurityGroupId = securityGroup3.Id}
            };
            securityGroupMemberQueryProcessor.CheckIfSecurityGroupMemberExistsOrSave(moreGroupMembers[0]);
            securityGroupMemberQueryProcessor.CheckIfSecurityGroupMemberExistsOrSave(moreGroupMembers[1]);
            securityGroupMemberQueryProcessor.CheckIfSecurityGroupMemberExistsOrSave(moreGroupMembers[2]);
        }
    }
}
