using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipstset.AzShop.Application.Products.GetProduct;

namespace Ipstset.AzShop.Api
{
    public class Constants
    {
        //todo: make secretless - move to environment settings
        //public const string ClientCredentialsClientIdForSystemUser = "dev_client";
        //public const string SystemUserId = "36015f54-cb54-4e29-9724-ec00a9bca9a8";
        public const int MaxRequestLimit = 100;

        public class Routes
        {
            public class Shops
            {
                public const string GetShop = "GetShop";
                public const string CreateShop = "CreateShop";
                public const string UpdateShop = "UpdateShop";
                //public const string DeleteFeed = "DeleteFeed";
            }

            public class Products
            {
                public const string GetProduct = "GetProduct";
                public const string CreateProduct = "CreateProduct";
                public const string UpdateProduct = "UpdateProduct";

                public const string ActivateProduct = "ActivateProduct";
                public const string DeactivateProduct = "DeactivateProduct";

                public const string UpdateCopy = "UpdateCopy";
                public const string UpdatePricing = "UpdatePricing";
                public const string UpdateOptions = "UpdateOptions";
            }

            public class Tokens
            {
                public const string CreateToken = "CreateToken";
            }

        }
    }
}
