
using library.Models;
using Npgsql;

namespace library.Data;

public class MemberRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public MemberRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    private static Member ReadMember(NpgsqlDataReader reader)
    {
        return new Member
        {
            MemberId = reader.GetInt64(0),
            FullName = reader.GetString(1),
            Email = reader.IsDBNull(2) ? null : reader.GetString(2),
            MemberType = reader.GetString(3)
        };
    }

    // READ: every member, with how many books each has out now.
    public async Task<List<Member>> GetAllAsync()
    {
        const string sql =
    "SELECT m.member_id, m.full_name, m.email, m.member_type, COUNT(l.loan_id) " +
    "FROM lending.member m " +
    "LEFT JOIN lending.loan l ON l.member_id = m.member_id AND l.returned_on IS NULL " +
    "GROUP BY m.member_id " +
    "ORDER BY m.full_name;";

        var members = new List<Member>();
        await using var command = _dataSource.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var member = ReadMember(reader);
            member.OpenLoans = reader.GetInt64(4);
            members.Add(member);
        }

        return members;
    }

    public async Task AddAsync(Member member)
    {
        const string sql = "INSERT INTO lending.member (full_name, email, member_type) VALUES (@name, @email, @type);";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("name", member.FullName);
        command.Parameters.AddWithValue("email", (object?)member.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("type", member.MemberType);

        await command.ExecuteNonQueryAsync();
    }


    public async Task<Member?> GetByIdAsync(long id)
    {
        const string sql = "SELECT member_id, full_name, email, member_type FROM lending.member WHERE member_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? ReadMember(reader) : null;
    }

    public async Task UpdateAsync(Member member)
    {
        const string sql = "UPDATE lending.member SET full_name = @name, email = @email, member_type = @type WHERE member_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("name", member.FullName);
        command.Parameters.AddWithValue("email", (object?)member.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("type", member.MemberType);
        command.Parameters.AddWithValue("id", member.MemberId);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(long id)
    {
        const string sql = "DELETE FROM lending.member WHERE member_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);

        await command.ExecuteNonQueryAsync();
    }


}
