using NUnit.Framework;
using osu.Framework.Allocation;

namespace FlickTest.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneFlickTestGame : FlickTestTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        [BackgroundDependencyLoader]
        private void load()
        {
            AddGame(new FlickTestGame());
        }
    }
}
