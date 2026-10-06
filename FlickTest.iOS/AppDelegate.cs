using FlickTest.Game;
using osu.Framework.iOS;

namespace FlickTest.iOS
{
    /// <inheritdoc />
    public class AppDelegate : GameApplicationDelegate
    {
        /// <inheritdoc />
        protected override osu.Framework.Game CreateGame() => new FlickTestGame();
    }
}
