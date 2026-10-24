using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK;
using osuTK.Input;

namespace FlickTest.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneSettings : ManualInputManagerTestScene
    {
        private ScreenStack stack;
        private MainScreen screen;

        [SetUpSteps]
        public void SetUpSteps()
        {
            AddStep("reset screen", () =>
            {
                Content.Clear();
                Content.Add(stack = new ScreenStack(screen = new MainScreen()) { RelativeSizeAxes = Axes.Both });
            });
            AddUntilStep("main screen loaded", () => screen.IsLoaded);

            AddStep("reset settings", () =>
            {
                BallSettings.FlickWindow.SetDefault();
                BallSettings.DragScale.SetDefault();
                BallSettings.StopSpeed.SetDefault();
            });
        }

        [Test]
        public void TestRightClickOpensSettings()
        {
            AddStep("right click background", () =>
            {
                InputManager.MoveMouseTo(screen.ToScreenSpace(new Vector2(20)));
                InputManager.Click(MouseButton.Right);
            });
            AddAssert("settings screen shown", () => stack.CurrentScreen is SettingsScreen);
        }

        [Test]
        public void TestEscapeReturns()
        {
            AddStep("push settings", () => screen.Push(new SettingsScreen()));
            AddUntilStep("settings loaded", () => stack.CurrentScreen is SettingsScreen settings && settings.IsLoaded);
            AddStep("press escape", () => InputManager.PressKey(Key.Escape));
            AddStep("release escape", () => InputManager.ReleaseKey(Key.Escape));
            AddAssert("back to main", () => stack.CurrentScreen is MainScreen);
        }

        [Test]
        public void TestClickBallOpensConfig()
        {
            AddStep("click first ball", () =>
            {
                InputManager.MoveMouseTo(screen.Balls.First());
                InputManager.Click(MouseButton.Left);
            });
            AddUntilStep("config visible", () => screen.Config.State.Value == Visibility.Visible);
        }

        [Test]
        public void TestApplyToAll()
        {
            List<Ball> balls = null;

            AddStep("spawn second ball", () =>
            {
                InputManager.MoveMouseTo(screen.ToScreenSpace(new Vector2(300)));
                InputManager.Click(MouseButton.Left);
            });
            AddUntilStep("two balls", () => screen.Balls.Count() == 2);
            AddStep("capture balls", () => balls = screen.Balls.ToList());
            AddStep("set first friction", () => balls[0].Friction.Value = 0.9f);
            AddStep("open config", () => screen.Config.Open(balls[0]));
            AddUntilStep("config visible", () => screen.Config.State.Value == Visibility.Visible);
            AddStep("apply to all", () => screen.Config.ApplyToAll());
            AddAssert("second ball synced", () => balls[1].Friction.Value == 0.9f);
        }
    }
}
