using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Extensions;
using MediatR;

namespace Ipstset.AzShop.Application.Products.GetProducts
{
    public class GetProductsHandler: IRequestHandler<GetProductsRequest, QueryResult<ProductResponse>>
    {
        private IProductReadOnlyRepository _repository;

        public GetProductsHandler(IProductReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<QueryResult<ProductResponse>> Handle(GetProductsRequest request, CancellationToken cancellationToken)
        {
            //if (!request.User.HasRole(Constants.UserRoles.Admin))
            //{
            //    //if not admin, limit product access to shopId
            //    if (string.IsNullOrWhiteSpace(request.ShopId))
            //        throw new BadRequestException("shopId required");

            //    ////check if user has access to shopid...
            //    //if(!request.User.HasAccessToShopId(request.ShopId))
            //    //    throw new NotAuthorizedException();
            //}
            
            return await _repository.GetProductsAsync(request);
        }
    }
}
