using System.Globalization;
using System.Windows.Controls;
using HuTaoHelper.Core.Localization;

namespace HuTaoHelper.Visual.View.Validation;

public class NotEmptyValidationRule : ValidationRule {
	public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
		return string.IsNullOrWhiteSpace($"{value}")
			? new ValidationResult(false, Translations.LocRequiredField)
			: ValidationResult.ValidResult;
	}
}