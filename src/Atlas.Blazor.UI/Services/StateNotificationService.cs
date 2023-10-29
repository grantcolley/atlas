using Atlas.Blazor.UI.Interfaces;

namespace Atlas.Blazor.UI.Services
{
    public class StateNotificationService : IStateNotificationService
    {
        private readonly Dictionary<string, Action> actions;
        private readonly Dictionary<string, Func<object, Task>> functions;

        public StateNotificationService()
        {
            actions = new Dictionary<string, Action>();
            functions = new Dictionary<string, Func<object, Task>>();
        }

        public void Register(string target, Func<object, Task> function)
        {
            if (functions.ContainsKey(target))
            {
                return;
            }

            functions.Add(target, function);
        }

        public void Register(string target, Action action)
        {
            if (actions.ContainsKey(target))
            {
                return;
            }

            actions.Add(target, action);
        }

        public void Deregister(string target)
        {
            actions.Remove(target);
            functions.Remove(target);
        }

        public void NotifyStateHasChanged(string target)
        {
            if (actions.ContainsKey(target))
            {
                actions[target].Invoke();
            }
        }

        public async Task NotifyStateHasChangedAsync(string target, object parameter)
        {
            if (functions.ContainsKey(target))
            {
                await functions[target].Invoke(parameter).ConfigureAwait(false);
            }
        }
    }
}
