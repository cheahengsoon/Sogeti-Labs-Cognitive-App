using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace CognitiveApp
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

		    NavigationPage nav = new NavigationPage(new MainPage()) {
		        BarBackgroundColor = Color.FromHex("#F2F6F2")
		    };

		    if(Device.RuntimePlatform == Device.Android) {
		        nav.BarTextColor = Color.Black;
		    }

		    MainPage = nav;
		}
	}
}
