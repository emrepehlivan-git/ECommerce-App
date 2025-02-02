using Ardalis.Result;
using MediatR;

namespace ECommerce.Application.Common.Interfaces;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}