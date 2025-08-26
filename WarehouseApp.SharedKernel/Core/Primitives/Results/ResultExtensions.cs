namespace WarehouseApp.SharedKernel.Core.Primitives.Results;

/// <summary>
/// Contains extension methods for the result class.
/// </summary>
public static class ResultExtensions
{
    #region MAP

    /// <summary>
    /// Maps the value of a successful <see cref="Result{T}"/> to a new value using the specified function. If the result is a failure, propagates the same error.
    /// </summary>
    /// <param name="result">The input result.</param>
    /// <param name="func">The mapping function to apply on success.</param>
    /// <returns>A new <see cref="Result{TOut}"/> with the mapped value or the original error.</returns>
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> func)
    {
        return result.IsSuccess
            ? Result.Success(func(result.Value))
            : Result.Failure<TOut>(result.Error);
    }

    /// <summary>
    /// Asynchronously maps the value of a successful <see cref="Task{Result{T}}"/> to a new value using the specified function. If the result is a failure, propagates the same error.
    /// </summary>
    /// <param name="resultTask">The input result task.</param>
    /// <param name="func">The mapping function to apply on success.</param>
    /// <returns>A task with a new <see cref="Result{TOut}"/> with the mapped value or the original error.</returns>
    public static async Task<Result<TOut>> Map<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, TOut> func)
    {
        var result = await resultTask;
        return result.IsSuccess
            ? Result.Success(func(result.Value))
            : Result.Failure<TOut>(result.Error);
    }

    #endregion

    #region BIND

    /// <summary>
    /// Binds a successful <see cref="Result{T}"/> to another result by applying the given function. Enables chaining operations that return <see cref="Result{T}"/> types.
    /// </summary>
    /// <param name="result">The input result.</param>
    /// <param name="func">The function to apply on success.</param>
    /// <returns>The result of the function or the original error.</returns>
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> func)
    {
        return result.IsSuccess ? func(result.Value) : Result.Failure<TOut>(result.Error);
    }

    /// <summary>
    /// Binds a successful <see cref="Result{T}"/> to a <see cref="result"/> without value.
    /// </summary>
    /// <param name="func">The input result.</param>
    /// <param name="func">The function to apply on success.</param>
    /// <returns>The result of the function or the original error.</returns>
    public static Result Bind<TIn>(this Result<TIn> result, Func<TIn, Result> func)
    {
        return result.IsSuccess ? func(result.Value) : Result.Failure(result.Error);
    }

    /// <summary>
    /// Asynchronously binds a successful <see cref="Result{T}"/> to another <see cref="Task{Result{T}}"/>.
    /// </summary>
    /// <param name="result">The input result.</param>
    /// <param name="func">The asynchronous function to apply on success.</param>
    /// <returns>A task with the result of the function or the original error.</returns>
    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> func)
    {
        return result.IsSuccess ? await func(result.Value) : Result.Failure<TOut>(result.Error);
    }

    /// <summary>
    /// Asynchronously binds a successful <see cref="Result{T}"/> to a <see cref="Task{Result}"/>.
    /// </summary>
    /// <param name="result">The input result.</param>
    /// <param name="func">The asynchronous function to apply on success.</param>
    /// <returns>A task with the result of the function or the original error.</returns>
    public static async Task<Result> Bind<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> func)
    {
        return result.IsSuccess ? await func(result.Value) : Result.Failure(result.Error);
    }

    /// <summary>
    /// Binds a <see cref="Task{Result{T}}"/> to another asynchronous result function.
    /// </summary>
    /// <param name="resultTask">The input result task.</param>
    /// <param name="func">The asynchronous function to apply on success.</param>
    /// <returns>A task with the result of the function or the original error.</returns>
    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Task<Result<TOut>>> func)
    {
        var result = await resultTask;
        return result.IsSuccess ? await func(result.Value) : Result.Failure<TOut>(result.Error);
    }

    /// <summary>
    /// Binds a <see cref="Task{Result{T}}"/> to a <see cref="Task{Result}"/> function.
    /// </summary>
    /// <param name="resultTask">The input result task.</param>
    /// <param name="func">The asynchronous function to apply on success.</param>
    /// <returns>A task with the result of the function or the original error.</returns>
    public static async Task<Result> Bind<TIn>(this Task<Result<TIn>> resultTask, Func<TIn, Task<Result>> func)
    {
        var result = await resultTask;
        return result.IsSuccess ? await func(result.Value) : Result.Failure(result.Error);
    }

    #endregion

    #region MATCH

    /// <summary>
    /// Matches a <see cref="Result{T}"/> and executes the appropriate function depending on success or failure.
    /// </summary>
    /// <param name="result">The input result.</param>
    /// <param name="onSuccess">Function to execute on success.</param>
    /// <param name="onFailure">Function to execute on failure.</param>
    /// <returns>The result of the executed function.</returns>
    public static TOut Match<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
    }

    /// <summary>
    /// Matches a <see cref="Result"/> and executes the appropriate function depending on success or failure.
    /// </summary>
    /// <param name="result">The input result.</param>
    /// <param name="onSuccess">Function to execute on success.</param>
    /// <param name="onFailure">Function to execute on failure.</param>
    /// <returns>The result of the executed function.</returns>
    public static TOut Match<TOut>(this Result result, Func<TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }

    /// <summary>
    /// Asynchronously matches a <see cref="Task{Result{T}}"/> and executes the appropriate function depending on success or failure.
    /// </summary>
    /// <param name="resultTask">The input result task.</param>
    /// <param name="onSuccess">Function to execute on success.</param>
    /// <param name="onFailure">Function to execute on failure.</param>
    /// <returns>A task with the result of the executed function.</returns>
    public static async Task<TOut> Match<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        var result = await resultTask;
        return result.Match(onSuccess, onFailure);
    }

    /// <summary>
    /// Asynchronously matches a <see cref="Task{Result}"/> and executes the appropriate function depending on success or failure.
    /// </summary>
    /// <param name="resultTask">The input result task.</param>
    /// <param name="onSuccess">Function to execute on success.</param>
    /// <param name="onFailure">Function to execute on failure.</param>
    /// <returns>A task with the result of the executed function.</returns>
    public static async Task<TOut> Match<TOut>(this Task<Result> resultTask, Func<TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        var result = await resultTask;
        return result.Match(onSuccess, onFailure);
    }

    #endregion

    #region ENSURE

    /// <summary>
    /// Ensures the given predicate is true for the value inside a successful <see cref="Result{T}"/>. If the predicate fails, returns a failure with the specified error.
    /// </summary>
    /// <param name="result">The input result.</param>
    /// <param name="predicate">Predicate to validate the value.</param>
    /// <param name="error">Error to return if the predicate fails.</param>
    /// <returns>The original result if valid, otherwise a failure result.</returns>
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (result.IsFailure)
        {
            return result;
        }

        return predicate(result.Value) ? result : Result.Failure<T>(error);
    }

    /// <summary>
    /// Asynchronously ensures the given predicate is true for the value inside a successful <see cref="Task{Result{T}}"/>. If the predicate fails, returns a failure with the specified error.
    /// </summary>
    /// <param name="resultTask">The input result task.</param>
    /// <param name="predicate">Predicate to validate the value.</param>
    /// <param name="error">Error to return if the predicate fails.</param>
    /// <returns>A task with the original result if valid, otherwise a failure result.</returns>
    public static async Task<Result<T>> Ensure<T>(this Task<Result<T>> resultTask, Func<T, bool> predicate, Error error)
    {
        var result = await resultTask;
        if (result.IsFailure)
        {
            return result;
        }

        var isValid = predicate(result.Value);
        if (isValid)
        {
            return result;
        }

        var finalError = result.Error == Error.None ? error : result.Error;
        return Result.Failure<T>(finalError);
    }

    #endregion

    #region TAP

    /// <summary>
    /// Executes an action on the value of a successful <see cref="Result{T}"/>. Does not alter the result.
    /// </summary>
    /// <param name="result">The input result.</param>
    /// <param name="action">Action to execute on success.</param>
    /// <returns>The original result.</returns>
    public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }

    /// <summary>
    /// Asynchronously executes an action on the value of a successful <see cref="Task{Result{T}}"/>. Does not alter the result.
    /// </summary>
    /// <param name="resultTask">The input result task.</param>
    /// <param name="action">Action to execute on success.</param>
    /// <returns>A task with the original result.</returns>
    public static async Task<Result<T>> Tap<T>(this Task<Result<T>> resultTask, Action<T> action)
    {
        var result = await resultTask;
        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }

    /// <summary>
    /// Asynchronously executes an action on the value of a successful <see cref="Result{T}"/>. Does not alter the result.
    /// </summary>
    /// <param name="result">The input result.</param>
    /// <param name="action">Asynchronous action to execute on success.</param>
    /// <returns>A task with the original result.</returns>
    public static async Task<Result<T>> TapAsync<T>(this Result<T> result, Func<T, Task> action)
    {
        if (result.IsSuccess)
        {
            await action(result.Value);
        }

        return result;
    }

    /// <summary>
    /// Asynchronously executes an action on the value of a successful <see cref="Task{Result{T}}"/>. Does not alter the result.
    /// </summary>
    /// <param name="resultTask">The input result task.</param>
    /// <param name="action">Asynchronous action to execute on success.</param>
    /// <returns>A task with the original result.</returns>
    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> resultTask, Func<T, Task> action)
    {
        var result = await resultTask;
        if (result.IsSuccess)
        {
            await action(result.Value);
        }

        return result;
    }

    #endregion

    #region TAP_ERROR

    /// <summary>
    /// Executes an action if the <see cref="Result{T}"/> is a failure (functional error), without altering the result.
    /// </summary>
    /// <param name="result">The input result.</param>
    /// <param name="onError">Action to execute on failure.</param>
    /// <returns>The original result.</returns>
    public static Result<T> TapError<T>(this Result<T> result, Action<Error> onError)
    {
        if (result.IsFailure)
        {
            onError(result.Error);
        }

        return result;
    }

    /// <summary>
    /// Asynchronously executes an action if the <see cref="Task{Result{T}}"/> ends in failure (functional error), without altering the result.
    /// </summary>
    /// <param name="resultTask">The input result task.</param>
    /// <param name="onError">Action to execute on failure.</param>
    /// <returns>A task with the original result.</returns>
    public static async Task<Result<T>> TapError<T>(this Task<Result<T>> resultTask, Action<Error> onError)
    {
        var result = await resultTask;
        if (result.IsFailure)
        {
            onError(result.Error);
        }

        return result;
    }

    #endregion
}