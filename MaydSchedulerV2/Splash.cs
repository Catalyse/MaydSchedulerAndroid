using Android.App;
using Android.OS;
using Android.Views;

namespace MaydSchedulerApp
{
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, NoHistory = true)]
    public class Splash : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            System.Threading.Thread.Sleep(1000); //Let's wait awhile...
            Start();
        }

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            Start();
            return base.DispatchTouchEvent(ev);
        }

        public void Start()
        {
            StartActivity(typeof(MainActivity));
        }
    }
}