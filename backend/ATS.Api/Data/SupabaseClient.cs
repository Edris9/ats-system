using Npgsql;
using Dapper;

namespace ATS.Api.Data;

public class SupabaseClient
{
    private readonly string _connectionString;

    public SupabaseClient(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Supabase")!;
        
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}