using System;
using System.Threading.Tasks;
using HuTaoHelper.Core;
using HuTaoHelper.Web.Client;

namespace HuTaoHelper.Web;

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
				Logging.PostEvent("Already claimed check-in reward");
				return;
			}
			
			// Then we get the list of reward for every day
			// just to show a nice message for user
			var homeEntity = (await client.GetHome()).ValidateResponseCode();
			var currentDay = signDayEntity.Data.TotalSignDay;

			// And finally we claim the reward
			var signResultEntity = (await client.DoSign()).ValidateResponseCode();

			var givenAward = homeEntity.Data.Awards[currentDay];
			Logging.PostEvent($"Reward claimed: {givenAward.Name} x{givenAward.Count}");
		} catch (Exception e) {
			Logging.PostEvent(e);
		}
	}
}