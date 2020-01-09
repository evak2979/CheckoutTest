
# Payment Gateway


A RESTful API that handles 3rd party payments. Current iteration allows the receipt and process of a payment, as well as the retrieving of an existing payment.

# Flow

## Submit Payment

![Submit Payment](https://github.com/evak2979/CheckoutTest/blob/master/CFLow.jpeg?raw=true)

## Retrieve Payment

![enter image description here](https://github.com/evak2979/CheckoutTest/blob/master/API%20Flowchart.jpeg?raw=true)

# Setting up locally

You will need .NET SDK 3.1 installed. The solution was built and compiled using VS 2019.

To run:

Open a Powershell window in the solution directory and execute the following command:

    docker-compose up

This will setup the required containers and provide run time access to the application. The default ports are 8800 for non-SSL and 8801 for SSL. These can be configured differently in the docker-compose.yml file location in the solution directory.

## Swagger
Once your containers are set up you can access the application's swagger here:

http://localhost:8800/swagger/index.html

## Credentials

At this moment in time no Authentication/Authorization is setup. In order to retrieve a payment however, one must provide the API with both the PaymentId and the MerchantId. This is to offer a small amount of protection, and to ensure that only a Merchant related to a particular payment can access that payment.

## Endpoints and parameters:

**Submit a Payment**
A POST Request at:

http://localhost:8800/

Example Request:

    {  
       "merchantDetails":  {  
    "      merchantId":  "bc104267-fea7-48a8-9734-64fd2b9bd42a"  
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

http://localhost:8800/paymentgateway

Example Request:

http://localhost:8800/paymentgateway?merchantId=bc104267-fea7-48a8-9734-64fd2b9bd42a&paymentId=64fd2b9b-fea7-48a8-9734-64fd2b9bd42a

Example Response:

    {  
       "cardNumber":  "1234567890123456",  
       "expiryDate":  "01/1010",  "currency":  
       "Pound",  "cvv":  "123",  
       "paymentResponseStatus":  "Successful",  
       "amount":  12345  
    }

## Logging:

The .NET Core logger is being used. An ErrorHandling Middleware has been added to try and catch errors on request level.  Individual logging takes place through the workflow, kept minimal - to be checked with team for GDPR concerns before including more information.

# Notes

- **Application Metrics**: Kept to a minimum through the RequestTimeTracking middleware. A quick search suggested an open source library that may help improve metrics ([https://www.app-metrics.io/](https://www.app-metrics.io/))
- **Containerization**: The app includes a docker-compose and a docker-file that allow it to be contenarized and deployed/ran on either linux or windows containers.
- **Authentication **: Skipped for now - a suggestion would be to use a cloud provider such as AWS, and API Gateway that allows Authorization through custom lambda functions. Furthermore API keys integrate nicely with a few popular alert software (such as datadog, new relic, etc.)
- **API Client**: Tested with POSTman; The API can serve a variety of clients, mobile, SPA/Web, desktop to name but a few.
- **Build script / CI**: Can deploy and run tests on each environment by using a powershell script and arranging build steps and artifact dependencies on T.C.
- **Encryption **: To begin with, the SSL version of the API should be used. Furthermore we should look into encrypting whatever information we decide to store about the merchant and the payment. We should also take GDPR into account and make sure we do not persist more than needed.
- **Data storage**: LiteDB has been used; this can be changed to any provider we so desire, as the Repository project is isolated, and DW functionality is exposed via an interface.

# Testing
docker-compose up includes running unit and integration tests. Acceptance tests, however, are not included. Acceptance tests launch the app itself in their own containers with ports 9900, 9901. This can either be achieved by testing via VS, or simply running dotnet test checkout.tests.acceptance.dll in the appropriate directory (TODO - move them to the main docker-compose file)
