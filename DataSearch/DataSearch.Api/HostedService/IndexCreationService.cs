using DataSearch.Api.Models;
using Redis.OM;

namespace DataSearch.Api.HostedService
{
    public class IndexCreationService : IHostedService
    {
        private readonly RedisConnectionProvider _connectionProvider;

        public IndexCreationService(RedisConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var info = (await _connectionProvider.Connection.ExecuteAsync("FT._LIST")).ToArray().Select(x => x.ToString());
            if (info.All(x => x != "person-idx"))
            {
                await _connectionProvider.Connection.CreateIndexAsync(typeof(Person));
            }

            //await _connectionProvider.Connection.CreateIndexAsync(typeof(Person));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
