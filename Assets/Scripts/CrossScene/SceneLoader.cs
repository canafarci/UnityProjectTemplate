using UnityEngine.SceneManagement;

using System;

using VContainer;
using VContainer.Unity;
using ProjectTemplate.CrossScene.Messages;
using ProjectTemplate.Infrastructure.PubSub;

namespace ProjectTemplate.CrossScene
{
	public class SceneLoader : IInitializable, IDisposable
	{
		[Inject] private ISubscriber<LoadSceneMessage> _loadSceneSubscriber;


		public void Initialize()
		{
			_loadSceneSubscriber.Subscribe(OnLoadSceneMessage);
		}

		private void OnLoadSceneMessage(LoadSceneMessage message)
		{
			SceneManager.LoadSceneAsync(message.sceneID);
		}

		public void Dispose()
		{
			_loadSceneSubscriber.Unsubscribe(OnLoadSceneMessage);
		}
	}
}