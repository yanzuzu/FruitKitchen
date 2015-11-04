using UnityEngine;
using System.Collections;

public class MailUI : MonoBehaviour {
	[SerializeField]
	private MailUnit m_unitObj;
	[SerializeField]
	private UIGrid m_girdobj;

	
	public void Setup( params object [] p_param )
	{
		for( int i = 0 ; i < NetWorkManager.Instance.mailData.Count ; i ++ )
		{
			GameObject TmpObj =  Instantiate( m_unitObj.gameObject ) as GameObject;
			TmpObj.transform.parent = m_girdobj.transform;
			TmpObj.transform.localScale = Vector3.one;
			TmpObj.SetActive( true );
			TmpObj.GetComponent< MailUnit >().Setup(NetWorkManager.Instance.mailData[i]);

			CPDebug.Log("MailUI = " + NetWorkManager.Instance.mailData[i].mailComFrom);
		}
	}

	public void OnClickCloseBtn()
	{
		PopupManager.Instance.CloseCurrentPopup();
	}
}
