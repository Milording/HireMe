using System.Collections.Generic;
using System.Linq;
using Lama.Common.Extensions;
using Lama.Orleans.Interfaces.Carts.Models;

namespace Lama.Orleans.Workers.Orders.PromoCodeHandlers
{
    public class DeliveryPromoCodeHandler : PromoCodeBaseHandler
    {
        public override decimal PriceForPercentDiscount => Price.Delivery;

        public override PriceDetails ApplyPromoCode(IReadOnlyCollection<CartProduct> products)
        {
            if (!Modifier.DiscountCategoryId.IsNullOrEmpty())
            {
                if (products.Any(p => p.HasCategory(Modifier.DiscountCategoryId)))
                    Price.Delivery = 0;

                return Price;
            }

            return ApplyDiscount(DiscountType.Delivery);
        }
    }
}