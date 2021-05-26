using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.AzShop.Application
{
    public class ErrorCodes
    {
        public const string SHOP_CREATE_NAME = "shop_create_name";

        public const string SHOP_UPDATE_ID = "shop_update_id";
        public const string SHOP_UPDATE_NAME = "shop_update_name";

        public const string PRODUCT_CREATE_SHOPID = "product_create_shopid";
        public const string PRODUCT_CREATE_PRODUCTCODE = "product_create_productcode";

        public const string PRODUCT_UPDATE_PRODUCTID = "product_update_productid";
        public const string PRODUCT_UPDATE_PRODUCTCODE = "product_update_productcode";
        public const string PRODUCT_UPDATE_TYPE = "product_update_type";

        public const string PRODUCT_UPDATEPRODUCTCODE_PRODUCTID = "product_updateproductcode_productid";
        public const string PRODUCT_UPDATEPRODUCTCODE_PRODUCTCODE = "product_updateproductcode_productcode";
    }
}
