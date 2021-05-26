using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Ipstset.AzShop.Application.Helpers;

namespace Ipstset.AzShop.Application.Products.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.ShopId).Must(ValidationHelper.ValidateId).WithErrorCode(ErrorCodes.PRODUCT_CREATE_SHOPID).WithMessage("not a valid id");
            RuleFor(x => x.ProductCode).NotEmpty().WithErrorCode(ErrorCodes.PRODUCT_CREATE_PRODUCTCODE).WithMessage("required");
        }
    }
}
