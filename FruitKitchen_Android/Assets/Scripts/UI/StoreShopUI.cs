using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class StoreShopUI : MonoBehaviour {
	[SerializeField]
	private UISprite m_bg;
	[SerializeField]
	GameObject m_StoreItemUI;
	[SerializeField]
	StoreItemUI m_StoreItemUIObj;
	
	[SerializeField]
	ShopItemUI m_ShopItemUIObj;
	
	public void Setup()
	{
		OnClickStoreBtn();
	}

	public void OnClickStoreBtn()
	{
		CPDebug.Log("OnClickStoreBtn start");
		m_bg.spriteName = "UI_Common_Store_Bk1" + DataManager.Instance.Language ;
		m_StoreItemUI.SetActive( true );
		m_StoreItemUIObj.Setup();
		m_ShopItemUIObj.gameObject.SetActive( false );
	}

	public void OnClickShopBtn()
	{
		CPDebug.Log("OnClickShopBtn start");
		m_bg.spriteName = "UI_Common_Store_Bk2" + DataManager.Instance.Language ;
		m_StoreItemUI.SetActive( false );
		m_ShopItemUIObj.gameObject.SetActive( true );
		m_ShopItemUIObj.Setup();

		//UniBillManager.Instance.BuyItem();
	}

	public void OnClickCloseBtn()
	{
		CPDebug.Log("OnClickCloseBtn start");
		PopupManager.Instance.CloseCurrentPopup();
	}
}
