using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Extensions;
using MediatR;

namespace Ipstset.AzShop.Application.Products.GetProduct
{
    public class GetProductHandler: IRequestHandler<GetProductRequest, ProductResponse>
    {
        public IProductReadOnlyRepository _repository;

        public GetProductHandler(IProductReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductResponse> Handle(GetProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null)
                throw new NotFoundException($"Product not found for id: {request.Id}");

            //if (!request.User.HasAccessToShopId(product.ShopId))
            //    throw new NotAuthorizedException();

            return product;
        }
    }
}
