using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Stripe.Client.Registrars;
using Soenneker.Stripe.PaymentIntents.Abstract;

namespace Soenneker.Stripe.PaymentIntents.Registrars;

/// <summary>
/// A .NET typesafe implementation of Stripe's Payment Intents API
/// </summary>
public static class StripePaymentIntentsUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IStripePaymentIntentsUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddStripePaymentIntentsUtilAsSingleton(this IServiceCollection services)
    {
        services.AddStripeClientUtilAsSingleton()
                .TryAddSingleton<IStripePaymentIntentsUtil, StripePaymentIntentsUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IStripePaymentIntentsUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddStripePaymentIntentsUtilAsScoped(this IServiceCollection services)
    {
        services.AddStripeClientUtilAsSingleton()
                .TryAddScoped<IStripePaymentIntentsUtil, StripePaymentIntentsUtil>();

        return services;
    }
}