using UnityEngine;
using System.Collections;

public class StartOptionPopUI : MonoBehaviour {
	public enum OptionUIType
	{
		Normal = 0,
		InGame = 1,
		GamePlay = 2,
	}

	[SerializeField]
	private UICheckbox m_EnBox;
	[SerializeField]
	private UICheckbox m_ChineseBox;
	[SerializeField]
	private UISlider m_soundSlide;
	[SerializeField]
	private UISlider m_musicSlide;
	[SerializeField]
	private GameObject m_FbBtn;
	[SerializeField]
	private UISprite m_bg;
	[SerializeField]
	private GameObject m_adjustUI;
	[SerializeField]
	private GameObject m_hideUI;
	[SerializeField]
	private GameObject m_backBtn;

	private GamePlayLayer playLayer;

	OptionUIType m_uiType = OptionUIType.Normal;

	public void Setup( params object [] p_param )
	{
		m_uiType = ( OptionUIType )p_param[0];

		if(p_param[1] != null)
		{
			playLayer = (GamePlayLayer)p_param[1];
		}

		switch( m_uiType )
		{
		case OptionUIType.Normal:
			m_FbBtn.SetActive( true );
			m_adjustUI.transform.localPosition = new Vector3( m_adjustUI.transform.localPosition.x , 0 , m_adjustUI.transform.localPosition.z );
			break;
		case OptionUIType.InGame:
			m_FbBtn.SetActive( false );
			m_adjustUI.transform.localPosition = new Vector3( m_adjustUI.transform.localPosition.x , 0 , m_adjustUI.transform.localPosition.z );
			break;
		case OptionUIType.GamePlay:
			m_FbBtn.SetActive( false );
			m_bg.spriteName = "UI_Option_In_Game_Bk" + DataManager.Instance.Language;
			m_adjustUI.transform.localPosition = new Vector3( m_adjustUI.transform.localPosition.x , 56 , m_adjustUI.transform.localPosition.z );
			m_hideUI.SetActive( false );
			m_backBtn.SetActive( true );
			break;
		}

		float SoundValue = LocalDBManager.Instance.GetSound();
		m_soundSlide.sliderValue = SoundValue;

		float MusicValue = LocalDBManager.Instance.GetMusic();
		m_musicSlide.sliderValue = MusicValue;

		Invoke("setInitLanguage",0.2f);
	}

	void setInitLanguage()
	{

		string TmpLanguage =  LocalDBManager.Instance.GetLanguage();
		CPDebug.Log("setInitLanguage TmpLanguage = " + TmpLanguage );
		if( TmpLanguage == "tw")
		{
			m_ChineseBox.isChecked = true;
			OnLanguageChinese();
		}else
			OnLanguageEn();
	}
	public void OnClickBackBtn()
	{
		if( DataManager.Instance.IsTeachMode )
			DataManager.Instance.m_InGameType = DataManager.InGameType.SmallStage;
		else
		{
			if(playLayer)
			{
				int keyNum = DataManager.Instance.StageKey;
				int totalMoveLeftNum = CSVManager.Instance.StageInfoTable.readFieldAsInt(keyNum, "TotalMove");
				int nowMoveLeftNum = playLayer.m_blockManager.moveLeftNum;

				if(nowMoveLeftNum < totalMoveLeftNum)
				{
					LifeManager.Instance.useLife();
				}
			}

			DataManager.Instance.m_InGameType = DataManager.InGameType.StageInfo;
		}

		SceneChangeManager.Instance.LoadScene("InGame");
		PopupManager.Instance.CloseCurrentPopup();

		SoundManager.Instance.PlayBGM("StartBG");
	}

	public void OnClickCloseBtn()
	{
		PopupManager.Instance.CloseCurrentPopup();
	}
	public void  OnClickFaceBookBtn()
	{
		if( NetWorkManager.Instance.fbIsLogin == false )
			FaceBookManager.Instance.LoginFB( null );
		else
			FaceBookManager.Instance.LogoutFB();
	}
	//On Turtorial UI
	public void OnOpenTurtorialClick()
	{
		PopupManager.Instance.CloseCurrentPopup();
		PopupManager.Instance.ShowTutorialUI();
	}

	public void OnOpenStoreBtnClick()
	{
		PopupManager.Instance.CloseCurrentPopup();
		PopupManager.Instance.ShowStoreShopUI();
	}

	public void OnLanguageChinese()
	{
		DataManager.Instance.Language = "";
		Localization.instance.currentLanguage = "tw";
	}

	public void OnLanguageEn()
	{
		DataManager.Instance.Language = "_en";
		Localization.instance.currentLanguage = "English";
	}

	public void OnSoundSlideChange( float factor )
	{
		SoundManager.Instance.SetSEVolume(factor);
		LocalDBManager.Instance.SetSound(factor);
	}

	public void OnMusicSlideChange( float factor )
	{
		SoundManager.Instance.SetBGMVolume(factor);
		LocalDBManager.Instance.SetMusic(factor);
	}

	public void OnSoundClose(bool Value )
	{
		CPDebug.Log("OnSoundClose~~Value = " +  Value );
		SoundManager.Instance.MuteSE(Value);
		//int IsMute = Value == true ? 1 : 0;
		//LocalDBManager.Instance.SetSoundMute(IsMute);
	}

	public void OnMusicClose( bool Value  )
	{
		CPDebug.Log("OnMusicClose~~Value = " +  Value );
		SoundManager.Instance.MuteBGM(Value);
		//int IsMute = Value == true ? 1 : 0;
		//LocalDBManager.Instance.SetMusicMute(IsMute);
	}

	public void OnClickFbImg()
	{
		Application.OpenURL("http://www.facebook.com/fruitkitchenpuzzlegame");
	}
}
