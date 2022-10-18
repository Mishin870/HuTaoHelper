using System.Net.Http;
using System.Threading.Tasks;
using HuTaoHelper.Core;

namespace HuTaoHelper.Web; 

/// <summary>
/// Wrapper for daily check-in logic
/// </summary>
public static class DailyCheckIn {
	private static readonly HttpClient Client = new HttpClient();
	
	public static async Task DoCheckInAsync(Account account) {
		// Just for testing purposes
		await Task.Delay(1000);
		Logging.PostEvent("Daily check-in completed!");
	}
}