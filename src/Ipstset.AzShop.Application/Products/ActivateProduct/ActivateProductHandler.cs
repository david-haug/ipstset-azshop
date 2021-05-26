using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Domain.Products;
using MediatR;

namespace Ipstset.AzShop.Application.Products.ActivateProduct
{
    public class ActivateProductHandler:IRequestHandler<ActivateProductRequest,ProductResponse>
    {
        private IProductRepository _repository;

        public ActivateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductResponse> Handle(ActivateProductRequest request, CancellationToken cancellationToken)
        {
            if (!request.User.HasRole(Constants.UserRoles.Admin))
                throw new NotAuthorizedException();

            var product = await _repository.GetAsync(Guid.Parse(request.ProductId));
            if(product==null)
                throw new NotFoundException($"Product not found for id: {request.ProductId}");

            product.Activate();
            await _repository.SaveAsync(product);

            return new ProductResponse
            {
                Id = product.Id.ToString(),
                ShopId = product.ShopId.ToString(),
                ProductCode = product.ProductCode,
                Type = product.Type,
                Copy = product.Copy,
                IsActive = product.IsActive,
                Pricing = product.Pricing,
                Options = product.Options
            };
        }
    }
}
