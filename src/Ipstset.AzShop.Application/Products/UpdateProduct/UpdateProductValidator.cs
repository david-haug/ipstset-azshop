using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FluentValidation;
using Ipstset.AzShop.Application.Helpers;

namespace Ipstset.AzShop.Application.Products.UpdateProduct
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.ProductId).Must(ValidationHelper.ValidateId).WithErrorCode(ErrorCodes.PRODUCT_UPDATE_PRODUCTID).WithMessage("not a valid id");
            RuleFor(x => x.ProductCode).NotEmpty().When(x => string.IsNullOrEmpty(x.Type))
                .WithErrorCode(ErrorCodes.PRODUCT_UPDATE_PRODUCTCODE).WithMessage("required when type is empty");
            RuleFor(x => x.Type).NotEmpty().When(x => string.IsNullOrEmpty(x.ProductCode))
                .WithErrorCode(ErrorCodes.PRODUCT_UPDATE_TYPE).WithMessage("required when product code is empty");
        }
    }
}
