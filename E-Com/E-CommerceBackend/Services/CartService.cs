using ActiveUp.Net.Groupware.vCalendar;
using ActiveUp.Net.Security.OpenPGP.Packets;
using Dapper;
using E_CommerceBackend.Database;
using E_CommerceBackend.DTOs;
using E_CommerceBackend.Entities;
using E_CommerceBackend.Interfaces;
using E_CommerceBackend.Services;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Data;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.Xml;

namespace E_CommerceBackend.Services
{
    public class CartService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IDapperDbConnection _dapperDbConnection;

        public CartService(AppDbContext appDbContext, IConfiguration configuration, IDapperDbConnection dapperDbConnection)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
            _dapperDbConnection = dapperDbConnection;
        }

        public async Task<int> AddinMasterDetail(CartMasterDetaildto cartMasterDetaildto)
        {
            //to check if userId Exists or not
            var data = await _appDbContext.CartMastertables.Where(c => c.UsertableId == cartMasterDetaildto.UsertableId).FirstOrDefaultAsync();
            if (data == null)       //if not then create new object and add it to table
            {
                var addmaster = new CartMastertable()
                {
                    UsertableId = cartMasterDetaildto.UsertableId
                };
                _appDbContext.CartMastertables.Add(addmaster);
                await _appDbContext.SaveChangesAsync();
            }

            //to get the details of particular UserTableId
            var cart = await _appDbContext.CartMastertables
            .Where(c => c.UsertableId == cartMasterDetaildto.UsertableId)
            .FirstOrDefaultAsync();

            //to check if ProductId already exists for particular cartMasterId
            var product = await _appDbContext.Cartdetailtables
            .Where(c => c.CartMasterId == cart.CartMasterId && c.ProductId == cartMasterDetaildto.ProductId).FirstOrDefaultAsync();
            if (product == null)        //if no the create a new object of CartDetailtable and Ass it to table
            {
                var adddetail = new Cartdetailtable()
                {
                    CartMasterId = cart.CartMasterId,
                    ProductId = cartMasterDetaildto.ProductId,
                    Quantity = cartMasterDetaildto.quantity
                };
                _appDbContext.Cartdetailtables.Add(adddetail);
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                product.Quantity = product.Quantity + cartMasterDetaildto.quantity;
                _appDbContext.Cartdetailtables.Update(product);
                await _appDbContext.SaveChangesAsync();
                //updating main product table
                var productmaster = await _appDbContext.Products.Where(p => p.Id == cartMasterDetaildto.ProductId).FirstOrDefaultAsync();
                productmaster.Stock = productmaster.Stock - 1;
                _appDbContext.Products.Update(productmaster);
                await _appDbContext.SaveChangesAsync();

            }
            // Calculate the total quantity of the product for the user
            var totalQuantity = await _appDbContext.Cartdetailtables
            .Where(c => c.CartMasterId == cart.CartMasterId)
            .SumAsync(c => c.Quantity);

            return totalQuantity;
        }




        public async Task<int> ReducefromMastertables(int ProductId, int UsertableId)
        {
            //to check if userId Exists or not
            var data = await _appDbContext.CartMastertables.Where(c => c.UsertableId == UsertableId).FirstOrDefaultAsync();
            
            //to get the details of particular UserTableId
            var cart = await _appDbContext.CartMastertables
            .Where(c => c.UsertableId == UsertableId)
            .FirstOrDefaultAsync();

            //to check if ProductId already exists for particular cartMasterId
            var product = await _appDbContext.Cartdetailtables
            .Where(c => c.CartMasterId == cart.CartMasterId && c.ProductId == ProductId).FirstOrDefaultAsync();

            product.Quantity = product.Quantity - 1;
            if(product.Quantity ==0)
            {
                _appDbContext.Cartdetailtables.Remove(product);
                await _appDbContext.SaveChangesAsync();
            }
            else {
                _appDbContext.Cartdetailtables.Update(product);
                await _appDbContext.SaveChangesAsync();
            }
            
            //Adding the products back to producttable
            var productmaster = await _appDbContext.Products.Where(p => p.Id == ProductId).FirstOrDefaultAsync();
            productmaster.Stock = productmaster.Stock + 1;
            _appDbContext.Products.Update(productmaster);
            await _appDbContext.SaveChangesAsync();
            
            // Calculate the total quantity of the product for the user
            var totalQuantity = await _appDbContext.Cartdetailtables
            .Where(c => c.CartMasterId == cart.CartMasterId)
            .SumAsync(c => c.Quantity);

            return totalQuantity;
        }



        public async Task<int> ReduceStockofProduct(int Productid)
        {
            var Getproduct = await _appDbContext.Products.Where(p => p.Id == Productid).FirstOrDefaultAsync();
            Getproduct.Stock -= 1;
            _appDbContext.Products.Update(Getproduct);
            await _appDbContext.SaveChangesAsync();
            return Getproduct.Stock;
        }

        public async Task<List<GetuseraddedProductdto>> FetchUserAddedProduct(int  UserId)
        {
            using (IDbConnection db = _dapperDbConnection.CreateConnection())
            {
                string sql = @"
                               SELECT p.Id, p.Brand, p.ProductName, p.ProductCode, p.SellingPrice, cdt.Quantity
                               FROM Products p
                               INNER JOIN (
                               SELECT ProductId, Quantity
                               FROM Cartdetailtables cdt
                               INNER JOIN CartMastertables cmt ON cdt.CartMasterId = cmt.CartMasterId
                               WHERE cmt.UsertableId = @UserId
                               ) AS cdt ON p.Id = cdt.ProductId
                               WHERE p.isDeleted = 0";

                var result = await db.QueryAsync<GetuseraddedProductdto>(sql, new { UserId = UserId });
                return result.ToList();
            }

        }

         public async Task<bool> ValidateDetail(Carddto card)
        {
            var carddetail = await _appDbContext.Cardtables.FirstOrDefaultAsync();
        
                if(carddetail!= null && carddetail.CardNumber== card.CardNumber && carddetail.ExpirtDate == card.ExpirtDate && carddetail.CVV == card.CVV)
                {
                    return true;
                }
                return false;
        }

        public async Task<string> Updatesalesdetails(int UserId, float total)
        {
            int totalEntries = _appDbContext.SalesMasters.Count(); // Generate the new InvoiceId
            string newInvoiceId = "ORD-" + (totalEntries + 1).ToString("D3");
            var user = await _appDbContext.Usertable.Where(u =>u.UsertableId == UserId).FirstOrDefaultAsync();
            var sales = new SalesMaster()
            {
                UsertableId = UserId,
                InvoiceId = newInvoiceId,
                InvoiceDate = DateTime.Now.ToString(),
                TotalPrice = total,
                DeliveryAddress = user.Address,
                DeliveryZipcode = user.ZipCode,
                DeliveryCountryId = user.CountryId,
                DeliveryStateId = user.StateId
            };
            _appDbContext.SalesMasters.Add(sales);
            await _appDbContext.SaveChangesAsync();

            var userProducts = await FetchUserAddedProduct(UserId);
           
            foreach (var Product in userProducts)
            {
                var addsalesdetail = new SalesDetail()
                {
                    InvoiceId = newInvoiceId,
                    ProductId = Product.Id,
                    ProductCode = Product.ProductCode,
                    SellingPrice = Product.SellingPrice,
                    Quantity = Product.Quantity
                };
                _appDbContext.SalesDetails.Add(addsalesdetail);
                await _appDbContext.SaveChangesAsync();

            }
            await _appDbContext.SaveChangesAsync();

            var cart = await _appDbContext.CartMastertables.Where(c => c.UsertableId == UserId).FirstOrDefaultAsync();
            var cartdetails = await _appDbContext.Cartdetailtables.Where(c => c.CartMasterId == cart.CartMasterId).ToListAsync();
            _appDbContext.Cartdetailtables.RemoveRange(cartdetails);
            await _appDbContext.SaveChangesAsync();

            //var all = await _appDbContext.SalesDetails.Where(s => s.InvoiceId == newInvoiceId).FirstOrDefaultAsync();
            //var allinone = await _appDbContext.SalesMasters.Where(s => s.InvoiceId == newInvoiceId).ToListAsync();
            return newInvoiceId;
        }





        public async Task<ResponseInvoiceDto> Updatesalesdetails2(int UserId, float total)
        {
            int totalEntries = _appDbContext.SalesMasters.Count(); // Generate the new InvoiceId
            string newInvoiceId = "ORD-" + (totalEntries + 1).ToString("D3");
            var user = await _appDbContext.Usertable.Where(u => u.UsertableId == UserId).FirstOrDefaultAsync();
            var sales = new SalesMaster()
            {
                UsertableId = UserId,
                InvoiceId = newInvoiceId,
                InvoiceDate = DateTime.Now.ToString(),
                TotalPrice = total,
                DeliveryAddress = user.Address,
                DeliveryZipcode = user.ZipCode,
                DeliveryCountryId = user.CountryId,
                DeliveryStateId = user.StateId
            };
            _appDbContext.SalesMasters.Add(sales);
            await _appDbContext.SaveChangesAsync();

            var userProducts = await FetchUserAddedProduct(UserId);

            foreach (var Product in userProducts)
            {
                var addsalesdetail = new SalesDetail()
                {
                    InvoiceId = newInvoiceId,
                    ProductId = Product.Id,
                    ProductCode = Product.ProductCode,
                    SellingPrice = Product.SellingPrice,
                    Quantity = Product.Quantity
                };
                _appDbContext.SalesDetails.Add(addsalesdetail);
                await _appDbContext.SaveChangesAsync();

            }
            await _appDbContext.SaveChangesAsync();

            var cart = await _appDbContext.CartMastertables.Where(c => c.UsertableId == UserId).FirstOrDefaultAsync();
            var cartdetails = await _appDbContext.Cartdetailtables.Where(c => c.CartMasterId == cart.CartMasterId).ToListAsync();
            _appDbContext.Cartdetailtables.RemoveRange(cartdetails);
            await _appDbContext.SaveChangesAsync();

            var salesmaster = await _appDbContext.SalesMasters.Where(s => s.UsertableId == UserId).OrderByDescending(s =>s.InvoiceId).FirstOrDefaultAsync();

            var salesdetail = await _appDbContext.SalesDetails.Where(s => s.InvoiceId == newInvoiceId).ToListAsync();

            var productDto = salesdetail.Adapt<List<ProductInvoiceDto>>();

            var state = await _appDbContext.State.Where(s => s.StateId == Int32.Parse(salesmaster.DeliveryStateId)).FirstOrDefaultAsync();
            var country = await _appDbContext.Country.Where(c => c.CountryId == Int32.Parse(salesmaster.DeliveryCountryId)).FirstOrDefaultAsync();

            var Receipt = new ResponseInvoiceDto()
            {
                invoiceId = salesmaster.InvoiceId,
                invoiceDate = salesmaster.InvoiceDate,
                subTotal = salesmaster.TotalPrice,
                address = salesmaster.DeliveryAddress,
                zipcode = salesmaster.DeliveryZipcode,
                state = state.statename,
                country = country.name,
                productInvoices = productDto
            };
            return Receipt;
        }


        public async Task<ResponseInvoiceDto> GetSalesDetails(int UserId)
        {
            //var salesmaster = await _appDbContext.SalesMasters.Where(s => s.UsertableId == UserId).OrderByDescending(s => s.InvoiceId).FirstOrDefaultAsync();
            var salesmaster = await _appDbContext.SalesMasters.Where(s => s.UsertableId == UserId).LastOrDefaultAsync();

            var salesdetail = await _appDbContext.SalesDetails.Where(s => s.InvoiceId == salesmaster.InvoiceId).ToListAsync();

            var productDto = salesdetail.Adapt<List<ProductInvoiceDto>>();
            
            var state = await _appDbContext.State.Where(s => s.StateId == Int32.Parse(salesmaster.DeliveryStateId)).FirstOrDefaultAsync();
            var country = await _appDbContext.Country.Where(c => c.CountryId == Int32.Parse(salesmaster.DeliveryCountryId)).FirstOrDefaultAsync();

            var Receipt = new ResponseInvoiceDto()
            {
                invoiceId = salesmaster.InvoiceId,
                invoiceDate = salesmaster.InvoiceDate,
                subTotal = salesmaster.TotalPrice,
                address = salesmaster.DeliveryAddress,
                zipcode = salesmaster.DeliveryZipcode,
                state = state.statename,
                country = country.name,
                productInvoices = productDto
            };
           

            return Receipt;
        }
    }
}












