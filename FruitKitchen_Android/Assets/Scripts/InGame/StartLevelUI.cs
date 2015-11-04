using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class StartLevelUI : MonoBehaviour {
	class SortData
	{
		public string url;
		public int score;
		public SortData( string p_url , int p_score)
		{
			url = p_url;
			score =  p_score;
		}
	}
	[SerializeField]
	private UILabel	 m_Leveltxt;
	[SerializeField]
	UILabel m_moveTxt;
	[SerializeField]
	UILabel m_bombTxt;
	[SerializeField]
	List<GameObject> m_starUI;
	[SerializeField]
	List<SortScoreUnit > m_bombSortList;
	[SerializeField]
	List<SortScoreUnit> m_moveSortList;
	[SerializeField]
	UILabel m_FriendMoveTitleText;
	[SerializeField]
	UILabel m_FriendBombTitleText;
	[SerializeField]
	UISprite FBBtnSprite;

	public void Setup(int BigStageIdx , int SmallStageIdx )
	{
		DataManager.Instance.BigStageNum = BigStageIdx;
		DataManager.Instance.SmallStageNum = SmallStageIdx;
		if( BigStageIdx == 1 )
		{
			if( SmallStageIdx <= 3 )
				m_Leveltxt.text = string.Format("T{0}-{1}" , BigStageIdx,SmallStageIdx);
			else
				m_Leveltxt.text = string.Format("{0}-{1}" , BigStageIdx,SmallStageIdx-3 );
		}else
			m_Leveltxt.text = string.Format("{0}-{1}" , BigStageIdx,SmallStageIdx );

		m_moveTxt.text = string.Format( Localization.Localize("95"),LocalDBManager.Instance.getBestMoveScore( BigStageIdx , SmallStageIdx ));
		m_bombTxt.text = string.Format( Localization.Localize("96"),LocalDBManager.Instance.getBestBombScore( BigStageIdx , SmallStageIdx ));

		StageRecordData TmpData =  LocalDBManager.Instance.GetStageRecord( BigStageIdx , SmallStageIdx );

		for( int i = 0 ; i < m_starUI.Count ; i ++ )
		{
			if( i +1  > TmpData.BestStar )
				m_starUI[i].SetActive( false );
			else
				m_starUI[i].SetActive( true );
		}

		string mode = LocalDBManager.Instance.GetLanguage ();
		
		if(mode == "tw")
		{
			FBBtnSprite.spriteName = "UI_BigStage_FB_Btn";
		}
		else
		{
			FBBtnSprite.spriteName = "UI_BigStage_FB_Btn_en";
		}

		if( NetWorkManager.Instance.fbIsLogin == false )
		{
			CPDebug.LogWarning("no fb login!!!");

			return;
		}
		List<SortData> TmpBombSortList = new List<SortData>();
		List<SortData> TmpMoveSortList = new List<SortData>();

		// add self data
		int myBombScore = LocalDBManager.Instance.getBestBombScore (BigStageIdx,SmallStageIdx);
		if(myBombScore != 0)
		{
			SortData MyData = new SortData( NetWorkManager.Instance.myData.PD_FB_Image , myBombScore ) ;
			TmpBombSortList.Add( MyData );
		}

		int myMoveScore = LocalDBManager.Instance.getBestMoveScore (BigStageIdx,SmallStageIdx);
		if(myMoveScore != 0)
		{
			SortData MyMoveData = new SortData( NetWorkManager.Instance.myData.PD_FB_Image , myMoveScore ) ;
			TmpMoveSortList.Add( MyMoveData );
		}

		for( int i = 0 ; i < NetWorkManager.Instance.myFriendData.Count ; i ++ )
		{
			if( NetWorkManager.Instance.myFriendData[i].PD_IsPlayThisGame == false ) continue;

			int friBombScore = NetWorkManager.Instance.myFriendData[i].getStageBestBombScore(BigStageIdx,SmallStageIdx);

			if(friBombScore != -1)
			{
				SortData FriendData = new SortData(NetWorkManager.Instance.myFriendData[i].PD_FB_Image , friBombScore);
				TmpBombSortList.Add(FriendData );
			}

			int friMoveScore = NetWorkManager.Instance.myFriendData[i].getStageBestMoveScore(BigStageIdx,SmallStageIdx);

			if(friMoveScore != -1)
			{
				SortData FriendMoveData = new SortData( NetWorkManager.Instance.myFriendData[i].PD_FB_Image  , friMoveScore);
				TmpMoveSortList.Add( FriendMoveData );
			}
		}

		if(TmpBombSortList.Count > 0 &&
		   TmpMoveSortList.Count > 0)
		{
			m_FriendMoveTitleText.gameObject.SetActive(true);
			m_FriendBombTitleText.gameObject.SetActive(true);

			m_FriendMoveTitleText.text = Localization.Localize("63");
			m_FriendBombTitleText.text = Localization.Localize("62");
		}

		TmpBombSortList.Sort( (x, y) => { return -x.score.CompareTo( y.score ); });
		TmpMoveSortList.Sort( (x, y) => { return -x.score.CompareTo( y.score ); });

		for( int i = 0 ; i < m_bombSortList.Count ; i ++ )
		{
			if( i >= TmpBombSortList.Count )
				m_bombSortList[i].gameObject.SetActive( false );
			else
			{
				m_bombSortList[i].gameObject.SetActive( true );
				m_bombSortList[i].Setup(TmpBombSortList[i].url , TmpBombSortList[i].score );
			}
		}


		for( int i = 0 ; i < m_moveSortList.Count ; i ++ )
		{
			if( i >= TmpBombSortList.Count )
				m_moveSortList[i].gameObject.SetActive( false );
			else
			{
				m_moveSortList[i].gameObject.SetActive( true );
				m_moveSortList[i].Setup(TmpMoveSortList[i].url , TmpMoveSortList[i].score );
			}
		}
	}


	public void OnClickPlayBtn()
	{
		if( LocalDBManager.Instance.GetLifeNum() <= 0 )
		{
			CPDebug.LogWarning("Heart is not enough!!");
			return;
		}
		SceneChangeManager.Instance.LoadScene("GamePlay");
	}

	public void OnClickChallengeBtn()
	{
		PopupManager.Instance.ShowChallengeUI();	
	}

	public void postFBText()
	{
		PopupManager.Instance.ShowMessageBox_ForFB (false);
	}
}
