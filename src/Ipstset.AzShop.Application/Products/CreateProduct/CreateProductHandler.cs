using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Exceptions;
using Ipstset.AzShop.Application.Extensions;
using Ipstset.AzShop.Application.Shops;
using Ipstset.AzShop.Domain.Products;
using MediatR;

namespace Ipstset.AzShop.Application.Products.CreateProduct
{
    public class CreateProductHandler: IRequestHandler<CreateProductRequest,ProductResponse>
    {
        private IProductRepository _productRepository;
        private IShopReadOnlyRepository _shopReadOnlyRepository;

        public CreateProductHandler(IProductRepository productRepository, IShopReadOnlyRepository shopReadOnlyRepository)
        {
            _productRepository = productRepository;
            _shopReadOnlyRepository = shopReadOnlyRepository;
        }

        public async Task<ProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        { 
            if (!request.User.HasRole(Constants.UserRoles.Admin))
                throw new NotAuthorizedException();

            var shop = await _shopReadOnlyRepository.GetByIdAsync(request.ShopId);
            if(shop == null)
                throw new BadRequestException($"Shop not found for id: {request.ShopId}");

            var product = Product.Create(Guid.Parse(request.ShopId), request.Type, request.ProductCode, request.Copy, request.IsActive, request.Options, request.Pricing);
            await _productRepository.SaveAsync(product);

            return product.ToProductResponse();
        }
    }
}
