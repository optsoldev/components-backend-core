﻿using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Data.Pagination;
using Optsol.Components.Infra.Firebase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Firebase.Clients
{
    public interface IStorageClient
    {
        void AddClient(IClient client);

        IClient GetClient(IQueryClient search);
    }
}
