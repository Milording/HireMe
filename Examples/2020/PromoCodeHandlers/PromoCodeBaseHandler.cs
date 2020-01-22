using System;
using System.Collections.Generic;
using System.Linq;
using Lama.Common.Extensions;
using Lama.Orleans.Interfaces.Carts.Models;
using Lama.Orleans.Interfaces.PromoCodes.Models;

namespace Lama.Orleans.Workers.Orders.PromoCodeHandlers
{
    public abstract class PromoCodeBaseHandler : IPromoCodeHandler
    {
        public decimal RemainedDiscount { get; set; }
        public virtual decimal PriceForPercentDiscount { get; set; }
        public PriceDetails Price { get; set; }
        public PriceDetails OriginOrderPrice { get; set; }
        public PromoCodeOrderModifier Modifier { get; set; }

        public virtual PriceDetails ApplyPromoCode(IReadOnlyCollection<CartProduct> products) => Price;

        internal decimal ApplyDiscount(decimal amountToApply)
        {
            if (amountToApply > RemainedDiscount)
            {
                var valueAfterDiscount = amountToApply - RemainedDiscount;
                RemainedDiscount = 0;
                return valueAfterDiscount;
            }

            RemainedDiscount -= amountToApply;
            return 0;
        }

        internal PriceDetails ApplyDiscount(DiscountType discount)
        {
            if (discount.HasFlag(DiscountType.Delivery))
                Price.Delivery = ApplyDiscount(Price.Delivery).AdjustPrecision();
            if (discount.HasFlag(DiscountType.Bags))
                Price.Bags = ApplyDiscount(Price.Bags).AdjustPrecision();
            if (discount.HasFlag(DiscountType.Items))
                Price.Items = ApplyDiscount(Price.Items).AdjustPrecision();

            return Price;
        }

        public decimal GetDiscountValue(PromoCodeOrderModifier modifier, decimal priceForPercentDiscount)
        {
            if (modifier.MaxDiscountPercent > 0)
            {
                var percentDiscount = priceForPercentDiscount / 100 * modifier.MaxDiscountPercent.Value;

                return Math.Min(percentDiscount, modifier.OnetimeDiscountLimit ?? modifier.Discount.GetValueOrDefault());
            }

            return modifier.OnetimeDiscountLimit ?? modifier.Discount.GetValueOrDefault();
        }

        protected decimal CalculatePromoProductsPrice(IReadOnlyCollection<CartProduct> products)
        {
            var discountProducts = products.Where(p => p.HasCategory(Modifier.DiscountCategoryId)).ToList();

            var productsPriceAfterDiscount = ApplyDiscount(discountProducts.Sum(product => product.Total));
            var notDiscountedProductsPrice = products.Sum(p => p.Total) - discountProducts.Sum(p => p.Total);

            return notDiscountedProductsPrice + productsPriceAfterDiscount;
        }

        [Flags]
        internal enum DiscountType
        {
            Items = 1,
            Delivery = 2,
            Bags = 4
        }
    }
}
