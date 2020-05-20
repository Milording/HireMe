using System.Collections.Generic;
using Lama.Orleans.Interfaces.Carts.Models;

namespace Lama.Orleans.Workers.Orders.PromoCodeHandlers
{
    public class AnyPromoCodeHandler : PromoCodeBaseHandler
    {
        public override decimal PriceForPercentDiscount => Price.Total;

        public override PriceDetails ApplyPromoCode(IReadOnlyCollection<CartProduct> products) =>
            ApplyDiscount(DiscountType.Delivery | DiscountType.Bags | DiscountType.Items);
    }
}