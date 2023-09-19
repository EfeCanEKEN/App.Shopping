using App.Data.Abstract;
using App.Data.Context;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace App.Mvc.Controllers
{
    [Authorize]
    public class CartController : Controller
    {

        private readonly IRepository<CartEntity> _cartRepository;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IRepository<CartItemEntity> _cartItemRepository;


        public CartController(IRepository<CartEntity> cartRepository, IRepository<UserEntity> userRepository, IRepository<CartItemEntity> cartItemRepository)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _cartItemRepository = cartItemRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var email = User.Identity.Name;

            var user = await _userRepository.Get().FirstOrDefaultAsync(u => u.Email == email);


            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var cart = await _cartRepository.Get()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == user.Id && c.Name == "DefaultCart");

            if (cart == null)
            {
                cart = new CartEntity { UserId = user.Id, CreatedAt = DateTime.Now, Name = "DefaultCart" };
                _cartRepository.Create(cart);
            }

            return View(cart);
        }


        public async Task<IActionResult> List()
        {
            var email = User.Identity.Name;

            var user = await _userRepository.Get().Include(u => u.Carts).FirstOrDefaultAsync(u => u.Email == email);

            return View(user.Carts);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var email = User.Identity.Name;
            var user = await _userRepository.Get().FirstOrDefaultAsync(u => u.Email == email);

            var cart = await _cartRepository.Get().Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.Id == id && c.UserId == user.Id);

            if (cart == null)
                return NotFound();

            decimal total = 0m;

            foreach (var item in cart.CartItems)
                total += item.Quantity * item.Product.Price;

            ViewBag.TotalPrice = total;

            return View(cart);
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> New([FromForm] CartEntity carts)
        {
            var email = User.Identity.Name;
            var user = await _userRepository.Get().FirstOrDefaultAsync(u => u.Email == email);

            var cartName = Request.Form["cartName"]; 
            var cart = new CartEntity { UserId = user.Id, Name = cartName, CreatedAt = DateTime.Now };
            _cartRepository.Create(cart);


            return RedirectToAction("List");
        }

       

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity, int cartId)
        {
            var email = User.Identity.Name;

            var user = await _userRepository.Get().FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var cart = await _cartRepository.Get()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == user.Id && c.Id == cartId);

            if (cart == null)
            {
                cart = await _cartRepository.Get()
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == user.Id && c.Name == "DefaultCart");

                if (cart == null)
                {
                    cart = new CartEntity { UserId = user.Id, CreatedAt = DateTime.Now, Name = "DefaultCart" };
                    _cartRepository.Create(cart);
                }
            }

            var cartItem = _cartItemRepository.Get()?.FirstOrDefault(ci => ci.ProductId == productId);

            
                cartItem = new CartItemEntity { CartId = cart.Id, ProductId = productId, Quantity = quantity };

                cart.CartItems.Add(cartItem);
                _cartRepository.Update(cart.Id, cart);


            return RedirectToAction("Index", "Home");
        }

    }
}