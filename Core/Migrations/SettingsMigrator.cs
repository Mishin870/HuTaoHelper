using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HuTaoHelper.Core.Migrations;

/// <summary>
/// Migrator for <c>Settings</c>
/// </summary>
public sealed class SettingsMigrator : Migrator {
	private readonly dynamic Root;

	public SettingsMigrator(string jsonText) {
		Root = JObject.Parse(jsonText);
		
		RegisterMigration(-1, 0, () => {
		});
		RegisterMigration(0, 1, () => {
		});
		RegisterMigration(1, 2, () => {
			Root.Language = "";
		});
	}

	public string GetResult() {
		return Root.ToString(Formatting.None);
	}

	protected override void RegisterMigration(long fromVersion, long toVersion, Action action) {
		base.RegisterMigration(fromVersion, toVersion, () => {
			action();
			Root.Version = toVersion;
		});
	}

	protected override long FetchVersion() {
		if (Root.ContainsKey("Version")) {
			if (long.TryParse(Root.Version.ToString(), out long version)) {
				return version;
			}
		}

		return -1;
	}
}