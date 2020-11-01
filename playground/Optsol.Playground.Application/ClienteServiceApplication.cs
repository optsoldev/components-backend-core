using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Result;
using Optsol.Components.Application.Service;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using Optsol.Playground.Application.ViewModels.Cliente;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Application
{
    public class ClienteServiceApplication : BaseServiceApplication<ClienteEntity, ClienteViewModel, ClienteViewModel, ClienteViewModel, ClienteViewModel>,
        IClienteServiceApplication
    {
        public ClienteServiceApplication(
            IMapper mapper,
            ILogger<BaseServiceApplication<ClienteEntity, ClienteViewModel, ClienteViewModel, ClienteViewModel, ClienteViewModel>> logger,
            IServiceResultFactory serviceResultFactory,
            IUnitOfWork unitOfWork,
            IReadRepository<ClienteEntity, Guid> readRepository,
            IWriteRepository<ClienteEntity, Guid> writeRepository)
            : base(mapper, logger, serviceResultFactory, unitOfWork, readRepository, writeRepository)
        {
        }
    }
}
