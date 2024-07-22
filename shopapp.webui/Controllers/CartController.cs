using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Extensions;
using shopapp.webui.Identity;
using shopapp.webui.Models;

namespace shopapp.webui.Controllers
{
    [Authorize]
    public class CartController: Controller
    {
        private ICartService _cartService;
        private UserManager<User> _userManager;
        private IOrderService _orderService;
        public CartController(ICartService cartService, UserManager<User> userManager, IOrderService orderService)
        {
            _cartService = cartService;
            _userManager = userManager;
            _orderService = orderService;
        }
        
        public IActionResult Index()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            return View(new CartModel() {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel() {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.AddToCart(userId, productId, quantity);

            return RedirectToAction("Index");
        }
        public IActionResult List()
        {

            return View();
        }

        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.DeleteFromCart(userId, productId);

            return RedirectToAction("Index");
        }

        public IActionResult Checkout() {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            var orderModel = new OrderModel
            {
                CartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.Name,
                        Price = (double)i.Product.Price,
                        ImageUrl = i.Product.ImageUrl,
                        Quantity = i.Quantity,
                    }).ToList()
                }
            };
            return View(orderModel);
        }

        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            if(ModelState.IsValid) {
                var userId = _userManager.GetUserId(User);
                
                var cart = _cartService.GetCartByUserId(userId);

                model.CartModel = new CartModel() {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.Name,
                        Price = (double)i.Product.Price,
                        ImageUrl = i.Product.ImageUrl,
                        Quantity = i.Quantity,
                    }).ToList()
                };
                
                var user = _userManager.FindByIdAsync(userId).Result;
                var payment = PaymentProcess(model, user);

                if(payment.Status == "success")
                {
                    SaveOrder(model, payment, userId);
                    ClearCart(model.CartModel.CartId);
                    return View("Success");
                } else {
                    TempData.Put("message", new AlertMessage() {
                        Title = "Payment Error.",
                        Message = payment.ErrorCode + " - " + payment.ErrorMessage,
                        AlertType = "danger"
                    });
                }
            }
            return View(model);
        }

        public IActionResult GetOrders() {
            var userId = _userManager.GetUserId(User);
            var orders = _orderService.GetOrders(userId);
            var orderListModel = new List<OrderListModel>();
            OrderListModel orderModel;
            foreach (var order in orders) {
                orderModel = new OrderListModel
                {
                    OrderId = order.OrderId,
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    Phone = order.Phone,
                    Email = order.Email,
                    FirstName = order.FirstName,
                    LastName = order.LastName,
                    Address = order.ShippingAddress.Address,
                    City = order.ShippingAddress.City,
                    PaymentType = order.PaymentType,
                    OrderState = order.OrderState,

                    OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                    {
                        OrderItemId = i.Id,
                        Name = i.Product.Name,
                        Price = (double)i.Price,
                        Quantity = i.Quantity,
                        ImageUrl = i.Product.ImageUrl
                    }).ToList()
                };

                orderListModel.Add(orderModel);

            }
            return View("Orders", orderListModel);
        }

        private void ClearCart(int cartId)
        {
            _cartService.ClearCart(cartId);
        }

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order
            {
                OrderNumber = new Random().Next(100000, 999999).ToString(),
                OrderState = EnumOrderState.completed,
                PaymentType = EnumPaymentType.CreditCard,
                PaymentId = payment.PaymentId,
                ConversationId = payment.ConversationId,
                OrderDate = DateTime.Now,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                UserId = userId,
                Note = model.Note,
                OrderItems = new List<entity.OrderItem>(),
                ShippingAddress = new ShippingAddress
                {
                    City = model.ShipModel.City,
                    Country = model.ShipModel.Country,
                    Address = model.ShipModel.Address,
                    ZipCode = model.ShipModel.ZipCode
                },
                BillingAddress = new BillingAddress
                {
                    City = model.BillModel.City,
                    Country = model.BillModel.Country,
                    Address = model.BillModel.Address,
                    ZipCode = model.BillModel.ZipCode
                },
                Card = new shopapp.entity.Card
                {
                    CardName = model.CardModel.CardName,
                    CardNumber = model.CardModel.CardNumber,
                    ExpirationMonth = model.CardModel.ExpirationMonth,
                    ExpirationYear = model.CardModel.ExpirationYear,
                    Cvc = model.CardModel.Cvc
                }
            };

            foreach (var item in model.CartModel.CartItems)
            {
                var orderItem = new entity.OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };
                
                order.OrderItems.Add(orderItem);
            }
            _orderService.Create(order);
        }

        private Payment PaymentProcess(OrderModel model, User user)
        {
            Options options = new Options
            {
                ApiKey = "",
                SecretKey = "",
                BaseUrl = "https://sandbox-api.iyzipay.com"
            };

            CreatePaymentRequest request = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = new Random().Next(111111111, 999999999).ToString(),
                Price = model.CartModel.TotalPrice().ToString(),
                PaidPrice = model.CartModel.TotalPrice().ToString(),
                Currency = Currency.TRY.ToString(),
                Installment = 1,
                BasketId = $"B{new Random().Next(10000, 100000)}",
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString()
            };

            PaymentCard paymentCard = new PaymentCard
            {
                CardHolderName = model.CardModel.CardName,
                CardNumber = model.CardModel.CardNumber,
                ExpireMonth = model.CardModel.ExpirationMonth,
                ExpireYear = model.CardModel.ExpirationYear,
                Cvc = model.CardModel.Cvc,
                RegisterCard = 0
            };
            request.PaymentCard = paymentCard;

            // paymentCard.CardNumber = "5528790000000008";
            // paymentCard.ExpireMonth = "12";
            // paymentCard.ExpireYear = "2030";
            // paymentCard.Cvc = "123";

            Buyer buyer = new Buyer
            {
                Id = $"BY{new Random().Next(100, 999)}",
                Name = model.FirstName,
                Surname = model.LastName,
                GsmNumber = model.Phone,
                Email = model.Email,
                IdentityNumber = "74300864791",
                LastLoginDate = "2015-10-05 12:43:35",
                RegistrationDate = "2013-04-21 15:12:09",
                RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                Ip = "85.34.78.112",
                City = model.ShipModel.City,
                Country = model.ShipModel.Country,
                ZipCode = model.ShipModel.ZipCode
            };
            request.Buyer = buyer;

            Address shippingAddress = new Address
            {
                ContactName = model.FirstName + " " + model.LastName,
                City = model.ShipModel.City,
                Country = model.ShipModel.Country,
                Description = model.ShipModel.Address,
                ZipCode = model.ShipModel.ZipCode
            };
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address
            {
                ContactName = model.FirstName + " " + model.LastName,
                City = model.BillModel.City,
                Country = model.BillModel.Country,
                Description = model.BillModel.Address,
                ZipCode = model.BillModel.ZipCode
            };
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();

            BasketItem basketItem;
            foreach (var item in model.CartModel.CartItems) {
                basketItem = new BasketItem
                {
                    Id = item.ProductId.ToString(),
                    Name = item.Name,
                    Category1 = "Phone",
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = (item.Price * item.Quantity).ToString()
                };

                basketItems.Add(basketItem);
            }
            request.BasketItems = basketItems;

            return Payment.Create(request, options);
        }
    
    }
}