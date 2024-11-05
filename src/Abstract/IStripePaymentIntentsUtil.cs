using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Stripe;

namespace Soenneker.Stripe.PaymentIntents.Abstract;

/// <summary>
/// A .NET typesafe implementation of Stripe's Payment Intents API for creating, retrieving, confirming, and updating payment intents.
/// </summary>
public interface IStripePaymentIntentsUtil : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Creates a new PaymentIntent for a specified customer with a given amount.
    /// </summary>
    /// <param name="stripeCustomerId">The unique identifier for the Stripe customer.</param>
    /// <param name="amount">The amount for the PaymentIntent, in the smallest currency unit (e.g., cents for USD).</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask{PaymentIntent}"/> that represents the asynchronous operation, containing the created PaymentIntent.</returns>
    ValueTask<PaymentIntent> Create(string stripeCustomerId, decimal amount, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an existing PaymentIntent by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the PaymentIntent to retrieve.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask{PaymentIntent}"/> that represents the asynchronous operation, containing the retrieved PaymentIntent.</returns>
    [Pure]
    ValueTask<PaymentIntent> Get(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirms an existing PaymentIntent, typically when the customer is redirected back after authentication.
    /// </summary>
    /// <param name="id">The unique identifier of the PaymentIntent to confirm.</param>
    /// <param name="returnUrl">The URL to redirect the customer to after confirmation.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask{PaymentIntent}"/> that represents the asynchronous operation, containing the confirmed PaymentIntent.</returns>
    ValueTask<PaymentIntent> Confirm(string id, string returnUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing PaymentIntent with additional metadata.
    /// </summary>
    /// <param name="id">The unique identifier of the PaymentIntent to update.</param>
    /// <param name="metadata">A dictionary of metadata to attach to the PaymentIntent.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask{PaymentIntent}"/> that represents the asynchronous operation, containing the updated PaymentIntent.</returns>
    ValueTask<PaymentIntent> Update(string id, Dictionary<string, string> metadata, CancellationToken cancellationToken = default);
}