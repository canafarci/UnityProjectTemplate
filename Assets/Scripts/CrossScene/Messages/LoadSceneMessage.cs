namespace ProjectTemplate.CrossScene.Messages
{
	public struct LoadSceneMessage
	{
		public int sceneID { get; private set; }

		public LoadSceneMessage(int sceneID)
		{
			this.sceneID = sceneID;
		}
	}
}