using FlickTest.Game;
using osu.Framework;
using osu.Framework.Platform;

namespace FlickTest.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"FlickTest"))
            using (osu.Framework.Game game = new FlickTestGame())
                host.Run(game);
        }
    }
}
