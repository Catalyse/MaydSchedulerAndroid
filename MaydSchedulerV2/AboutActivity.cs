using Android.App;
using Android.OS;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MaydSchedulerApp
{
    [Activity(Label = "About", Theme = "@android:style/Theme.Material")]
    public class AboutActivity : Activity
    {
        private Button privacy;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AboutPage);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            privacy = FindViewById<Button>(Resource.Id.privacyButton);
            privacy.Click += Privacy_Click;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void Privacy_Click(object sender, System.EventArgs e)
        {
            var uri = Android.Net.Uri.Parse("http://www.mayd.co/privacy/");
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }
    }
}