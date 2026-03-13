using System;
using UnityEngine;

namespace Echoes.Client.Application
{
    public record TestUserDto(Guid Id, string UserName);

    public class CSharp10Test : MonoBehaviour
    {
        public string ServerStatus { get; init; } = "Offline";

        private void Start()
        {
            const string version = "1.0";
            string header = $"[Server v{version}]";

            var user = new TestUserDto(Guid.NewGuid(), "DevPlayer");

            // 5. C# 9: Value-based equality check
            var userClone = user with { }; // Creates a shallow copy
            bool isSame = user == userClone;

            Debug.Log($"{header} Connection Success!");
            Debug.Log($"User: {user.UserName} | Hash Match: {isSame}");
        }
    }
}