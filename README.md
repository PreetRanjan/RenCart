# RenCart
An E-Commerce web application with .NET Core and ANgular.
## Description
This is an E-Commerce application made with ASP.NET Core for the backend API and Angular 11 for the Frontend.
* The API is working fully. It uses JWT for Authentication and Authorization purpose. 
* This API project is completed and ready to be consumed from any client with proper request format.
* For testing and simplicity at this moment this supports selling and buying books only. But as it is in development phase it has to be a proper e commerce site with many options.
### The project contains domain classes
* AppUser
* AppUserRole
* Book
* Cart
* CartItem
* Category
* Order
* OrderDetails
* WishList
* WishListItem
### Data Access Layer
It uses repository pattern for data access. Every model contains it's service class implementing an interface which is the abstraction layer for that service.
Also it uses Data Transfer Objects (DTO) for data request and response.
### Database design
This demostrates the database deisgn for the project. This is generated using EFCore Power tools:

![Image of Yaktocat](https://github.com/PreetRanjan/RenCart/blob/master/image_2021-02-24_194310.png)
### It uses Entity Framework Core 3.1 as the Data Access layer.
So it supports multiple database provider.
* Microsoft SQL Server
* SQLite
* Postgres SQL
* MySQL

### It uses Swagger for API documentation.

## Frontend Part
The frontend part of the web version of this application is made with Angular 11. It is not fully completed yet.
Libraries used in the Angular project
* NgxSpinner
* Bootstrap 4.5
* More to add
## Mobile App
It uses Xamarin.Forms for cross platform mobile app development. It is not completed yet.
