using System.Collections.Generic;
using System.Linq;
using Lama.Common.Extensions;
using Lama.Orleans.Interfaces.Carts.Models;

namespace Lama.Orleans.Workers.Orders.PromoCodeHandlers
{
    public class ProductsPromoCodeHandler : PromoCodeBaseHandler
    {
        public override decimal PriceForPercentDiscount => Price.Items + Price.Bags;

        public override PriceDetails ApplyPromoCode(IReadOnlyCollection<CartProduct> products)
        {
            if (!Modifier.DiscountCategoryId.IsNullOrEmpty())
            {
                if (products.Any(p => p.HasCategory(Modifier.DiscountCategoryId)))
                    Price.Items = CalculatePromoProductsPrice(products);

                return Price;
            }

            return ApplyDiscount(DiscountType.Bags | DiscountType.Items);
        }
    }
}