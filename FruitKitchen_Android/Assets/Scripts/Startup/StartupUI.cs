using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class StartupUI : MonoBehaviour {
	[SerializeField]
	private List< GameObject> m_btnImgList;
	[SerializeField]
	private UISprite m_knifeImg;
	[SerializeField]
	private UISprite m_titleImg;
	[SerializeField]
	private UITexture m_myFbImg;
	[SerializeField]
	private GameObject m_myFbIniImg;

	private float m_btnFadeInTime = 0.8f;

	void Start()
	{
		EventManager.Instance.registerEvent( EventManager.EventName.FBLoginOK , OnFBLoginOK );
		EventManager.Instance.registerEvent( EventManager.EventName.FBLogoutOK , OnFBLoginOK );
	}
	
	public void OnFBLoginOK( params object [] p_param )
	{
		if(  NetWorkManager.Instance.fbIsLogin == true )
		{
			FaceBookManager.Instance.SetFbImg(m_myFbImg , NetWorkManager.Instance.myData.PD_FB_Image);
			m_myFbImg.gameObject.SetActive( true );
			m_myFbIniImg.gameObject.SetActive( false );
		}else
		{
			m_myFbIniImg.gameObject.SetActive( true );
			m_myFbImg.gameObject.SetActive( false );
		}
	}
	public void Setup( params object [] p_param )
	{
		CPDebug.Log("Setup start");
		// start to show btn
		/*for( int i = 0 ; i < m_btnImgList.Count ; i ++ )
		{
			m_btnImgList[i].color = new Color( 1,1,1,0 );
		}*/
		this.gameObject.SetActive( true );
		if( NetWorkManager.Instance.fbIsLogin == true )
			FaceBookManager.Instance.SetFbImg(m_myFbImg , NetWorkManager.Instance.myData.PD_FB_Image);

	}
	public void OnTileAniFinish()
	{
		CPDebug.Log("OnTileAniFinish");
		//show the Knife
		Hashtable TmpTable = new Hashtable();
		TmpTable.Add("position",new Vector3( 382f , 328f , 0f ));
		TmpTable.Add("time",0.4f);
		TmpTable.Add("islocal" , true );
		TmpTable.Add("easetype" , iTween.EaseType.linear );
		TmpTable.Add ("oncomplete","OnKinfeDone");
		TmpTable.Add ("oncompletetarget",this.gameObject);
		iTween.MoveTo(m_knifeImg.gameObject,TmpTable);
	}

	void OnKinfeDone()
	{
		m_titleImg.spriteName = "UI_Game_Logo2";
		m_knifeImg.gameObject.SetActive( false );
		for( int i = 0 ; i < m_btnImgList.Count ; i ++ )
		{
			m_btnImgList[i].SetActive( true );
		}

		// start to show btn
		/*for( int i = 0 ; i < m_btnImgList.Count ; i ++ )
		{
			StartCoroutine( ShowBtnFadeIn(m_btnImgList[i]));
		}*/
	}

	IEnumerator ShowBtnFadeIn(UISprite p_sprite )
	{
		CPDebug.Log("ShowBtnFadeIn p_sprite = " + p_sprite );
		float TmpAlpha = 0;
		while( TmpAlpha <= 1 )
		{
			p_sprite.color = new Color( 1,1,1,TmpAlpha );
			TmpAlpha += 1.0f/m_btnFadeInTime*Time.deltaTime;
			yield return null;
		}
		p_sprite.color = new Color( 1,1,1,1 );
	}

	// On option UI open
	public void OnOptionBtnClick()
	{
		CPDebug.Log("OnOptionBtnClick");
		PopupManager.Instance.ShowGameStarOptionUI(StartOptionPopUI.OptionUIType.Normal);
	}

	// pkay Game
	public void PlayGame()
	{
		CPDebug.Log("PlayGame start");
		SceneChangeManager.Instance.LoadScene("InGame");
	}
}
