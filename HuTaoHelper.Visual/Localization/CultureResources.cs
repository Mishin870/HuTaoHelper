using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using HuTaoHelper.Core.Core;
using HuTaoHelper.Core.Localization;

namespace HuTaoHelper.Visual.Localization;

public static class CultureResources {
	public static readonly List<CultureInfo> SupportedCultures = new();

	static CultureResources() {
		foreach (var directory in Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory)) {
			try {
				var directoryInfo = new DirectoryInfo(directory);
				var culture = CultureInfo.GetCultureInfo(directoryInfo.Name);

				if (directoryInfo.GetFiles(Path.GetFileNameWithoutExtension(
					    typeof(Translations).Assembly.Location) + ".resources.dll").Length <= 0) continue;
					
				SupportedCultures.Add(culture);
			} catch (ArgumentException) {
			}
		}
	}

	public static Translations GetResourceInstance() {
		return new SubTranslations();
	}

	private static ObjectDataProvider? CachedResourceProvider;

	public static ObjectDataProvider ResourceProvider {
		get {
			if (CachedResourceProvider == null) {
				var provider = Application.Current.FindResource("Translations");

				CachedResourceProvider = provider as ObjectDataProvider
				                         ?? throw new ArgumentException("Translations provider is null");
			}

			return CachedResourceProvider;
		}
	}

	public static void ChangeCulture(CultureInfo culture) {
		if (!SupportedCultures.Contains(culture)) {
			Logging.PostEvent($"Culture {culture} is not available");
			return;
		}
			
		Translations.Culture = culture;
		ResourceProvider.Refresh();
	}
}