create proc  ClearDatabase
as
begin
delete products
delete ProductCategories
delete companies
delete AspNetUsers
end