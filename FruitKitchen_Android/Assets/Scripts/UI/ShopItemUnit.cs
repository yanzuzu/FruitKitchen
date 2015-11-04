using UnityEngine;
using System.Collections;

public class ShopItemUnit : MonoBehaviour {
	[SerializeField]
	private UISprite m_itemImg;
	[SerializeField]
	private UILabel m_itemNum;
	[SerializeField]
	private UILabel m_itemBuyTxt;
	[SerializeField]
	private UILabel m_itemIconNum;

	PurchasableItem m_item;
	public void Setup(PurchasableItem p_item)
	{
		m_item = p_item;

		int ItemKey = DataManager.Instance.GetItemKeyByItemName(p_item.name);
		int StoreItemId = CSVManager.Instance.StoreSaleTable.readFieldAsInt( ItemKey , "StoreItemId" );
		m_itemImg.spriteName = CSVManager.Instance.ItemInfoTable.readFieldAsString(StoreItemId,"ImageName");
		m_itemNum.text = string.Format( "X {0}", CSVManager.Instance.StoreSaleTable.readFieldAsString(ItemKey,"ItemCount"));
		m_itemBuyTxt.text = string.Format("{0}${1}", p_item.isoCurrencySymbol.ToString() ,  p_item.priceInLocalCurrency.ToString());
		m_itemIconNum.gameObject.SetActive( true );
		if( p_item.name ==  "fruit_gold_v001")
		{
			m_itemIconNum.text = "15";
			m_itemIconNum.transform.localPosition = new Vector3(-160,m_itemIconNum.transform.localPosition.y,m_itemIconNum.transform.localPosition.z);
		}else if( p_item.name == "fruit_move_v001" )
		{
			m_itemIconNum.text = "+5";
			m_itemIconNum.transform.localPosition = new Vector3(-145,m_itemIconNum.transform.localPosition.y,m_itemIconNum.transform.localPosition.z);
		}else
			m_itemIconNum.gameObject.SetActive( false );

		  
	}

	void OnClickBuyItem()
	{
		CPDebug.Log("OnClickBuyItem");
		UniBillManager.Instance.BuyItem( m_item );
	}
}
