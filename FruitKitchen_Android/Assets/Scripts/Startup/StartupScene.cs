using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class StartupScene : MonoBehaviour {
	[SerializeField]
	private List<UISprite> m_logoImgList;
	[SerializeField]
	private StartupUI m_startUI;

	private float m_showTime = 1.5f;
	private float m_splashFadingPeriod = 1.0f;
	// Use this for initialization
	void Start () {
		// life manager init
		LifeManager.Instance.initLifeManager();

		StartCoroutine( PlayLogoImg());
		// init music sound
		float TmpSound = LocalDBManager.Instance.GetSound();
		SoundManager.Instance.SetSEVolume( TmpSound );
		float TmpMusic = LocalDBManager.Instance.GetMusic();
		SoundManager.Instance.SetBGMVolume( TmpMusic );
		// init language
		string TmpLanguage = LocalDBManager.Instance.GetLanguage();
		if( TmpLanguage == "English" )
			DataManager.Instance.Language = "_en";
		else
			DataManager.Instance.Language = "";

		// init Music and Sound  Mute
		SoundManager.Instance.MuteBGM(LocalDBManager.Instance.GetMusicMute());
		SoundManager.Instance.MuteSE(LocalDBManager.Instance.GetSoundMute());

	}

	IEnumerator PlayLogoImg()
	{
		CPDebug.Log("Yanzu PlayLogoImg start!!!");
		if( DataManager.Instance.m_IsShowLogo == true )
		{
			for( int i = 0 ; i < m_logoImgList.Count ; i ++ )
			{
				// show the logo
				float TmpAlpha = 0.0f;
				m_logoImgList[i].gameObject.SetActive( true );

				while( TmpAlpha <= 1.0f )
				{
					TmpAlpha += 1.0f/m_splashFadingPeriod*Time.deltaTime;
					m_logoImgList[i].color = new Color( 1,1,1,TmpAlpha );
					yield return null;
				}
				// stay
				yield return new WaitForSeconds( m_showTime );
				// show off
				m_logoImgList[i].gameObject.SetActive( true );
				TmpAlpha = 1.0f;
				while( TmpAlpha >= 0.0f )
				{
					TmpAlpha -= 1.0f/m_splashFadingPeriod*Time.deltaTime;
					m_logoImgList[i].color = new Color( 1,1,1,TmpAlpha );
					yield return null;
				}
				m_logoImgList[i].gameObject.SetActive( false );
			}
		}
		SoundManager.Instance.PlayBGM("StartBG");
		m_startUI.Setup();
		//OnLoginFBOK ("1242343534");

	}
	void OnClickFBLoginBtn()
	{
		if( NetWorkManager.Instance.fbIsLogin == true )
		{
			CPDebug.Log("FB is Login!!");
			return;
		}
		FaceBookManager.Instance.LoginFB(OnLoginFBOK);
	}
	void OnLoginFBOK( string p_FBid )
	{
		CPDebug.Log("OnLoginFBOK p_FBid = " + p_FBid );
	}
	
	void Update()
	{
		if( Input.GetKeyDown( KeyCode.A ) )
		{
			PlayerPrefs.DeleteAll();
		}

		if(Input.GetKeyDown( KeyCode.F ))
		{
			PopupManager.Instance.VisibleLoading(true);

			NetWorkManager.Instance.myData = new MyPersonalData("1594618099", "李昀浩", "");
			NetWorkManager.Instance.fbIsLogin = true;
	//		NetWorkManager.Instance.parse_getFriendScoreInfo(DataManager.Instance.m_TmpFbData.id);
			NetWorkManager.Instance.parse_StartCheckData();
		}

		if( Input.GetKeyDown(KeyCode.S ) )
		{
			LocalDBManager.Instance.ClearBackPack();
		}

		if( Input.GetKeyDown( KeyCode.Q ) )
		{

			LocalDBManager.Instance.clearStageScoreRecordTable();
			LocalDBManager.Instance.updateRecordStage(1, 23);

	/*		int big = 1;
			int small = 23;

			DataManager.Instance.recordBigStageNum = big;
			DataManager.Instance.recordSmallStageNum = small;
			LocalDBManager.Instance.updateRecordStage(big, small);

			for(int i = 1; i<= big; i++)
			{
				for(int j = 1; j<= small; j++)
				{
					if(i == 1 &&
					   j <= 3)
					{
						continue;
					}

					System.Random rnd = new System.Random();
					int random = rnd.Next(3) + 1;

					StageRecordData data = new StageRecordData(i, j, random, 10, 10);
					LocalDBManager.Instance.SetStageRecord(i, j, data);
				}
			}*/


			CPDebug.Log("clearStageScoreRecordTable");
		}

		if( Input.GetKeyDown(KeyCode.W ) )
		{
			CPDebug.Log("Add BackPack ");
			LocalDBManager.Instance.addOneOwnItem(11, 1);
			LocalDBManager.Instance.addOneOwnItem(2, 1);
		}

	}
}
