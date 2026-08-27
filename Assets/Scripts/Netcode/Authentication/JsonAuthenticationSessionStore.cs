using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Netcode;
using UnityEngine;

namespace Netcode.Authentication
{
    public sealed class JsonAuthenticationSessionStore : IAuthenticationSessionStore
    {
        private readonly string _path;

        public JsonAuthenticationSessionStore(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(nameof(path));
            _path = path;
        }

        public async Task<AuthenticationResult?> LoadASync(CancellationToken ctx)
        {
            ctx.ThrowIfCancellationRequested();
            if (!File.Exists(_path))
                return null;

            await using FileStream stream = new(
                _path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true
                );

            using StreamReader reader = new(stream, Encoding.UTF8);

            string json = await reader.ReadToEndAsync();
            ctx.ThrowIfCancellationRequested();
            
            AuthenticationSessionDto dto = JsonUtility.FromJson<AuthenticationSessionDto>(json);
            if (dto is null || string.IsNullOrWhiteSpace(dto.accessToken))
                return null;
            if (string.IsNullOrWhiteSpace(dto.refreshToken))
                Debug.LogWarning("Loaded authentication session without refresh token");
            else
                Debug.Log("Loaded authentication session");
            Debug.Log("Returning with " + dto.accessToken + " and " + dto.refreshToken);
            //TODO here breaks
            return new AuthenticationResult(new AccessToken(dto.accessToken), new AccessToken(dto.refreshToken));
        }

        public async Task SaveAsync(AuthenticationResult authResult, CancellationToken ctx)
        {
            if (string.IsNullOrWhiteSpace(authResult.AccessToken.Value))
                throw new ArgumentException(nameof(authResult.AccessToken.Value));
            AuthenticationSessionDto dto = new()
            {
                accessToken = authResult.AccessToken.Value,
                refreshToken = authResult.RefreshToken.Value
            };

            var json = JsonUtility.ToJson(dto, prettyPrint: true);
            
            ctx.ThrowIfCancellationRequested();

            await using FileStream stream = new(
                _path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                useAsync: true
                );

            await using StreamWriter writer = new(stream, Encoding.UTF8);
            await writer.WriteAsync(json.AsMemory(), ctx);
            Debug.Log("Saved authentication session");
        }

        public Task ClearAsync(CancellationToken ctx)
        {
            ctx.ThrowIfCancellationRequested();
            
            if (File.Exists(_path))
                File.Delete(_path);
            
            return Task.CompletedTask;
        }
    }
}