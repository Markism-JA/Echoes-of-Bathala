using System;
using System.Collections.Generic;

namespace Echoes.Shared.Network.Contract
{
    [Serializable]
    public class ErrorResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();

        public ErrorResponse() { }

        public ErrorResponse(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
