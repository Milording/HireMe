using System.Collections.Generic;
using Lama.Orleans.Interfaces.Carts.Models;
using Lama.Orleans.Interfaces.PromoCodes.Models;

namespace Lama.Orleans.Workers.Orders.PromoCodeHandlers
{
    public interface IPromoCodeHandler
    {
        PriceDetails Price { get; set; }
        PromoCodeOrderModifier Modifier { get; set; }

        PriceDetails ApplyPromoCode(IReadOnlyCollection<CartProduct> products);

    }
}