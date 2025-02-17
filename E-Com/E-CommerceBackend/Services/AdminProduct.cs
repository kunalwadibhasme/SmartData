using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using E_CommerceBackend.Database;
using E_CommerceBackend.DTOs;
using E_CommerceBackend.Entities;
using E_CommerceBackend.Interfaces;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;

namespace E_CommerceBackend.Services
{
    public class AdminProductService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IDapperDbConnection _DapperdbConnection;
        private readonly IWebHostEnvironment _env;

        public AdminProductService(AppDbContext appDbContext, IConfiguration configuration, IDapperDbConnection dapperDbConnection, IWebHostEnvironment env)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
            _DapperdbConnection = dapperDbConnection;
            _env = env;
        }

        //public async Task<AdminProductResponsedto> GetAllProductsAsync()
        //{
        //    var products = await _appDbContext.Products.Where(p => !p.isDeleted).ToListAsync();
        //    return new AdminProductResponsedto { 
        //        Status = 200, 
        //        Message = "Products retrieved successfully.", 
        //        Data = products 
        //    };
        //}
        public async Task<AdminProductResponsedto> GetAllProductsAsync()
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM Products WHERE isDeleted = 0 AND Stock>1";
                var products = (await db.QueryAsync<Product>(sql)).ToList();

                return new AdminProductResponsedto
                {
                    Status = 200,
                    Message = "Products retrieved successfully.",
                    Data = products
                };
            }
        }

        public async Task<AdminProductResponsedto> GetProductsByUserAsync(int id)
        {
           var product = await _appDbContext.Products.Where(p => p.UsertableId == id && !p.isDeleted).ToListAsync();
           if(product == null)
            {
                return new AdminProductResponsedto
                {
                    Status = 404,
                    Message = "Product not found.",
                    Data = null
                };
            }
            return new AdminProductResponsedto
            {
                Status = 200,
                Message = "Products retrieved successfully.",
                Data = product
            };
            //using (IDbConnection db = _DapperdbConnection.CreateConnection())
            //{
            //    string sql = "SELECT * FROM Products WHERE  UsertableId = @id AND isDeleted = 0 ";
            //    var products = (await db.QueryAsync<Product>(sql)).ToList();

            //    return new AdminProductResponsedto
            //    {
            //        Status = 200,
            //        Message = "Products retrieved successfully.",
            //        Data = product
            //    };
            //}
        }




        //public async Task<AdminProductResponsedto> GetProductByIdAsync(int productId)
        //{
        //    var product = await _appDbContext.Products.Where(p => p.Id == productId && !p.isDeleted).FirstOrDefaultAsync();
        //    if (product == null)
        //    {
        //        return new AdminProductResponsedto { 
        //            Status = 404, 
        //            Message = "Product not found.", 
        //            Data = null 
        //        };
        //    }

        //    return new AdminProductResponsedto { 
        //        Status = 200, 
        //        Message = "Product retrieved successfully.", 
        //        Data = product 
        //    };
        //}

        public async Task<AdminProductResponsedto> GetProductByIdAsync(int productId)
        {
            using (IDbConnection db = _DapperdbConnection.CreateConnection())
            {
                string sql = "SELECT * FROM Products WHERE Id = @ProductId AND isDeleted = 0";

                var product = await db.QueryFirstOrDefaultAsync<Product>(sql, new { ProductId = productId });

                if (product == null)
                {
                    return new AdminProductResponsedto
                    {
                        Status = 404,
                        Message = "Product not found.",
                        Data = null
                    };
                }

                return new AdminProductResponsedto
                {
                    Status = 200,
                    Message = "Product retrieved successfully.",
                    Data = product
                };
            }
        }


        private async Task<bool> ProductCodeExistsAsync(string productCode)
        { 
            return await _appDbContext.Products.AnyAsync(p => p.ProductCode == productCode); 
        }

        public async Task<AdminProductResponsedto> AddProductAsync(Productdto productDto)
        {
            if (await ProductCodeExistsAsync(productDto.productCode)) 
            { 
                return new AdminProductResponsedto { Status = 400, Message = "Product code already exists.", Data = null }; 
            }

            string savedImagePath = ImagePath(productDto.productImagePath);
            //if (!string.IsNullOrEmpty(productDto.productImagePath))
            //{ i have ImagePath method in LoginRegister.cs file ans i want to call it in AdminProduct.cs file
            //    var base64Image = productDto.productImagePath;
            //    var imageBytes = Convert.FromBase64String(base64Image.Split(',')[0]); // Skip the prefix (e.g., "data:image/png;base64,")

            //    // Generate a unique file path and save the image
            //    var uniqueFileName = $"{Guid.NewGuid()}.png";
            //    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/NewFolder");
            //    savedImagePath = uniqueFileName;

            //    // Ensure the directory exists
            //    if (!Directory.Exists(folderPath))
            //    {
            //        Directory.CreateDirectory(folderPath);
            //    }

            //    await System.IO.File.WriteAllBytesAsync(savedImagePath, imageBytes);
            //}


            var product = new Product
            {
                UsertableId= productDto.UsertableId,
                ProductName = productDto.productName,
                ProductCode = productDto.productCode,
                ProductImagePath = savedImagePath,
                Category = productDto.category,
                Brand = productDto.brand,
                SellingPrice = productDto.sellingPrice,
                PurchasePrice = productDto.purchasePrice,
                PurchaseDate = productDto.purchaseDate,
                Stock = productDto.stock,
                isDeleted = false
            };

            _appDbContext.Products.Add(product);
            await _appDbContext.SaveChangesAsync();

            return new AdminProductResponsedto { 
                Status = 200, 
                Message = "Product added successfully.", 
                Data = product 
            };
        }

        public string ImagePath(IFormFile image)
        {
            if (image != null)
            {
                var folderPath = Path.Combine(_env.WebRootPath, "profile_images");
                // Ensure the directory exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                // Generate a unique file name
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);
                // Save the file asynchronously
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream); // Synchronous operation for this case
                }
                // Return relative path
                return $"/profile_images/{fileName}";
            }
            return $"/profile_images/default.jpg";
        }


        public async Task<AdminProductResponsedto> UpdateProductAsync(int productId, Productdto productDto)
        {
            var product = await _appDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return new AdminProductResponsedto { 
                    Status = 404, 
                    Message = "Product not found.", 
                    Data = null 
                };
            }

            if (await ProductCodeExistsAsync(productDto.productCode) && product.ProductCode != productDto.productCode) 
            { 
                return new AdminProductResponsedto { Status = 400, Message = "Product code already exists.", Data = null }; 
            }
            string savedImagePath = ImagePath(productDto.productImagePath);


            product.ProductName = productDto.productName;
            product.ProductCode = productDto.productCode;
            product.ProductImagePath = savedImagePath;
            product.Category = productDto.category;
            product.Brand = productDto.brand;
            product.SellingPrice = productDto.sellingPrice;
            product.PurchasePrice = productDto.purchasePrice;
            product.PurchaseDate = productDto.purchaseDate;
            product.Stock = productDto.stock;
            product.isDeleted = false;

            _appDbContext.Products.Update(product);
            await _appDbContext.SaveChangesAsync();

            return new AdminProductResponsedto { 
                Status = 200, 
                Message = "Product updated successfully.", 
                Data = product 
            };
        }

        public async Task<AdminProductResponsedto> DeleteProductAsync(int productId)
        {
            var product = await _appDbContext.Products
                .Where(p => p.Id == productId && !p.isDeleted)
                .FirstOrDefaultAsync();
            if (product == null)
            {
                return new AdminProductResponsedto
                {
                    Status = 404,
                    Message = "Product not found.",
                    Data = null
                };
            }

            product.isDeleted = true;
            _appDbContext.Products.Update(product);
            await _appDbContext.SaveChangesAsync();

            return new AdminProductResponsedto
            {
                Status = 200,
                Message = "Product marked as deleted successfully.",
                Data = product
            };
        }

    }

}
