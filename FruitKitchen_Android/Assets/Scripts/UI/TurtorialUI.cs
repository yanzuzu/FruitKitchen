using UnityEngine;
using System.Collections;

public class TurtorialUI : MonoBehaviour {

	public void OnClickCloseBtn()
	{
		CPDebug.Log("OnClickCloseBtn start");
		PopupManager.Instance.CloseCurrentPopup();
	}
}
