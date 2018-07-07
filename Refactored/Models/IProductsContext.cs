using Microsoft.EntityFrameworkCore;

namespace Refactored.Models
{
	public interface IProductsContext
	{
		DbSet<Product> Product { get; set; }
		DbSet<ProductOption> ProductOption { get; set; }
	}
}