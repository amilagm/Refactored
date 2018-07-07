using System;
using Microsoft.AspNetCore.Mvc;
using Refactored.Logic;
using Refactored.Models;

namespace Refactored.Controllers
{
	[ApiController]
	[Route("/products/{productId}/[controller]")]
    public class OptionsController : Controller
	{
		private readonly IProductRepository _productRepository;


		public OptionsController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}
		
      
        [HttpGet]
        public IActionResult Index(Guid productId)
        {
	        try
	        {
		        var options = _productRepository.GetOptions(productId);

		        return Ok(new ProductOptions
		        {
			        Items = options
		        });
	        }
	        catch (EntityDoesNotExistException)
	        {
		        return NotFound(new ErrorResult
		        {
			        ErrorCode = 4050,
			        Message = $"There is no existing product with the id {productId}"
		        });
	        }


		}
		
		[HttpGet("{id}")]
        public IActionResult Get(Guid productId, Guid id)
        {
			try
			{
				var option = _productRepository.GetOption(id, productId);

				return Ok(option);
			}
			catch (EntityDoesNotExistException)
			{
				return NotFound(new ErrorResult
				{
					ErrorCode = 4040,
					Message = $"There is no product option with the id {id} for the product id {productId}"
				});
			}

		}

		[HttpPost]
        public IActionResult CreateOption(Guid productId, ProductOption option)
        {
	       

			try
	        {
		        var saved = _productRepository.SaveOption(productId, option);

		        return Ok(saved);
	        }
	        catch (EntityDoesNotExistException)
	        {
		        return NotFound(new ErrorResult
		        {
			        ErrorCode = 4060,
			        Message = $"There is no product with the id {productId}"
		        });
	        }
        }

        [Route("{id}")]
        [HttpPut]
        public IActionResult UpdateOption(Guid productId, Guid id, ProductOption option)
        {
			

	        try
	        {
		        var updated = _productRepository.UpdateOption(option,id, productId );

		        return Ok(updated);
	        }
	        catch (EntityDoesNotExistException)
	        {
		        return NotFound(new ErrorResult
		        {
			        ErrorCode = 4070,
			        Message = $"There is no product with the id {productId}"
		        });
	        }
		}

        [HttpDelete("{id}")]
        public IActionResult DeleteOption(Guid productId, Guid id)
        {
			try
			{
				_productRepository.DeleteOption(id, productId);

				return Ok();
			}
			catch (EntityDoesNotExistException)
			{
				return NotFound(new ErrorResult
				{
					ErrorCode = 4080,
					Message = $"There is no option with the id {id} for the product id {productId}"
				});
			}
		}

		
	}
}
