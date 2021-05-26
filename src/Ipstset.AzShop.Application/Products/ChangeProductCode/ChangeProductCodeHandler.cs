using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Specifications;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.ValueObjects;
using MediatR;

namespace Ipstset.AzShop.Application.Products.ChangeProductCode
{
    public class ChangeProductCodeHandler: IRequestHandler<ChangeProductCodeRequest, ProductResponse>
    {
        private IProductRepository _repository;
        private IProductReadOnlyRepository _readOnlyRepository;

        public ChangeProductCodeHandler(IProductRepository repository, IProductReadOnlyRepository readOnlyRepository)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task<ProductResponse> Handle(ChangeProductCodeRequest request, CancellationToken cancellationToken)
        {
            if (!request.User.HasRole(Constants.UserRoles.Admin))
                throw new NotAuthorizedException();

            var product = await _repository.GetAsync(Guid.Parse(request.ProductId));
            if (product == null)
                throw new NotFoundException($"Product not found for id: {request.ProductId}");

            var specification = new ProductCodeIsUniqueToShop(_readOnlyRepository);
            if(!specification.IsSatisifiedBy(product.ShopId, request.ProductCode))
                throw new BadRequestException($"Product code: {request.ProductId} already exists for shop id: {product.ShopId}");

            product.ChangeProductCode(request.ProductCode);
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
