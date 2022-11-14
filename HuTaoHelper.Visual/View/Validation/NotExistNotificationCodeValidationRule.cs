using System.Globalization;
using System.Windows.Controls;
using HuTaoHelper.Core.Core;
using HuTaoHelper.Core.Localization;

namespace HuTaoHelper.Visual.View.Validation;

public class NotExistNotificationCodeValidationRule : ValidationRule {
	public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
		return Settings.Instance.GetNotificationTarget($"{value}") != null
			? new ValidationResult(false, Translations.LocNotificationAlreadyExist)
			: ValidationResult.ValidResult;
	}
}