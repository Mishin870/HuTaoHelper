using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Win32.TaskScheduler;

namespace HuTaoHelper.Visual.Control; 

public static class TasksSchedulerHelper {
	public const string TASK_DAILY_CHECKIN = "HuTaoHelperDailyCheckIn";

	private const string ROOT_FOLDER = "Mishin870";
	
	private static TaskFolder GetFolder() {
		using var service = new TaskService();
		
		return service.RootFolder.CreateFolder(ROOT_FOLDER, null, false);
	}
	
	public static bool IsExist(string name) {
		using var service = new TaskService();
		
		var folder = GetFolder();
		return folder.Tasks.Any(task => task.Name == name);
	}

	public static void Create(string name, string command, string description = "") {
		using var service = new TaskService();

		var folder = GetFolder();
		var task = service.NewTask();
		task.RegistrationInfo.Description = description;
		task.Triggers.Add(new DailyTrigger { DaysInterval = 1 });
		task.Principal.RunLevel = TaskRunLevel.Highest;

		var assembly = Assembly.GetEntryAssembly();
		if (assembly == null) throw new ApplicationException("No entry assembly");

		var path = Path.GetDirectoryName(assembly.Location);
		if (path == null) throw new ApplicationException("No entry assembly");
		
		task.Actions.Add(new ExecAction(Path.Combine(path, "HuTaoHelper.Console.exe"), command,
			path));
		
		folder.RegisterTaskDefinition(name, task);
	}

	public static void Delete(string name) {
		using var service = new TaskService();

		var folder = GetFolder();
		folder.DeleteTask(name, false);
	}
}