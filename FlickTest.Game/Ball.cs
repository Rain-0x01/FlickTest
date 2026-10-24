using System;
using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace FlickTest.Game
{
    public partial class Ball : Circle
    {
        public readonly BindableFloat BallSize = new(100) { MinValue = 20, MaxValue = 300 };
        public readonly BindableFloat GlowRadius = new(30) { MinValue = 0, MaxValue = 100 };
        public readonly BindableFloat Friction = new(0.4f) { MinValue = 0.05f, MaxValue = 0.95f };
        public readonly BindableFloat Bounce = new(0.75f) { MinValue = 0, MaxValue = 1 };
        public readonly Bindable<Colour4> BallColour = new(Colour4.White);

        private Vector2 velocity;
        private bool dragging;
        private Vector2 dragOffset;
        private readonly Queue<(Vector2 pos, double time)> dragHistory = new();

        public Ball()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            BallSize.BindValueChanged(v => Size = new Vector2(v.NewValue), true);
            GlowRadius.BindValueChanged(_ => updateAppearance());
            BallColour.BindValueChanged(_ => updateAppearance());

            updateAppearance();
        }

        private void updateAppearance()
        {
            Colour = BallColour.Value;

            Color4 colour = BallColour.Value;
            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Glow,
                Colour = new Color4(colour.R, colour.G, colour.B, 0.6f),
                Radius = GlowRadius.Value,
            };
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            // 中键/右键删除这个球
            if (e.Button is MouseButton.Middle or MouseButton.Right)
                Expire();
            // 左键也返回 true，使本球进入 ButtonDownInputQueue，拖拽才能触发
            return true;
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (e.Button != MouseButton.Left) return false;

            (Parent as MainScreen)?.ShowBallConfig(this);
            return true;
        }

        protected override bool OnDragStart(DragStartEvent e)
        {
            dragging = true;
            dragOffset = Position - e.MousePosition;
            velocity = Vector2.Zero;
            dragHistory.Clear();
            dragHistory.Enqueue((e.MousePosition, Time.Current));
            this.ScaleTo(BallSettings.DragScale.Value, 200, Easing.OutQuint);
            return true;
        }

        protected override void OnDrag(DragEvent e)
        {
            Position = e.MousePosition + dragOffset;
            double now = Time.Current;
            dragHistory.Enqueue((e.MousePosition, now));
            while (dragHistory.Count > 1 && now - dragHistory.Peek().time > BallSettings.FlickWindow.Value)
                dragHistory.Dequeue();
        }

        protected override void OnDragEnd(DragEndEvent e)
        {
            dragging = false;
            if (dragHistory.Count >= 2)
            {
                var (firstPos, firstTime) = dragHistory.Peek();
                float dt = (float)Math.Max((Time.Current - firstTime) / 1000.0, 0.001);
                velocity = (e.MousePosition - firstPos) / dt;
            }
            else
            {
                velocity = Vector2.Zero;
            }
            this.ScaleTo(1f, 300, Easing.OutQuint);
        }

        protected override void Update()
        {
            base.Update();
            if (dragging || Parent == null) return;
            float dt = (float)(Clock.ElapsedFrameTime / 1000.0);
            if (dt <= 0) return;
            Position += velocity * dt;
            // 帧率无关的时间衰减摩擦力 (每秒衰减)
            velocity *= MathF.Pow(Friction.Value, dt);
            if (velocity.Length < BallSettings.StopSpeed.Value)
                velocity = Vector2.Zero;
            // Anchor.Centre 下的活动范围
            float boundX = Math.Max(0, (Parent.DrawWidth - DrawWidth) / 2f);
            float boundY = Math.Max(0, (Parent.DrawHeight - DrawHeight) / 2f);
            if (X < -boundX)
            {
                X = -boundX;
                velocity.X = MathF.Abs(velocity.X) * Bounce.Value;
            }
            else if (X > boundX)
            {
                X = boundX;
                velocity.X = -MathF.Abs(velocity.X) * Bounce.Value;
            }
            if (Y < -boundY)
            {
                Y = -boundY;
                velocity.Y = MathF.Abs(velocity.Y) * Bounce.Value;
            }
            else if (Y > boundY)
            {
                Y = boundY;
                velocity.Y = -MathF.Abs(velocity.Y) * Bounce.Value;
            }
        }
    }
}
