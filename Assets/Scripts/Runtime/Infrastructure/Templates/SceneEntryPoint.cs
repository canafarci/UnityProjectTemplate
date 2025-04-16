using Cysharp.Threading.Tasks;
using ProjectTemplate.Runtime.CrossScene.Signals;

namespace ProjectTemplate.Runtime.Infrastructure.Templates
{
	public abstract class SceneEntryPoint : SignalListener
	{
		protected abstract UniTaskVoid EnterScene();

		protected override void SubscribeToEvents()
		{
			_signalBus.Subscribe<SceneActivatedSignal>(OnSceneActivatedSignalHandler);
		}
		
		private void OnSceneActivatedSignalHandler(SceneActivatedSignal signal)
		{
			EnterScene();
		}

		protected override void UnsubscribeFromEvents()
		{
			_signalBus.Unsubscribe<SceneActivatedSignal>(OnSceneActivatedSignalHandler);
		}
	}
}