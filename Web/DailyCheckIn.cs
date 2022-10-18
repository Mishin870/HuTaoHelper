using System;
using System.Threading.Tasks;
using HuTaoHelper.Core;
using HuTaoHelper.Web.Client;
using HuTaoHelper.Web.Entities;

namespace HuTaoHelper.Web;

/// <summary>
/// Wrapper for daily check-in logic
/// </summary>
public static class DailyCheckIn {
	public static async Task DoCheckInAsync(Account account) {
		try {
			const string ACT_ID = "e202102251931481";
			var cookie = account.Cookies.ToCookie();
			var client = new ApiClient(cookie);

			// Firstly, we need to check is reward already claimed or not
			// and get current day
			var signDayEntity = (await client.GetExecuteRequest<SignDayEntity>(
				$"https://sg-hk4e-api.hoyolab.com/event/sol/info?act_id={ACT_ID}",
				message => {
					message.WithReferer($"https://act.hoyolab.com/ys/event/signin-sea-v3/index.html?act_id={ACT_ID}");
				})).ValidateResponseCode();

			if (signDayEntity.Data.IsSign) {
				Logging.PostEvent("Already claimed chedk-in reward");
				return;
			}
			
			// Then we get the list of reward for every day
			// just to show a nice message for user
			var homeEntity = (await client.GetExecuteRequest<HomeEntity>(
				$"https://sg-hk4e-api.hoyolab.com/event/sol/home?lang=ru-ru&act_id={ACT_ID}",
				message => {
					message.WithReferer($"https://act.hoyolab.com/");
				})).ValidateResponseCode();
			var currentDay = signDayEntity.Data.TotalSignDay;

			// And finally we claim the reward
			var signResultEntity = (await client.PostExecuteRequest<SignResultEntity>(
				$"https://sg-hk4e-api.hoyolab.com/event/sol/sign",
				new JsonContent(new {
					act_id = ACT_ID,
				}),
				message => {
					message.WithReferer($"https://act.hoyolab.com/ys/event/signin-sea-v3/index.html?act_id={ACT_ID}");
				})).ValidateResponseCode();

			var givenAward = homeEntity.Data.Awards[currentDay];
			Logging.PostEvent($"Reward claimed: {givenAward.Name} x{givenAward.Count}");
		} catch (Exception e) {
			Logging.PostEvent(e.ToString());
		}
	}
}