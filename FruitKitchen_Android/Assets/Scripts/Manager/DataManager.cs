using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// define the global data in here
public class DataManager : ManagerComponent<DataManager> {
	public enum InGameType
	{
		None,
		SmallStage,
		StageInfo,
	}
	public InGameType m_InGameType = InGameType.None;
	public int BigStageNum = 1;
	public int SmallStageNum = 4;

	public int recordBigStageNum = 0;
	public int recordSmallStageNum = 0;

	public int StageKey = 0;

	public string Language = "";

	public bool IsTeachMode = false;
	public int getSmallStageNum(int BigStage)
	{
		List<int> TmpKey =  CSVManager.Instance.StageInfoTable.GetKeys();

		int smallStageNum = 0;
		for(int i = 0; i < TmpKey.Count; i++)
		{
			int bigStage = CSVManager.Instance.StageInfoTable.readFieldAsInt(TmpKey[i], "BigStage");

			if(bigStage == BigStage)
			{
				smallStageNum++;
			}
		}

		return smallStageNum;
	}

	public GameObject FindChild(GameObject pRoot, string pName)
	{
		return pRoot.transform.Find(pName).gameObject;
	}

	public FaceBookData m_TmpFbData;
	public FaceBookFriendData m_TmpFbFriendData;

	private Dictionary< string , int > m_ItemStoreKeyMap = new Dictionary<string, int>();
	// get Store item Key by Store item Name
	public int GetItemKeyByItemName( string ItemName )
	{
		if( m_ItemStoreKeyMap.Count == 0 )
		{
			foreach( int Key in  CSVManager.Instance.StoreSaleTable.GetKeys())
			{
				m_ItemStoreKeyMap[ CSVManager.Instance.StoreSaleTable.readFieldAsString( Key , "ItunesId" )] = Key;
			}

		}

		return m_ItemStoreKeyMap.ContainsKey( ItemName ) ? m_ItemStoreKeyMap[ItemName] : -1;
	}


	public bool m_IsShowLogo = true;
}
