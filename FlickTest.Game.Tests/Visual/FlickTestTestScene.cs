using osu.Framework.Testing;

namespace FlickTest.Game.Tests.Visual
{
    public abstract partial class FlickTestTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new FlickTestTestSceneTestRunner();

        private partial class FlickTestTestSceneTestRunner : FlickTestGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
