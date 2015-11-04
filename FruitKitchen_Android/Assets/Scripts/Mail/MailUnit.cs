using UnityEngine;
using System.Collections;

public class MailUnit : MonoBehaviour {
	[SerializeField]
	private UILabel m_title;
	[SerializeField]
	private UILabel m_content;
	[SerializeField]
	private UILabel m_content2;
	[SerializeField]
	private UISprite m_itemicon;
	[SerializeField]
	private UILabel m_itemCount;
	[SerializeField]
	private UITexture m_fbImg;

	MailData m_data;
	public void Setup( params object [] p_param )
	{
		m_data =  p_param[0] as MailData;
		switch( m_data.mailType )
		{
		case Config.MESSAGE_TYPE_CHALLENG_FROM_OTHER:
			ChallengFromOther();
			break;
		case Config.MESSAGE_TYPE_CHALLENG_GIFT :
			ChallengGift();
			break;
		case Config.MESSAGE_TYPE_GIFT:
			MessageTypeGift();
			break;
		case Config.MESSAGE_INVITE_UNLOCK:
			InviteUnlock();
			break;

		}

	}
	// on click action
	public void OnClickAction()
	{
		switch( m_data.mailType )
		{
		case Config.MESSAGE_TYPE_CHALLENG_FROM_OTHER:

			ChallengData chaData = NetWorkManager.Instance.getChallengDataByFBUid(m_data.mailComFrom);

			if(chaData != null)
			{
				DataManager.Instance.BigStageNum = chaData.challengBigStage;
				DataManager.Instance.SmallStageNum = chaData.challengSmallStage;

				string Key = string.Format("{0}:{1}" , DataManager.Instance.BigStageNum ,  DataManager.Instance.SmallStageNum );
				DataManager.Instance.StageKey = InGameSceneMain.Instance.StageInfoToKey[Key];
				
				NetWorkManager.Instance.challengFriendFBUid = m_data.mailComFrom;
				ChallengData TmpData =  NetWorkManager.Instance.getChallengDataByFBUid( m_data.mailComFrom );
				NetWorkManager.Instance.challengScoreType = TmpData.challengScoreType;
				NetWorkManager.Instance.isPlayChallengMode = true;
				NetWorkManager.Instance.isBeChalleng = true;
				SceneChangeManager.Instance.LoadScene("GamePlay");
			}

			break;
		case Config.MESSAGE_TYPE_CHALLENG_GIFT :
			NetWorkManager.Instance.parse_startGainMailItemProcess(m_data.mailOringeStr);
			break;
		case Config.MESSAGE_TYPE_GIFT:
			NetWorkManager.Instance.parse_startGainMailItemProcess(m_data.mailOringeStr);
			break;
		case Config.MESSAGE_INVITE_UNLOCK:
			NetWorkManager.Instance.startUnlockHelp(m_data.mailOringeStr , m_data.mailComFrom , int.Parse(m_data.mailParam1) , int.Parse(m_data.mailParam2) );
			break;
			
		}
		PopupManager.Instance.CloseCurrentPopup();
	}
	void ChallengFromOther()
	{
		ChallengData TmpData =  NetWorkManager.Instance.getChallengDataByFBUid( m_data.mailComFrom );
		string TmpStr =  Localization.Localize("57");
		m_title.text = TmpStr;
		string TmpStr2 = Localization.Localize("64");
		CPDebug.Log("FBuid !!!! = " + m_data.mailComFrom  );
		PersonalData TmpFriendData = NetWorkManager.Instance.getFriendData( m_data.mailComFrom  );
		if( TmpFriendData == null ) return;

		FaceBookManager.Instance.SetFbImg( m_fbImg , TmpFriendData.PD_FB_Image );
		m_content.gameObject.SetActive( true );
		m_content.text = string.Format( TmpStr2 , TmpFriendData.PD_FB_Name );

		string TmpContent2 = Localization.Localize("65");

		// stage info
		string TmpStageTypeStr = TmpData.challengScoreType == Config.CHALLENG_SCORE_TYPE_MOVE ? Localization.Localize("63") : Localization.Localize("62");
		m_content2.text = string.Format( TmpContent2 , TmpData.challengBigStage , TmpData.challengSmallStage , TmpStageTypeStr, TmpData.challengScore );
		m_content2.gameObject.SetActive( true );
	}

