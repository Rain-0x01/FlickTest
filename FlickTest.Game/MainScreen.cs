using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osuTK.Graphics;

namespace FlickTest.Game
{
    public partial class MainScreen : Screen
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            var mid = new Color4(0.42f, 0.28f, 0.85f, 1f);

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new ColourInfo
                    {
                        TopLeft = new Color4(0.2f, 0.4f, 0.95f, 1f),
                        TopRight = mid,
                        BottomLeft = mid,
                        BottomRight = new Color4(0.65f, 0.15f, 0.85f, 1f),
                    },
                },
                new Ball(),
            };
        }
    }
}
