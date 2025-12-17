using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CornerShop.Core.Models
{

    public enum ProductCategory
    {
        Beverage,
        Fruit,
        Dairy,
        Vegetable,
        Meat,
        Frozen,
    }
    public enum UserLevel
    {
        Standard,
        Gold,
        Silver,
        Bronze,

    }
    public enum UserRole
    {
        Admin,
        Customer
    }

    public enum Currency
    {
        SEK,
        USD,
        EUR,
    }

}
