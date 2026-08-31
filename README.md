[![](https://img.shields.io/nuget/v/soenneker.stripe.paymentintents.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.stripe.paymentintents/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.stripe.paymentintents/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.stripe.paymentintents/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.stripe.paymentintents.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.stripe.paymentintents/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.stripe.paymentintents/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.stripe.paymentintents/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Stripe.PaymentIntents

Create, retrieve, update, confirm, cancel, capture, and list Stripe payment intents through a reusable Stripe.net service.

## Installation

```bash
dotnet add package Soenneker.Stripe.PaymentIntents
```

## Configuration

```json
{
  "Stripe": {
    "SecretKey": "sk_test_..."
  }
}
```

## Usage

```csharp
using Soenneker.Stripe.PaymentIntents.Abstract;
using Soenneker.Stripe.PaymentIntents.Registrars;
using Stripe;

services.AddStripePaymentIntentsUtilAsScoped();

PaymentIntent intent = await paymentIntentsUtil.Create(
    stripeCustomerId: "cus_...",
    amount: 49.95m,
    idempotencyKey: $"order-{orderId}",
    cancellationToken: cancellationToken);

PaymentIntent confirmed = await paymentIntentsUtil.Confirm(
    intent.Id,
    returnUrl: "https://example.com/payment/complete",
    cancellationToken: cancellationToken);
```

`Create` accepts a USD amount in dollars and rounds fractional cents away from zero. Automatic payment methods are enabled by default; when `paymentMethodTypes` is supplied, those explicit types take precedence.

`List` returns at most the first 100 payment intents for a customer. `Cancel` and `Capture` change payment state in Stripe, and Stripe API errors propagate to the caller.
