using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace FlickTest.Game
{
    public enum BallSetting
    {
        FlickWindow,
        DragScale,
        StopSpeed,
    }

    public static class BallSettings
    {
        public static BindableFloat FlickWindow { get; } = new(80f) { MinValue = 10f, MaxValue = 300f };
        public static BindableFloat DragScale { get; } = new(1.25f) { MinValue = 1f, MaxValue = 2f };
        public static BindableFloat StopSpeed { get; } = new(10f) { MinValue = 0f, MaxValue = 100f };

        private static BallConfig config;

        public static void Load(Storage storage)
        {
            config?.Dispose();

            config = new BallConfig(storage);
            config.BindWith(BallSetting.FlickWindow, FlickWindow);
            config.BindWith(BallSetting.DragScale, DragScale);
            config.BindWith(BallSetting.StopSpeed, StopSpeed);
        }

        private class BallConfig : IniConfigManager<BallSetting>
        {
            protected override string Filename => "flick.ini";

            public BallConfig(Storage storage)
                : base(storage)
            {
            }

            protected override void InitialiseDefaults()
            {
                SetDefault(BallSetting.FlickWindow, 80f, 10f, 300f);
                SetDefault(BallSetting.DragScale, 1.25f, 1f, 2f);
                SetDefault(BallSetting.StopSpeed, 10f, 0f, 100f);
            }
        }
    }
}
