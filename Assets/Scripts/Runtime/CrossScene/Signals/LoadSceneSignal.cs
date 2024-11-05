namespace ProjectTemplate.Runtime.CrossScene.Signals
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