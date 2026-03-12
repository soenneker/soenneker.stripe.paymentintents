using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Stripe;

namespace Soenneker.Stripe.PaymentIntents.Abstract;

/// <summary>
/// A utility for interacting with Stripe Payment Intents, supporting creation, update, confirmation, capture, cancelation, and listing operations.
/// </summary>
public interface IStripePaymentIntentsUtil : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Creates a new Stripe PaymentIntent for a customer with optional idempotency and method configurations.
    /// </summary>
    /// <param name="stripeCustomerId">The Stripe customer ID associated with the PaymentIntent.</param>
    /// <param name="amount">The amount in USD dollars (will be converted to cents).</param>
    /// <param name="idempotencyKey">Optional idempotency key to prevent duplicate PaymentIntents during retries.</param>
    /// <param name="paymentMethodTypes">Optional specific payment method types (e.g. "card").</param>
    /// <param name="automaticPaymentMethods">Optional configuration for automatic payment method detection.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The created PaymentIntent.</returns>
    ValueTask<PaymentIntent> Create(string stripeCustomerId, decimal amount, string? idempotencyKey = null, IEnumerable<string>? paymentMethodTypes = null,
        PaymentIntentAutomaticPaymentMethodsOptions? automaticPaymentMethods = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a PaymentIntent by its ID.
    /// </summary>
    /// <param name="id">The ID of the PaymentIntent.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The retrieved PaymentIntent.</returns>
    [Pure]
    ValueTask<PaymentIntent> Get(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates metadata for a PaymentIntent.
    /// </summary>
    /// <param name="id">The ID of the PaymentIntent.</param>
    /// <param name="metadata">The metadata key-value pairs to set.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The updated PaymentIntent.</returns>
    ValueTask<PaymentIntent> Update(string id, Dictionary<string, string> metadata, CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirms a PaymentIntent, typically as part of a client-side or redirect flow.
    /// </summary>
    /// <param name="id">The ID of the PaymentIntent to confirm.</param>
    /// <param name="returnUrl">The URL to redirect the customer after confirmation (if applicable).</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The confirmed PaymentIntent.</returns>
    ValueTask<PaymentIntent> Confirm(string id, string returnUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a PaymentIntent before it is captured or confirmed.
    /// </summary>
    /// <param name="id">The ID of the PaymentIntent to cancel.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The canceled PaymentIntent.</returns>
    ValueTask<PaymentIntent> Cancel(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Captures a previously confirmed PaymentIntent that was created with manual capture enabled.
    /// </summary>
    /// <param name="id">The ID of the PaymentIntent to capture.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The captured PaymentIntent.</returns>
    ValueTask<PaymentIntent> Capture(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of PaymentIntents for a specific customer.
    /// </summary>
    /// <param name="customerId">The Stripe customer ID to filter the PaymentIntents.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A collection of PaymentIntents.</returns>
    [Pure]
    ValueTask<IEnumerable<PaymentIntent>> List(string customerId, CancellationToken cancellationToken = default);
}