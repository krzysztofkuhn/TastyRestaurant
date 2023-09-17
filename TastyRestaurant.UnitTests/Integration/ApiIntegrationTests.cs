using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TastyRestaurant.WebApi.Contracts.V1;
using TastyRestaurant.WebApi.Contracts.V1.Consts;
using TastyRestaurant.WebApi.Contracts.V1.Requests;

namespace TastyRestaurant.UnitTests.Integration
{
    public class ApiIntegrationTests
    {
        private HttpClient _httpClient;
        public ApiIntegrationTests()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>();
            _httpClient = webApplicationFactory.CreateDefaultClient();
        }

        [Fact]
        public async Task OrderPut_Success_When_Changed_Order_Items_List()
        {
            var orderId = Guid.Parse("dba48868-30c7-4368-bc2d-dd9312a94f62");
            var payload = new UpdateOrderRequest(OrderStatusEnumContract.Created, new[]
            {
                new OrderItemRequest
                {
                    MenuItemId = Guid.Parse("ee8c89d9-77b9-486b-bb18-b5632c9ad55a"),
                    Quantity = 3
                },
                new OrderItemRequest
                {
                    MenuItemId = Guid.Parse("98063fa9-07b6-48e0-9c3e-504e5844ac68"),
                    Quantity = 2
                }
            });
            var stringPayload = JsonConvert.SerializeObject(payload);

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(ApiRoutes.Orders.Update.Replace("{orderId:guid}", orderId.ToString()), httpContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.True(!string.IsNullOrEmpty(result));
        }
    }
}
