﻿namespace Atlas.Blazor.Server.Models
{
    public class InitialApplicationState
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? IdToken { get; set; }
    }
}
