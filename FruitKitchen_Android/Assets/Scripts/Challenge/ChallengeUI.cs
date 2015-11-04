using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ChallengeUI : MonoBehaviour {
	[SerializeField]
	private UILabel m_challengeTxt;
	[SerializeField]
	private ChallengeUnit m_unitObj;
	[SerializeField]
	private UIGrid m_gridObj;
	[SerializeField]
	private UILabel m_searchTxt;

	List< GameObject > m_unitList = new List<GameObject>();

	public string m_ChallengeFBUid = "";
	private int m_challengeType = Config.CHALLENG_SCORE_TYPE_BOMB;
	public void Setup( string p_searchTxt = "" )
	{
		CPDebug.Log("ChallengeUI Setup");

		for( int i = 0 ; i < m_unitList.Count ; i ++ )
			DestroyImmediate( m_unitList[i] );

		m_unitList.Clear();

		m_challengeTxt.text =  ( Config.CHALLENG_MAX_NUM - NetWorkManager.Instance.myChallengData.Count ).ToString();
		for( int i = 0 ; i < NetWorkManager.Instance.myFriendData.Count ; i ++ )
		{
			if( NetWorkManager.Instance.myFriendData[i].PD_IsPlayThisGame == false ) continue;
			// check search Txt
			if( string.IsNullOrEmpty( p_searchTxt ) == false )
			{
				if( NetWorkManager.Instance.myFriendData[i].PD_FB_Name.IndexOf(p_searchTxt) == -1 ) continue;
			}
			GameObject TmpObj =  Instantiate( m_unitObj.gameObject ) as GameObject;
			TmpObj.transform.parent = m_gridObj.transform;
			TmpObj.transform.localScale = Vector3.one;
			TmpObj.SetActive( true );
			TmpObj.GetComponent< ChallengeUnit >().Setup(NetWorkManager.Instance.myFriendData[i], this);
			m_unitList.Add( TmpObj );
		}
		m_gridObj.Reposition();
	}
	
	public void OnClickCloseBtn()
	{
		PopupManager.Instance.CloseCurrentPopup();
	}
	public void OnBombCheck()
	{
		m_challengeType = Config.CHALLENG_SCORE_TYPE_BOMB;

		CPDebug.Log ("choice Bomb");
	}

	public void OnPosCheck()
	{
		m_challengeType = Config.CHALLENG_SCORE_TYPE_MOVE;

		CPDebug.Log ("choice Move");
	}
	public void OnChallengeBtn()
	{
		CPDebug.Log("OnChallengeBtn start");
		if( m_ChallengeFBUid == string.Empty )
		{
			CPDebug.Log("challenge FB id is empty!!");
			return;
		}
		// check is can challenge
		if( NetWorkManager.Instance.parse_checkCanChalleng() == false )
		{
			CPDebug.Log("u can't challenge!!");
			return;
		}

		if( NetWorkManager.Instance.checkCanChallengToSomeOne(m_ChallengeFBUid) == false )
		{
			CPDebug.Log("this friend can't challenge!!");
			return;
		}

		NetWorkManager.Instance.isPlayChallengMode = true;
		NetWorkManager.Instance.isBeChalleng = false;
		NetWorkManager.Instance.challengFriendFBUid = m_ChallengeFBUid;
		NetWorkManager.Instance.challengScoreType = m_challengeType;

		PopupManager.Instance.CloseCurrentPopup ();

		// go to game scene
		SceneChangeManager.Instance.LoadScene("GamePlay");

	}

	public void OnSubmit()
	{
		CPDebug.Log("OnSubmit m_searchTxt = " + m_searchTxt.text );
		if( string.IsNullOrEmpty( m_searchTxt.text ) == false)
			Setup(m_searchTxt.text );
	}
}
