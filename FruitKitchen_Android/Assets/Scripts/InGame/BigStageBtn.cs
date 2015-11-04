using UnityEngine;
using System.Collections;

public class BigStageBtn : MonoBehaviour {
	[SerializeField]
	private int m_BigStageIdx;
	[SerializeField]
	private UILabel m_startTxt;

	bool m_isCanClick = true;
	void Start()
	{
		m_startTxt.text = LocalDBManager.Instance.getStageTotalStarNum(m_BigStageIdx).ToString();

		// check is can click
		string TmpBigStageStr= LocalDBManager.Instance.getRecordBigStage();
		int TmpNum = 1;
		if( string.Empty !=  TmpBigStageStr )
			TmpNum = int.Parse(TmpBigStageStr);
		if( TmpNum < m_BigStageIdx )
		{
			m_isCanClick = false;
			m_startTxt.gameObject.SetActive( false );
		}

	}

	public void OnClickBigStageBtn()
	{
		// check is can click
		if( m_isCanClick == false )
		{
			CPDebug.Log("111111111111~~~~" + m_BigStageIdx );
			CPDebug.Log("222222222222~~~~" + DataManager.Instance.recordBigStageNum );
			if( DataManager.Instance.recordBigStageNum + 1 == m_BigStageIdx )
			{
				int StageCount = InGameSceneMain.Instance.StageInfoData[DataManager.Instance.recordBigStageNum].Count;
				CPDebug.Log("333333333333333333~~~~" + StageCount);
				CPDebug.Log("4444444444444444444~~~~" + DataManager.Instance.recordSmallStageNum);
				if( DataManager.Instance.recordSmallStageNum == StageCount )
					PopupManager.Instance.ShowMessageBox( (int)Config.Message_Type_Enum.Message_Type_Unlock_Stage );


			}
			return;
		}
		InGameSceneMain.Instance.ChangeUI( InGameSceneMain.InGameUIType.SmallStage , m_BigStageIdx  );

	}
}
