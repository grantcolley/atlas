﻿namespace Atlas.Blazor.Components.Interfaces
{
    public interface IStateNotification
    {
        void Deregister(string target);
        void Register(string target, Action action);
        void Register(string target, Func<object, Task> functiona);
        void NotifyStateHasChanged(string target);
        Task NotifyStateHasChangedAsync(string target, object parameter);
    }
}
