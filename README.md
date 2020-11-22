# Cat-Side-Down-Api #

## .NET Core 3.1 Web API ##

* REST architectural design, 
* RESTful web API (CRUD operations through HTTP verbs)
* Clean Code
* SOA (Service Oriented Architecture)
* Dependency Injection

### Technologies used: ###

* .NET Core 3.1
* C#
* Entity Framework Core
* Sqlite
* NSwag
* JwtBearer

> Usage:

An Api which generates Upside Down kitten pictures.

> Use - cases:

Login (Authenticated - Authorized user) can:
* Call the service API (https://cataas.com) and and finally see the upcoming pictured flipped upside down.
* LogOut
* Get his/her user information

Non-Logged in or Authenticated user can:
* Register himself

The Authentication and Authorization is happening throught a generated Jwt Bearer Token.

> A Swagger page is generated when project starts where the user has to:
1) Log in and receive a token. (Error handling for "non existing user" - non correct username or password).
2) Provide the token using the Authorize button with the form : Bearer provided_token
3) Log out (If Logged in).
4) Flip the image (If Logged in).
5) Get user details (If Logged in).
6) Register.
7) Get (Health check Api).
