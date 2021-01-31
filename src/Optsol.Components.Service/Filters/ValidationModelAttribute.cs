using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Application.Results;
using Optsol.Components.Domain.Notifications;
using Optsol.Components.Service.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Service.Filters
{
    public class ValidationModelAttribute : ActionFilterAttribute
    {

        protected readonly IResponseFactory _responseFactory;
        protected readonly NotificationContext _notificationContext;

        public ValidationModelAttribute(IResponseFactory responseFactory, NotificationContext notificationContext)
        {
            _responseFactory = responseFactory;
            _notificationContext = notificationContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var existsArgumets = context.ActionArguments.Any();
            if (existsArgumets)
            {

                var listOfBaseDataTransferObject = ResolverBaseDataTransferObject(context.ActionArguments);

                var responseFromBaseDataTransferObject = GetResponseFromBaseDataTransferObject(listOfBaseDataTransferObject);

                var responseIsInvalid = responseFromBaseDataTransferObject.Failure;
                if (responseIsInvalid)
                {
                    context.Result = new BadRequestObjectResult(responseFromBaseDataTransferObject);
                }

            }
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        private Response GetResponseFromBaseDataTransferObject(List<BaseDataTransferObject> listOfBaseDataTransferObject)
        {
            var serviceResult = new ServiceResult();
            foreach (var baseDataTransferObject in listOfBaseDataTransferObject)
            {
                baseDataTransferObject.Validate();
                _notificationContext.AddNotifications(baseDataTransferObject.Notifications);
            }

            return _responseFactory.Create(serviceResult);
        }

        private readonly Func<IDictionary<string, object>, List<BaseDataTransferObject>> ResolverBaseDataTransferObject
            = (actionArguments) =>
            {
                List<BaseDataTransferObject> listOfBaseDataTransferObject = new List<BaseDataTransferObject>();

                var argumentsBaseDataTransfers = actionArguments.Where(args => args.Value.GetType().BaseType.Equals(typeof(BaseDataTransferObject)));
                foreach (var argumentDataTransfer in argumentsBaseDataTransfers)
                {
                    var baseDataTransferObject = argumentDataTransfer.Value as BaseDataTransferObject;
                    listOfBaseDataTransferObject.Add(baseDataTransferObject);
                }

                return listOfBaseDataTransferObject;
            };

    }
}
