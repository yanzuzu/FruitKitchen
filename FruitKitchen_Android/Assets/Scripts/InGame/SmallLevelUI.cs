using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SmallLevelUI : MonoBehaviour {
	[SerializeField]
	SmallStagebtn m_unitObj;
	[SerializeField]
	UIGrid m_uiGridObj;
	[SerializeField]
	UILabel m_moveTxt;
	[SerializeField]
	UILabel m_bombTxt;
	[SerializeField]
	UISprite FBBtnSprite;

	List< GameObject > m_smallUnitList = new List<GameObject>();
	public void Setup(int BigStageNum )
	{
		List<int> TmpSmallIdList =  InGameSceneMain.Instance.StageInfoData[ BigStageNum ];
		for( int i  = 0 ; i < m_smallUnitList.Count ; i ++ )
			DestroyImmediate( m_smallUnitList[i] );

		for( int i = 0 ; i < TmpSmallIdList.Count ; i ++ )
		{
			GameObject TmpObj = Instantiate( m_unitObj.gameObject ) as GameObject;
			TmpObj.transform.parent = m_uiGridObj.transform;
			TmpObj.transform.localScale = Vector3.one;
			m_smallUnitList.Add( TmpObj );
			TmpObj.SetActive(true);
			TmpObj.GetComponent< SmallStagebtn>().Setup(BigStageNum , TmpSmallIdList[i]);
		}
		m_uiGridObj.Reposition();

		m_moveTxt.text = string.Format( Localization.Localize("97") , LocalDBManager.Instance.getAverageMoveScore( BigStageNum ).ToString());
		m_bombTxt.text = string.Format( Localization.Localize("98") , LocalDBManager.Instance.getAverageBombScore( BigStageNum ).ToString());

		string mode = LocalDBManager.Instance.GetLanguage ();
		
		if(mode == "tw")
		{
			FBBtnSprite.spriteName = "UI_BigStage_FB_Btn";
		}
		else
		{
			FBBtnSprite.spriteName = "UI_BigStage_FB_Btn_en";
		}
	}

	public void postFBText()
	{
		PopupManager.Instance.ShowMessageBox_ForFB (true);
	}
}
