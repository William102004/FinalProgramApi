using Api.eCommerce.EC;
using Library.eCommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library.eCommerce.Util;

namespace Api.eCommerce.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ILogger<ShoppingCartController> _logger;

        public ShoppingCartController(ILogger<ShoppingCartController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]

        public IEnumerable<Item?> Get()
        {
            return new ShoppingCartEC().Get();
        }

        [HttpGet("{id}")]

        public Item? GetById(int id)
        {
            return new ShoppingCartEC().Get()
                .FirstOrDefault(i => i?.Id == id);
        }

        [HttpDelete("{id}")]

        public Item? Delete(int id)
        {
            return new ShoppingCartEC().Delete(id);
        }

        [HttpPost]

        public Item? AddOrUpdate([FromBody]Item item)
        {
            var newItem = new ShoppingCartEC().AddOrUpdate(item);
            return item;
        }

        [HttpPost("Search")]

        public IEnumerable<Item> Search([FromBody]QueryRequest query)
        {
            return new ShoppingCartEC().Get(query.Query);
        }

        [HttpDelete("Checkout")]

        public void CheckOut()
        {
            new ShoppingCartEC().CheckOut();
        }

       


    }
}
