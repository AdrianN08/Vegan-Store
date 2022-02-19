using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeganStore.Models.Entities;
using VeganStore.Web.API.Data;
using VeganStore.Web.API.Filters;
using VeganStore.Web.API.Repository;

namespace VeganStore.Web.API.Controllers
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {

        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailsController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails()
        {
            var result = await _orderDetailService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int id)
        {
            var orderDetail = await _orderDetailService.GetByIdAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return orderDetail;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return BadRequest();
            }

            var entity = await _orderDetailService.GetByIdAsync(id);
            entity.OrderId = orderDetail.OrderId;
            entity.Order = orderDetail.Order;
            entity.ProductId = orderDetail.ProductId;
            entity.Quantity = orderDetail.Quantity;
            entity.Price = orderDetail.Price;

            try
            {
                await _orderDetailService.UpdateAsync(entity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<OrderDetail>> PostOrderDetail(OrderDetail orderDetail)
        {
            await _orderDetailService.AddAsync(orderDetail);

            return Ok(new { id = orderDetail.Id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail = await _orderDetailService.GetByIdAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            await _orderDetailService.RemoveAsync(orderDetail);
            return NoContent();
        }

        private bool OrderDetailExists(int id)
        {
            if (_orderDetailService.GetByIdAsync(id) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
