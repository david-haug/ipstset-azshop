using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Ipstset.AzShop.Application.Shops.CreateShop
{
    public class CreateShopValidator: AbstractValidator<CreateShopRequest>
    {
        public CreateShopValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(ErrorCodes.SHOP_CREATE_NAME).WithMessage("required");
        }
    }
}
