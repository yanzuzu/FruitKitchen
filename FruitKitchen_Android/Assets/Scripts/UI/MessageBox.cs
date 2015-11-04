using UnityEngine;
using System.Collections;

public class MessageBox : MonoBehaviour {

	public int messageType;
	public UILabel textLabel;
	public UILabel okBtnLabel;
	public UILabel cancelBtnLabel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Setup( params object [] p_param )
	{
		messageType = (int)p_param [0];

		Config.Message_Type_Enum enumType = (Config.Message_Type_Enum)messageType;
		switch(enumType)
		{
		case Config.Message_Type_Enum.Message_Type_Unlock_Stage:
		{
			textLabel.text = Localization.Localize("72");
			okBtnLabel.text = Localization.Localize("73");
			cancelBtnLabel.text = Localization.Localize("47");
		}break;
		}
	}

	public void OnOkBtnClick()
	{
		CPDebug.Log ("MesasgeBox Ok messageType = " + messageType );
		switch(messageType)
		{
		case (int)Config.Message_Type_Enum.Message_Type_Unlock_Stage:
		{
			UniBillManager.Instance.BuyItem("stage_unlock_v001");
		}break;
		}

		OnCancelBtnClick();
	}

	public void OnCancelBtnClick()
	{
		CPDebug.Log ("MesasgeBox Cancel");
		PopupManager.Instance.CloseCurrentPopup();
	}
}
