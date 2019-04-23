using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using TesteJuntos.Domain.Models.Identity;

namespace TesteJuntos.Infra.Stores
{
    public class RoleStore : IRoleStore<ApiRole>
    {
        public async Task<IdentityResult> CreateAsync(ApiRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = ConnectionFactory.GetTesteJuntosConnection())
            {
                await connection.OpenAsync(cancellationToken);
                role.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO [ApiRole] ([Name], [NormalizedName])
                    VALUES (@{nameof(ApiRole.Name)}, @{nameof(ApiRole.NormalizedName)});
                    SELECT CAST(SCOPE_IDENTITY() as int)", role);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(ApiRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = ConnectionFactory.GetTesteJuntosConnection())
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($@"UPDATE [ApiRole] SET
                    [Name] = @{nameof(ApiRole.Name)},
                    [NormalizedName] = @{nameof(ApiRole.NormalizedName)}
                    WHERE [Id] = @{nameof(ApiRole.Id)}", role);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApiRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = ConnectionFactory.GetTesteJuntosConnection())
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($"DELETE FROM [ApiRole] WHERE [Id] = @{nameof(ApiRole.Id)}", role);
            }

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(ApiRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(ApiRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(ApiRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(ApiRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(ApiRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public async Task<ApiRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = ConnectionFactory.GetTesteJuntosConnection())
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApiRole>($@"SELECT * FROM [ApiRole]
                    WHERE [Id] = @{nameof(roleId)}", new { roleId });
            }
        }

        public async Task<ApiRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = ConnectionFactory.GetTesteJuntosConnection())
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApiRole>($@"SELECT * FROM [ApiRole]
                    WHERE [NormalizedName] = @{nameof(normalizedRoleName)}", new { normalizedRoleName });
            }
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }
    }
}

/*
CREATE TABLE[dbo].[ApiRole]
(

   [Id] INT NOT NULL PRIMARY KEY IDENTITY,

   [Name] NVARCHAR(256) NOT NULL,

   [NormalizedName] NVARCHAR(256) NOT NULL
)

GO

CREATE INDEX[IX_ApiRole_NormalizedName] ON[dbo].[ApiRole]
([NormalizedName])


CREATE TABLE[dbo].[ApiUser]
(

[Id] INT NOT NULL PRIMARY KEY IDENTITY,

[UserName] NVARCHAR(256) NOT NULL,

[NormalizedUserName] NVARCHAR(256) NOT NULL,

[Email] NVARCHAR(256) NULL,
    [NormalizedEmail] NVARCHAR(256) NULL,
    [EmailConfirmed] BIT NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NULL,
    [PhoneNumber] NVARCHAR(50) NULL,
    [PhoneNumberConfirmed] BIT NOT NULL,
    [TwoFactorEnabled] BIT NOT NULL
)

GO

CREATE INDEX[IX_ApiUser_NormalizedUserName] ON[dbo].[ApiUser]
([NormalizedUserName])

GO

CREATE INDEX[IX_ApiUser_NormalizedEmail] ON[dbo].[ApiUser]
([NormalizedEmail])

CREATE TABLE[dbo].[ApiUserRole]
(

[UserId] INT NOT NULL,
	[RoleId] INT NOT NULL
    PRIMARY KEY([UserId], [RoleId]),
    CONSTRAINT[FK_ApiUserRole_User] FOREIGN KEY([UserId]) REFERENCES[ApiUser] ([Id]),
    CONSTRAINT[FK_ApiUserRole_Role] FOREIGN KEY([RoleId]) REFERENCES[ApiRole] ([Id])
)
*/
