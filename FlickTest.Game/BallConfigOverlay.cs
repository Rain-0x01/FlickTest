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
            colourProxy.BindValueChanged(v => apply(b => b.BallColour.Value = v.NewValue));

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4(0, 0, 0, 0.7f),
                },
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(480, 700),
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Color4(0.1f, 0.1f, 0.15f, 1f),
                        },
                        new FillFlowContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0, 8),
                            Children = new Drawable[]
                            {
                                new SpriteText
                                {
                                    Text = "Ball",
                                    Font = new FontUsage(size: 24),
                                    Margin = new MarginPadding { Bottom = 6 },
                                },
                                new SettingRow("Size", sizeProxy),
                                new SettingRow("Glow radius", glowProxy),
                                new SettingRow("Friction", frictionProxy),
                                new SettingRow("Bounce", bounceProxy),
                                new BasicColourPicker
                                {
                                    Width = 200,
                                    Current = colourProxy,
                                },
                                new FillFlowContainer
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Direction = FillDirection.Horizontal,
                                    Spacing = new Vector2(10, 0),
                                    Margin = new MarginPadding { Top = 8 },
                                    Children = new Drawable[]
                                    {
                                        new BasicButton
                                        {
                                            Size = new Vector2(140, 40),
                                            Text = "Apply to all",
                                            Action = ApplyToAll,
                                        },
                                        new BasicButton
                                        {
                                            Size = new Vector2(100, 40),
                                            Text = "Close",
                                            Action = Hide,
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };
        }

        public void Open(Ball ball)
        {
            target = ball;

            sizeProxy.Value = ball.BallSize.Value;
            glowProxy.Value = ball.GlowRadius.Value;
            frictionProxy.Value = ball.Friction.Value;
            bounceProxy.Value = ball.Bounce.Value;
            colourProxy.Value = ball.BallColour.Value;

            Show();
        }

        private void apply(Action<Ball> action)
        {
            if (target != null)
                action(target);
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

        protected override void PopIn() => this.FadeIn(150, Easing.OutQuint);

        protected override void PopOut() => this.FadeOut(150, Easing.OutQuint);

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key != Key.Escape) return false;

            Hide();
            return true;
        }
    }
}
