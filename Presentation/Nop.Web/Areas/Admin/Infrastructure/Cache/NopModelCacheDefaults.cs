﻿using Nop.Core.Caching;

namespace Nop.Web.Areas.Admin.Infrastructure.Cache
{
    public static partial class NopModelCacheDefaults
    {
        /// <summary>
        /// Key for nopCommerce.com news cache
        /// </summary>
        public static CacheKey OfficialNewsModelKey => new("Nop.pres.admin.official.news");
        
        /// <summary>
        /// Key for categories caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static CacheKey CategoriesListKey => new("Nop.pres.admin.categories.list-{0}", CategoriesListPrefixCacheKey);
        public static string CategoriesListPrefixCacheKey => "Nop.pres.admin.categories.list";

        /// <summary>
        /// Key for manufacturers caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static CacheKey ManufacturersListKey => new("Nop.pres.admin.manufacturers.list-{0}", ManufacturersListPrefixCacheKey);
        public static string ManufacturersListPrefixCacheKey => "Nop.pres.admin.manufacturers.list";

        /// <summary>
        /// Key for vendors caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static CacheKey VendorsListKey => new("Nop.pres.admin.vendors.list-{0}", VendorsListPrefixCacheKey);
        public static string VendorsListPrefixCacheKey => "Nop.pres.admin.vendors.list";

        /// <summary>
        /// Key for nature of business caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static CacheKey NatureOfBusinesssListKey => new("Nop.pres.admin.natureOfBusiness.list-{0}", NatureOfBusinessListPrefixCacheKey);
        public static string NatureOfBusinessListPrefixCacheKey => "Nop.pres.admin.natureOfBusiness.list";
    }
}
