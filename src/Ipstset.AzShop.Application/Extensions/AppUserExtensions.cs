using System;
using System.Collections.Generic;
using System.Text;
using Ipstset.AzShop.Application.Specifications;

namespace Ipstset.AzShop.Application.Extensions
{
    public static class AppUserExtensions
    {
        //public static bool HasAccessToFeedResponse(this AppUser user, FeedResponse feedResponse)
        //{
        //    var specification = new UserHasFeedAccess(user);
        //    return specification.IsSatisifedBy(feedResponse);
        //}

        public static bool HasAccessToShopId(this AppUser user, string shopId)
        {
            var specification = new UserHasShopAccess(user);
            return specification.IsSatisifiedBy(shopId);
        }
    }
}
