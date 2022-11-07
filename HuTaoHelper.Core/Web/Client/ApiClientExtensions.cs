using HuTaoHelper.Core.Core;
using HuTaoHelper.Core.Web.Entities;

namespace HuTaoHelper.Core.Web.Client; 

public static class ApiClientExtensions {
	public static async Task<SignDayEntity> GetSignDay(this ApiClient client) {
		return await client.GetRequestAsync<SignDayEntity>(
			$"{Constants.ApiSignDay}{Constants.ApiActId}",
			message => {
				message.WithReferer($"{Constants.ApiSignDayRef}{Constants.ApiActId}");
			});
	}
	
	public static async Task<HomeEntity> GetHome(this ApiClient client) {
		return await client.GetRequestAsync<HomeEntity>(
			$"{Constants.ApiHome}{Constants.ApiActId}",
			message => {
				message.WithReferer($"{Constants.ApiHomeRef}");
			});
	}

	public static async Task<SignResultEntity> DoSign(this ApiClient client) {
		return await client.PostRequestAsync<SignResultEntity>(
			$"{Constants.ApiSign}",
			new JsonContent(new {
				act_id = Constants.ApiActId,
			}),
			message => {
				message.WithReferer(
					$"{Constants.ApiSignRef}{Constants.ApiActId}");
			});
	}
	
	public static async Task<UserAccountInfoEntity> GetUserAccountInfo(this ApiClient client) {
		return await client.GetRequestAsync<UserAccountInfoEntity>(
			$"{Constants.ApiUserAccount}{Timestamp()}",
			message => {
				message.WithReferer($"{Constants.ApiUserAccountRef}");
			});
	}
	
	public static async Task<GameAccountsInfoEntity> GetGameAccountsInfo(this ApiClient client, string accountId) {
		return await client.GetRequestAsync<GameAccountsInfoEntity>(
			$"{Constants.ApiGameAccounts}{accountId}",
			message => {
				message.WithReferer($"{Constants.ApiGameAccountsRef}");
			});
	}

	private static long Timestamp() {
		const int NUMBER = 10000 * 1000;
		return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / NUMBER;
	}
}