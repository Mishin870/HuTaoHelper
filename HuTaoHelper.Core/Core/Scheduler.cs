namespace HuTaoHelper.Core.Core; 

/// <summary>
/// Scheduler for binding to delayed events
/// </summary>
public static class Scheduler {
	/// <summary>
	/// Actions before app shutdown
	/// </summary>
	public static readonly SchedulerChannel ChannelShutdown = new();
}

/// <summary>
/// Specific scheduler channel
/// </summary>
public class SchedulerChannel {
	private readonly Queue<Action> Actions = new();

	public void Post(Action action) {
		Actions.Enqueue(action);
	}

	public void RunAll() {
		while (Actions.TryDequeue(out var action)) {
			action();
		}
	}
}