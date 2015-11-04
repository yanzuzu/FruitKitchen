using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class StoreItemUI : MonoBehaviour {
	[SerializeField]
	private StoreItemUnit m_unitObj;
	[SerializeField]
	private StoreItemUnitForHeart m_heartunitObj;
	[SerializeField]
	private UIGrid m_girdObj;

	//StoreItemUnitForHeart mHeartObj;
	//int mHeartKey;
	
	List< StoreItemUnitForHeart > m_unitObjList = new List<StoreItemUnitForHeart>();
	public void Setup()
	{
		for( int i = 0 ; i < m_unitObjList.Count ; i ++ )
			DestroyImmediate( m_unitObjList[i].gameObject );
		m_unitObjList.Clear();

		Dictionary< int , int > TmpBackPack  =  LocalDBManager.Instance.GetBackPack();
		foreach( int Key in TmpBackPack.Keys )
		{
			GameObject TmpObj = null;
			// determine is heart?
			//if( CSVManager.Instance.ItemInfoTable.readFieldAsInt(Key,"Effect" ) == (int)Config.Item_Effect_Enum.Item_Effect_Love_Heart )
			//{
			TmpObj = Instantiate( m_heartunitObj.gameObject ) as GameObject;
			StoreItemUnitForHeart TmpComponentObj = TmpObj.GetComponent< StoreItemUnitForHeart >();
			//mHeartKey = Key ;
			TmpComponentObj.Setup( Key );
			/*}else
			{
				TmpObj = Instantiate( m_unitObj.gameObject ) as GameObject;
				TmpObj.GetComponent< StoreItemUnit >().Setup( Key );
			}*/
			TmpObj.transform.parent = m_girdObj.transform;
			TmpObj.transform.localScale = Vector3.one;
			TmpObj.SetActive( true );

			m_unitObjList.Add( TmpComponentObj );
		}
		m_girdObj.Reposition();

	}

	public void OnClickOK()
	{
		// to get all unit obj use Item
		Dictionary<int , int > Result = new Dictionary<int, int>();
		for( int i = 0 ; i < m_unitObjList.Count ; i ++ )
		{
			if( m_unitObjList[i].m_UseCount != 0 )
			{
				Result[ m_unitObjList[i].m_Key ] = m_unitObjList[i].m_UseCount;
				if(  IsHeart(m_unitObjList[i].m_Key) )
				{
					int TmpLifeNum = LocalDBManager.Instance.GetLifeNum();
					int DiffLifeNum = Config.MAX_PLAY_NUM - TmpLifeNum;
					int UseCount = Mathf.Min( DiffLifeNum ,m_unitObjList[i].m_UseCount );
					// sub item
					bool TmpOK = LocalDBManager.Instance.decreaseItemNum( m_unitObjList[i].m_Key , UseCount);
					if( TmpOK )
					{
						LifeManager.Instance.hasChangeLifeNum = true;
						LifeManager.Instance.increaseLife(UseCount);
					}
					
				}else
					LocalDBManager.Instance.decreaseItemNum( m_unitObjList[i].m_Key , m_unitObjList[i].m_UseCount);

			}
		}

		Setup();
		if( GamePlayLayer.Instance != null )
			GamePlayLayer.Instance.UseShopItem( Result);
	}
	
	bool IsHeart(int m_Key)
   	{
		return CSVManager.Instance.ItemInfoTable.readFieldAsInt(m_Key,"Effect" ) == (int)Config.Item_Effect_Enum.Item_Effect_Love_Heart;
   	}

}
