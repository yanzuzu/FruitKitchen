using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InGameSceneMain : MonoBehaviour {
	public static InGameSceneMain Instance;

	public enum InGameUIType
	{
		BigStage = 1,
		SmallStage= 2,
		StartLevel = 3,
	}

	InGameUIType m_currentUIType  = InGameUIType.BigStage;
	public InGameUIType m_CurrentUIType
	{
		get{return m_currentUIType;}
	}
	[SerializeField]
	private SmallLevelUI m_smallLevelUI;

	public SmallLevelUI m_SmallLevelUI
	{
		get{return m_smallLevelUI;}
	}

	[SerializeField]
	private BigLevelUI m_bigLevelUI;

	public BigLevelUI m_BigLevelUI
	{
		get{return m_bigLevelUI;}
	}

	[SerializeField]
	private StartLevelUI m_startLevelUI;
	
	public StartLevelUI m_StartLevelUI
	{
		get{return m_startLevelUI;}
	}


	public Dictionary< int , List<int> > StageInfoData = new Dictionary<int, List<int>>();
	public Dictionary< string , int > StageInfoToKey = new Dictionary<string, int>();
	void Awake()
	{
		Instance = this;
	}
	// Use this for initialization
	void Start () {

		// get Stage info
		List<int> TmpKey =  CSVManager.Instance.StageInfoTable.GetKeys();
		for( int i = 0 ; i < TmpKey.Count ; i ++ )
		{
			int TmpBigStage = CSVManager.Instance.StageInfoTable.readFieldAsInt(TmpKey[i], "BigStage");
			int TmpSmallStage = CSVManager.Instance.StageInfoTable.readFieldAsInt(TmpKey[i], "SmallStage");
			if( StageInfoData.ContainsKey( TmpBigStage ) == false )
				StageInfoData[ TmpBigStage ] = new List<int>();
			if( StageInfoData[ TmpBigStage ].Contains( TmpSmallStage ) == false )
				StageInfoData[ TmpBigStage ].Add(TmpSmallStage);
			
			string Key = string.Format("{0}:{1}" , TmpBigStage ,  TmpSmallStage );
			StageInfoToKey[ Key] = TmpKey[i];
		}

		if( DataManager.Instance.m_InGameType == DataManager.InGameType.None )
		{
			m_currentUIType = InGameUIType.BigStage;
			ChangeUI( m_currentUIType );
		}
		else if( DataManager.Instance.m_InGameType == DataManager.InGameType.SmallStage )
		{
			m_currentUIType = InGameUIType.SmallStage;
			ChangeUI( m_currentUIType , DataManager.Instance.BigStageNum );
		}else
		{
			m_currentUIType = InGameUIType.StartLevel;
			ChangeUI( m_currentUIType , DataManager.Instance.BigStageNum , DataManager.Instance.SmallStageNum);
		}

	}

	public void ChangeUI(InGameUIType p_uitype ,int p_bigStageIdx = -1 , int p_smallStageIdx = -1 )
	{
		switch( p_uitype )
		{
		case InGameUIType.BigStage:
			m_smallLevelUI.gameObject.SetActive (false );
			m_bigLevelUI.GetComponent< BigLevelUI >().Setup();
			m_bigLevelUI.gameObject.SetActive( true );
			m_startLevelUI.gameObject.SetActive( false );
			break;

		case InGameUIType.SmallStage:
			m_smallLevelUI.gameObject.SetActive ( true  );
			m_smallLevelUI.Setup(p_bigStageIdx);
			m_bigLevelUI.gameObject.SetActive( false );
			m_startLevelUI.gameObject.SetActive( false );
			break;
		case InGameUIType.StartLevel:
			m_startLevelUI.gameObject.SetActive ( true  );
			string Key = string.Format("{0}:{1}" , p_bigStageIdx ,  p_smallStageIdx );
			DataManager.Instance.StageKey = StageInfoToKey[Key];
			m_startLevelUI.Setup(p_bigStageIdx , p_smallStageIdx );
			m_smallLevelUI.gameObject.SetActive( false );
			m_bigLevelUI.gameObject.SetActive( false );

			break;
		}
		m_currentUIType = p_uitype;
	}
	
}
