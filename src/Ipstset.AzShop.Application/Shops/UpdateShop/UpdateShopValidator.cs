using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Ipstset.AzShop.Application.Shops.UpdateShop
{
    public class UpdateShopValidator : AbstractValidator<UpdateShopRequest>
    {
        public UpdateShopValidator()
        {
            RuleFor(x => x.Id).Must(ValidateId).WithErrorCode(ErrorCodes.SHOP_UPDATE_ID).WithMessage("not a valid id");
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(ErrorCodes.SHOP_UPDATE_NAME).WithMessage("required");
        }

        private bool ValidateId(string id)
        {
            return Guid.TryParse(id, out var result);
        }


    }
}
