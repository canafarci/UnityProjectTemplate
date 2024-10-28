namespace ProjectTemplate.CrossScene.Messages
{
	public struct LoadSceneSignal
	{
		public int sceneID { get; private set; }

		public LoadSceneSignal(int sceneID)
		{
			this.sceneID = sceneID;
		}
	}
}