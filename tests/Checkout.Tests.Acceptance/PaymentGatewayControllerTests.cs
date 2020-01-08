using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Checkout.Web.Models;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Checkout.Tests.Acceptance
{
    [Collection("Container collection")]
    public class PaymentGatewayControllerTests
    {
        private HttpClient _httpClient;

        public PaymentGatewayControllerTests()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            _httpClient = new HttpClient(clientHandler);
        }

        [Fact]
        public async Task GivenASubmitPaymentRequest_WhenValidPayment_ShouldReturn200SuccessCode()
        {
            // given
            var submitPaymentRequest = new SubmitPaymentRequest
            {
                CardDetails = new CardDetails
                {
                    CVV = 123,
                    Currency = "Pound",
                    ExpiryDate = "01/2020",
                    CardNumber = 1234567890123456
                },
                MerchantDetails = new MerchantDetails
                {
                    MerchantId = Guid.NewGuid()
                },
                Amount = 12345
            };
            var jsonRequest = JsonConvert.SerializeObject(submitPaymentRequest);

            // when
            var submitPaymentResponse = await _httpClient.PostAsync("https://localhost:9901/paymentgateway",
                new StringContent(jsonRequest, Encoding.UTF8, "application/json"));

            // then
            submitPaymentResponse.IsSuccessStatusCode.ShouldBeTrue();

            var responseContent = await submitPaymentResponse.Content.ReadAsStringAsync();
            var paymentInformation = JsonConvert.DeserializeObject<SubmitPaymentResponse>(responseContent);

            paymentInformation.PaymentId.ShouldBeOfType<Guid>();
            paymentInformation.PaymentResponseStatus.ShouldBe("Successful");
        }

        [Fact]
        public async Task GivenAPaymentRequest_WhenPaymentExists_ShouldReturn200SuccessCodeAndPaymentInformation()
        {
            // given
            var submitPaymentRequest = new SubmitPaymentRequest
            {
                CardDetails = new CardDetails
                {
                    CVV = 123,
                    Currency = "Pound",
                    ExpiryDate = "01/2020",
                    CardNumber = 1234567890123456
                },
                MerchantDetails = new MerchantDetails
                {
                    MerchantId = Guid.NewGuid()
                },
                Amount = 12345
            };
            
            var jsonRequest = JsonConvert.SerializeObject(submitPaymentRequest);
            var submitPaymentResponse = await _httpClient.PostAsync("https://localhost:9901/paymentgateway",
                new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
            var responseContent = await submitPaymentResponse.Content.ReadAsStringAsync();
            var paymentInformation = JsonConvert.DeserializeObject<SubmitPaymentResponse>(responseContent);

            // when
            var retrievePaymentResponse = await _httpClient.GetAsync($"https://localhost:9901/paymentgateway?paymentId={paymentInformation.PaymentId}&merchantId={submitPaymentRequest.MerchantDetails.MerchantId}");

            // then
            retrievePaymentResponse.IsSuccessStatusCode.ShouldBeTrue();
            var content = await retrievePaymentResponse.Content.ReadAsStringAsync();

            var retrievedPayment = JsonConvert.DeserializeObject<RetrievePaymentResponse>(content);

            retrievedPayment.Amount.ShouldBe(submitPaymentRequest.Amount);
            retrievedPayment.CVV.ShouldBe("***");
            retrievedPayment.CardNumber.ShouldBe("1234********3456");
            retrievedPayment.ExpiryDate.ShouldBe(submitPaymentRequest.CardDetails.ExpiryDate);
            retrievedPayment.Currency.ShouldBe(submitPaymentRequest.CardDetails.Currency);
            retrievedPayment.PaymentResponseStatus.ShouldBe("Successful");
        }

        [Fact]
        public async Task GivenAPaymentRequest_WhenPaymenDoesNotExist_ShouldReturn404()
        {
            // given + when
            var retrievePaymentResponse = await _httpClient.GetAsync($"https://localhost:9901/paymentgateway?paymentId={Guid.NewGuid()}&merchantId={Guid.NewGuid()}");

            // then
            retrievePaymentResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenASubmitPaymentRequest_WhenPaymentUnsuccessful_ShouldReturn400()
        {
            // given
            var submitPaymentRequest = new SubmitPaymentRequest
            {
                CardDetails = new CardDetails
                {
                    CVV = 123,
                    Currency = "Pound",
                    ExpiryDate = "01/2020",
                    CardNumber = 1234567890123456
                },
                MerchantDetails = new MerchantDetails
                {
                    MerchantId = Guid.NewGuid()
                },
                Amount = -1000
            };
            var jsonRequest = JsonConvert.SerializeObject(submitPaymentRequest);

            // when
            var submitPaymentResponse = await _httpClient.PostAsync("https://localhost:9901/paymentgateway",
                new StringContent(jsonRequest, Encoding.UTF8, "application/json"));

            // then
            submitPaymentResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}

