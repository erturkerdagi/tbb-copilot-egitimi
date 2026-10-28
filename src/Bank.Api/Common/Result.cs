namespace Bank.Api.Common;

public enum ErrorType
{
    Validation,
    NotFound,
    BusinessRule
}

/// <summary>
/// İş hatası. Kod makineler için, mesaj insanlar için.
/// Mesajlarda kişisel veri (ad, TCKN, IBAN) bulunmaz.
/// </summary>
public sealed record Error(string Code, string Message, ErrorType Type);

/// <summary>
/// İş kuralı hatalarında exception fırlatmak yerine kullanılan sonuç tipi.
/// </summary>
public sealed class Result<T>
{
    private Result(T? value, Error? error)
    {
        Value = value;
        Error = error;
    }

    public T? Value { get; }

    public Error? Error { get; }

    public bool IsSuccess => Error is null;

    public static Result<T> Success(T value) => new(value, null);

    public static Result<T> Failure(Error error) => new(default, error);
}
