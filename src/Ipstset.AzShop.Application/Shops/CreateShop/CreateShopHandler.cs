using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Domain.Shops;
using MediatR;

namespace Ipstset.AzShop.Application.Shops.CreateShop
{
    public class CreateShopHandler : IRequestHandler<CreateShopRequest, ShopResponse>
    {
        private IShopRepository _repository;

        public CreateShopHandler(IShopRepository shopRepository)
        {
            _repository = shopRepository;
        }

        public async Task<ShopResponse> Handle(CreateShopRequest request, CancellationToken cancellationToken)
        {
            if (!request.User.HasRole(Constants.UserRoles.Admin))
                throw new NotAuthorizedException();

            var shop = Shop.Create(request.Name);
            await _repository.SaveAsync(shop);

            return new ShopResponse
            {
                Id = shop.Id.ToString(),
                Name = shop.Name
            };
        }
    }
}