	void ChallengGift()
	{
		string TmpStr =  Localization.Localize("58");
		m_title.text = TmpStr;
		string TmpStr2 = Localization.Localize("66");
		CPDebug.Log("FBuid !!!! = " + m_data.mailComFrom  );
		PersonalData TmpFriendData = NetWorkManager.Instance.getFriendData( m_data.mailComFrom  );
		if( TmpFriendData == null ) return;

		FaceBookManager.Instance.SetFbImg( m_fbImg , TmpFriendData.PD_FB_Image );

		m_content.gameObject.SetActive( true );
		m_content.text = string.Format( TmpStr2 , TmpFriendData.PD_FB_Name );

		string TmpContent2 = Localization.Localize("67");
		
		// stage info
		string TmpStageTypeStr = int.Parse(m_data.mailParam1) == (int)Config.CHALLENG_SCORE_TYPE_MOVE ? Localization.Localize("63") : Localization.Localize("62");
		m_content2.gameObject.SetActive( true );
		m_content2.text = string.Format( TmpContent2 , m_data.mailParam6 ,  m_data.mailParam7 , TmpStageTypeStr,  m_data.mailParam2 ,  m_data.mailParam3);

		m_itemicon.spriteName = CSVManager.Instance.ItemInfoTable.readFieldAsString(int.Parse(m_data.mailParam4) , "ImageName");
		m_itemCount.text = m_data.mailParam5;
	}

	void MessageTypeGift()
	{
		string TmpStr =  Localization.Localize("59");
		m_title.text = TmpStr;
		string TmpStr2 = Localization.Localize("68");
		CPDebug.Log("MessageTypeGift FBuid !!!! = " + m_data.mailComFrom  );
		PersonalData TmpFriendData = NetWorkManager.Instance.getFriendData( m_data.mailComFrom  );
		if( TmpFriendData == null ) return;

		FaceBookManager.Instance.SetFbImg( m_fbImg , TmpFriendData.PD_FB_Image );

		m_content.gameObject.SetActive( true );
		m_content.text = string.Format( TmpStr2 , TmpFriendData.PD_FB_Name );

		m_itemicon.spriteName = CSVManager.Instance.ItemInfoTable.readFieldAsString(int.Parse(m_data.mailParam1) , "ImageName");
		m_itemCount.text = m_data.mailParam2;
	}

	void InviteUnlock()
	{
		string inviteType = m_data.mailParam1 ;
		string inviteParam  = m_data.mailParam2 ;
		CPDebug.Log("InviteUnlock FBuid !!!! = " + m_data.mailComFrom  );
		PersonalData TmpFriendData = NetWorkManager.Instance.getFriendData( m_data.mailComFrom  );
		if( TmpFriendData == null ) return;

		FaceBookManager.Instance.SetFbImg( m_fbImg , TmpFriendData.PD_FB_Image );


		if( int.Parse(inviteType) == (int)Config.MESSAGE_INVITE_UNLOCK_BIG_STAGE )
		{
			string TmpStr2 = Localization.Localize("88");
			m_content.gameObject.SetActive( true );
			m_content.text = string.Format( TmpStr2 , TmpFriendData.PD_FB_Name , inviteParam );
		}else if( int.Parse(inviteType) == (int)Config.MESSAGE_INVITE_UNLOCK_GOLD_ITEM  )
		{
			string TmpStr3 = Localization.Localize("89");
			int TmpParam = int.Parse( inviteParam );
			if( TmpParam == (int)Config.Operate_Mode_Enum.Operate_Mode_Lignting )
			{
				m_content.text = string.Format( TmpStr3 , TmpFriendData.PD_FB_Name , Localization.Localize("90") );
			}else if( TmpParam == (int)Config.Operate_Mode_Enum.Operate_Mode_Shovel )
			{
				m_content.text = string.Format( TmpStr3 , TmpFriendData.PD_FB_Name , Localization.Localize("91") );
			}else if( TmpParam == (int)Config.Operate_Mode_Enum.Operate_Mode_Bomber )
			{
				m_content.text = string.Format( TmpStr3 , TmpFriendData.PD_FB_Name , Localization.Localize("92") );
			}
			m_content.gameObject.SetActive( true );
		}
	}
}
