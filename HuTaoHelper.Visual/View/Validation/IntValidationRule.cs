using System.Globalization;
using System.Windows.Controls;
using HuTaoHelper.Core.Localization;

namespace HuTaoHelper.Visual.View.Validation;

public class IntValidationRule : ValidationRule {
	public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
		return int.TryParse($"{value}", out _)
			? ValidationResult.ValidResult
			: new ValidationResult(false, Translations.LocWrongIntFormat);
	}
}