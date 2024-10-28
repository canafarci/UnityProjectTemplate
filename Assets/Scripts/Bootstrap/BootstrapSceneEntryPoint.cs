using VContainer;
using VContainer.Unity;

using ProjectTemplate.CrossScene.Messages;
using ProjectTemplate.Infrastructure.SignalBus;

namespace ProjectTemplate.Bootstrap
{
	public class BootstrapSceneEntryPoint : IInitializable
	{
		[Inject] private SignalBus _signalBus;

		public void Initialize()
		{
			//this method can be async to initialize SDKs, load resources etc.
			_signalBus.Fire(new LoadSceneSignal(1)); //publish load scene message
		}
	}
}