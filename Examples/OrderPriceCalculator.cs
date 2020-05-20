using System;
using System.Collections.Generic;
using Lama.Common.Extensions;
using Lama.Orleans.Interfaces.Carts;
using Lama.Orleans.Interfaces.Carts.Models;
using Lama.Orleans.Interfaces.PromoCodes;
using Lama.Orleans.Interfaces.PromoCodes.Models;
using Lama.Orleans.Workers.Orders.PromoCodeHandlers;

namespace Lama.Orleans.Workers.Orders
{
    public class OrderPriceCalculator : IOrderPriceCalculator
    {
        public PriceDetails Calculate(PriceDetails originOrderPrice, PromoCodeOrderModifier modifier, List<CartProduct> products)
        {
            var price = PriceDetails.AdjustPrecision(originOrderPrice);
            price.Delivery = originOrderPrice.Delivery - originOrderPrice.DeliveryDiscount;

            if (modifier == null
                || price.Points > 0
                || price.DeliveryPoints > 0)
                return price;

            var promoCodeHandler = PromoCodeHandlerFactory.Create(price, modifier);

            return promoCodeHandler.ApplyPromoCode(products);
        }

        public long CalculateDeliveryPointsDiscount(decimal deliveryPrice, long availablePoints, decimal shopDeliveryPointsDiscount, PayWithPointsType type)
        {
            if (type != PayWithPointsType.Delivery
                || availablePoints < shopDeliveryPointsDiscount)
                return 0;

            return Math.Min(deliveryPrice, shopDeliveryPointsDiscount).Ceiling();
        }

        public decimal CalculateDiscount(PriceDetails originPrice, PromoCodeOrderModifier modifier, List<CartProduct> products)
        {
            if (modifier == null)
                return 0;

            var discountedPrice = Calculate(originPrice, modifier, products);

            return (originPrice.Total - discountedPrice.Total).AdjustPrecision();
        }
    }
}
