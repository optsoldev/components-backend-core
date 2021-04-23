using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Domain.Notifications;
using Optsol.Components.Service.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Service.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ValidationModelAttribute : TypeFilterAttribute
    {
        public ValidationModelAttribute() : base(typeof(ValidationModelFilterAttribute))
        {

        }
    }

    public class ValidationModelFilterAttribute : ActionFilterAttribute
    {
        protected readonly IResponseFactory _responseFactory;
        protected readonly NotificationContext _notificationContext;

        public ValidationModelFilterAttribute(IResponseFactory responseFactory, NotificationContext notificationContext)
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

        private Response GetResponseFromBaseDataTransferObject(List<BaseDataTransferObject> listOfBaseDataTransferObject)
        {
            foreach (var baseDataTransferObject in listOfBaseDataTransferObject)
            {
                baseDataTransferObject.Validate();
                _notificationContext.AddNotifications(baseDataTransferObject.Notifications);
            }

            return _responseFactory.Create();
        }

        private List<BaseDataTransferObject> ResolverBaseDataTransferObject(IDictionary<string, object> actionArguments)
        {
            var listOfBaseDataTransferObject = new List<BaseDataTransferObject>();

            var argumentsBaseDataTransfers = actionArguments.Where(args => args.Value.GetType().BaseType.Equals(typeof(BaseDataTransferObject)));
            foreach (var argumentDataTransfer in argumentsBaseDataTransfers)
            {
                var baseDataTransferObject = argumentDataTransfer.Value as BaseDataTransferObject;
                listOfBaseDataTransferObject.Add(baseDataTransferObject);
            }

            return listOfBaseDataTransferObject;
        }

    }
}
