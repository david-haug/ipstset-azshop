using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Application.Specifications;
using Xunit;
namespace Ipstset.AzShop.Application.Tests.Specifications
{
    public class UserHasShopAccessShould
    {
        [Fact]
        public void Return_true_when_admin()
        {
            var user = new AppUser { Roles = new []{"admin"}};
            var sut = new UserHasShopAccess(user);

            var actual = sut.IsSatisifiedBy(Guid.NewGuid().ToString());
            Assert.True(actual);
        }

        [Fact]
        public void Return_true_given_readonly_role_with_shop_id()
        {
            var shopId = Guid.NewGuid().ToString();
            var role = $"readonly_{shopId}";
            var user = new AppUser { Roles = new[] { role } };
            var sut = new UserHasShopAccess(user);

            var actual = sut.IsSatisifiedBy(shopId);
            Assert.True(actual);
        }

        [Fact]
        public void Return_false_given_readonly_role_with_different_shop_id()
        {
            var shopId = Guid.NewGuid().ToString();
            var role = $"readonly_{Guid.NewGuid()}";
            var user = new AppUser { Roles = new[] { role } };
            var sut = new UserHasShopAccess(user);

            var actual = sut.IsSatisifiedBy(shopId);
            Assert.False(actual);
        }

    }
}
