using System;
using System.Collections.Generic;
using Refactored.Models;

namespace Refactored.Logic
{
	public interface IProductRepository
	{
		List<Product> GetAllProducts();
		List<Product> GetByName(string name);
		Product Get(Guid id);
		Product Save(Product product);
		Product Update(Guid id, Product product);
		void Delete(Guid id);
		List<ProductOption> GetOptions(Guid productId);
		ProductOption GetOption(Guid optionId, Guid productId);
		ProductOption SaveOption(Guid productId, ProductOption option);
		ProductOption UpdateOption(ProductOption option, Guid optionId, Guid productId);
		void DeleteOption(Guid optionId, Guid productId);
	}
}