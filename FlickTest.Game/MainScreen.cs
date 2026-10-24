using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace FlickTest.Game
{
    public partial class MainScreen : Screen
    {
        public IEnumerable<Ball> Balls => InternalChildren.OfType<Ball>();

        public BallConfigOverlay Config { get; private set; }

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
                Config = new BallConfigOverlay { Depth = -1 },
            };
        }

        public void ShowBallConfig(Ball ball) => Config.Open(ball);

        // 背景左键单击 -> 在该处生成一个新球
        protected override bool OnClick(ClickEvent e)
        {
            if (e.Button != MouseButton.Left) return false;

            AddInternal(new Ball
            {
                Position = ToLocalSpace(e.ScreenSpaceMousePosition) - ChildSize / 2,
            });
            return true;
        }

        // 背景右键 -> 进入全局设置页
        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (e.Button != MouseButton.Right) return false;

            this.Push(new SettingsScreen());
            return true;
        }
    }
}
