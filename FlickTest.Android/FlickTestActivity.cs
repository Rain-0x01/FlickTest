using Android.App;
using osu.Framework.Android;

namespace FlickTest.Android
{
    [Activity(ConfigurationChanges = DEFAULT_CONFIG_CHANGES, Exported = true, LaunchMode = DEFAULT_LAUNCH_MODE, MainLauncher = true)]
    public class FlickTestActivity : AndroidGameActivity
    {
        protected override osu.Framework.Game CreateGame() => new Game.FlickTestGame();
    }
}
