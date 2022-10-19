using System.Globalization;
using System.Windows.Controls;

namespace HuTaoHelper.View.Validation;

public class NotEmptyValidationRule : ValidationRule {
	public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
		return string.IsNullOrWhiteSpace($"{value}")
			? new ValidationResult(false, "Field is required")
			: ValidationResult.ValidResult;
	}
}