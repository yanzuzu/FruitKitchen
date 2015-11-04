using UnityEngine;
using System.Collections;

public class StoreItemUnitForHeart : MonoBehaviour {
	[SerializeField]
	private UISprite m_itemImg;
	[SerializeField]
	private UILabel m_itemCount;
	[SerializeField]
	private UILabel m_useCountTxt;

	[SerializeField]
	private GameObject m_itemUI;
	[SerializeField]
	private GameObject m_multiImg;
	[SerializeField]
	private GameObject m_ItemNumSelectUI;
	[SerializeField]
	private UILabel m_ItemIconNum;

	public int m_UseCount = 0;
	public int m_Key = 0;
	int m_maxCount = 0 ;
	public void Setup( params object [] p_param )
	{
		m_Key = (int)p_param[0];
		// determine is in Game Play ? adjust the pos
		if( IsHeart() == false )
		{
			if( GamePlayLayer.Instance == null )
			{
				// not in GamePlay Scene
				m_itemUI.transform.localPosition = new Vector3(69,0,0);
				m_multiImg.transform.localPosition = new Vector3(13,0,0);
				m_itemCount.gameObject.transform.localPosition = new Vector3(114,0,0);
				m_ItemNumSelectUI.SetActive(false);
			}else
			{
				m_itemUI.transform.localPosition = new Vector3(0,0,0);
				m_multiImg.transform.localPosition = new Vector3(-110.5186f,0.4f,0);
				m_itemCount.gameObject.transform.localPosition = new Vector3(-75,0,0);
				m_ItemNumSelectUI.SetActive(true);
			}
		}
		// speceil 1 and 3 item need to show num
		if( m_Key == 1 )
			m_ItemIconNum.text = "15";
		else if( m_Key == 3 )
			m_ItemIconNum.text = "5";
		else
			m_ItemIconNum.gameObject.SetActive( false );
		m_maxCount = LocalDBManager.Instance.getOwnItemNum(m_Key);
		m_itemImg.spriteName =  CSVManager.Instance.ItemInfoTable.readFieldAsString( m_Key , "ImageName" );
		m_itemImg.MakePixelPerfect();
		m_itemCount.text = m_maxCount.ToString();
		UpdateCount();
	}
	bool IsHeart()
	{
		return CSVManager.Instance.ItemInfoTable.readFieldAsInt(m_Key,"Effect" ) == (int)Config.Item_Effect_Enum.Item_Effect_Love_Heart;
	}
	public void OnClickAddHeart()
	{
		m_UseCount ++;
		if( m_UseCount > m_maxCount ) m_UseCount = m_maxCount;
		UpdateCount();
	}

	public void OnClickSubHeart()
	{
		m_UseCount --;
		if( m_UseCount < 0 ) m_UseCount = 0;
		UpdateCount();
	}

	void UpdateCount()
	{
		m_useCountTxt.text = m_UseCount.ToString();
	}
}
