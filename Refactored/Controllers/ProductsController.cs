using System;
using Microsoft.AspNetCore.Mvc;
using Refactored.Logic;
using Refactored.Models;

namespace Refactored.Controllers
{
	[ApiController]
	[Route("[controller]")]
    public class ProductsController : Controller
	{
		private readonly IProductRepository _productRepository;


		public ProductsController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}


		[HttpGet]
		public IActionResult Index([FromQuery] string name)
		{

			var items = string.IsNullOrWhiteSpace(name)
				? _productRepository.GetAllProducts()
				: _productRepository.GetByName(name);

			var products = new Products(items);

			return Ok(products);
		}


		[HttpGet("{id}")]
		public IActionResult Get(Guid id)
		{
			var product = _productRepository.Get(id);

			if (product == null)
			{
				return NotFound(
					new ErrorResult
					{
						ErrorCode = 4040,
						Message = "There is no product with the given id"
					});
			}

			return Ok(product);
		}

		
		[HttpPost]
		public IActionResult Post(Product product)
		{

			var saved = _productRepository.Save(product);

			return Ok(saved);
		}

	

		[Route("{id}")]
        [HttpPut]
        public IActionResult Put(Guid id, Product product)
        {
		

	        try
	        {
		        var updated = _productRepository.Update(id, product);

		        return Ok(updated);
	        }
	        catch (EntityDoesNotExistException)
	        {
		        return BadRequest(new ErrorResult
		        {
					ErrorCode = 4050,
					Message = $"There is no existing product with the id {id}"
		        });
	        }


        }

        [Route("{id}")]
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
			try
			{
				_productRepository.Delete(id);

				return Ok();
			}
			catch (EntityDoesNotExistException)
			{
				return BadRequest(new ErrorResult
				{
					ErrorCode = 4060,
					Message = $"There is no existing product with the id {id}"
				});
			}
		}

		
	}
}
