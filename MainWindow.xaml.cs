using System.Windows;
using HuTaoHelper.Control;
using HuTaoHelper.Core;

namespace HuTaoHelper {
	public partial class MainWindow {
		public MainWindow() {
			InitializeComponent();
		}

		private async void Test_OnClick(object sender, RoutedEventArgs e) {
			Settings.Load();
			
			var handles = Automation.FindGameHandles();
			if (handles.Count == 0) return;
			
			await handles[0].AutologinAsync(new Account {
				Login = "test@test.ru",
				Password = "test",
				Name = "TestAccount"
			});
		}
	}
}