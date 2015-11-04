using UnityEngine;
using System.Collections;

public class GiftUI : MonoBehaviour {
	[SerializeField]
	private GiftUnit m_unitObj;
	[SerializeField]
	private UIGrid m_gridObj;


	public void Setup( params object [] p_param )
	{
		for( int i = 0 ; i < NetWorkManager.Instance.myFriendData.Count ; i ++ )
		{
			PersonalData TmpData =  NetWorkManager.Instance.myFriendData[i];

			if( TmpData.PD_IsPlayThisGame == false ) continue;
			GameObject TmpObj =  Instantiate( m_unitObj.gameObject ) as GameObject;
			TmpObj.transform.parent = m_gridObj.transform;
			TmpObj.transform.localScale = Vector3.one;
			TmpObj.SetActive( true );
			TmpObj.GetComponent< GiftUnit >().Setup( TmpData );
		}
	}

	void OnClickCloseBtn()
	{
		PopupManager.Instance.CloseCurrentPopup();
	}
}
