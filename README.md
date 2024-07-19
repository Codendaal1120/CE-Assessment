[![unit-tests](https://github.com/Codendaal1120/CE-TEST/actions/workflows/unit-tests.yml/badge.svg)](https://github.com/Codendaal1120/CE-TEST/actions/workflows/unit-tests.yml)

# Technical Assessment

# Requirements

### Application Entry points 
- A .NET console application which can execute the business logic listed below. Write the results of the logic below to the console output. 
-  An ASP.NET application, which can execute the business logic listed below. Implement this using a controller which displays an HTML table with the results. 

### Business logic 
Create the following methods in a shared library: 
-  Fetch all orders with status IN_PROGRESS from the API 
-  From these orders, compile a list of the top 5 products sold (product name, GTIN and total quantity), order these by the total quantity sold in descending order 
-  Pick one of the products from these orders and use the API to set the stock of this product to 25. 

### Testing 
-  A unit test testing the expected outcome of the “top 5” functionality based on dummy input. 
-  You can create more unit tests if that is the way you usually work.

# Implementation
For the implementation I decided to just use 2 services (initially). I could have opted to use MediatR, but decided to just keep it simple.  

I created a very simple ASP.NET MVC site to show the top products sold with a button to update the product's stock to 25 as well as a console application which can be used to get the same functionality.

From my Initial look at the api spec, it seemed like I needed to calls "v2/orders/" to fetch the orders and "v2/Products" to update the product stock. After building the OrdersService and ProductsService, along with the unit tests I found that the stock does in fact not update via the PATCH endpoint. I then implemented the OffersService to call "v2/Offers", which does allow for stock updates. Since I already built and tested the ProductsService, I decided just to keep it in, marking it as obsolete. 

### Running
To run, you need to add the API key to the appsettings.json or user secrets in VS.
