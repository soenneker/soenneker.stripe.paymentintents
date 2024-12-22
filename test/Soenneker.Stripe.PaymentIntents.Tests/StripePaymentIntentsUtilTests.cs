using Soenneker.Stripe.PaymentIntents.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Stripe.PaymentIntents.Tests;

[Collection("Collection")]
public class StripePaymentIntentsUtilTests : FixturedUnitTest
{
    private readonly IStripePaymentIntentsUtil _util;

    public StripePaymentIntentsUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IStripePaymentIntentsUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
