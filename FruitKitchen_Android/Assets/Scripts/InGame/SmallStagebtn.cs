using UnityEngine;
using System.Collections;

public class SmallStagebtn : MonoBehaviour {
	[SerializeField]
	UILabel m_txt;
	[SerializeField]
	UISprite m_bg;
	[SerializeField]
	UISprite star1Sprite;
	[SerializeField]
	UISprite star2Sprite;
	[SerializeField]
	UISprite star3Sprite;

	int m_bigStage;
	int m_smallStage;

	bool m_IsCanClick = true;
	public void OnClickSmallStageBtn()
	{
		if( m_IsCanClick == false ) return;
		// check is teach 
		if( m_bigStage == 1 && m_smallStage <= 3 )
		{
			DataManager.Instance.IsTeachMode = true;
			// play Game
			DataManager.Instance.BigStageNum = m_bigStage;
			DataManager.Instance.SmallStageNum = m_smallStage;
			string Key = string.Format("{0}:{1}" , m_bigStage ,  m_smallStage );
			DataManager.Instance.StageKey = InGameSceneMain.Instance.StageInfoToKey[Key];
			// check heart
			if( LocalDBManager.Instance.GetLifeNum() <= 0 )
			{
				CPDebug.LogWarning("Heart is not enough!!");
				return;
			}
			SceneChangeManager.Instance.LoadScene("GamePlay");
		}else
		{
			DataManager.Instance.IsTeachMode = false;
			InGameSceneMain.Instance.ChangeUI(InGameSceneMain.InGameUIType.StartLevel  ,m_bigStage ,  m_smallStage );
		}
	}

	public void Setup(int BigStageIdx , int SmallStageIdx )
	{
		m_bigStage = BigStageIdx ;
		m_smallStage = SmallStageIdx;
		if( BigStageIdx == 1 )
		{	
			if( SmallStageIdx <= 3 )
				m_txt.text = string.Format("T{0}-{1}" , BigStageIdx,SmallStageIdx );
			else
				m_txt.text = string.Format("{0}-{1}" , BigStageIdx,SmallStageIdx - 3);
		}else
			m_txt.text = string.Format("{0}-{1}" , BigStageIdx,SmallStageIdx );
		// check lock
		string TmpSmallStr = LocalDBManager.Instance.getRecordSmallStage();
		int TmpNum = 1;
		if( string.Empty != TmpSmallStr )
			TmpNum = int.Parse(TmpSmallStr);

		int recordBigStage = int.Parse(LocalDBManager.Instance.getRecordBigStage ());
		if( TmpNum < 4 && BigStageIdx == 1 && recordBigStage == 1) TmpNum = 4;

		if(BigStageIdx == recordBigStage)
		{
			if( SmallStageIdx > TmpNum )
			{
				m_bg.spriteName = "UI_SmallStage_levelLocked";
				m_txt.gameObject.SetActive( false );
				m_IsCanClick = false;

				hideStar();
			}else
			{
				m_bg.spriteName = "UI_SmallStage_levelUnlocked";
				m_txt.gameObject.SetActive( true );
				m_IsCanClick = true;

				showStageStar();
			}
		}
		else
		{
			m_bg.spriteName = "UI_SmallStage_levelUnlocked";
			m_txt.gameObject.SetActive( true );
			m_IsCanClick = true;

			showStageStar();
		}
	}

	public void showStageStar()
	{
		if(m_bigStage == 1 &&
		   m_smallStage <= 3)
		{
			hideStar();
			return;
		}

		int recordStar = LocalDBManager.Instance.getStageStarNum(m_bigStage, m_smallStage);
		
		if(recordStar == (int)Config.Pass_Stage_Enum.Pass_Stage_Star_1)
		{
			star1Sprite.spriteName = "UI_SmallStage_levelInfo_Star_1";
		}
		else if(recordStar == (int)Config.Pass_Stage_Enum.Pass_Stage_Star_2)
		{
			star1Sprite.spriteName = "UI_SmallStage_levelInfo_Star_1";
			star2Sprite.spriteName = "UI_SmallStage_levelInfo_Star_1";
		}
		else if(recordStar == (int)Config.Pass_Stage_Enum.Pass_Stage_Star_3)
		{
			star1Sprite.spriteName = "UI_SmallStage_levelInfo_Star_1";
			star2Sprite.spriteName = "UI_SmallStage_levelInfo_Star_1";
			star3Sprite.spriteName = "UI_SmallStage_levelInfo_Star_1";
		}
	}

	public void hideStar()
	{
		star1Sprite.gameObject.SetActive(false);
		star2Sprite.gameObject.SetActive(false);
		star3Sprite.gameObject.SetActive(false);
	}
}
