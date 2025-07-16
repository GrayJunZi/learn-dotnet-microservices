using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using AuthLibrary.Attributes;
using AuthLibrary.Constants.Authentication;
using Microsoft.AspNetCore.Mvc;
using ResponseWrapperLibrary.Models.Requests.Products;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : BaseController
    {
        [HttpPost("add")]
        [MustHavePermission(AppService.Product, AppFeature.Products, AppAction.Create)]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductRequest createProduct)
        {
            var response = await Sender.Send(new CreateProductCommand { CreateProduct = createProduct });
            if (!response.IsSuccessful)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("update")]
        [MustHavePermission(AppService.Product, AppFeature.Products, AppAction.Update)]
        public async Task<IActionResult> UpdateProductAsync([FromBody] UpdateProductRequest updateProduct)
        {
            var response = await Sender.Send(new UpdateProductCommand { UpdateProduct = updateProduct });
            if (!response.IsSuccessful)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [MustHavePermission(AppService.Product, AppFeature.Products, AppAction.Delete)]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var response = await Sender.Send(new DeleteProductCommand { ProductId = id });
            if (!response.IsSuccessful)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("{id}")]
        [MustHavePermission(AppService.Product, AppFeature.Products, AppAction.Read)]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var response = await Sender.Send(new GetProductByIdQuery { ProductId = id });
            if (!response.IsSuccessful)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("all")]
        [MustHavePermission(AppService.Product, AppFeature.Products, AppAction.Read)]
        public async Task<IActionResult> GetProductsAsync()
        {
            var response = await Sender.Send(new GetProductsQuery());
            if (!response.IsSuccessful)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
