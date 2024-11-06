using System.Collections;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace DefaultNamespace
{
	public class BootstrapInitializationTests
	{
		[UnityTest]
		public IEnumerator Bootstrap_Scene_Index_Is_Zero()
		{
			//load bootstrap Scene
			yield return SceneManager.LoadSceneAsync("Bootstrap");
			yield return null;
			Assert.IsTrue(SceneManager.GetActiveScene().buildIndex == 0);
			yield return null; // Allow Unity to process the frame
		}
		
		[UnityTest]
		public IEnumerator Scene_At_Index_Zero_Is_Named_Bootstrap()
		{
			//load bootstrap Scene
			yield return SceneManager.LoadSceneAsync(0);
			yield return null;
			Assert.IsTrue(SceneManager.GetActiveScene().name == "Bootstrap");
			yield return null; // Allow Unity to process the frame
		}
		
	}
}