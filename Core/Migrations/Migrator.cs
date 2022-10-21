using System;
using System.Collections.Generic;

namespace HuTaoHelper.Core.Migrations;

/// <summary>
/// Smart migration service for any system entity
/// </summary>
public abstract class Migrator {
	private readonly Dictionary<long, MigrationDefinition> Definitions = new();

	protected abstract long FetchVersion();

	/// <summary>
	/// Run all possible migrations. This method will continiously check if current entity version has
	/// possible migrations and run them
	/// </summary>
	/// <exception cref="ArgumentException"></exception>
	public void Run() {
		var lastVersion = long.MinValue;

		while (true) {
			var currentVersion = FetchVersion();

			if (lastVersion == currentVersion) {
				throw new ArgumentException("Infinite loop detected. " +
				                            $"Version is not changed after migration! Version: {currentVersion}");
			}

			lastVersion = currentVersion;

			if (Definitions.ContainsKey(currentVersion)) {
				var definition = Definitions[currentVersion];
				definition.Action();
			} else {
				return;
			}
		}
	}

	/// <summary>
	/// Register a migration from version to version
	/// </summary>
	/// <param name="fromVersion">From which version</param>
	/// <param name="toVersion">To which version</param>
	/// <param name="action">Action to perform on migration</param>
	protected virtual void RegisterMigration(long fromVersion, long toVersion, Action action) {
		Definitions[fromVersion] = new MigrationDefinition(action, toVersion);
	}

	private sealed record MigrationDefinition(Action Action, long ToVersion) {
		public readonly Action Action = Action;
		public readonly long ToVersion = ToVersion;
	}
}