namespace NEvo.Monads;

public readonly partial struct Maybe<T>
{
    public bool TryGetValue(out T value)
    {
        if (_hasValue)
        {
            value = _value;
            return true;
        }

        value = default;
        return false;
    }

    public Maybe<TResult> Cast<TResult>() => _hasValue && _value is TResult ? (TResult)(object)_value : Maybe.None;

    //TODO: ITS NOT MATCH? ITS MORE LIKE MAP? NAD MY MAP IS REALY BIND, NOT MAYBE?
    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none) => _hasValue ? some(_value) : none();

    public async Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> some, Func<Task<TResult>> none) => _hasValue ? await some(_value) : await none();

    public async Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> some, Func<TResult> none) => _hasValue ? await some(_value) : none();

    public async Task<TResult> MatchAsync<TResult>(Func<T, TResult> some, Func<Task<TResult>> none) => _hasValue ? some(_value) : await none();

    public Either<Exception, TResult> Match<TResult>(Func<T, TResult> some, Func<Exception> none) => _hasValue ? some(_value) : Either.Failure<TResult>(none());
    public Either<Exception, TResult> Match<TResult>(Func<T, Either<Exception, TResult>> some, Func<Exception> none) => _hasValue ? some(_value) : Either.Failure<TResult>(none());

    public async Task<Either<Exception, TResult>> MatchAsync<TResult>(Func<T, Task<Either<Exception, TResult>>> some, Func<Exception> none) => _hasValue ? await some(_value) : Either.Failure<TResult>(none());

    public Maybe<TResult> Map<TResult>(Func<T, TResult> convert) => _hasValue ? convert(_value) : new Maybe<TResult>();

    public async Task<Maybe<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> convert) => _hasValue ? await convert(_value) : new Maybe<TResult>();

    public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> convert) => _hasValue ? convert(_value) : new Maybe<TResult>();

    public async Task<Maybe<TResult>> BindAsync<TResult>(Func<T, Task<Maybe<TResult>>> convert) => _hasValue ? await convert(_value) : new Maybe<TResult>();
}
