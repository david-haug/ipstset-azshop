using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Extensions;
using Ipstset.AzShop.Application.Specifications;
using Ipstset.AzShop.Domain.Products;
using MediatR;

namespace Ipstset.AzShop.Application.Products.UpdateProduct
{
    public class UpdateProductHandler: IRequestHandler<UpdateProductRequest,ProductResponse>
    {
        private IProductRepository _productRepository;
        private IProductReadOnlyRepository _productReadOnlyRepository;

        public UpdateProductHandler(IProductRepository productRepository,
            IProductReadOnlyRepository productReadOnlyRepository)
        {
            _productRepository = productRepository;
            _productReadOnlyRepository = productReadOnlyRepository;
        }

        public async Task<ProductResponse> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            if (!request.User.HasRole(Constants.UserRoles.Admin))
                throw new NotAuthorizedException();

            var product = await _productRepository.GetAsync(Guid.Parse(request.ProductId));
            if (product == null)
                throw new NotFoundException($"Product not found for id: {request.ProductId}");

            //ProductCode
            if (!string.IsNullOrEmpty(request.ProductCode))
            {
                if (request.ProductCode != product.ProductCode)
                {
                    var specification = new ProductCodeIsUniqueToShop(_productReadOnlyRepository);
                    if (!specification.IsSatisifiedBy(product.ShopId, request.ProductCode))
                        throw new BadRequestException($"Product code: {request.ProductCode} already exists for shop id: {product.ShopId}");
                }

                product.ChangeProductCode(request.ProductCode);
            }

            //Type
            if (!string.IsNullOrEmpty(request.Type))
            {
                product.ChangeType(request.Type);
            }

            await _productRepository.SaveAsync(product);
            return product.ToProductResponse();
        }
    }
}
