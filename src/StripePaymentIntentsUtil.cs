using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Stripe.Client.Abstract;
using Soenneker.Stripe.PaymentIntents.Abstract;
using Soenneker.Utils.AsyncSingleton;
using Stripe;

namespace Soenneker.Stripe.PaymentIntents;

///<inheritdoc cref="IStripePaymentIntentsUtil"/>
public class StripePaymentPaymentIntentsUtil : IStripePaymentIntentsUtil
{
    private readonly AsyncSingleton<PaymentIntentService> _service;

    public StripePaymentPaymentIntentsUtil(IStripeClientUtil stripeUtil)
    {
        _service = new AsyncSingleton<PaymentIntentService>(async (cancellationToken, _) =>
        {
            StripeClient client = await stripeUtil.Get(cancellationToken).NoSync();

            var service = new PaymentIntentService(client);
            return service;
        });
    }

    public async ValueTask<PaymentIntent> Create(string stripeCustomerId, decimal amount, CancellationToken cancellationToken = default)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100),
            Currency = "usd",
            Customer = stripeCustomerId,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        };

        PaymentIntent response = await (await _service.Get(cancellationToken).NoSync()).CreateAsync(options, cancellationToken: cancellationToken).NoSync();

        return response;
    }

    public async ValueTask<PaymentIntent> Get(string id, CancellationToken cancellationToken = default)
    {
        PaymentIntent response = await (await _service.Get(cancellationToken).NoSync()).GetAsync(id, cancellationToken: cancellationToken).NoSync();

        return response;
    }

    public async ValueTask<PaymentIntent> Update(string id, Dictionary<string, string> metadata, CancellationToken cancellationToken = default)
    {
        var options = new PaymentIntentUpdateOptions
        {
            Metadata = metadata
        };

        PaymentIntent response = await (await _service.Get(cancellationToken).NoSync()).UpdateAsync(id, options, cancellationToken: cancellationToken).NoSync();

        return response;
    }

    public async ValueTask<PaymentIntent> Confirm(string id, string returnUrl, CancellationToken cancellationToken = default)
    {
        var options = new PaymentIntentConfirmOptions
        {
            ReturnUrl = returnUrl
        };

        PaymentIntent response = await (await _service.Get(cancellationToken).NoSync()).ConfirmAsync(id, options, cancellationToken: cancellationToken).NoSync();

        return response;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _service.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        return _service.DisposeAsync();
    }
}