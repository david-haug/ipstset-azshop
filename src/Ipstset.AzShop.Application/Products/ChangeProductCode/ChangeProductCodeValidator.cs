using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Ipstset.AzShop.Application.Helpers;

namespace Ipstset.AzShop.Application.Products.ChangeProductCode
{
    public class ChangeProductCodeValidator : AbstractValidator<ChangeProductCodeRequest>
    {
        public ChangeProductCodeValidator()
        {
            RuleFor(x => x.ProductId).Must(ValidationHelper.ValidateId).WithErrorCode(ErrorCodes.PRODUCT_UPDATEPRODUCTCODE_PRODUCTID).WithMessage("not a valid id");
            RuleFor(x => x.ProductCode).NotEmpty().WithErrorCode(ErrorCodes.PRODUCT_UPDATEPRODUCTCODE_PRODUCTCODE).WithMessage("required");
        }
    }
}
