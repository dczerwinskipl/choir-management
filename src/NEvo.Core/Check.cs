using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace NEvo.Core;

/// <summary>
/// Helper methods to simplify param checking
/// </summary>
public class Check
{
    /// <summary>
    /// Returns value provided as first argument or throw exception if value is null
    /// </summary>
    /// <typeparam name="TValue">Type of value</typeparam>
    /// <param name="value">Value to check</param>
    /// <param name="message">Optional, message for exception</param>
    /// <param name="paramName">Optional, parameter name for exception</param>
    /// <returns>Provided value</returns>
    /// <exception cref="ArgumentNullException">Throws when provided value is null</exception>
    public static TValue Null<TValue>(TValue? value, string? message = null, [CallerArgumentExpression("value")] string? paramName = null)
    {
        return value ?? throw new ArgumentNullException(paramName, message);
    }

    /// <summary>
    /// Returns value provided as first argument or throw exception if value is null or empty
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <param name="message">Optional, message for exception</param>
    /// <param name="paramName">Optional, parameter name for exception</param>
    /// <returns>Provided value</returns>
    /// <exception cref="ArgumentNullException">Throws when provided value is null or empty</exception>
    public static string NullOrEmpty(string? value, string? message = null, [CallerArgumentExpression("value")] string? paramName = null)
    {
        return !string.IsNullOrEmpty(value) ? value : throw new ArgumentNullException(paramName, message);
    }

    /// <summary>
    /// Returns value provided as first argument or throw exception if value is set to default
    /// </summary>
    /// <typeparam name="TValue">Type of value</typeparam>
    /// <param name="value">Value to check</param>
    /// <param name="message">Optional, message for exception</param>
    /// <param name="paramName">Optional, parameter name for exception</param>
    /// <returns>Provided value</returns>
    /// <exception cref="ArgumentNullException">Throws when provided value is set to default</exception>
    public static TValue Default<TValue>(TValue value, string? message = null, [CallerArgumentExpression("value")] string? paramName = null) where TValue : struct
    {
        return !EqualityComparer<TValue>.Default.Equals(value, default) ? value : throw new ArgumentNullException(paramName, message);
    }

    public static void Annotations<TObject>(TObject obj)
    {
        var validationContext = new ValidationContext(obj);
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(obj, validationContext, validationResults);

        if (validationResults.Any())
        {
            if (validationResults.Count > 1)
                throw new AggregateException(validationResults.Select(r => new ArgumentException(r.ErrorMessage)));
            else
                throw new ArgumentException(validationResults.First().ErrorMessage);
        }
    }

    public static TValue Annotations<TObject, TValue>(TObject obj, string targetName, TValue value, [CallerArgumentExpression("value")] string? paramName = null)
    {
        var validationContext = new ValidationContext(obj);
        validationContext.MemberName = targetName;
        validationContext.DisplayName = targetName;
        validationContext.Items.Add(targetName, value);
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateProperty(value, validationContext, validationResults);

        return validationResults.Any() ?
            validationResults.Count > 1 ? throw new AggregateException(validationResults.Select(r => new ArgumentException(r.ErrorMessage, paramName)))
                                        : throw new ArgumentException(validationResults.First().ErrorMessage, paramName)
            : value;
    }
}

