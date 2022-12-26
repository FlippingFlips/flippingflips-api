using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FF.Api.Base
{
    public abstract class FlipsApiControllerBase : ControllerBase
    {
        protected readonly IMediator mediator;

        public FlipsApiControllerBase(IMediator mediator = null)
        {
            this.mediator = mediator;
        }
    }
}
