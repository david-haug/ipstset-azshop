using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Domain.Shops;
using MediatR;

namespace Ipstset.AzShop.Application.Shops.UpdateShop
{
    public class UpdateShopHandler: IRequestHandler<UpdateShopRequest,ShopResponse>
    {
        private IShopRepository _repository;

        public UpdateShopHandler(IShopRepository repository)
        {
            _repository = repository;
        }

        public async Task<ShopResponse> Handle(UpdateShopRequest request, CancellationToken cancellationToken)
        {
            if (!request.User.HasRole(Constants.UserRoles.Admin))
                throw new NotAuthorizedException();

            var shop = await _repository.GetAsync(Guid.Parse(request.Id));
            if (shop==null)
                throw new NotFoundException($"Shop not found for id: {request.Id}");

            var shopChanged = false;
            if (request.Name != shop.Name)
            {
                shop.ChangeName(request.Name);
                shopChanged = true;
            }
                
            if(shopChanged)
                await _repository.SaveAsync(shop);

            return new ShopResponse
            {
                Id = shop.Id.ToString(),
                Name = shop.Name
            };
        }
    }
}
