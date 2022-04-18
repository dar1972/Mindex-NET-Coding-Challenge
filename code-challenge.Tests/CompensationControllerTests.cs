using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using code_challenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTest
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            Compensation compensation = new Compensation()
            {
                EffectiveDate = DateTime.Now,
                Salary = 79000.00M,
                EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f"
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(compensation.EmployeeId, newCompensation.EmployeeId);
            Assert.IsNotNull(newCompensation.CompensationId);
        }


        [TestMethod]
        public void CreateCompensation_ForSameEmployee_Returns_Created()
        {
            // Arrange
            Compensation compensation = new Compensation()
            {
                EffectiveDate = DateTime.Now.AddDays(-365).AddHours(4),
                Salary = 110000.00M,
                EmployeeId = "1b7839309-3348-463b-a7e3-5de1c168beb3"
            };

            // Arrange
            Compensation promotionCompensation = new Compensation()
            {
                EffectiveDate = DateTime.Now,
                Salary = 120000.00M,
                EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3"
            };


            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var response = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json")).Result;

            // Assert #1
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(compensation.EmployeeId, newCompensation.EmployeeId);
            Assert.IsNotNull(newCompensation.CompensationId);


            // Create a promotion
            var promotionRequestContent = new JsonSerialization().ToJson(promotionCompensation);

            var promotionResponse = _httpClient.PostAsync("api/compensation",
           new StringContent(promotionRequestContent, Encoding.UTF8, "application/json")).Result;


            // Assert the promotion Results
            Assert.AreEqual(HttpStatusCode.Created, promotionResponse.StatusCode);
            var secondCompensation = promotionResponse.DeserializeContent<Compensation>();
            Assert.AreEqual(promotionCompensation.Salary, secondCompensation.Salary);
            Assert.AreEqual(promotionCompensation.EffectiveDate, secondCompensation.EffectiveDate);
            Assert.AreEqual(promotionCompensation.EmployeeId, secondCompensation.EmployeeId);
            Assert.IsNotNull(secondCompensation.CompensationId);

            GetCompensation_ForSameEmployee_Returns_ProperComensation(promotionCompensation);

        }

        public void GetCompensation_ForSameEmployee_Returns_ProperComensation(Compensation compensation)
        {
            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{compensation.EmployeeId}");
            var getReponse = getRequestTask.Result;

            // Assert that we get the most recent one
            Assert.AreEqual(HttpStatusCode.OK, getReponse.StatusCode);
            var getCompensation = getReponse.DeserializeContent<Compensation>();
            Assert.AreEqual(compensation.EffectiveDate, getCompensation.EffectiveDate);
            Assert.AreEqual(compensation.Salary, getCompensation.Salary);

        }


        /// <summary>
        /// Tests if the salaries are put with the current salary first and the older salary later you still get the proper salaries when doing creates and
        /// </summary>
        [TestMethod]
        public void CreateCompensation_PutSalaryOutOfOrder_Returns_Created()
        {
            // Arrange
            Compensation compensation = new Compensation()
            {
                EffectiveDate = DateTime.Now.AddDays(-365),
                Salary = 70000.00M,
                EmployeeId = "45ADC30A0-F7AD-4DF5-B04S-53599FE39609"
            };

            // Arrange
            Compensation promotionCompensation = new Compensation()
            {
                EffectiveDate = DateTime.Now,
                Salary = 100000.00M,
                EmployeeId = "45ADC30A0-F7AD-4DF5-B04S-53599FE39609"
            };

            var promotionRequestContent = new JsonSerialization().ToJson(promotionCompensation);

            var promotionResponse = _httpClient.PostAsync("api/compensation",
           new StringContent(promotionRequestContent, Encoding.UTF8, "application/json")).Result;


            // Assert #1
            Assert.AreEqual(HttpStatusCode.Created, promotionResponse.StatusCode);
            var secondCompensation = promotionResponse.DeserializeContent<Compensation>();
            Assert.AreEqual(promotionCompensation.Salary, secondCompensation.Salary);
            Assert.AreEqual(promotionCompensation.EffectiveDate, secondCompensation.EffectiveDate);
            Assert.AreEqual(promotionCompensation.EmployeeId, secondCompensation.EmployeeId);
            Assert.IsNotNull(secondCompensation.CompensationId);


            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var response = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json")).Result;

            // Assert #2
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(compensation.EmployeeId, newCompensation.EmployeeId);
            Assert.IsNotNull(newCompensation.CompensationId);

            GetCompensation_ForSameEmployee_Returns_ProperComensation(promotionCompensation);
        }

        [TestMethod]
        public void GetCompensationById_Returns_FailBecauseOfSpaces()
        {
            // Arrange
            var EmployeeId = "D235131M- 0093-n585-a7e3-5de1c04504eb3";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{EmployeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }

    }
}