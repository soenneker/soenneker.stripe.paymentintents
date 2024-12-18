using Soenneker.Stripe.PaymentIntents.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;


namespace Soenneker.Stripe.PaymentIntents.Tests;

[Collection("Collection")]
public class StripePaymentPaymentIntentsUtilTests : FixturedUnitTest
{
    private readonly IStripePaymentIntentsUtil _util;

    public StripePaymentPaymentIntentsUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IStripePaymentIntentsUtil>();
    }
}
