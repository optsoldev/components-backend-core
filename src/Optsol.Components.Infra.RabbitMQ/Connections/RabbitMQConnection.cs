using Microsoft.Extensions.Logging;
using Optsol.Components.Shared.Settings;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;

namespace Optsol.Components.Infra.RabbitMQ.Connections
{
    public class RabbitMQConnection : IRabbitMQConnection, IDisposable
    {
        private bool _disposed = false;

        private readonly RabbitMQSettings _rabbitMQSettings;

        private readonly ILogger _logger;

        private IConnection _connection;

        private readonly object _lock = new();

        public RabbitMQConnection(RabbitMQSettings rabbitMQSettings, ILoggerFactory logger)
        {
            _logger = logger?.CreateLogger<RabbitMQConnection>();
            _logger?.LogInformation("Inicializando RabbitMQConnection");

            _rabbitMQSettings = rabbitMQSettings ?? throw new ArgumentNullException(nameof(rabbitMQSettings));

            CreateConnection();
        }

        public bool Connected
        {
            get
            {
                var isConnected = _connection != null && _connection.IsOpen && !_disposed;
                return isConnected;
            }
        }

        public bool Disconnected
        {
            get
            {
                return !Connected;
            }
        }

        public void Connect()
        {
            _logger?.LogInformation("Conectando client ao RabbitMQ");

            lock (_lock)
            {
                var policy = Policy.Handle<BrokerUnreachableException>()
                    .Or<SocketException>()
                    .WaitAndRetry(3, attemp => TimeSpan.FromSeconds(Math.Pow(2, attemp)), (ex, time) =>
                    {
                        _logger?.LogWarning("Não foi possível se conectar  tentativa após: {TimeOut}s {ExceptionMessage}", $"{time.TotalSeconds:n1}", ex.Message);
                    });

                policy.Execute(() =>
                {
                    CreateConnection();
                });

                if (Connected)
                {
                    _connection.ConnectionShutdown += (sender, connection) =>
                    {
                        if (_disposed) return;

                        _logger?.LogWarning("Tentando reconexão...");

                        Connect();
                    };

                    _connection.CallbackException += (sender, connection) =>
                    {
                        if (_disposed) return;

                        _logger?.LogWarning("Tentando reconexão...");

                        Connect();
                    };

                    _connection.ConnectionBlocked += (sender, connection) =>
                    {
                        if (_disposed) return;

                        _logger?.LogWarning("Tentando reconexão...");

                        Connect();
                    };
                }
                else
                {
                    _logger?.LogCritical("FATAL ERRO: Não foi possível se conectar ao RabbitMQ");
                }
            }
        }

        public IModel CreateModel()
        {
            if (Disconnected)
            {
                throw new InvalidOperationException("Nenhuma conexão foi estabelecida com o RabbitMQ");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _logger?.LogInformation($"Método: { nameof(Dispose) }()");

            if (!_disposed && disposing)
            {
                _connection.Dispose();
            }
            _disposed = true;
        }

        private void CreateConnection()
        {
            try
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = _rabbitMQSettings.HostName,
                    Port = _rabbitMQSettings.Port
                };

                var userValidPasswordValid = !string.IsNullOrEmpty(_rabbitMQSettings.UserName) && !string.IsNullOrEmpty(_rabbitMQSettings.Password);
                if (userValidPasswordValid)
                {

                    connectionFactory.UserName = _rabbitMQSettings.UserName;
                    connectionFactory.Password = _rabbitMQSettings.Password;
                }


                _connection = connectionFactory.CreateConnection();

            }
            catch (Exception ex)
            {
                _logger?.LogError("Não foi possível criar a conexão com o RabbitMQ: {ExceptionMessage}", ex.Message);
            }
        }
    }
}
