using HuTaoHelper.Core.Core;
using HuTaoHelper.Core.Localization;
using HuTaoHelper.Core.Web.Client;

namespace HuTaoHelper.Core.Web.Tools;

/// <summary>
/// Wrapper for daily check-in logic
/// </summary>
public static class DailyCheckIn {
	public static async Task DoCheckInAsync(Account account) {
		try {
			var cookie = account.Cookies.ToCookie();
			var client = new ApiClient(cookie);

			// Firstly, we need to check is reward already claimed or not
			// and get current day
			var signDayEntity = (await client.GetSignDay()).ValidateResponseCode();

			if (signDayEntity.Data.IsSign) {
				Logging.PostEvent(Translations.LocCheckInAlready);
				return;
			}
			
			// Then we get the list of reward for every day
			// just to show a nice message for user
			var homeEntity = (await client.GetHome()).ValidateResponseCode();
			var currentDay = signDayEntity.Data.TotalSignDay;

			// And finally we claim the reward
			var signResultEntity = (await client.DoSign()).ValidateResponseCode();

			var givenAward = homeEntity.Data.Awards[currentDay];

			Logging.PostEvent(Translations.LocCheckInReward
				.Replace("$1", givenAward.Name)
				.Replace("$2", givenAward.Count.ToString()));
		} catch (Exception e) {
			Logging.PostEvent(e);
		}
	}
}