using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.AzShop.Application.Specifications
{
    public class UserHasShopAccess : ISpecification<string>
    {
        private AppUser _user;
        public UserHasShopAccess(AppUser user)
        {
            _user = user;
        }

        public bool IsSatisifiedBy(string shopId)
        {
            return _user.HasRole(Constants.UserRoles.Admin) || _user.HasRole($"readonly_{shopId}");
        }
    }
}
