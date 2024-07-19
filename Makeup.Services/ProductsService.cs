using AutoMapper;
using Makeup.Entities;
using Makeup.Services.DTO.Brands;
using Makeup.Services.DTO.Categories;
using Makeup.Services.DTO.Orders;
using Makeup.Services.DTO.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Makeup.Services
{
    public class ProductsService
    {
        private readonly Makeup2024Context _dbContext;
        
        private readonly IMapper _mapper;
        public ProductsService(Makeup2024Context dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<ProductDto>> GetAll()
        {
            var products = await _dbContext.Products
            .Include(p => p.Brands)
            .Include(p => p.Categories)
            .ToListAsync();

            //return products.Select(p => new ProductDto
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //    Description = p.Description,
            //    Price = p.Price,
            //    Image = p.Image,
            //    Code = p.Code,
            //    Brands = p.Brands.Select(pb => new BrandDto
            //    {
            //        Id = pb.Id,
            //        Logo = pb.Logo,
            //        Name = pb.Name,
            //        Description = pb.Description
            //    }).ToList(),
            //    Categories = p.Categories.Select(pc => new CategoryDto
            //    {
            //        Id = pc.Id,
            //        Name = pc.Name,
            //        Description = pc.Description
            //    }).ToList()
            //}).ToList();

            return _mapper.Map<List<ProductDto>>(products);
        }
        public async Task<List<ProductDto>> Search(string searchTerm)
        {
            var products = await _dbContext.Products
            .Include(p => p.Brands)
            .Include(p => p.Categories)
            .Where(b => b.Name.Contains(searchTerm))
            .ToListAsync();

            //return products.Select(p => new ProductDto
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //    Description = p.Description,
            //    Price = p.Price,
            //    Image = p.Image,
            //    Code = p.Code,
            //    Brands = p.Brands.Select(pb => new BrandDto
            //    {
            //        Id = pb.Id,
            //        Logo = pb.Logo,
            //        Name = pb.Name,
            //        Description = pb.Description
            //    }).ToList(),
            //    Categories = p.Categories.Select(pc => new CategoryDto
            //    {
            //        Id = pc.Id,
            //        Name = pc.Name,
            //        Description = pc.Description
            //    }).ToList()
            //}).ToList();

            return _mapper.Map<List<ProductDto>>(products);
        }
        public async Task<ProductDto> GetById(int productId)
        {
            var product = await _dbContext.Products
            .Include(p => p.Brands)
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return null!;
            }

            //return new ProductDto
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    Price = product.Price,
            //    Image = product.Image,
            //    Code = product.Code,
            //    Brands = product.Brands.Select(pb => new BrandDto
            //    {
            //        Id = pb.Id,
            //        Logo = pb.Logo,
            //        Name = pb.Name,
            //        Description = pb.Description
            //    }).ToList(),
            //    Categories = product.Categories.Select(pc => new CategoryDto
            //    {
            //        Id = pc.Id,
            //        Name = pc.Name,
            //        Description = pc.Description
            //    }).ToList()
            //};

            return _mapper.Map<ProductDto>(product);

        }

        public async Task<ProductDto> Create(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.Brands = _dbContext.Brands.Where(a =>
           productDto!.Brands!.Contains(a.Id)).ToList();
            product.Categories = _dbContext.Categories.Where(g =>
           productDto!.Categories!.Contains(g.Id)).ToList();
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto> Update(int id, ProductDto productDto)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return null!;
            }
            _mapper.Map(productDto, product);
            _dbContext.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto> UpdateImage(int id, byte[] imageData)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return null!;
            }
            product.Image = imageData;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }
        public async Task<bool> Delete(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<OrderDto>> GetProductPurchases(int productId)
        {
            var product = await _dbContext.Products
            .Include(b => b.OrderDetails)
            .ThenInclude(od => od.Order)
            .FirstOrDefaultAsync(b => b.Id == productId);
            if (product == null)
            {
                return null!;
            }
            var orders = product.OrderDetails.Select(od => od.Order).ToList();
            return _mapper.Map<List<OrderDto>>(orders);
        }
        public async Task<List<RevenueSummaryDto>> GetRevenueSummary()
        {
            var products = await _dbContext.Products
            .Include(b => b.OrderDetails)
            .ThenInclude(od => od.Order)
            .ToListAsync();
            var revenueSummary = products.Select(product => new RevenueSummaryDto
            {
                ProductId = product.Id,
                Revenues = product.OrderDetails.GroupBy(od => new {
                od.Order.OrderDateTime.Year,
                od.Order.OrderDateTime.Month
            })
            .Select(g => new RevenueDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalRevenue = g.Sum(od => od.UnitPrice * od.Quantity)
            })
            .OrderByDescending(r => r.Year)
            .ThenBy(r => r.Month)
            .ToList()
            })
            .ToList();
            return revenueSummary;
        }
    }
}
