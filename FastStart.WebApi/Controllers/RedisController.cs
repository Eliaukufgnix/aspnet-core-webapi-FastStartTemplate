using FastStart.Domain;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace FastStart.WebApi.Controllers
{
    /// <summary>
    /// Redis
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Redis")]
    public class RedisController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        private readonly IServer _server;

        public RedisController(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = redis.GetDatabase();
            _server = redis.GetServer(redis.GetEndPoints().First());
        }

        /// <summary>
        /// 获取Redis信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRedisInfo")]
        public async Task<ResultModel<RedisInfoVO>> GetRedisInfo()
        {
            try
            {
                var dbSize = await _server.DatabaseSizeAsync();
                var info = await _server.InfoAsync();

                var vo = new RedisInfoVO
                {
                    Version = GetInfoValue(info, "Server", "redis_version"),
                    Os = GetInfoValue(info, "Server", "os"),
                    Mode = GetInfoValue(info, "Server", "redis_mode"),
                    DbSize = (int)dbSize,
                    Uptime = int.TryParse(GetInfoValue(info, "Server", "uptime_in_seconds"), out var uptime) ? uptime : 0,
                    TcpPort = GetInfoValue(info, "Server", "tcp_port"),
                    AofEnabled = GetInfoValue(info, "Persistence", "aof_enabled") == "1"
                };

                vo.Memory = new RedisMemoryVO
                {
                    UsedMemory = long.TryParse(GetInfoValue(info, "Memory", "used_memory"), out var usedMem) ? usedMem : 0,
                    UsedMemoryHuman = GetInfoValue(info, "Memory", "used_memory_human"),
                    UsedMemoryPeak = long.TryParse(GetInfoValue(info, "Memory", "used_memory_peak"), out var usedMemPeak) ? usedMemPeak : 0,
                    UsedMemoryPeakHuman = GetInfoValue(info, "Memory", "used_memory_peak_human"),
                    MaxMemoryHuman = GetInfoValue(info, "Memory", "maxmemory_human"),
                    MemFragmentationRatio = double.TryParse(GetInfoValue(info, "Memory", "mem_fragmentation_ratio"), out var fragRatio) ? fragRatio : 0,
                    Allocator = GetInfoValue(info, "Memory", "allocator"),
                    RssHuman = GetInfoValue(info, "Memory", "rss_human")
                };

                vo.Persistence = new RedisPersistenceVO
                {
                    RdbChangesSinceLastSave = int.TryParse(GetInfoValue(info, "Persistence", "rdb_changes_since_last_save"), out var rdbChanges) ? rdbChanges : 0,
                    LastSaveTime = long.TryParse(GetInfoValue(info, "Persistence", "rdb_last_save_time"), out var lastSave) ? lastSave : 0,
                    LastSaveStatus = GetInfoValue(info, "Persistence", "rdb_last_save_status")
                };

                var keyspaceHits = long.TryParse(GetInfoValue(info, "Stats", "keyspace_hits"), out var hits) ? hits : 0;
                var keyspaceMisses = long.TryParse(GetInfoValue(info, "Stats", "keyspace_misses"), out var misses) ? misses : 0;

                vo.Stats = new RedisStatsVO
                {
                    TotalConnectionsReceived = long.TryParse(GetInfoValue(info, "Stats", "total_connections_received"), out var connRec) ? connRec : 0,
                    TotalCommandsProcessed = long.TryParse(GetInfoValue(info, "Stats", "total_commands_processed"), out var cmdProc) ? cmdProc : 0,
                    TotalNetInputBytes = long.TryParse(GetInfoValue(info, "Stats", "total_net_input_bytes"), out var netIn) ? netIn : 0,
                    TotalNetOutputBytes = long.TryParse(GetInfoValue(info, "Stats", "total_net_output_bytes"), out var netOut) ? netOut : 0,
                    KeyspaceHits = keyspaceHits,
                    KeyspaceMisses = keyspaceMisses,
                    KeyspaceHitRate = keyspaceHits + keyspaceMisses > 0
                        ? $"{((double)keyspaceHits / (keyspaceHits + keyspaceMisses) * 100):F2}%"
                        : "0%"
                };

                // 解析命令统计信息
                var commandStatsSection = info.FirstOrDefault(x => x.Key == "Commandstats");
                if (commandStatsSection != null)
                {
                    foreach (var item in commandStatsSection)
                    {
                        if (item.Key.StartsWith("cmdstat_"))
                        {
                            var command = item.Key.Substring(8);
                            var stats = item.Value.Split(',');
                            long calls = 0;
                            long usec = 0;
                            long usecPerCall = 0;

                            foreach (var stat in stats)
                            {
                                var parts = stat.Trim().Split('=');
                                if (parts.Length == 2)
                                {
                                    switch (parts[0])
                                    {
                                        case "calls":
                                            long.TryParse(parts[1], out calls);
                                            break;

                                        case "usec":
                                            long.TryParse(parts[1], out usec);
                                            break;

                                        case "usec_per_call":
                                            long.TryParse(parts[1], out usecPerCall);
                                            break;
                                    }
                                }
                            }

                            vo.Stats.CommandStats.Add(new RedisCommandStatVO
                            {
                                Command = command,
                                Calls = calls,
                                Usec = usec,
                                UsecPerCall = usecPerCall
                            });
                        }
                    }
                }

                vo.Cpu = new RedisCpuVO
                {
                    UsedCpuSys = double.TryParse(GetInfoValue(info, "CPU", "used_cpu_sys"), out var cpuSys) ? cpuSys : 0,
                    UsedCpuUser = double.TryParse(GetInfoValue(info, "CPU", "used_cpu_user"), out var cpuUser) ? cpuUser : 0,
                    UsedCpuSysChildren = double.TryParse(GetInfoValue(info, "CPU", "used_cpu_sys_children"), out var cpuSysChildren) ? cpuSysChildren : 0,
                    UsedCpuUserChildren = double.TryParse(GetInfoValue(info, "CPU", "used_cpu_user_children"), out var cpuUserChildren) ? cpuUserChildren : 0
                };

                vo.ConnectedClients = int.TryParse(GetInfoValue(info, "Clients", "connected_clients"), out var clients) ? clients : 0;
                vo.ConnectedSlaves = int.TryParse(GetInfoValue(info, "Replication", "connected_slaves"), out var slaves) ? slaves : 0;

                return ResultModel<RedisInfoVO>.Success(vo);
            }
            catch (Exception ex)
            {
                return ResultModel<RedisInfoVO>.Fail($"获取Redis信息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取Redis信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetInfoValue(IGrouping<string, KeyValuePair<string, string>>[] info, string section, string key)
        {
            var sectionData = info.FirstOrDefault(x => x.Key == section);
            if (sectionData == null) return "";
            return sectionData.FirstOrDefault(x => x.Key == key).Value;
        }

        /// <summary>
        /// 获取Redis状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRedisStatus")]
        public async Task<ResultModel<RedisStatusVO>> GetRedisStatus()
        {
            try
            {
                var isConnected = _redis.IsConnected;
                var db = _redis.GetDatabase();
                await db.PingAsync();
                return ResultModel<RedisStatusVO>.Success(new RedisStatusVO { Status = "online" });
            }
            catch
            {
                return ResultModel<RedisStatusVO>.Success(new RedisStatusVO { Status = "offline" });
            }
        }

        /// <summary>
        /// 获取Redis键列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRedisKeys")]
        public async Task<ResultModel<RedisKeyListVO>> GetRedisKeys([FromQuery] RedisKeyQueryDTO query)
        {
            try
            {
                var keys = new List<RedisKeyVO>();
                var pattern = string.IsNullOrEmpty(query.KeyWord) ? "*" : $"*{query.KeyWord}*";
                var total = 0;

                await foreach (var key in _server.KeysAsync(pattern: pattern))
                {
                    total++;
                }

                var skip = (query.PageIndex - 1) * query.PageSize;
                var pagedKeys = new List<RedisKey>();

                await foreach (var key in _server.KeysAsync(pattern: pattern, pageSize: (int)query.PageSize))
                {
                    if (skip > 0)
                    {
                        skip--;
                        continue;
                    }
                    if (pagedKeys.Count < query.PageSize)
                    {
                        pagedKeys.Add(key);
                    }
                }

                foreach (var key in pagedKeys)
                {
                    var ttl = await _db.KeyTimeToLiveAsync(key);
                    var type = await _db.KeyTypeAsync(key);
                    string value = "";
                    long size = 0;

                    switch (type)
                    {
                        case RedisType.String:
                            value = await _db.StringGetAsync(key);
                            size = value.Length;
                            break;

                        case RedisType.List:
                            var listLen = await _db.ListLengthAsync(key);
                            value = $"列表长度: {listLen}";
                            size = listLen;
                            break;

                        case RedisType.Set:
                            var setLen = await _db.SetLengthAsync(key);
                            value = $"集合大小: {setLen}";
                            size = setLen;
                            break;

                        case RedisType.SortedSet:
                            var zsetLen = await _db.SortedSetLengthAsync(key);
                            value = $"有序集合大小: {zsetLen}";
                            size = zsetLen;
                            break;

                        case RedisType.Hash:
                            var hashLen = await _db.HashLengthAsync(key);
                            value = $"哈希字段数: {hashLen}";
                            size = hashLen;
                            break;
                    }

                    keys.Add(new RedisKeyVO
                    {
                        Key = key.ToString(),
                        Type = type.ToString().ToLower(),
                        Ttl = ttl.HasValue ? (int)ttl.Value.TotalSeconds : -1,
                        Value = value,
                        Size = size
                    });
                }

                return ResultModel<RedisKeyListVO>.Success(new RedisKeyListVO
                {
                    List = keys,
                    Total = total
                });
            }
            catch (Exception ex)
            {
                return ResultModel<RedisKeyListVO>.Fail($"获取Redis键失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取Redis键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRedisKeyValue")]
        public async Task<ResultModel<RedisKeyVO>> GetRedisKeyValue([FromQuery] string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return ResultModel<RedisKeyVO>.Fail("键名不能为空");
                }

                var redisKey = (RedisKey)key;
                var type = await _db.KeyTypeAsync(redisKey);
                var ttl = await _db.KeyTimeToLiveAsync(redisKey);
                string value = "";

                switch (type)
                {
                    case RedisType.String:
                        value = await _db.StringGetAsync(redisKey);
                        break;

                    case RedisType.List:
                        var listValues = await _db.ListRangeAsync(redisKey);
                        value = string.Join(", ", listValues.Select(x => x.ToString()));
                        break;

                    case RedisType.Set:
                        var setValues = await _db.SetMembersAsync(redisKey);
                        value = string.Join(", ", setValues.Select(x => x.ToString()));
                        break;

                    case RedisType.SortedSet:
                        var zsetValues = await _db.SortedSetRangeByRankAsync(redisKey);
                        value = string.Join(", ", zsetValues.Select(x => x.ToString()));
                        break;

                    case RedisType.Hash:
                        var hashEntries = await _db.HashGetAllAsync(redisKey);
                        value = string.Join(", ", hashEntries.Select(x => $"{x.Name}={x.Value}"));
                        break;

                    default:
                        value = "不支持的数据类型";
                        break;
                }

                return ResultModel<RedisKeyVO>.Success(new RedisKeyVO
                {
                    Key = key,
                    Type = type.ToString().ToLower(),
                    Ttl = ttl.HasValue ? (int)ttl.Value.TotalSeconds : -1,
                    Value = value,
                    Size = value.Length
                });
            }
            catch (Exception ex)
            {
                return ResultModel<RedisKeyVO>.Fail($"获取键值失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 删除Redis键
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteRedisKey")]
        public async Task<ResultModel<bool>> DeleteRedisKey([FromBody] DeleteRedisKeyDTO dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Key))
                {
                    return ResultModel<bool>.Fail("键名不能为空");
                }
                var result = await _db.KeyDeleteAsync(dto.Key);
                return ResultModel<bool>.Success(result);
            }
            catch (Exception ex)
            {
                return ResultModel<bool>.Fail($"删除键失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 批量删除Redis键
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteRedisKeys")]
        public async Task<ResultModel<int>> DeleteRedisKeys([FromBody] DeleteRedisKeysDTO dto)
        {
            try
            {
                if (dto.Keys == null || dto.Keys.Length == 0)
                {
                    return ResultModel<int>.Fail("键名数组不能为空");
                }
                var redisKeys = dto.Keys.Select(k => (RedisKey)k).ToArray();
                var result = await _db.KeyDeleteAsync(redisKeys);
                return ResultModel<int>.Success((int)result);
            }
            catch (Exception ex)
            {
                return ResultModel<int>.Fail($"批量删除键失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 修改Redis键
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateRedisKey")]
        public async Task<ResultModel<bool>> UpdateRedisKey([FromBody] UpdateRedisKeyDTO dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Key))
                {
                    return ResultModel<bool>.Fail("键名不能为空");
                }
                await _db.StringSetAsync(dto.Key, dto.Value, dto.Ttl.HasValue ? TimeSpan.FromSeconds(dto.Ttl.Value) : null);
                return ResultModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ResultModel<bool>.Fail($"更新键失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 添加Redis键
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddRedisKey")]
        public async Task<ResultModel<bool>> AddRedisKey([FromBody] AddRedisKeyDTO dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Key))
                {
                    return ResultModel<bool>.Fail("键名不能为空");
                }
                var exists = await _db.KeyExistsAsync(dto.Key);
                if (exists)
                {
                    return ResultModel<bool>.Fail("键已存在");
                }
                await _db.StringSetAsync(dto.Key, dto.Value, dto.Ttl.HasValue ? TimeSpan.FromSeconds(dto.Ttl.Value) : null);
                return ResultModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ResultModel<bool>.Fail($"添加键失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 清空数据库
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("FlushDb")]
        public async Task<ResultModel<bool>> FlushDb()
        {
            try
            {
                await _server.FlushDatabaseAsync();
                return ResultModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ResultModel<bool>.Fail($"清空数据库失败: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Redis信息
    /// </summary>
    public class RedisInfoVO
    {
        public string Version { get; set; } = "";
        public string Os { get; set; } = "";
        public string Mode { get; set; } = "";
        public int DbSize { get; set; }
        public int ConnectedClients { get; set; }
        public int ConnectedSlaves { get; set; }
        public int Uptime { get; set; }
        public string TcpPort { get; set; } = "";
        public bool AofEnabled { get; set; }
        public RedisMemoryVO? Memory { get; set; }
        public RedisPersistenceVO? Persistence { get; set; }
        public RedisStatsVO? Stats { get; set; }
        public RedisCpuVO? Cpu { get; set; }
    }

    /// <summary>
    /// Redis内存信息
    /// </summary>
    public class RedisMemoryVO
    {
        public long UsedMemory { get; set; }
        public string UsedMemoryHuman { get; set; } = "";
        public long UsedMemoryPeak { get; set; }
        public string UsedMemoryPeakHuman { get; set; } = "";
        public string MaxMemoryHuman { get; set; } = "";
        public double MemFragmentationRatio { get; set; }
        public string Allocator { get; set; } = "";
        public string RssHuman { get; set; } = "";
    }

    /// <summary>
    /// Redis持久化信息
    /// </summary>
    public class RedisPersistenceVO
    {
        public int RdbChangesSinceLastSave { get; set; }
        public long LastSaveTime { get; set; }
        public string LastSaveStatus { get; set; } = "";
    }

    /// <summary>
    /// RedisCPU信息
    /// </summary>
    public class RedisCpuVO
    {
        public double UsedCpuSys { get; set; }
        public double UsedCpuUser { get; set; }
        public double UsedCpuSysChildren { get; set; }
        public double UsedCpuUserChildren { get; set; }
    }

    /// <summary>
    /// Redis命令统计信息
    /// </summary>
    public class RedisCommandStatVO
    {
        public string Command { get; set; } = "";
        public long Calls { get; set; }
        public long Usec { get; set; }
        public long UsecPerCall { get; set; }
    }

    /// <summary>
    /// Redis统计信息
    /// </summary>
    public class RedisStatsVO
    {
        public long TotalConnectionsReceived { get; set; }
        public long TotalCommandsProcessed { get; set; }
        public long TotalNetInputBytes { get; set; }
        public long TotalNetOutputBytes { get; set; }
        public long KeyspaceHits { get; set; }
        public long KeyspaceMisses { get; set; }
        public string KeyspaceHitRate { get; set; } = "";
        public List<RedisCommandStatVO> CommandStats { get; set; } = new();
    }

    /// <summary>
    /// Redis状态
    /// </summary>
    public class RedisStatusVO
    {
        public string Status { get; set; } = "offline";
        public string? Message { get; set; }
    }

    /// <summary>
    /// Redis键
    /// </summary>
    public class RedisKeyVO
    {
        public string Key { get; set; } = "";
        public string Type { get; set; } = "";
        public int Ttl { get; set; }
        public string Value { get; set; } = "";
        public long Size { get; set; }
    }

    /// <summary>
    /// Redis键列表
    /// </summary>
    public class RedisKeyListVO
    {
        public List<RedisKeyVO> List { get; set; } = new();
        public int Total { get; set; }
    }

    /// <summary>
    /// Redis键查询参数
    /// </summary>
    public class RedisKeyQueryDTO
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? KeyWord { get; set; }
        public string? DataType { get; set; }
    }

    /// <summary>
    /// 修改Redis键
    /// </summary>
    public class UpdateRedisKeyDTO
    {
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
        public int? Ttl { get; set; }
    }

    /// <summary>
    /// 添加Redis键
    /// </summary>
    public class AddRedisKeyDTO
    {
        public string Key { get; set; } = "";
        public string Type { get; set; } = "";
        public string Value { get; set; } = "";
        public int? Ttl { get; set; }
    }

    /// <summary>
    /// 删除Redis键
    /// </summary>
    public class DeleteRedisKeysDTO
    {
        public string[] Keys { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// 删除Redis键
    /// </summary>
    public class DeleteRedisKeyDTO
    {
        public string Key { get; set; } = "";
    }
}