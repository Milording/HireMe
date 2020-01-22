using System.Collections.Generic;
using System.Linq;
using Lama.Common.Extensions;
using Lama.Orleans.Interfaces.Carts.Models;

namespace Lama.Orleans.Workers.Orders.PromoCodeHandlers
{
    public class ProductPromoCodeHandler : PromoCodeBaseHandler
    {
        public override decimal PriceForPercentDiscount => Price.Items + Price.Bags;

        public override PriceDetails ApplyPromoCode(IReadOnlyCollection<CartProduct> products)
        {
            if (products.IsNullOrEmpty())
                return Price;

            var discountProducts = products
                .Where(product => product.Id == Modifier.DiscountProductId)
                .ToList();
            var productsDiscount = CalculateProductsPercentDiscount(discountProducts, Modifier.DiscountPercent.GetValueOrDefault())
                .AdjustPrecision();
            if (discountProducts.Sum(product => product.Qty) > Modifier.MaxDiscountProductAmount)
                productsDiscount = 0;

            Price.Items = OriginOrderPrice.Items - productsDiscount;

            return Price;
        }


        private static decimal CalculateProductsPercentDiscount(List<CartProduct> products, decimal discountPercent)
            => products.Sum(product => product.Total) * discountPercent / (decimal) 100.0;
    }
}