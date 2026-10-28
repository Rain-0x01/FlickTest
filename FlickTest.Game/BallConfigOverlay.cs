using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace FlickTest.Game
{
    public partial class BallConfigOverlay : OverlayContainer
    {
        private readonly BindableFloat sizeProxy = new(100) { MinValue = 20, MaxValue = 300 };
        private readonly BindableFloat glowProxy = new(30) { MinValue = 0, MaxValue = 100 };
        private readonly BindableFloat frictionProxy = new(0.4f) { MinValue = 0.05f, MaxValue = 0.95f };
        private readonly BindableFloat bounceProxy = new(0.75f) { MinValue = 0, MaxValue = 1 };
        private readonly Bindable<Colour4> colourProxy = new(Colour4.White);

        private Ball target;
        private Container panel;
        private BasicColourPicker colourPicker;

        public BallConfigOverlay()
        {
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            sizeProxy.BindValueChanged(v => apply(b => b.BallSize.Value = v.NewValue));
            glowProxy.BindValueChanged(v => apply(b => b.GlowRadius.Value = v.NewValue));
            frictionProxy.BindValueChanged(v => apply(b => b.Friction.Value = v.NewValue));
            bounceProxy.BindValueChanged(v => apply(b => b.Bounce.Value = v.NewValue));

            var swatchCircle = new Circle
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(30),
            };

            colourProxy.BindValueChanged(v =>
            {
                apply(b => b.BallColour.Value = v.NewValue);
                swatchCircle.Colour = v.NewValue;
            }, true);

            var swatch = new ClickableContainer
            {
                Position = new Vector2(170, 0),
                Size = new Vector2(40),
                Action = toggleColourPicker,
                Children = new Drawable[]
                {
                    new Circle
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = new Color4(1, 1, 1, 0.35f),
                    },
                    swatchCircle,
                },
            };

            colourPicker = new BasicColourPicker
            {
                Width = 200,
                Current = colourProxy,
            };

            panel = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AutoSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = new Color4(0.1f, 0.1f, 0.15f, 1f),
                    },
                    new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 8),
                        Margin = new MarginPadding(20),
                        Children = new Drawable[]
                        {
                            new SpriteText
                            {
                                Text = "Ball",
                                Font = new FontUsage(size: 24),
                                Margin = new MarginPadding { Bottom = 6 },
                            },
                            new Container
                            {
                                Size = new Vector2(440, 40),
                                Children = new Drawable[]
                                {
                                    new SpriteText
                                    {
                                        Text = "Colour",
                                        Position = new Vector2(0, 10),
                                        Font = new FontUsage(size: 16),
                                    },
                                    swatch,
                                },
                            },
                            new SettingRow("Size", sizeProxy),
                            new SettingRow("Glow radius", glowProxy),
                            new SettingRow("Friction", frictionProxy),
                            new SettingRow("Bounce", bounceProxy),
                            colourPicker,
                            new BasicButton
                            {
                                Size = new Vector2(140, 40),
                                Margin = new MarginPadding { Top = 8 },
                                Text = "Apply to all",
                                Action = ApplyToAll,
                            },
                        },
                    },
                },
            };

            InternalChildren = new Drawable[] { panel };
        }

        public void Open(Ball ball)
        {
            target = ball;

            sizeProxy.Value = ball.BallSize.Value;
            glowProxy.Value = ball.GlowRadius.Value;
            frictionProxy.Value = ball.Friction.Value;
            bounceProxy.Value = ball.Bounce.Value;
            colourProxy.Value = ball.BallColour.Value;
            colourPicker.Alpha = 0;

            Show();
        }

        private void apply(Action<Ball> action)
        {
            if (target != null)
                action(target);
        }

        private void toggleColourPicker()
        {
            colourPicker.Alpha = colourPicker.Alpha > 0 ? 0 : 1;
        }

        public void ApplyToAll()
        {
            if (target == null || Parent is not MainScreen main) return;

            foreach (var ball in main.Balls.ToList())
            {
                if (ball == target) continue;

                ball.BallSize.Value = sizeProxy.Value;
                ball.GlowRadius.Value = glowProxy.Value;
                ball.Friction.Value = frictionProxy.Value;
                ball.Bounce.Value = bounceProxy.Value;
                ball.BallColour.Value = colourProxy.Value;
            }
        }

        protected override void PopIn() => this.FadeIn(100, Easing.OutQuint);

        protected override void PopOut()
        {
            ClearTransforms();
            GetContainingFocusManager()?.ChangeFocus(null);
            Alpha = 0;
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (panel.ReceivePositionalInputAt(e.ScreenSpaceMousePosition))
                return true;

            Hide();
            return true;
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key != Key.Escape) return false;

            Hide();
            return true;
        }
    }
}
