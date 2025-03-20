using MercadoLibro.Features.CartFeature.Repositories;
using MercadoLibro.Features.General.DTOs;
using MercadoLibro.Features.RefreshTokenFeature;
using MercadoLibro.Features.UserFeature.Repository;
using MercadoLibroDB.Models;

namespace MercadoLibro.Features.CartFeature.Service
{
    public class CartService(
        CartRepository cartRepository,
        CartLineRepository cartLineRepository,
        UserRepository userRepository
    )
    {
        public List<ErrorHttp> Errors = [];

        readonly CartRepository _cartRepository = cartRepository;
        readonly CartLineRepository _cartLineRepository = cartLineRepository;
        readonly UserRepository _userRepository = userRepository;

        public async Task<Cart?> Get(string? email)
        {
            Cart? cart;
            User? user;

            if (email is null)
            {
                Errors.Add(new ErrorHttp("Error, invalid token", 400));
                return null;
            }

            user = await _userRepository.Get(email);

            if (user is null)
            {
                Errors.Add(new ErrorHttp("User not exist", 400));
                return null;
            }

            cart = await _cartRepository.Get(user.Id);

            if (cart is null)
            {
                await CreateCart(email);
                if (HasErrors()) return null;
            }
            else
                cart!.CartLine = await _cartLineRepository.GetAll(user.Id);
            
            return cart;
        }

        public async Task Add(
            string? email,
            string isbn,
            int quantity
        )
        {
            User? user;

            if (email is null)
            {
                Errors.Add(new ErrorHttp("Error, invalid token", 400));
                return;
            }

            user = await _userRepository.Get(email);

            if(user is null)
            {
                Errors.Add(new ErrorHttp("User not exist", 400));
                return;
            }

            _ = await _cartRepository.Get(user.Id) ?? await CreateCart(email);

            if (HasErrors()) return;

            //verify if the isbn exist

            CartLine cartLine = new()
            {
                ISBN = isbn,
                Quantity = quantity,
                UserID = user.Id
            };

            await _cartLineRepository.AddAsync(cartLine);
            await _cartLineRepository.SaveChangesAsync();
        }

        public async Task Delete(
            string? email,
            string isbn
        )
        {
            CartLine? cartLine;
            User? user;

            if (email is null)
            {
                Errors.Add(new ErrorHttp("Error, invalid token", 400));
                return;
            }

            user = await _userRepository.Get(email);

            if (user is null)
            {
                Errors.Add(new ErrorHttp("User not exist", 400));
                return;
            }

            cartLine = await _cartLineRepository.Remove(user.Id, isbn);

            if(cartLine is null)
            {
                Errors.Add(new ErrorHttp("Error to try remove the book", 400));
                return;
            }

            await _cartLineRepository.SaveChangesAsync();
        }

        private async Task<Cart?> CreateCart(string? email)
        {
            User? user;

            if (email is null)
            {
                Errors.Add(new ErrorHttp("email is null", 400));
                return null;
            }

            user = await _userRepository.Get(email);
            
            if(user is null)
            {
                Errors.Add(new ErrorHttp("User not exist", 400));
                return null;
            }

            Cart cart = new()
            {
                UserID = user.Id
            };
            await _cartRepository.AddAsync(cart);
            await _cartRepository.SaveChangesAsync();

            return cart;
        }

        public bool HasErrors() =>
            Errors.Count > 0;
        
    }
}
