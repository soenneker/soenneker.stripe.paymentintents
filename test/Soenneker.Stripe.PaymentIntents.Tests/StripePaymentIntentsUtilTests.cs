using Soenneker.Stripe.PaymentIntents.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Stripe.PaymentIntents.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class StripePaymentIntentsUtilTests : HostedUnitTest
{
    private readonly IStripePaymentIntentsUtil _util;

    public StripePaymentIntentsUtilTests(Host host) : base(host)
    {
        _util = Resolve<IStripePaymentIntentsUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
