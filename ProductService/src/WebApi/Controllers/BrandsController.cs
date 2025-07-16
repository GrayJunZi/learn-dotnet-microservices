using Application.Features.Brands.Commands;
using AuthLibrary.Attributes;
using AuthLibrary.Constants.Authentication;
using Microsoft.AspNetCore.Mvc;
using ResponseWrapperLibrary.Models.Requests.Products;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class BrandsController : BaseController
    {
        [HttpPost("add")]
        [MustHavePermission(AppService.Product, AppFeature.Brands, AppAction.Create)]
        public async Task<IActionResult> CreateBrandAsync([FromBody] CreateBrandRequest createBrand)
        {
            var response = await Sender.Send(new CreateBrandCommand { CreateBrand = createBrand });
            if (!response.IsSuccessful)
                return BadRequest(response); 

            return Ok(response);
        }

        [HttpPut("update")]
        [MustHavePermission(AppService.Product, AppFeature.Brands, AppAction.Update)]
        public async Task<IActionResult> UpdateBrandAsync([FromBody] UpdateBrandRequest updateBrand)
        {
            var response = await Sender.Send(new UpdateBrandCommand { UpdateBrand = updateBrand });
            if (!response.IsSuccessful)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
