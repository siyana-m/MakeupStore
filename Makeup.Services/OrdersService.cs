//using Makeup.Entities;
//using Makeup.Services.DTO.Orders;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Makeup.Services
//{
//    public class OrdersService
//    {
//        private readonly Makeup2024Context _dbContext;
//        public OrdersService(Makeup2024Context dbContext)
//        {
//            _dbContext = dbContext;
//        }
//        public async Task<OrderDto> GetLatestOrderByUser(string username)
//        {
//            var order = await _dbContext.Orders
//            .Include(x => x.OrderDetails)
//            .ThenInclude(x => x.Product)
//            .Include(x => x.Customer)
//            .OrderByDescending(x => x.OrderDateTime)
//            .LastOrDefaultAsync(x => x.Customer.EmailAddress == username && !x.IsPaid);

//            if (order == null || order.IsPaid)
//            {
//                var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.EmailAddress == username);
//                if (customer == null)
//                {
//                    customer = new Customer()
//                    {
//                        EmailAddress = username,
//                        FullName = "",
//                        PhoneNumber = ""
//                    };
//                    _dbContext.Add(customer);
//                    await _dbContext.SaveChangesAsync();
//                }
//                order = new Order()
//                {
//                    OrderDateTime = DateTime.Now,
//                    PaymentMethod = "card",
//                    DeliveryAddress = "",
//                    IsPaid = false,
//                    IsDelivered = false,
//                    CustomerId = customer.Id
//                };
//                _dbContext.Add(order);
//                await _dbContext.SaveChangesAsync();
//            }

//            return new OrderDto()
//            {
//                Id = order.Id,
//                CustomerId = order.CustomerId,
//                DeliveryAddress = order.DeliveryAddress,
//                IsDelivered = order.IsDelivered,
//                IsPaid = order.IsPaid,
//                OrderDateTime = order.OrderDateTime,
//                Customer = new DTO.Orders.CustomerDto()
//                {
//                    Id = order.Customer.Id,
//                    EmailAddress = order.Customer.EmailAddress,
//                    FullName = order.Customer.FullName,
//                    PhoneNumber = order.Customer.PhoneNumber,
//                },
//                PaymentMethod = order.PaymentMethod,
//                OrderDetails = order.OrderDetails.Select(d => new OrderDetailsDto()
//                {
//                    ProductId = d.ProductId,
//                    Quantity = d.Quantity,
//                    UnitPrice = d.UnitPrice,
//                    Product = new DTO.Products.ProductDto()
//                    {
//                        Id = d.ProductId,
//                        Name = d.Product.Name
//                    }
//                }).ToList()
//            };
//        }
//        public async Task AddProduct(int orderId, int productId)
//        {
//            var order = await _dbContext.Orders.Include(x =>
//           x.OrderDetails).FirstOrDefaultAsync(x => x.Id == orderId);
//            if (order == null)
//            {
//                return;
//            }
//            var product = await _dbContext.Products.FindAsync(productId);
//            if (product == null)
//            {
//                return;
//            }
//            var existing = order.OrderDetails.FirstOrDefault(x => x.ProductId == productId);
//            if (existing != null)
//            {
//                existing.Quantity++;
//            }
//            else
//            {
//                order.OrderDetails.Add(new OrderDetail()
//                {
//                    ProductId = productId,
//                    Quantity = 1,
//                    UnitPrice = product.Price!.Value
//                });
//            }
//            await _dbContext.SaveChangesAsync();
//        }
//        public async Task AddProduct(string username, int productId)
//        {
//            var order = await this.GetLatestOrderByUser(username);
//            await this.AddProduct(order.Id, productId);
//        }
//        public async Task SetOrderPaid(string username)
//        {
//            var order = await this.GetLatestOrderByUser(username);
//            if (order != null)
//            {
//                order.IsPaid = true;
//                await _dbContext.SaveChangesAsync();
//            }
//        }
//    }

//}


using Makeup.Entities;
using Makeup.Services.DTO.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Makeup.Services
{
    public class OrdersService
    {
        private readonly Makeup2024Context _dbContext;

        public OrdersService(Makeup2024Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderDto> GetLatestOrderByUser(string username)
        {
            var order = await _dbContext.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.Customer)
                .OrderByDescending(x => x.OrderDateTime)
                .FirstOrDefaultAsync(x => x.Customer.EmailAddress == username && !x.IsPaid);

            if (order == null || order.IsPaid)
            {
                // If there is no unpaid order or the latest order is paid, create a new order
                order = await CreateNewOrderForUser(username);
            }

            return MapOrderToDto(order);
        }

        private async Task<Order> CreateNewOrderForUser(string username)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.EmailAddress == username);
            if (customer == null)
            {
                customer = new Customer()
                {
                    EmailAddress = username,
                    FullName = "",
                    PhoneNumber = ""
                };
                _dbContext.Add(customer);
                await _dbContext.SaveChangesAsync();
            }

            var newOrder = new Order()
            {
                OrderDateTime = DateTime.Now,
                PaymentMethod = "card",
                DeliveryAddress = "",
                IsPaid = false,
                IsDelivered = false,
                CustomerId = customer.Id
            };
            _dbContext.Add(newOrder);
            await _dbContext.SaveChangesAsync();

            return newOrder;
        }

        public async Task AddProduct(int orderId, int productId)
        {
            var order = await _dbContext.Orders
                .Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null)
            {
                return;
            }

            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null)
            {
                return;
            }

            var existing = order.OrderDetails.FirstOrDefault(x => x.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                order.OrderDetails.Add(new OrderDetail()
                {
                    ProductId = productId,
                    Quantity = 1,
                    UnitPrice = product.Price!.Value
                });
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddProduct(string username, int productId)
        {
            var order = await this.GetLatestOrderByUser(username);
            await this.AddProduct(order.Id, productId);
        }

        public async Task SetOrderPaid(string username)
        {
            var order = await this.GetLatestOrderByUser(username);
            if (order != null)
            {
                var orderEntity = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == order.Id);
                if (orderEntity != null)
                {
                    orderEntity.IsPaid = true;
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        private OrderDto MapOrderToDto(Order order)
        {
            return new OrderDto()
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                DeliveryAddress = order.DeliveryAddress,
                IsDelivered = order.IsDelivered,
                IsPaid = order.IsPaid,
                OrderDateTime = order.OrderDateTime,
                Customer = new DTO.Orders.CustomerDto()
                {
                    Id = order.Customer.Id,
                    EmailAddress = order.Customer.EmailAddress,
                    FullName = order.Customer.FullName,
                    PhoneNumber = order.Customer.PhoneNumber,
                },
                PaymentMethod = order.PaymentMethod,
                OrderDetails = order.OrderDetails.Select(d => new OrderDetailsDto()
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    Product = new DTO.Products.ProductDto()
                    {
                        Id = d.ProductId,
                        Name = d.Product.Name
                    }
                }).ToList()
            };
        }
    }
}
