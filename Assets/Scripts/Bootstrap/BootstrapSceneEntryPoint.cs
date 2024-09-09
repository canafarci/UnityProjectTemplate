using ProjectTemplate.CrossScene.Messages;
using ProjectTemplate.Infrastructure.PubSub;
using VContainer;
using VContainer.Unity;

namespace ProjectTemplate.Bootstrap
{
	public class BootstrapSceneEntryPoint : IInitializable
	{
		[Inject] private IPublisher<LoadSceneMessage> _loadScenePublisher;

		public void Initialize()
		{
			//this method can be async to initialize SDKs, load resources etc.
			_loadScenePublisher.Publish(new LoadSceneMessage(1)); //publish load scene message
		}
	}
}