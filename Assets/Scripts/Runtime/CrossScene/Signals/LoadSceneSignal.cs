using ProjectTemplate.Runtime.CrossScene.Scenes.Enums;

namespace ProjectTemplate.Runtime.CrossScene.Signals
{
	public struct LoadSceneSignal
	{
		public SceneID sceneID { get; private set; }

		public LoadSceneSignal(SceneID sceneID)
		{
			this.sceneID = sceneID;
		}
	}
}