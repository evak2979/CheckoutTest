# Payment Gateway


A RESTful API that handles 3rd party payments. Current iteration allows the receipt and process of a payment, as well as the retrieving of an existing payment.

## Setting up locally

Open a Powershell window in the solution directory and execute the following command:

    docker-compose up

This will setup the required containers and provide run time access to the application. The default ports are 8800 for non-SSL and 8801 for SSL. These can be configured differently in the docker-compose.yml file location in the solution directory.

## Swagger
Once your containers are set up you can access the application's swagger here:

http://localhost:8800/swagger/index.html

## Credentials

At this moment in time no Authentication/Authorization is setup. In order to retrieve a payment however, one must provide the API with both the PaymentId and the MerchantId. This is to offer a small amount of protection, and to ensure that only a Merchant related to a particular payment can access that payment.

Authentication/Authorization suggestions for future reference:

- Incorporate IdentityServer4 and utilize the UseIdentityServer Middleware.
- Use a cloud provider such as AWS and allow them to handle Authentication/Authorization (e.g. API Gateway + Lambda Authorizer)

## Endpoints and parameters:

**Submit a Payment**
A POST Request at:

http://localhost:8800/

Example Request:

    {  
       "merchantDetails":  {  
    "      id":  "bc104267-fea7-48a8-9734-64fd2b9bd42a"  
       },  
    "cardDetails":  {  
       "cardNumber":  1234567890123456,
       "expiryDate":  "01/2023",
       "currency":  "Pound",  
       "cvv":  123  },  
       "amount":  12345  
    
    }
   
   Example Response:

       {  
       "paymentId":  "9bc89d15-d550-4db4-86d7-84254e5c9066",  
       "paymentResponseStatus":  "Successful"  
       }

**Retrieve a Payment**

A GET Request at:

http://localhost:8800/

Example Request:

http://localhost:8800/?merchantId=bc104267-fea7-48a8-9734-64fd2b9bd42a&paymentId=64fd2b9b-fea7-48a8-9734-64fd2b9bd42a

Example Response:

    {  
       "cardNumber":  "1234567890123456",  
       "expiryDate":  "01/1010",  "currency":  
       "Pound",  "cvv":  "123",  
       "paymentResponseStatus":  "Successful",  
       "amount":  12345  
    }

## Logging:

The .NET Core logger is being used. An ErrorHandling Middleware has been added to try and catch errors on request level.  

# Notes
