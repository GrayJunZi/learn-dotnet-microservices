using Application.Features.InventoryItems.Commands;
using Application.Features.InventoryItems.Queries;
using AuthLibrary.Attributes;
using AuthLibrary.Constants.Authentication;
using Microsoft.AspNetCore.Mvc;
using ResponseWrapperLibrary.Models.Requests.Inventories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class InventoryItemsController : BaseController
    {
        [HttpPut("update")]
        [MustHavePermission(AppService.Inventory,AppFeature.InventoryItems,AppAction.Update)]
        public async Task<IActionResult> ChangeItemStatus([FromBody] ChangeInventoryItemStatusRequest changeInventoryItemStatus)
        {
            var response = await Sender.Send(new ChangeInventoryItemStatusCommand { ChangeInventoryItemStatus = changeInventoryItemStatus });
            if (!response.IsSuccessful)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("all")]
        [MustHavePermission(AppService.Inventory, AppFeature.InventoryItems, AppAction.Read)]
        public async Task<IActionResult> GetInventoryItems()
        {
            var response = await Sender.Send(new GetInventoryItemsQuery());
            if (!response.IsSuccessful)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("{id}")]
        [MustHavePermission(AppService.Inventory, AppFeature.InventoryItems, AppAction.Read)]
        public async Task<IActionResult> GetInventoryItemById(int id)
        {
            var response = await Sender.Send(new GetInventoryItemByIdQuery { ItemId = id });
            if (!response.IsSuccessful)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("product/{productId}")]
        [MustHavePermission(AppService.Inventory, AppFeature.InventoryItems, AppAction.Read)]
        public async Task<IActionResult> GetInventoryItemsByProductId(int productId)
        {
            var response = await Sender.Send(new GetInventoryItemsByProductIdQuery { ProductId = productId });
            if (!response.IsSuccessful)
                return BadRequest(response);
            
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [MustHavePermission(AppService.Inventory,AppFeature.InventoryItems,AppAction.Delete)]
        public async Task<IActionResult> DeleteItemById(int id)
        {
            var response = await Sender.Send(new DeleteInventoryItemCommand { ItemId = id });
            if(!response.IsSuccessful)
                return BadRequest(response);
            
            return Ok(response);
        }
    }
}