using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Ipstset.AzShop.Application.Exceptions;
using MediatR;

namespace Ipstset.AzShop.Application.Behaviors
{
    /// <summary>
    /// Wraps domain exceptions into app-specific exceptions
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                //need to await result to allow exception to be caught within this behavior...
                var result = await next();
                return result;
            }
            //map domain exceptions to application layer exceptions
            catch (ArgumentException ex)
            {
                //throw new BadRequestException(ex.ParamName);
                throw new ValidationException(new List<ValidationFailure> { new ValidationFailure(ex.Message,"required")});
                
            }
            catch (ApplicationException ex)
            {
                throw new BadRequestException(ex.Message);
            }


        }
    }
}
