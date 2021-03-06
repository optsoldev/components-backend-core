﻿using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Storage.Queue;
using Optsol.Components.Shared.Settings;

namespace Optsol.Components.Test.Utils.Storage.Queue
{
    public class QueueStorageTestDois : QueueStorageBase, IQueueStorageTestDois
    {
        public QueueStorageTestDois(StorageSettings settings, ILoggerFactory logger) : base(settings, logger)
        {
        }

        public override string QueueName { get => "teste-queue-dois"; }
    }
}
