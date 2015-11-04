using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopItemUI : MonoBehaviour {
	[SerializeField]
	private ShopItemUnit m_itemUnit;
	[SerializeField]
	private UIGrid m_grid;

	List< ShopItemUnit> m_unitList = new List<ShopItemUnit>();

	void Start()
	{
		EventManager.Instance.registerEvent( EventManager.EventName.IAP_BuyOK , OnBuyOK );
	}
	void OnBuyOK( params object [] p_param )
	{
		PurchasableItem Tmpitem =  p_param[0] as PurchasableItem;
		CPDebug.Log("Tmpitem = " + Tmpitem.name );
		if( Tmpitem.name == "stage_unlock_v001" )
		{
			LocalDBManager.Instance.unlockNextBigStage();
		}else
		{
			int TmpKey = DataManager.Instance.GetItemKeyByItemName( Tmpitem.name );
			int StoreItemId = CSVManager.Instance.StoreSaleTable.readFieldAsInt(TmpKey,"StoreItemId");
			int ItemCount = CSVManager.Instance.StoreSaleTable.readFieldAsInt(TmpKey,"ItemCount");
			LocalDBManager.Instance.addOneOwnItem(StoreItemId,ItemCount);
		}
	}
	public void Setup()
	{
		for( int i = 0 ; i < m_unitList.Count ; i ++ )
			DestroyImmediate( m_unitList[i].gameObject );

		PurchasableItem [] TmpItemList =  UniBillManager.Instance.GetIAPItem();
		for( int i = 0 ; i < TmpItemList.Length ; i ++ )
		{
			if( TmpItemList[i].name == "stage_unlock_v001") continue;
			GameObject Tmpobj =  Instantiate( m_itemUnit.gameObject ) as GameObject;
			Tmpobj.transform.parent = m_grid.transform;
			Tmpobj.transform.localScale = Vector3.one;
			Tmpobj.SetActive( true );
			ShopItemUnit TmpUnitObj = Tmpobj.GetComponent< ShopItemUnit >();
			TmpUnitObj.Setup(TmpItemList[i]);
			m_unitList.Add( TmpUnitObj );

		}
		m_grid.Reposition();
	}
}
