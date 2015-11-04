using UnityEngine;
using System.Collections;

public class MessageBox_Tip : MonoBehaviour {

	public UILabel textLabel;
	public UILabel okBtnLabel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Setup( params object [] p_param )
	{
		string messageMain = (string)p_param [0];
		string messageOkBtn = (string)p_param [1];

		textLabel.text = messageMain;
		okBtnLabel.text = messageOkBtn;
	}

	public void OnOkBtnClick()
	{
		PopupManager.Instance.CloseCurrentPopup ();
	}
}
