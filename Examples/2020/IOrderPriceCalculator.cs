using System.Collections.Generic;
using Lama.Orleans.Interfaces.Carts;
using Lama.Orleans.Interfaces.Carts.Models;
using Lama.Orleans.Interfaces.PromoCodes.Models;

namespace Lama.Orleans.Interfaces.PromoCodes
{
    public interface IOrderPriceCalculator
    {
        PriceDetails Calculate(PriceDetails originOrderPrice, PromoCodeOrderModifier modifier, List<CartProduct> products);

        long CalculateDeliveryPointsDiscount(decimal deliveryPrice, long availablePoints, decimal shopDeliveryPointsDiscount, PayWithPointsType type);

        decimal CalculateDiscount(PriceDetails originPrice, PromoCodeOrderModifier modifier, List<CartProduct> products);
    }
}
