using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Stripe.Client;
using Soenneker.Stripe.Client.Abstract;
using Soenneker.Stripe.OpenApiClient.Models;
using Soenneker.Stripe.PaymentIntents.Abstract;

namespace Soenneker.Stripe.PaymentIntents;

public sealed class StripePaymentIntentsUtil(IStripeClientUtil client) : IStripePaymentIntentsUtil
{
    public async ValueTask<PaymentIntent> Create(string stripeCustomerId, decimal amount, string? idempotencyKey = null,
        IEnumerable<string>? paymentMethodTypes = null, PostPaymentIntentsXWwwFormUrlencodedRequestAutomaticPaymentMethods? automaticPaymentMethods = null, CancellationToken cancellationToken = default)
    {
        var options = new PostPaymentIntentsXWwwFormUrlencodedRequest
        {
            Amount = checked((int)decimal.Round(amount * 100, 0, MidpointRounding.AwayFromZero)),
            Currency = "usd", Customer = stripeCustomerId
        };
        if (paymentMethodTypes != null)
            options.AllowedPaymentMethodTypes = paymentMethodTypes.Select(value => (PostPaymentIntentsXWwwFormUrlencodedRequestAllowedPaymentMethodTypesItem?)
                Microsoft.Kiota.Abstractions.Helpers.EnumHelpers.GetEnumValue<PostPaymentIntentsXWwwFormUrlencodedRequestAllowedPaymentMethodTypesItem>(value)).ToList();
        else
            options.AutomaticPaymentMethods = automaticPaymentMethods ?? new() { Enabled = true };
        var api = await client.Get(cancellationToken);
        return await api.V1.Payment_intents.PostAsync(options, c => new StripeRequestOptions { IdempotencyKey = idempotencyKey }.Apply(c.Headers), cancellationToken)
            ?? throw new InvalidOperationException("Stripe returned an empty payment intent.");
    }

    public async ValueTask<PaymentIntent> Confirm(string id, string returnUrl, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        return await api.V1.Payment_intents[id].Confirm.PostAsync(new() { ReturnUrl = returnUrl }, cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Stripe returned an empty payment intent.");
    }
    public async ValueTask<PaymentIntent> Get(string id, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        return await api.V1.Payment_intents[id].GetAsync(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Stripe returned an empty intent.");
    }

    public async ValueTask<PaymentIntent> Update(string id, Dictionary<string, string> metadata, CancellationToken cancellationToken = default)
    {
        var options = new PostPaymentIntentsIntentXWwwFormUrlencodedRequest();
        options.Metadata = new() { PostPaymentIntentsIntentXWwwFormUrlencodedRequestMetadataAnyOf1 = new() { AdditionalData = metadata.ToDictionary(pair => pair.Key, pair => (object)pair.Value) } };
        var api = await client.Get(cancellationToken);
        return await api.V1.Payment_intents[id].PostAsync(options, cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Stripe returned an empty intent.");
    }

    public async ValueTask<IEnumerable<PaymentIntent>> List(string customerId, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        var result = new List<PaymentIntent>();
        string? cursor = null;
        do
        {
            var page = await api.V1.Payment_intents.GetAsync(c => { c.QueryParameters.Customer = customerId; c.QueryParameters.Limit = 100; c.QueryParameters.StartingAfter = cursor; }, cancellationToken);
            if (page?.Data is not { Count: > 0 }) break;
            result.AddRange(page.Data);
            cursor = page.HasMore == true ? page.Data[^1].Id : null;
        } while (cursor != null);
        return result;
    }
    public async ValueTask<PaymentIntent> Cancel(string id, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        return await api.V1.Payment_intents[id].Cancel.PostAsync(new(), cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Stripe returned an empty intent.");
    }
    public async ValueTask<PaymentIntent> Capture(string id, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        return await api.V1.Payment_intents[id].Capture.PostAsync(new(), cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Stripe returned an empty intent.");
    }
    public void Dispose() { }
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
