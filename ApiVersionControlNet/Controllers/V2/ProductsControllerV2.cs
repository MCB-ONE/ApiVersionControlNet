using ApiVersionControlNet.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ApiVersionControlNet.Controllers.V2
{

    // Sintaxis con version 
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private const string ApiTestURL = "https://fakestoreapi.com/products";
        private readonly HttpClient _httpClient;

        public ProductsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Mapear la ruta hacia la versión que pertenece
        [MapToApiVersion("2.0")]
        [HttpGet(Name = "GetProductsData")]

        public async Task<IActionResult> GetProductsDataAsync()
        {

            var response = await _httpClient.GetAsync(ApiTestURL);

            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            var productsData = JsonSerializer.Deserialize<List<ProductV2>>(responseString);

            return Ok(productsData);
        }
    }
}
