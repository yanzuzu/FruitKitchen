using UnityEngine;
using System.Collections;

public class StoreItemUnit : MonoBehaviour {
	[SerializeField]
	private UISprite m_itemImg;
	[SerializeField]
	private UILabel m_itemCount;

	public void Setup( params object [] p_param )
	{
		int TmpKey =  (int)p_param[0];
		int TmpCount = LocalDBManager.Instance.getOwnItemNum(TmpKey);
		m_itemImg.spriteName =  CSVManager.Instance.ItemInfoTable.readFieldAsString( TmpKey , "ImageName" );
		m_itemImg.MakePixelPerfect();
		m_itemCount.text = TmpCount.ToString();
	}
}
