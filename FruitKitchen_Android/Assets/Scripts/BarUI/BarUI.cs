using UnityEngine;
using System.Collections;

public class BarUI : MonoBehaviour {
	[SerializeField]
	private UILabel m_timeTxt;

	public UILabel mailNumLabel;
	public UISprite mailNumBk;
	private int preMailNum = 0;

	void Start()
	{
		// get Time txt
		StartCoroutine( GetInfo());
	}

	IEnumerator GetInfo()
	{
		while( true )
		{
			m_timeTxt.text = LifeManager.Instance.getNextRecoverSecondStr();
			if( m_timeTxt.text == "00:00" )
				m_timeTxt.gameObject.SetActive( false );
			else
				m_timeTxt.gameObject.SetActive( true );

			showMailNum();
			yield return new WaitForSeconds( 1 );
		}
	}
	public void OnClickSettingBtn()
	{
		PopupManager.Instance.ShowGameStarOptionUI(StartOptionPopUI.OptionUIType.InGame);
	}
	public void OnClickMailBtn()
	{
		PopupManager.Instance.ShowMailUI();
	}

	public void OnClickGiftBtn()
	{
		PopupManager.Instance.ShowGiftUI();
	}
	public void OnClickBackBtn()
	{
		switch( InGameSceneMain.Instance.m_CurrentUIType )
		{
		case InGameSceneMain.InGameUIType.BigStage:
			DataManager.Instance.m_IsShowLogo = false;
			SceneChangeManager.Instance.LoadScene("Startup");
			break;
		case InGameSceneMain.InGameUIType.SmallStage:
			InGameSceneMain.Instance.ChangeUI(InGameSceneMain.InGameUIType.BigStage );
			break;
		case InGameSceneMain.InGameUIType.StartLevel:
			InGameSceneMain.Instance.ChangeUI(InGameSceneMain.InGameUIType.SmallStage , DataManager.Instance.BigStageNum  );
			break;
		}
	}

	public void showMailNum()
	{
		int mailNum = NetWorkManager.Instance.mailData.Count;

		if(mailNum <= 0)
		{
			if(preMailNum > 0)
			{
				mailNumLabel.gameObject.SetActive(false);
				mailNumBk.gameObject.SetActive(false);
			}
		}
		else
		{
			if(preMailNum <= 0)
			{
				mailNumLabel.gameObject.SetActive(true);
				mailNumBk.gameObject.SetActive(true);
			}

			mailNumLabel.text = mailNum.ToString();
		}

		preMailNum = mailNum;
	}
}
