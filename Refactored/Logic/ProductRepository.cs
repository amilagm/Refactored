using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Refactored.Models;

namespace Refactored.Logic
{
	public class ProductRepository : IProductRepository
	{
		private readonly ProductsContext _productsContext;

		public ProductRepository(ProductsContext productsContext)
		{
			_productsContext = productsContext;
		}

		public List<Product> GetAllProducts()
		{
			return _productsContext.Product.ToList();
		}

	    public List<Product> GetByName(string name)
	    {
	        return _productsContext.Product
	            .Where(p => EF.Functions.Like(p.Name,$"%{name}%")).ToList();
	    }

	    public Product Get(Guid id)
	    {
	        var product =  _productsContext.Product.Find(id);

            if(product == null)
                throw  new EntityDoesNotExistException();

	        return product;
	    }

		public Product Save(Product product)
		{
            product.Id = Guid.NewGuid();

		    _productsContext.Product.Add(product);
		    _productsContext.SaveChanges();

		    return _productsContext.Product.Find(product.Id);


		}

		public Product Update(Guid id, Product product)
		{
		   var exists = _productsContext.Product.Any(p => p.Id ==  id);

            if(!exists)
                throw new EntityDoesNotExistException();

		    product.Id = id;

		    _productsContext.Product.Update(product);

		    _productsContext.SaveChanges();

		    return _productsContext.Product.Find(id);
		}

		public void Delete(Guid id)
		{
		    var product = _productsContext.Product.Find(id);

		    if (product == null)
		        throw new EntityDoesNotExistException();
            
		    _productsContext.Product.Remove(product);

		    _productsContext.SaveChanges();
		   
        }

		public List<ProductOption> GetOptions(Guid productId)
		{
		    var exists = _productsContext.Product.Any(p => p.Id == productId);

		    if (!exists)
		        throw new EntityDoesNotExistException();

            var options = _productsContext.ProductOption.Where(o => o.ProductId == productId).ToList();
            
		    return options;
		}

        public ProductOption GetOption(Guid optionId, Guid productId)
		{
		    var option = _productsContext.ProductOption.FirstOrDefault(o => o.Id == optionId && o.ProductId == productId);

		    if (option == null)
		        throw new EntityDoesNotExistException();

            return option;
        }

		public ProductOption SaveOption(Guid productId, ProductOption option)
		{
		    var exists = _productsContext.Product.Any(p => p.Id == productId);

		    if (!exists)
		        throw new EntityDoesNotExistException();

		    option.Id = Guid.NewGuid();

		    _productsContext.ProductOption.Add(option);

		    _productsContext.SaveChanges();

		    return _productsContext.ProductOption.Find(option.Id);
		}

		public ProductOption UpdateOption(ProductOption option, Guid optionId, Guid productId)
		{
		    var exists = _productsContext.ProductOption.Any(o => o.Id == optionId && o.ProductId == productId);

		    if (!exists)
		        throw new EntityDoesNotExistException();
            
		    option.Id = optionId;

		    option.ProductId = productId;

		    _productsContext.ProductOption.Update(option);

		    _productsContext.SaveChanges();

		    return _productsContext.ProductOption.Find(option.Id);
        }

		public void DeleteOption(Guid optionId, Guid productId)
		{
		    var option = _productsContext.ProductOption.FirstOrDefault(o => o.Id == optionId && o.ProductId == productId);

		    if (option == null)
		        throw new EntityDoesNotExistException();

		    _productsContext.ProductOption.Remove(option);

		    _productsContext.SaveChanges();
		}
	}
}
