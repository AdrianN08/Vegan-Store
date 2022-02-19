using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeganStore.Models.Entities;
using VeganStore.Web.API.Filters;
using VeganStore.Web.API.Repository;

namespace VeganStore.Web.API.Controllers
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {        
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(string appUserId = null)
        {
            var orders = new List<Order>();
            IEnumerable<Order> items = new List<Order>();
            if (appUserId != null)
            {
                items = await _orderService.GetAllWhereAsync(x => x.AppUserId == appUserId);
            }
            else
            {
                items = await _orderService.GetAllAsync();
            }
            foreach (var item in items)
            {
                orders.Add(item);
            }
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            var entity = await _orderService.GetByIdAsync(id);
            entity.ShippingDate = order.ShippingDate;
            entity.OrderTotal = order.OrderTotal;
            entity.OrderStatus = order.OrderStatus;
            entity.PaymentStatus = order.PaymentStatus;
            entity.SessionId = order.SessionId;
            entity.PaymentIntentId = order.PaymentIntentId;
            entity.FirstName = order.FirstName;
            entity.LastName = order.LastName;
            entity.PhoneNumber = order.PhoneNumber;
            entity.StreetAddress = order.StreetAddress;
            entity.City = order.City;
            entity.PostalCode = order.PostalCode;

            try
            {
                await _orderService.UpdateAsync(entity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            await _orderService.AddAsync(order);

            return Ok(order.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderService.RemoveAsync(order);
            return NoContent();
        }

        private bool OrderExists(int id)
        {
            if (_orderService.GetByIdAsync(id) != null)
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
