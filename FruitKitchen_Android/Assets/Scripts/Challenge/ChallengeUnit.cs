using UnityEngine;
using System.Collections;

public class ChallengeUnit : MonoBehaviour {
	[SerializeField]
	private UITexture m_friendImg;
	[SerializeField]
	private UILabel m_friendName;
	[SerializeField]
	private UILabel m_friendContent;
	[SerializeField]
	private ChallengeUI m_motherUI;
	[SerializeField]
	public UICheckbox checkBox;
	
	PersonalData m_data;
	public void Setup( params object [] p_param )
	{
		m_data = p_param[0] as PersonalData;
		m_motherUI = p_param[1] as ChallengeUI;
		m_friendName.text = m_data.PD_FB_Name;
		FaceBookManager.Instance.SetFbImg( m_friendImg , m_data.PD_FB_Image );
		if( NetWorkManager.Instance.checkCanChallengToSomeOne( m_data.PD_FB_Uid ) == true )
			m_friendContent.gameObject.SetActive( false );
		else
		{
			m_friendContent.gameObject.SetActive( true );
			string TmpStr = Localization.Localize("56");

			string nextTime = NetWorkManager.Instance.getChallengUntilTime(m_data.PD_FB_Uid );
			CPDebug.Log("nextTime = " + nextTime);
			m_friendContent.text = string.Format("{0}{1}", TmpStr, nextTime);
		}
	}

	public void VisibleCheckImg()
	{
		if(checkBox.isChecked)
		{
			m_motherUI.m_ChallengeFBUid = m_data.PD_FB_Uid;
		}
	}

}
