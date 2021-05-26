using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Extensions;
using Ipstset.AzShop.Domain.Products;
using MediatR;

namespace Ipstset.AzShop.Application.Products.DeactivateProduct
{
    public class DeactivateProductHandler: IRequestHandler<DeactivateProductRequest,ProductResponse>
    {
        private IProductRepository _repository;

        public DeactivateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductResponse> Handle(DeactivateProductRequest request, CancellationToken cancellationToken)
        {
            if (!request.User.HasRole(Constants.UserRoles.Admin))
                throw new NotAuthorizedException();

            var product = await _repository.GetAsync(Guid.Parse(request.ProductId));
            if (product == null)
                throw new NotFoundException($"Product not found for id: {request.ProductId}");

            product.Deactivate();
            await _repository.SaveAsync(product);

            return product.ToProductResponse();
        }
    }
}
