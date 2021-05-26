using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Extensions;
using Ipstset.AzShop.Domain.Shops;
using MediatR;

namespace Ipstset.AzShop.Application.Shops.GetShop
{
    public class GetShopHandler : IRequestHandler<GetShopRequest, ShopResponse>
    {
        public IShopReadOnlyRepository _repository;

        public GetShopHandler(IShopReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<ShopResponse> Handle(GetShopRequest request, CancellationToken cancellationToken)
        {
            //if (!request.User.HasAccessToShopId(request.Id))
            //    throw new NotAuthorizedException();

            var shop = await _repository.GetByIdAsync(request.Id);
            if (shop == null)
                throw new NotFoundException($"Shop not found for id: {request.Id}");

            return shop;
        }
    }
}
