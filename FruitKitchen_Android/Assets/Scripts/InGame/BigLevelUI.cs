using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BigLevelUI : MonoBehaviour {
	[SerializeField]
	List< UISprite > m_stageBtnImgList ;

	void Start()
	{
		EventManager.Instance.registerEvent( EventManager.EventName.IAP_BuyOK , onBuyOK );
	}

	void onBuyOK( params object [] p_param)
	{
		PurchasableItem Tmpitem = p_param[0] as PurchasableItem;
		if( Tmpitem.name == "stage_unlock_v001" )
		{
			LocalDBManager.Instance.unlockNextBigStage();
			Setup();
		}
	}
	public void Setup( params object [] p_param)
	{
	 	string TmpBigStageStr= LocalDBManager.Instance.getRecordBigStage();
		int TmpNum = 1;
		if( string.Empty !=  TmpBigStageStr)
			TmpNum = int.Parse(TmpBigStageStr);

		for( int i = 0 ; i <  m_stageBtnImgList.Count ; i ++ )
		{
			if( i + 1 > TmpNum )
				m_stageBtnImgList[i].spriteName =  string.Format( "UI_BigStage_boardStage_{0}_lock" , i + 1 );
			else
				m_stageBtnImgList[i].spriteName =  string.Format( "UI_BigStage_boardStage_{0}" , i + 1 );
		}

	}
}
