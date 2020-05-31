using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Octogram.Chats.Application.Web.Commands;
using Octogram.Chats.Infrastructure.Repository.EntityFrameworkCore;

namespace Octogram.Chats.Application.Web.Behaviors
{
public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly RepositoryDbContext _dbContext;
		private readonly ICommandsBus _commandsBus;
		private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

		public TransactionBehavior(
			RepositoryDbContext dbContext,
			ICommandsBus commandsBus,
			ILogger<TransactionBehavior<TRequest, TResponse>> logger)
		{
			_dbContext = dbContext
				?? throw new ArgumentNullException(nameof(dbContext));
			_commandsBus = commandsBus
				?? throw new ArgumentNullException(nameof(commandsBus));
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

				await _commandsBus.SendAllAsync(cancellationToken);
			}

			return response;
		}
	}
}
