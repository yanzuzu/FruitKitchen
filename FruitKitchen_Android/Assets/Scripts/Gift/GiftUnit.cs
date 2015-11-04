using UnityEngine;
using System.Collections;

public class GiftUnit : MonoBehaviour {
	[SerializeField]
	private UITexture m_friendImg;
	[SerializeField]
	private UILabel m_friendName;
	[SerializeField]
	private UILabel m_friendContent;
	[SerializeField]
	private GameObject m_giftBtn;
	PersonalData m_data;
	public void Setup( params object [] p_param )
	{
		m_data = p_param[0] as PersonalData;
		m_friendName.text = m_data.PD_FB_Name;
		FaceBookManager.Instance.SetFbImg( m_friendImg , m_data.PD_FB_Image );
		DailyGiftData TmpGiftData =  NetWorkManager.Instance.getDailyGiftDataByFBUid( m_data.PD_FB_Uid );
		if( TmpGiftData != null )
		{
			// has gift
			m_giftBtn.SetActive( false );
			string TmpTxt = Localization.Localize("70");
			string nextTime = NetWorkManager.Instance.getLimmitUnitTime(TmpGiftData.dailyGiftlimmitTime );
			m_friendContent.text = string.Format(TmpTxt, nextTime);
		}else
		{
			m_giftBtn.SetActive( true );
			m_friendContent.gameObject.SetActive( false );
		}
	}

	public void OnClickGiftBtn()
	{
		NetWorkManager.Instance.parse_sendDailyGift(m_data.PD_FB_Uid);
		PopupManager.Instance.CloseCurrentPopup();
	}

}
