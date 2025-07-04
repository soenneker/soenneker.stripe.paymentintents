using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Enums.CurrencyCodes;
using Soenneker.Extensions.String;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Stripe.Client.Abstract;
using Soenneker.Stripe.PaymentIntents.Abstract;
using Soenneker.Utils.AsyncSingleton;
using Stripe;

namespace Soenneker.Stripe.PaymentIntents;

///<inheritdoc cref="IStripePaymentIntentsUtil"/>
public sealed class StripePaymentIntentsUtil : IStripePaymentIntentsUtil
{
    private readonly AsyncSingleton<PaymentIntentService> _paymentIntentService;

    public StripePaymentIntentsUtil(IStripeClientUtil stripeUtil)
    {
        _paymentIntentService = new AsyncSingleton<PaymentIntentService>(async (cancellationToken, _) =>
        {
            StripeClient client = await stripeUtil.Get(cancellationToken).NoSync();
            return new PaymentIntentService(client);
        });
    }

    public async ValueTask<PaymentIntent> Create(string stripeCustomerId, decimal amount, string? idempotencyKey = null,
        IEnumerable<string>? paymentMethodTypes = null, PaymentIntentAutomaticPaymentMethodsOptions? automaticPaymentMethods = null,
        CancellationToken cancellationToken = default)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long) (amount * 100),
            Currency = CurrencyCode.Usd,
            Customer = stripeCustomerId,
            AutomaticPaymentMethods = automaticPaymentMethods ?? new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        };

        if (paymentMethodTypes != null)
            options.PaymentMethodTypes = [.. paymentMethodTypes];

        PaymentIntentService service = await _paymentIntentService.Get(cancellationToken).NoSync();

        if (idempotencyKey.IsNullOrEmpty())
            return await service.CreateAsync(options, cancellationToken: cancellationToken).NoSync();

        var requestOptions = new RequestOptions
        {
            IdempotencyKey = idempotencyKey
        };

        return await service.CreateAsync(options, requestOptions, cancellationToken).NoSync();
    }

    public async ValueTask<PaymentIntent> Get(string id, CancellationToken cancellationToken = default)
    {
        PaymentIntentService service = await _paymentIntentService.Get(cancellationToken).NoSync();
        return await service.GetAsync(id, cancellationToken: cancellationToken).NoSync();
    }

    public async ValueTask<PaymentIntent> Update(string id, Dictionary<string, string> metadata, CancellationToken cancellationToken = default)
    {
        var options = new PaymentIntentUpdateOptions
        {
            Metadata = metadata
        };

        PaymentIntentService service = await _paymentIntentService.Get(cancellationToken).NoSync();
        return await service.UpdateAsync(id, options, cancellationToken: cancellationToken).NoSync();
    }

    public async ValueTask<PaymentIntent> Confirm(string id, string returnUrl, CancellationToken cancellationToken = default)
    {
        var options = new PaymentIntentConfirmOptions
        {
            ReturnUrl = returnUrl
        };

        PaymentIntentService service = await _paymentIntentService.Get(cancellationToken).NoSync();
        return await service.ConfirmAsync(id, options, cancellationToken: cancellationToken).NoSync();
    }

    public async ValueTask<PaymentIntent> Cancel(string id, CancellationToken cancellationToken = default)
    {
        PaymentIntentService service = await _paymentIntentService.Get(cancellationToken).NoSync();
        return await service.CancelAsync(id, cancellationToken: cancellationToken).NoSync();
    }

    public async ValueTask<PaymentIntent> Capture(string id, CancellationToken cancellationToken = default)
    {
        PaymentIntentService service = await _paymentIntentService.Get(cancellationToken).NoSync();
        return await service.CaptureAsync(id, cancellationToken: cancellationToken).NoSync();
    }

    public async ValueTask<IEnumerable<PaymentIntent>> List(string customerId, CancellationToken cancellationToken = default)
    {
        var options = new PaymentIntentListOptions
        {
            Customer = customerId,
            Limit = 100
        };

        PaymentIntentService service = await _paymentIntentService.Get(cancellationToken).NoSync();
        return await service.ListAsync(options, cancellationToken: cancellationToken).NoSync();
    }

    public void Dispose()
    {
        _paymentIntentService.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _paymentIntentService.DisposeAsync();
    }
}