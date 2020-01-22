using System;
using Lama.Orleans.Interfaces.Carts.Models;
using Lama.Orleans.Interfaces.PromoCodes.Models;

namespace Lama.Orleans.Workers.Orders.PromoCodeHandlers
{
    public static class PromoCodeHandlerFactory
    {
        public static IPromoCodeHandler Create(PriceDetails originOrderOrderPrice, PromoCodeOrderModifier modifier)
        {
            var price = new PriceDetails
            {
                Delivery = originOrderOrderPrice.Delivery,
                Items = originOrderOrderPrice.Items,
                Bags = originOrderOrderPrice.Bags
            };

            var handler = GetPromoCodeHandlerByScope(modifier.Scope);
            handler.Price = price;
            handler.OriginOrderPrice = originOrderOrderPrice;
            handler.Modifier = modifier;
            handler.RemainedDiscount = handler.GetDiscountValue(modifier, handler.PriceForPercentDiscount);
            return handler;
        }

        private static PromoCodeBaseHandler GetPromoCodeHandlerByScope(PromoCodeScope scope) =>
            scope switch
            {
                PromoCodeScope.Delivery => new DeliveryPromoCodeHandler(),
                PromoCodeScope.Products => new ProductsPromoCodeHandler(),
                PromoCodeScope.ProductsWithFreeDelivery => new ProductsWithFreeDeliveryPromoCodeHandler(),
                PromoCodeScope.Any => new AnyPromoCodeHandler(),
                PromoCodeScope.Product => new ProductPromoCodeHandler(),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(scope),
                    scope,
                    "It's not possible to create a promo code handler for this scope.")
            };
    }
}