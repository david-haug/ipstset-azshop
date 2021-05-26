using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Extensions;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.ValueObjects;
using MediatR;

namespace Ipstset.AzShop.Application.Products.UpdateCopy
{
    public class UpdateCopyHandler:IRequestHandler<UpdateCopyRequest, ProductResponse>
    {
        private IProductRepository _repository;

        public UpdateCopyHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductResponse> Handle(UpdateCopyRequest request, CancellationToken cancellationToken)
        {
            if (!request.User.HasRole(Constants.UserRoles.Admin))
                throw new NotAuthorizedException();

            var product = await _repository.GetAsync(Guid.Parse(request.ProductId));
            if (product == null)
                throw new NotFoundException($"Product not found for id: {request.ProductId}");

            product.UpdateCopy(new ProductCopy
            {
                Title = request.Title,
                Description = request.Description,
                ShortDescription = request.ShortDescription,
                ThumbnailUrl = request.ThumbnailUrl
            });

            await _repository.SaveAsync(product);

            return product.ToProductResponse();
        }
    }
}
