using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace FlickTest.Game
{
    public partial class SettingsScreen : Screen
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            var backButton = new BasicButton
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                Margin = new MarginPadding(20),
                Size = new Vector2(48),
                Action = this.Exit,
            };

            backButton.Add(new SpriteIcon
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(18),
                Icon = FontAwesome.Solid.ChevronLeft,
            });

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4(0.07f, 0.07f, 0.11f, 1f),
                },
                new FillFlowContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 10),
                    Children = new Drawable[]
                    {
                        new SpriteText
                        {
                            Text = "Ball Settings",
                            Font = new FontUsage(size: 28),
                            Margin = new MarginPadding { Bottom = 10 },
                        },
                        new SettingRow("Flick window (ms)", BallSettings.FlickWindow),
                        new SettingRow("Drag scale", BallSettings.DragScale),
                        new SettingRow("Stop speed", BallSettings.StopSpeed),
                        new SpriteText
                        {
                            Text = "Esc or the arrow to return",
                            Font = new FontUsage(size: 14),
                            Colour = new Color4(0.7f, 0.7f, 0.8f, 1f),
                            Margin = new MarginPadding { Top = 10 },
                        },
                    },
                },
                backButton,
            };
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key != Key.Escape) return false;

            this.Exit();
            return true;
        }
    }

    internal partial class SettingRow : Container
    {
        public SettingRow(string label, BindableFloat bindable)
        {
            Size = new Vector2(440, 26);

            var value = new SpriteText
            {
                Position = new Vector2(390, 3),
                Font = new FontUsage(size: 16),
            };

            AddRange(new Drawable[]
            {
                new SpriteText
                {
                    Text = label,
                    Position = new Vector2(0, 3),
                    Font = new FontUsage(size: 16),
                },
                new BasicSliderBar<float>
                {
                    Position = new Vector2(170, 3),
                    Size = new Vector2(200, 20),
                    Current = bindable,
                    KeyboardStep = 1,
                },
                value,
            });

            bindable.BindValueChanged(v => value.Text = v.NewValue.ToString("0.00"), true);
        }
    }
}
