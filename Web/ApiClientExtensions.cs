using System.Threading.Tasks;
using HuTaoHelper.Core;
using HuTaoHelper.Web.Client;
using HuTaoHelper.Web.Entities;

namespace HuTaoHelper.Web; 

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
}