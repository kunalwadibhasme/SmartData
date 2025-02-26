using Microsoft.EntityFrameworkCore;
using ProductMicroservice.Database;
using ProductMicroservice.Dtos;
using ProductMicroservice.Entities;

namespace ProductMicroservice.Service
{
    public class ProductService
    {
        private readonly AppDbContext _appDbContext;
        public ProductService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> AddProduct(AddproductDto addProductDto)
        {
            var product = await _appDbContext.products.AnyAsync(p => p.Name == addProductDto.Name);
            if(product)
            {
                return new { status = 400, message = "Product Already Exits" };
            }

            var Product = new Product
            {
                Name = addProductDto.Name,
                rate = addProductDto.rate,
                isDeleted = false,
            };

            await _appDbContext.products.AddAsync(Product);
            await _appDbContext.SaveChangesAsync();
            return new { status = 200, message = "Product Added Successfully" };
        }

        public async Task<object> GetAllProducts()
        {
            var product = await _appDbContext.products.ToListAsync();
            if (product != null)
            {
                return new { status = 200, message = "All Product", Data = product };
            }
            else
            {
                return new { status = 400, message = "No Prdouct Found" };
            }
        }

        public async Task<object> GetProductById(int Id)
        {
            var product = await _appDbContext.products.Where(p => p.Id == Id).FirstOrDefaultAsync();    
            if(product != null)
            {
                return new { status = 200, message = "Product Found", Data = product };
            }
            else
            {
                return new { status = 400, message = "No Prdouct Found" };
            }
        }

        public async Task<object> DeleteProduct(int Id)
        {
            var product = await _appDbContext.products.Where(p => p.Id == Id).FirstOrDefaultAsync();
            if (product != null)
            {
                product.isDeleted = true;
                _appDbContext.products.Update(product);
                await _appDbContext.SaveChangesAsync();
                return new { status = 200, message = "Product Deleted Successfully" };
            }
            else
            {
                return new { status = 400, message = "No Prdouct Found with this Id" };
            }
        }
        public async Task<object> UpdateProduct(int Id, AddproductDto addproductDto)
        {
            var product = await _appDbContext.products.Where(p => p.Id == Id).FirstOrDefaultAsync();
            if (product != null)
            {
                product.Name = addproductDto.Name;
                product.rate = addproductDto.rate;
                product.isDeleted = false;
                _appDbContext.products.Update(product);
                await _appDbContext.SaveChangesAsync();
                return new { status = 200, message = "Product Updated Successfully" };
            }
            else
            {
                return new { status = 400, message = "No Prdouct Found with this Id" };
            }
        }
    }
}
