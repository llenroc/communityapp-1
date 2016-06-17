using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using TechreadyForms.Droid;

[Activity(Exported  = true,Theme = "@style/Theme.Splash", //Indicates the theme to use for this activity
             MainLauncher = true, //Set it as boot activity
             NoHistory = true)] //Doesn't place it in back stack
public class SplashActivity : Activity
{
    protected override void OnCreate(Bundle bundle)
    {
        base.OnCreate(bundle);

        RequestedOrientation = ScreenOrientation.Portrait;


        System.Threading.Thread.Sleep(3000); //Let's wait awhile...
        this.StartActivity(typeof(MainActivity));



    }
}