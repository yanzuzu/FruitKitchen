using UnityEngine;
using System.Collections;

public class SceneChangeManager : ManagerComponent<SceneChangeManager> {

	public void LoadScene( string p_sceneName )
	{
		Application.LoadLevelAsync(p_sceneName);
	}
}
