using UnityEngine;
using System.Collections;

public class UniBillManager : ManagerComponent< UniBillManager > {

	// Use this for initialization
	void Start () {
		Unibiller.onBillerReady += onInitialised;
		Unibiller.onPurchaseComplete += onPurchaseOK;
		Unibiller.onPurchaseFailed += onPurchaseFailed;
		Unibiller.onPurchaseCancelled += OnPurChaseCancel;

		Unibiller.Initialise();
	}
	private void onInitialised(UnibillState result) {
		CPDebug.Log("UniBillManager!!! onInitialised result = " + result );
		//PurchasableItem Tmpitem =  Unibiller.GetPurchasableItemById("com.pigtest.120item");
		//CPDebug.Log("Tmpitem = " + Tmpitem.name );

	}

	private  void OnPurChaseCancel(PurchasableItem item)
	{
		CPDebug.Log("OnPurChaseCancel item = "+ item.name  );
		PopupManager.Instance.VisibleLoading( false );
	}

	private void onPurchaseFailed(PurchasableItem item)
	{
		CPDebug.Log("onPurchaseFailed item = "+ item.name  );
		PopupManager.Instance.VisibleLoading( false );
	}
	private void onPurchaseOK(PurchasableItem item)
	{
		CPDebug.Log("onPurchaseOK item = "+ item.name  );
		PopupManager.Instance.VisibleLoading( false );
		EventManager.Instance.onEvent(EventManager.EventName.IAP_BuyOK , item );
	}

	public void BuyItem(string p_buyItemStr )
	{
		PopupManager.Instance.VisibleLoading( true );
		Unibiller.initiatePurchase(p_buyItemStr);
	}
	public void BuyItem( PurchasableItem p_buyItem)
	{
		CPDebug.Log("BuyItem!!!!");
		PopupManager.Instance.VisibleLoading( true );
		Unibiller.initiatePurchase(p_buyItem.name);
	}

	public PurchasableItem []  GetIAPItem()
	{
		return Unibiller.AllPurchasableItems;
	}
}
