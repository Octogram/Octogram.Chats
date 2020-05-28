using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore;

namespace Messenger.Web.Behaviors
{
public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly RepositoryDbContext _dbContext;
		private readonly IMediator _mediator;
		private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

		public TransactionBehavior(
			RepositoryDbContext dbContext,
			IMediator mediator,
			ILogger<TransactionBehavior<TRequest, TResponse>> logger)
		{
			_dbContext = dbContext
				?? throw new ArgumentNullException(nameof(dbContext));
			_mediator = mediator 
				?? throw new ArgumentNullException(nameof(mediator));
			_logger = logger
				?? throw new ArgumentNullException(nameof(logger));
		}

		/// <inheritdoc />
		public async Task<TResponse> Handle(
			TRequest request,
			CancellationToken cancellationToken,
			RequestHandlerDelegate<TResponse> next)
		{
			TResponse response;
			
			if (_dbContext.Database.CurrentTransaction != null)
			{
				_logger.LogInformation($"Transaction {_dbContext.Database.CurrentTransaction.TransactionId} already started.");
				return await next();
			}

			await using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
			{
				_logger.LogInformation($"---- Transaction started {transaction.TransactionId} ----");
				response = await next();
				
				try
				{
					await _dbContext.SaveChangesAsync(cancellationToken);
					await transaction.CommitAsync(cancellationToken);
					_logger.LogInformation($"---- Transaction committed {transaction.TransactionId}.");
				}
				catch (Exception)
				{
					await transaction.RollbackAsync(cancellationToken);
					_logger.LogInformation($"---- Transaction rollback {transaction.TransactionId}.");
					throw;
				}
			}

			return response;
		}
	}
}
