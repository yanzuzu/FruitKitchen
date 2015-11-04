using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

public class TeachModeFinger
{
	public Vector3 fingerPosition;
	public int fingerRotateAngle = -1;
	public int fingerType = -1;
}

public class TeachModeAllowContent
{
	public int allowActionSlotIndex = 0;
	public int allowActionType = 0;
}

public class TeachModeStep
{
	public TeachModeStep(int Step)
	{
		stepIndex = Step;
	}

	public int stepIndex = -1;
	public int showMessageId = -1;
	public string markName = "";
	public Vector3 showMessagePos;
	public TeachModeFinger teachFinger = new TeachModeFinger();
	public List< TeachModeAllowContent > allowContents = new List<TeachModeAllowContent>();

	public void addAllowContent(TeachModeAllowContent Content)
	{
		allowContents.Add (Content);
	}

	public bool isAllow(int SlotIndex, int ActionType)
	{
		if(allowContents.Count <= 0)
		{
			return false;
		}

		for(int i = 0; i < allowContents.Count; i++)
		{
			TeachModeAllowContent content = allowContents[i];

			if(content == null)
			{
				continue;
			}

			if(ActionType == Config.TEACH_MODE_ALLOW_ACIOTN_SPACE_TOUCH ||
			   ActionType == Config.TEACH_MODE_ALLOW_ACIOTN_PRESS_FREE_MOVE_BTN ||
			   ActionType == Config.TEACH_MODE_ALLOW_ACIOTN_PRESS_BOMB_DIRECT_BTN ||
			   ActionType == Config.TEACH_MODE_ALLOW_ACIOTN_PRESS_BOMB_HORIZONTAL_BTN)
			{
				if(content.allowActionType == ActionType)
				{
					return true;
				}
			}
			else
			{
				if(content.allowActionSlotIndex == SlotIndex &&
				   content.allowActionType == ActionType)
				{
					return true;
				}
			}
		}

		return false;
	}
}

public class GamePlayLayer : MonoBehaviour {
	public static GamePlayLayer Instance;
	
	[SerializeField]
	public BlockManager m_blockManager;

/*	[SerializeField]
	UIButton m_btnObj;

	[SerializeField]
	UILabel m_label;

	[SerializeField]
	UISprite m_img;

	[SerializeField]
	GameObject mblockUnitObj;

	[SerializeField]
	UIPanel m_Panel;

	[SerializeField]
	float TestNum = 2;*/

	/// <summary>
	/// ////////
	/// </summary>
	[SerializeField]
	UILabel m_Moneylabel;

	[SerializeField]
	UILabel m_FreeMoveNumlabel;

	[SerializeField]
	UILabel m_BombDirectNumlabel;

	[SerializeField]
	UILabel m_NowLeftMoveNumlabel;

	[SerializeField]
	UILabel m_NowFreeMoveNumlabel;
	
	[SerializeField]
	UILabel m_NowBombNumlabel;

	[SerializeField]
	UIButton m_TipBtn;

	[SerializeField]
	UIButton m_ShuffleBtn;

	[SerializeField]
	UIButton m_BomberBtn;

	[SerializeField]
	UILabel m_BomberBtnlabel;
	
	[SerializeField]
	UIButton m_ShovelBtn;

	[SerializeField]
	UILabel m_ShovelBtnlabel;

	[SerializeField]
	UIButton m_LigntingBtn;

	[SerializeField]
	UILabel m_LigntingBtnlabel;
	
	[SerializeField]
	UIButton m_EraseKnifeBtn;

	[SerializeField]
	UILabel m_EraseKnifeBtnlabel;

	[SerializeField]
	UIButton m_BombTwiceBtn;

	[SerializeField]
	UILabel m_BombTwiceBtnlabel;
	
	[SerializeField]
	UIButton m_BombUpgradeBtn;

	[SerializeField]
	UILabel m_BombUpgradeBtnlabel;

	public bool isChallengMode = false;
	public int stage_Pass_Record = (int)Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Null;
	public int stageSelectTag = 0;
	public int bigStageNum = 0;
	public int smallStageNum = 0;
	public bool isBeChalleng = false;
	public bool beChallengWin = false;
	public bool isTeachMode = false;
	public bool isShowTip = false;
	public int nowTeachModeStep = 0;
	public bool isWaitForNextTeachStep = false;
	public string challengFriendFBUid = "";
	public GameObject StageTargetMainBk;
	public GameObject StageTarget1;
	public GameObject StageTarget2;
	public GameObject StageTarget3;
	public GameObject StageTarget4;
	public GameObject GameResultMainObj;
	public GameObject MessageUI;
	public GameObject tipObj;
	public UISprite winFlagSprite = null;
	public UISprite loseFlagSprite = null;
	public UISprite fingerSprite;
	public UISprite teachMark;
	public UISprite challengTargetSprite;
	public UILabel challengTargetLabel;
	
	/// <summary>
	/// number member
	/// </summary>
	bool m_IsShowFieldItems = false;
	public List< TeachModeStep > teachModeSteps = new List<TeachModeStep>();


	//List< GameObject > m_TmpObjList = new List<GameObject>();
	void Awake()
	{
		Instance = this;

		if(m_ShuffleBtn)
		{
			m_ShuffleBtn.gameObject.SetActive(false);
		}

		if(fingerSprite)
		{
			fingerSprite.gameObject.SetActive(false);
		}

		if(teachMark)
		{
			teachMark.gameObject.SetActive(false);
		}

		if(MessageUI)
		{
			MessageUI.gameObject.SetActive(false);
		}

		if(m_TipBtn)
		{
			m_TipBtn.gameObject.SetActive(false);
		}
	}

	public void UseShopItem( Dictionary< int , int > p_useItemMap )
	{
		//p_useItemMap.Add (11, 1);

		foreach (KeyValuePair<int , int> item in p_useItemMap)
		{
			int itemId = item.Key;
			int itemNum = item.Value;

			int itemEffect = CSVManager.Instance.ItemInfoTable.readFieldAsInt(itemId, "Effect");

			Config.Item_Effect_Enum enumType = (Config.Item_Effect_Enum)itemEffect;

			switch(enumType)
			{
			case Config.Item_Effect_Enum.Item_Effect_Coin:
			{
				int realNum = itemNum * Config.COIN_NUM_PER_UNIT;
				m_blockManager.addOwnCoins(realNum);
			}break;
			/*case Config.Item_Effect_Enum.Item_Effect_Love_Heart:
			{
				if(LifeManager.Instance.leftPlayNum >= Config.MAX_PLAY_NUM)
				{
					return;
				}

				int realUseItemNum = Config.MAX_PLAY_NUM - LifeManager.Instance.leftPlayNum;

				if(realUseItemNum > itemNum)
				{
					LifeManager.Instance.increaseLife(itemNum);
				}
				else
				{
					LifeManager.Instance.increaseLife(realUseItemNum);
				}
			}break;*/
			case Config.Item_Effect_Enum.Item_Effect_Move:
			{
				int realNum = itemNum * Config.MOVE_NUM_PER_UNIT;
				m_blockManager.increaseLeftMoveNum(realNum);
			}break;
			case Config.Item_Effect_Enum.Item_Effect_Bomb_Twice:
			{
				m_blockManager.addFieldItemNum((int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Twice, itemNum);
			}break;
			case Config.Item_Effect_Enum.Item_Effect_Bomb_Up:
			{
				m_blockManager.addFieldItemNum((int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Upgrade, itemNum);
			}break;
			case Config.Item_Effect_Enum.Item_Effect_Knife_Erase:
			{
				m_blockManager.addFieldItemNum((int)Config.Operate_Mode_Enum.Operate_Mode_Erase_Knife, itemNum);
			}break;
			case Config.Item_Effect_Enum.Item_Effect_Bomb_Lignting:
			{
				m_blockManager.addFieldItemNum((int)Config.Operate_Mode_Enum.Operate_Mode_Lignting, itemNum);
			}break;
			case Config.Item_Effect_Enum.Item_Effect_Bomb_Shovel:
			{
				m_blockManager.addFieldItemNum((int)Config.Operate_Mode_Enum.Operate_Mode_Shovel, itemNum);
			}break;
			case Config.Item_Effect_Enum.Item_Effect_Bomb_Bomber:
			{
				m_blockManager.addFieldItemNum((int)Config.Operate_Mode_Enum.Operate_Mode_Bomber, itemNum);
			}break;
			}
		}
	}

	// Use this for initialization
	void Start () {

		StartCoroutine(MyUpdate());

		showFieldButtons (false);

		m_FreeMoveNumlabel.text = Config.NEED_COIN_FREE_MOVE.ToString();
		m_BombDirectNumlabel.text = Config.NEED_COIN_BOMB_DIRECT.ToString();

		updateOwnCoins ();

		if(NetWorkManager.Instance.isPlayChallengMode)
		{
			isChallengMode = true;
			isBeChalleng = NetWorkManager.Instance.isBeChalleng;
			beChallengWin = false;

			challengFriendFBUid = NetWorkManager.Instance.challengFriendFBUid;

			NetWorkManager.Instance.isPlayChallengMode = false;
			NetWorkManager.Instance.challengFriendFBUid = "";
		}

		if(!isChallengMode)
		{
			CPDebug.Log("!isChallengMode");
			updateTargets((int)Config.Pass_Stage_Enum.Pass_Stage_Star_1, false);
		}

		bigStageNum = DataManager.Instance.BigStageNum;
		smallStageNum = DataManager.Instance.SmallStageNum;

		if(bigStageNum == 1 &&
		   smallStageNum <= 3)
		{
			m_blockManager.isTeachMode = true;
			isTeachMode = true;

			m_blockManager.initBlocksForTeachMode(bigStageNum, smallStageNum);

			initTeachModeStep();

//			if(fingerSprite)
//			{
//				fingerSprite.transform.localScale = new Vector3(100, 80, 1);
//			}
		}
		else
		{
			m_blockManager.initBlocks(bigStageNum, smallStageNum);

		}

		checkItemLock ();

		if(isTeachMode)
		{
			doNextTeachStep();
		}
		else if(isChallengMode)
		{
			StageTargetMainBk.SetActive(false);

			int chanllengType = NetWorkManager.Instance.challengScoreType;

			if(chanllengType == Config.CHALLENG_SCORE_TYPE_MOVE)
			{
				challengTargetSprite.spriteName = "UI_InGame_Target_Free_Fall";
				challengTargetLabel.text = Localization.Localize("63");
			}
			else
			{
				challengTargetSprite.spriteName = "UI_InGame_Target_Total_Bomb";
				challengTargetLabel.text = Localization.Localize("62");
			}

			challengTargetSprite.gameObject.SetActive(true);
			challengTargetLabel.gameObject.SetActive(true);
		}

		SoundManager.Instance.PlayBGM("Music_In_Play");

		StartCoroutine (waitForLoad());

//		CPDebug.Log ("111111~~ Key = " + DataManager.Instance.StageKey);

//		for( int i = 0 ; i < 10 ; i ++ )
//		{
//			GameObject TmpObj = Instantiate( mblockUnitObj ) as GameObject;
//			TmpObj.SetActive( true );
//			TmpObj.transform.parent = m_Panel.transform;
//			TmpObj.transform.localScale = Vector3.one;
//			TmpObj.transform.localPosition = new Vector3(i*20,0,0);
//			m_TmpObjList.Add(TmpObj);
//		}

		//StartCoroutine(MyUpdate());
	}

	private int waitForLoadCount = 0;
	IEnumerator waitForLoad()
	{
		while(true)
		{
			if(waitForLoadCount != 0)
			{
				checkTip();
				yield break;
			}
			else
			{
				waitForLoadCount = 1;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void initTeachModeStep()
	{
		if(teachMark)
		{
			teachMark.gameObject.SetActive(true);
		}

		if(bigStageNum == 1 &&
		   smallStageNum == 1)
		{
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step1 = new TeachModeStep(1);
			step1.markName = "teach_1_1";

			TeachModeAllowContent content1_1 = new TeachModeAllowContent();
			content1_1.allowActionSlotIndex = 41;
			content1_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_DOWN;
			step1.addAllowContent(content1_1);

			TeachModeAllowContent content1_2 = new TeachModeAllowContent();
			content1_2.allowActionSlotIndex = 32;
			content1_2.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_UP;
			step1.addAllowContent(content1_2);

			step1.showMessageId = 15;
			step1.showMessagePos = new Vector3(0, 84, 0);
			step1.teachFinger.fingerType = Config.TEACH_MODE_FINGER_SWIPE;
			step1.teachFinger.fingerPosition = new Vector3(-15, -124, 1);
			step1.teachFinger.fingerRotateAngle = 270;

			teachModeSteps.Add(step1);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step2 = new TeachModeStep(2);
			step2.markName = "teach_1_2";
			
			TeachModeAllowContent content2_1 = new TeachModeAllowContent();
			content2_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SPACE_TOUCH;
			step2.addAllowContent(content2_1);

			step2.showMessageId = 16;
			step2.showMessagePos = new Vector3(0, 84, 0);
			step2.teachFinger.fingerType = Config.TEACH_MODE_FINGER_ARROW;
			step2.teachFinger.fingerPosition = new Vector3(-129, -316, 1);
			step2.teachFinger.fingerRotateAngle = 90;
			
			teachModeSteps.Add(step2);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step3 = new TeachModeStep(3);
			step3.markName = "teach_1_3";
	
			TeachModeAllowContent content3_1 = new TeachModeAllowContent();
			content3_1.allowActionSlotIndex = 16;
			content3_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_UP;
			step3.addAllowContent(content3_1);

			TeachModeAllowContent content3_2 = new TeachModeAllowContent();
			content3_2.allowActionSlotIndex = 25;
			content3_2.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_DOWN;
			step3.addAllowContent(content3_2);

			step3.showMessageId = 17;
			step3.showMessagePos = new Vector3(0, 84, 0);;
			step3.teachFinger.fingerType = Config.TEACH_MODE_FINGER_SWIPE;
			step3.teachFinger.fingerPosition = new Vector3(108, -255, 1);
			step3.teachFinger.fingerRotateAngle = 270;
			
			teachModeSteps.Add(step3);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step4 = new TeachModeStep(4);
			step4.markName = "teach_1_4";
			
			TeachModeAllowContent content4_1 = new TeachModeAllowContent();
			content4_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SPACE_TOUCH;
			step4.addAllowContent(content4_1);
			
			step4.showMessageId = 18;
			step4.showMessagePos = new Vector3(0, -179, 0);
			step4.teachFinger.fingerType = Config.TEACH_MODE_FINGER_ARROW;
			step4.teachFinger.fingerPosition = new Vector3(-127, -367, 1);
			step4.teachFinger.fingerRotateAngle = 0;
			
			teachModeSteps.Add(step4);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step5 = new TeachModeStep(5);
			step5.markName = "teach_1_5";
			
			TeachModeAllowContent content5_1 = new TeachModeAllowContent();
			content5_1.allowActionSlotIndex = 15;
			content5_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_UP;
			step5.addAllowContent(content5_1);
			
			TeachModeAllowContent content5_2 = new TeachModeAllowContent();
			content5_2.allowActionSlotIndex = 24;
			content5_2.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_DOWN;
			step5.addAllowContent(content5_2);
			
			step5.showMessageId = 19;
			step5.showMessagePos = new Vector3(0, 84, 0);
			step5.teachFinger.fingerType = Config.TEACH_MODE_FINGER_SWIPE;
			step5.teachFinger.fingerPosition = new Vector3(44, -257, 1);
			step5.teachFinger.fingerRotateAngle = 0;
			
			teachModeSteps.Add(step5);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step6 = new TeachModeStep(6);
			step6.markName = "teach_all";
			
			TeachModeAllowContent content6_1 = new TeachModeAllowContent();
			content6_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SPACE_TOUCH;
			step6.addAllowContent(content6_1);
			
			step6.showMessageId = 20;
			step6.showMessagePos = new Vector3(0, 84, 0);
			
			teachModeSteps.Add(step6);
		}
		else if(bigStageNum == 1 &&
		        smallStageNum == 2)
		{
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step1 = new TeachModeStep(1);
			step1.markName = "teach_2_1";
			
			TeachModeAllowContent content1_1 = new TeachModeAllowContent();
			content1_1.allowActionSlotIndex = 22;
			content1_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_UP;
			step1.addAllowContent(content1_1);

			TeachModeAllowContent content1_2 = new TeachModeAllowContent();
			content1_2.allowActionSlotIndex = 31;
			content1_2.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_DOWN;
			step1.addAllowContent(content1_2);
			
			step1.showMessageId = 21;
			step1.showMessagePos = new Vector3(0, 84, 0);
			step1.teachFinger.fingerType = Config.TEACH_MODE_FINGER_SWIPE;
			step1.teachFinger.fingerPosition = new Vector3(-88, -190, 1);
			step1.teachFinger.fingerRotateAngle = 270;
			
			teachModeSteps.Add(step1);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step2 = new TeachModeStep(2);
			step2.markName = "teach_2_2";
			
			TeachModeAllowContent content2_1 = new TeachModeAllowContent();
			content2_1.allowActionSlotIndex = 20;
			content2_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_UP;
			step2.addAllowContent(content2_1);
			
			TeachModeAllowContent content2_2 = new TeachModeAllowContent();
			content2_2.allowActionSlotIndex = 29;
			content2_2.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_DOWN;
			step2.addAllowContent(content2_2);
			
			step2.showMessageId = 22;
			step2.showMessagePos = new Vector3(0, 84, 0);
			step2.teachFinger.fingerType = Config.TEACH_MODE_FINGER_SWIPE;
			step2.teachFinger.fingerPosition = new Vector3(-209, -185, 1);
			step2.teachFinger.fingerRotateAngle = 0;
			
			teachModeSteps.Add(step2);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step3 = new TeachModeStep(3);
			step3.markName = "teach_2_3";
			
			TeachModeAllowContent content3_1 = new TeachModeAllowContent();
			content3_1.allowActionSlotIndex = 20;
			content3_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SPACE_TOUCH;
			step3.addAllowContent(content3_1);

			step3.showMessageId = 23;
			step3.showMessagePos = new Vector3(0, -176, 0);
			step3.teachFinger.fingerType = Config.TEACH_MODE_FINGER_ARROW;
			step3.teachFinger.fingerPosition = new Vector3(-124, -375, 1);
			step3.teachFinger.fingerRotateAngle = 0;
			
			teachModeSteps.Add(step3);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step4 = new TeachModeStep(4);
			step4.markName = "teach_2_4";
			
			TeachModeAllowContent content4_1 = new TeachModeAllowContent();
			content4_1.allowActionSlotIndex = 34;
			content4_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_UP;
			step4.addAllowContent(content4_1);
			
			TeachModeAllowContent content4_2 = new TeachModeAllowContent();
			content4_2.allowActionSlotIndex = 43;
			content4_2.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_DOWN;
			step4.addAllowContent(content4_2);
			
			step4.showMessageId = 24;
			step4.showMessagePos = new Vector3(0, 84, 0);
			step4.teachFinger.fingerType = Config.TEACH_MODE_FINGER_SWIPE;
			step4.teachFinger.fingerPosition = new Vector3(122, -124, 1);
			step4.teachFinger.fingerRotateAngle = 0;
			
			teachModeSteps.Add(step4);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step5 = new TeachModeStep(5);
			step5.markName = "teach_2_5";
			
			TeachModeAllowContent content5_1 = new TeachModeAllowContent();
			content5_1.allowActionSlotIndex = 16;
			content5_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_TOUCH;
			step5.addAllowContent(content5_1);
			
			step5.showMessageId = 25;
			step5.showMessagePos = new Vector3(0, -78, 0);
			step5.teachFinger.fingerType = Config.TEACH_MODE_FINGER_TOUCH;
			step5.teachFinger.fingerPosition = new Vector3(150, -229, 1);
			step5.teachFinger.fingerRotateAngle = 270;
			
			teachModeSteps.Add(step5);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step6 = new TeachModeStep(6);
			step6.markName = "teach_all";
			
			TeachModeAllowContent content6_1 = new TeachModeAllowContent();
			content6_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SPACE_TOUCH;
			step6.addAllowContent(content6_1);
			
			step6.showMessageId = 26;
			step6.showMessagePos = new Vector3(0, 84, 0);
			
			teachModeSteps.Add(step6);
		}
		else if(bigStageNum == 1 &&
		        smallStageNum == 3)
		{
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step1 = new TeachModeStep(1);
			step1.markName = "teach_3_1";

			TeachModeAllowContent content1_1 = new TeachModeAllowContent();
			content1_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SPACE_TOUCH;
			step1.addAllowContent(content1_1);
			
			step1.showMessageId = 27;
			step1.showMessagePos = new Vector3(0, -240, 0);
			step1.teachFinger.fingerType = Config.TEACH_MODE_FINGER_ARROW;
			step1.teachFinger.fingerPosition = new Vector3(15, -374, 1);
			step1.teachFinger.fingerRotateAngle = 270;
			
			teachModeSteps.Add(step1);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step2 = new TeachModeStep(2);
			step2.markName = "teach_3_2";
			
			TeachModeAllowContent content2_1 = new TeachModeAllowContent();
			content2_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_PRESS_FREE_MOVE_BTN;
			step2.addAllowContent(content2_1);
			
			step2.showMessageId = 28;
			step2.showMessagePos = new Vector3(0, -240, 0);
			step2.teachFinger.fingerType = Config.TEACH_MODE_FINGER_TOUCH;
			step2.teachFinger.fingerPosition = new Vector3(59, -450, 1);
			step2.teachFinger.fingerRotateAngle = 270;
			
			teachModeSteps.Add(step2);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step3 = new TeachModeStep(3);
			step3.markName = "teach_3_3";
			
			TeachModeAllowContent content3_1 = new TeachModeAllowContent();
			content3_1.allowActionSlotIndex = 33;
			content3_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_TOUCH;
			step3.addAllowContent(content3_1);
			
			step3.showMessageId = 29;
			step3.showMessagePos = new Vector3(0, 84, 0);
			step3.teachFinger.fingerType = Config.TEACH_MODE_FINGER_TOUCH;
			step3.teachFinger.fingerPosition = new Vector3(81, -85, 1);
			step3.teachFinger.fingerRotateAngle = 0;
			
			teachModeSteps.Add(step3);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step4 = new TeachModeStep(4);
			step4.markName = "teach_3_4";
			
			TeachModeAllowContent content4_1 = new TeachModeAllowContent();
			content4_1.allowActionSlotIndex = 58;
			content4_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_TOUCH;
			step4.addAllowContent(content4_1);
			
			step4.showMessageId = 30;
			step4.showMessagePos = new Vector3(0, 269, 0);
			step4.teachFinger.fingerType = Config.TEACH_MODE_FINGER_TOUCH;
			step4.teachFinger.fingerPosition = new Vector3(-49, 101, 1);
			step4.teachFinger.fingerRotateAngle = 0;
			
			teachModeSteps.Add(step4);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step5 = new TeachModeStep(5);
			step5.markName = "teach_all";
			
			TeachModeAllowContent content5_1 = new TeachModeAllowContent();
			content5_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SPACE_TOUCH;
			step5.addAllowContent(content5_1);
			
			step5.showMessageId = 31;
			step5.showMessagePos = new Vector3(0, 84, 0);
			
			teachModeSteps.Add(step5);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step6 = new TeachModeStep(6);
			step6.markName = "teach_3_5";
			
			TeachModeAllowContent content6_1 = new TeachModeAllowContent();
			content6_1.allowActionSlotIndex = 39;
			content6_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_RIGHT;
			step6.addAllowContent(content6_1);
			
			TeachModeAllowContent content6_2 = new TeachModeAllowContent();
			content6_2.allowActionSlotIndex = 40;
			content6_2.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_LEFT;
			step6.addAllowContent(content6_2);
			
			step6.showMessageId = 32;
			step6.showMessagePos = new Vector3(0, 195, 0);
			step6.teachFinger.fingerType = Config.TEACH_MODE_FINGER_SWIPE;
			step6.teachFinger.fingerPosition = new Vector3(-15, -124, 1);
			step6.teachFinger.fingerRotateAngle = 180;
			
			teachModeSteps.Add(step6);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step7 = new TeachModeStep(7);
			step7.markName = "teach_3_6";
			
			TeachModeAllowContent content7_1 = new TeachModeAllowContent();
			content7_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_PRESS_BOMB_DIRECT_BTN;
			step7.addAllowContent(content7_1);
			
			step7.showMessageId = 33;
			step7.showMessagePos = new Vector3(0, -240, 0);
			step7.teachFinger.fingerType = Config.TEACH_MODE_FINGER_TOUCH;
			step7.teachFinger.fingerPosition = new Vector3(143, -452, 1);
			step7.teachFinger.fingerRotateAngle = 180;
			
			teachModeSteps.Add(step7);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step8 = new TeachModeStep(8);
			step8.markName = "teach_3_7";
			
			TeachModeAllowContent content8_1 = new TeachModeAllowContent();
			content8_1.allowActionSlotIndex = 40;
			content8_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_TOUCH;
			step8.addAllowContent(content8_1);
			
			step8.showMessageId = 34;
			step8.showMessagePos = new Vector3(0, 110, 0);
			step8.teachFinger.fingerType = Config.TEACH_MODE_FINGER_TOUCH;
			step8.teachFinger.fingerPosition = new Vector3(-58, -25, 1);
			step8.teachFinger.fingerRotateAngle = 0;
			
			teachModeSteps.Add(step8);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step9 = new TeachModeStep(9);
			step9.markName = "teach_3_8";
			
			TeachModeAllowContent content9_1 = new TeachModeAllowContent();
			content9_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_PRESS_BOMB_HORIZONTAL_BTN;
			step9.addAllowContent(content9_1);
			
			step9.showMessageId = 35;
			step9.showMessagePos = new Vector3(0, 167, 0);
			step9.teachFinger.fingerType = Config.TEACH_MODE_FINGER_TOUCH;
			step9.teachFinger.fingerPosition = new Vector3(-56, 25, 1);
			step9.teachFinger.fingerRotateAngle = 0;
			
			teachModeSteps.Add(step9);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step10 = new TeachModeStep(10);
			step10.markName = "teach_3_7";
			
			TeachModeAllowContent content10_1 = new TeachModeAllowContent();
			content10_1.allowActionSlotIndex = 40;
			content10_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_TOUCH;
			step10.addAllowContent(content10_1);
			
			step10.showMessageId = 36;
			step10.showMessagePos = new Vector3(0, 110, 0);
			step10.teachFinger.fingerType = Config.TEACH_MODE_FINGER_TOUCH;
			step10.teachFinger.fingerPosition = new Vector3(-58, -25, 1);
			step10.teachFinger.fingerRotateAngle = 0;
			
			teachModeSteps.Add(step10);
			////////////////////////////////////////////////////////////////////////////////
			TeachModeStep step11 = new TeachModeStep(11);
			step11.markName = "teach_all";
			
			TeachModeAllowContent content11_1 = new TeachModeAllowContent();
			content11_1.allowActionType = Config.TEACH_MODE_ALLOW_ACIOTN_SPACE_TOUCH;
			step11.addAllowContent(content11_1);
			
			step11.showMessageId = 37;
			step11.showMessagePos = new Vector3(0, 84, 0);
			
			teachModeSteps.Add(step11);
		}
	}

	public void doNextTeachStep()
	{
		if(isWaitForNextTeachStep)
		{
			return;
		}

		TeachModeStep preStep = getNowTeachStep ();

		if(preStep != null)
		{

		}

		nowTeachModeStep++;

		TeachModeStep step = getNowTeachStep ();

		if(step != null)
		{
		//	int messageId = step.showMessageId;
			showFinger(step);

			if(bigStageNum == 1 &&
			   smallStageNum == 3)
			{
				if(step.stepIndex == 9)
				{
					m_blockManager.enableAllTouchObj(false);
				}
			}
		}
		else
		{
			if(fingerSprite)
			{
				fingerSprite.gameObject.SetActive(false);
			}

			if(teachMark)
			{
				teachMark.gameObject.SetActive(false);
			}

			hideMessage();

			handleGameOver();
		}

		isWaitForNextTeachStep = false;
	}

	void showFinger(TeachModeStep Step)
	{
		teachMark.spriteName = Step.markName;

		StopCoroutine ("startShiningTeachArrowIn");
		StopCoroutine ("startShiningTeachArrowOut");

		int messageId = Step.showMessageId;
		Vector3 pos = Step.showMessagePos;
		
		addFixMessageById_Pos(messageId, pos);

		fingerSprite.gameObject.SetActive(true);
		fingerSprite.alpha = 1;
		fingerSprite.gameObject.transform.localPosition = Step.teachFinger.fingerPosition;
		int angle = Step.teachFinger.fingerRotateAngle;

		if(Step.teachFinger.fingerType == Config.TEACH_MODE_FINGER_ARROW)
		{
			CPDebug.Log("showFinger = " + angle);
			fingerSprite.spriteName = "swipeArrow";
			fingerSprite.transform.Rotate(0, 0, angle);

			UISpriteAnimation spriteAnimation = fingerSprite.GetComponent<UISpriteAnimation>();
			
			if(spriteAnimation)
			{
				spriteAnimation.enabled = false;
			}

			fingerSprite.transform.localScale = new Vector3(121, 121, 1);

			StartCoroutine("startShiningTeachArrowOut", fingerSprite);
		}
		else if(Step.teachFinger.fingerType == Config.TEACH_MODE_FINGER_TOUCH)
		{
			fingerSprite.spriteName = "tapAnimation_1";
			fingerSprite.transform.localScale = new Vector3(121, 121, 1);

			UISpriteAnimation spriteAnimation = fingerSprite.GetComponent<UISpriteAnimation>();
			
			if(spriteAnimation)
			{
				spriteAnimation.enabled = true;
				spriteAnimation.gameObject.transform.Rotate(0, 0, angle);
				spriteAnimation.SetScale(121, 121, 1);
				spriteAnimation.namePrefix = "tapAnimation_";
			}
		}
		else if(Step.teachFinger.fingerType == Config.TEACH_MODE_FINGER_SWIPE)
		{
			fingerSprite.spriteName = "swipeAnimation_1";
			fingerSprite.transform.localScale = new Vector3(200, 107, 1);

			UISpriteAnimation spriteAnimation = fingerSprite.GetComponent<UISpriteAnimation>();

			if(spriteAnimation)
			{
				spriteAnimation.enabled = true;
				spriteAnimation.gameObject.transform.Rotate(0, 0, angle);
				spriteAnimation.SetScale(200, 107, 1);
				spriteAnimation.namePrefix = "swipeAnimation_";
			}
		}
		else
		{
			fingerSprite.gameObject.SetActive(false);
		}
	}

	private float m_shingingPeriod = 1.0f;
	IEnumerator startShiningTeachArrowIn(Object Obj)
	{
		UISprite sprite = (UISprite)Obj;

		float TmpAlpha = 0.0f;
		
		while( TmpAlpha <= 1.0f )
		{
			TmpAlpha += 1.0f / m_shingingPeriod*Time.deltaTime;
			sprite.alpha = TmpAlpha;
			
			yield return null;
		}

		StartCoroutine ("startShiningTeachArrowOut", Obj);

		yield return new WaitForSeconds( m_showTime );
	}

	IEnumerator startShiningTeachArrowOut(Object Obj)
	{
		UISprite sprite = (UISprite)Obj;
		
		float TmpAlpha = 1.0f;
		
		while( TmpAlpha >= 0 )
		{
			TmpAlpha -= 1.0f / m_shingingPeriod*Time.deltaTime;
			sprite.alpha = TmpAlpha;
			
			yield return null;
		}

		StartCoroutine ("startShiningTeachArrowIn", Obj);
		
		yield return new WaitForSeconds( m_showTime );
	}

	public TeachModeStep getNowTeachStep()
	{
		for(int i = 0; i < teachModeSteps.Count; i++)
		{
			TeachModeStep step = teachModeSteps[i];

			if(step == null)
			{
				continue;
			}

			if(step.stepIndex == nowTeachModeStep)
			{
				return step;
			}
		}

		return null;
	}

	// on Btn Click
	public void OnBtnClick()
	{
/*		CPDebug.Log("OnBtnClick start");
		m_btnObj.transform.localScale = new Vector3( TestNum,TestNum,TestNum);
		m_label.text = "yanzu!!";
		m_img.spriteName = "UI_InGame_BtnPause_press";
		m_img.MakePixelPerfect();

		for( int i = 0 ; i < m_TmpObjList.Count ; i ++ )
			DestroyImmediate(m_TmpObjList[i]);*/
		//m_label.gameObject.SetActive( false );
	}

	public void updateOwnCoins()
	{
		int nMoney = m_blockManager.getOwnCoins ();
		m_Moneylabel.text = nMoney.ToString ();
	}

	IEnumerator MyUpdate()
	{
		while(true)
		{
			// process 
//			CPDebug.Log("MyUpdate tim = " + Time.time);
			yield return new WaitForSeconds( 1.0f);
		}
	}

	/// <summary>
	/// Btn Click Area
	/// </summary>
	private Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]>();
	public void OnTouchToolBtn()
	{
		if(m_blockManager.gameOver)
		{
			return;
		}
		
		if(isShowTip)
		{
			return;
		}

		if(isTeachMode)
		{
			return;
		}

		if (m_IsShowFieldItems)
		{
			showFieldButtons(false);
		}
		else
		{
			showFieldButtons(true);
		}
	}

	void Callback(FBResult result)
	{
//		lastResponseTexture = null;
//		if (result.Error != null)
//			lastResponse = "Error Response:\n" + result.Error;
//		else if (!ApiQuery.Contains("/picture"))
//			lastResponse = "Success Response:\n" + result.Text;
//		else
//		{
//			lastResponseTexture = result.Texture;
//			lastResponse = "Success Response:\n";
//		}
	}

	public void showFieldButtons(bool show)
	{
		m_blockManager.enableAllTouchObj (!show);

		m_BomberBtn.gameObject.SetActive (show);
		m_ShovelBtn.gameObject.SetActive (show);
		m_LigntingBtn.gameObject.SetActive (show);
		m_EraseKnifeBtn.gameObject.SetActive (show);
		m_BombTwiceBtn.gameObject.SetActive (show);
		m_BombUpgradeBtn.gameObject.SetActive (show);

		m_IsShowFieldItems = show;

		if(show)
		{
			int bomberNum = m_blockManager.getFieldItemNum((int)Config.Operate_Mode_Enum.Operate_Mode_Bomber);
			m_BomberBtnlabel.text = bomberNum.ToString();
			if(bomberNum == 0) m_BomberBtnlabel.color = Color.red; else m_BomberBtnlabel.color = Color.white;
			GameObject BomberChild = DataManager.Instance.FindChild(m_BomberBtn.gameObject, "UnlockMark");
			if(m_blockManager.isLockItem((int)Config.Operate_Mode_Enum.Operate_Mode_Bomber))
			{
				if(BomberChild) BomberChild.SetActive(true);
			}
			else
			{
				if(BomberChild) BomberChild.SetActive(false);
			}

			int shovelNum = m_blockManager.getFieldItemNum((int)Config.Operate_Mode_Enum.Operate_Mode_Shovel);
			m_ShovelBtnlabel.text = shovelNum.ToString();
			if(shovelNum == 0) m_ShovelBtnlabel.color = Color.red; else m_ShovelBtnlabel.color = Color.white;
			GameObject ShovelChild = DataManager.Instance.FindChild(m_ShovelBtn.gameObject, "UnlockMark");
			if(m_blockManager.isLockItem((int)Config.Operate_Mode_Enum.Operate_Mode_Shovel))
			{
				if(ShovelChild) ShovelChild.SetActive(true);
			}
			else
			{
				if(ShovelChild) ShovelChild.SetActive(false);
			}

			int lightingNum = m_blockManager.getFieldItemNum((int)Config.Operate_Mode_Enum.Operate_Mode_Lignting);
			m_LigntingBtnlabel.text = lightingNum.ToString();
			if(lightingNum == 0) m_LigntingBtnlabel.color = Color.red; else m_LigntingBtnlabel.color = Color.white;
			GameObject LigntingChild = DataManager.Instance.FindChild(m_LigntingBtn.gameObject, "UnlockMark");
			if(m_blockManager.isLockItem((int)Config.Operate_Mode_Enum.Operate_Mode_Lignting))
			{
				if(LigntingChild) LigntingChild.SetActive(true);
			}
			else
			{
				if(LigntingChild) LigntingChild.SetActive(false);
			}

			int upgradeNum = m_blockManager.getFieldItemNum((int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Upgrade);
			m_BombUpgradeBtnlabel.text = upgradeNum.ToString();
			if(upgradeNum == 0) m_BombUpgradeBtnlabel.color = Color.red; else m_BombUpgradeBtnlabel.color = Color.white;

			int twiceNum = m_blockManager.getFieldItemNum((int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Twice);
			m_BombTwiceBtnlabel.text = twiceNum.ToString();
			if(twiceNum == 0) m_BombTwiceBtnlabel.color = Color.red; else m_BombTwiceBtnlabel.color = Color.white;

			int eraseKnifeNum = m_blockManager.getFieldItemNum((int)Config.Operate_Mode_Enum.Operate_Mode_Erase_Knife);
			m_EraseKnifeBtnlabel.text = eraseKnifeNum.ToString();
			if(eraseKnifeNum == 0) m_EraseKnifeBtnlabel.color = Color.red; else m_EraseKnifeBtnlabel.color = Color.white;
		}
	}

	public void updatePlayInfo(int Type, int Num)
	{
		Config.Player_Info_Enum enumType = (Config.Player_Info_Enum)Type;

		switch(enumType)
		{
		case Config.Player_Info_Enum.Player_Info_Left_Move_Num:
		{
			if(Num < 0)
			{
				Num = 0;
			}

			m_NowLeftMoveNumlabel.text = Num.ToString();

			if(Num > 0 &&
			   Num <= 5)
			{
				m_NowLeftMoveNumlabel.color = Color.red;
			}
			else
			{
				m_NowLeftMoveNumlabel.color = Color.white;
			}
		}break;
		case Config.Player_Info_Enum.Player_Info_Free_Fall_Num:
		{
			m_NowFreeMoveNumlabel.text = Num.ToString();
		}break;
		case Config.Player_Info_Enum.Player_Info_Bombed_Block_Num:
		{
			m_NowBombNumlabel.text = Num.ToString();
		}break;
		}
	}

	public void OnTouchFreeMoveBtn()
	{
		if(m_blockManager.gameOver)
		{
			return;
		}

		if(isShowTip)
		{
			return;
		}

		int Mode = (int)Config.Operate_Mode_Enum.Operate_Mode_Free_Move;
		enterOperateMode (Mode);
	}

	public void OnTouchBombDirectBtn()
	{
		if(m_blockManager.gameOver)
		{
			return;
		}
		
		if(isShowTip)
		{
			return;
		}

		int Mode = (int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Direct;
		enterOperateMode (Mode);
	}

	public void OnGameOptionBtn()
	{
		CPDebug.Log("OnGameOptionBtn start");
		PopupManager.Instance.ShowGameStarOptionUI(StartOptionPopUI.OptionUIType.GamePlay, this);
	}

	public void OnTipBtn()
	{
		CPDebug.Log("OnTipBtn start");
		
	}

	public void OnBomberBtn()
	{
		int Mode = (int)Config.Operate_Mode_Enum.Operate_Mode_Bomber;
		enterOperateMode (Mode);
	}

	public void OnShovelBtn()
	{
		int Mode = (int)Config.Operate_Mode_Enum.Operate_Mode_Shovel;
		enterOperateMode (Mode);
	}

	public void OnLigntingBtn()
	{
		int Mode = (int)Config.Operate_Mode_Enum.Operate_Mode_Lignting;
		enterOperateMode (Mode);
	}

	public void OnEraseKnifeBtn()
	{
		int Mode = (int)Config.Operate_Mode_Enum.Operate_Mode_Erase_Knife;
		enterOperateMode (Mode);
	}

	public void OnBombTwiceBtn()
	{
		int Mode = (int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Twice;
		enterOperateMode (Mode);
	}

	public void OnBombUpgradeBtn()
	{
		int Mode = (int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Upgrade;
		enterOperateMode (Mode);
	}

	public void enterOperateMode(int Mode)
	{
		if(isTeachMode)
		{
			if(Mode == (int)Config.Operate_Mode_Enum.Operate_Mode_Free_Move)
			{
				TeachModeStep step = getNowTeachStep();

				if(step != null)
				{
					bool isAllow = step.isAllow(0, Config.TEACH_MODE_ALLOW_ACIOTN_PRESS_FREE_MOVE_BTN);

					if(!isAllow)
					{
						return;
					}

					doNextTeachStep();
				}
			}
			else if(Mode == (int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Direct)
			{
				TeachModeStep step = getNowTeachStep();
				
				if(step != null)
				{
					bool isAllow = step.isAllow(0, Config.TEACH_MODE_ALLOW_ACIOTN_PRESS_BOMB_DIRECT_BTN);
					
					if(!isAllow)
					{
						return;
					}
					
					doNextTeachStep();
				}
			}
			else
			{
				return;
			}
		}

		showFieldButtons (false);
		m_blockManager.enterOperateMode (Mode);
	}

	public int getStageInfoAsInt(string ColumnName)
	{
		int key = DataManager.Instance.StageKey;
		return CSVManager.Instance.StageInfoTable.readFieldAsInt (key, ColumnName);
	}

	public string getStageInfoAsString(string ColumnName)
	{
		int key = DataManager.Instance.StageKey;
		return CSVManager.Instance.StageInfoTable.readFieldAsString (key, ColumnName);
	}

	public void updateToNowTargetPage()
	{
		if(isChallengMode)
		{
			return;
		}

		Config.Pass_Stage_Record_Enum enumType = (Config.Pass_Stage_Record_Enum)stage_Pass_Record;

		switch(enumType)
		{
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Null:
		{
			updateTargets((int)Config.Pass_Stage_Enum.Pass_Stage_Star_1, true);
		}break;
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_1:
		{
			updateTargets((int)Config.Pass_Stage_Enum.Pass_Stage_Star_2, true);
		}break;
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_2:
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_3:
		{
			updateTargets((int)Config.Pass_Stage_Enum.Pass_Stage_Star_3, true);
		}break;
		}
	}

	public void targetOneStarPress()
	{
		updateTargets((int)Config.Pass_Stage_Enum.Pass_Stage_Star_1, true);
	}

	public void targetTwoStarPress()
	{
		updateTargets((int)Config.Pass_Stage_Enum.Pass_Stage_Star_2, true);
	}

	public void targetThreeStarPress()
	{
		updateTargets((int)Config.Pass_Stage_Enum.Pass_Stage_Star_3, true);
	}

	public void updateTargets(int StarNum, bool FromPress)
	{
		if(FromPress)
		{
			if(stageSelectTag == StarNum)
			{
				return;
			}
		}

		if(stageSelectTag != StarNum)
		{
			StageTarget1.SetActive (false);
			StageTarget2.SetActive (false);
			StageTarget3.SetActive (false);
			StageTarget4.SetActive (false);
		}

		stageSelectTag = StarNum;
		changeStageTargetMainBk (StarNum);
		changeStageTargetStarImage (stage_Pass_Record);

		int targetAskNum = 0;
		int nowHaveNum = 0;
		int targetType = 0;
		int nowUseIndex = 1;
		bool drawLockTarget = false;

		Config.Pass_Stage_Enum enumType = (Config.Pass_Stage_Enum)StarNum;

		for(int i = 0; i < 4; i++)
		{
			switch(i)
			{
			case 0:
			{
				targetType = getStageInfoAsInt("FruitTarget1");
				if(targetType == 0)
				{
					continue;
				}

				switch(enumType)
				{
				case Config.Pass_Stage_Enum.Pass_Stage_Star_1:{targetAskNum = getStageInfoAsInt("FruitTarget1_Star1_Num");}break;
				case Config.Pass_Stage_Enum.Pass_Stage_Star_2:{targetAskNum = getStageInfoAsInt("FruitTarget1_Star2_Num");}break;
				case Config.Pass_Stage_Enum.Pass_Stage_Star_3:{targetAskNum = getStageInfoAsInt("FruitTarget1_Star3_Num");}break;
				}

				if(targetAskNum == 0 &&
				   (getStageInfoAsInt("FruitTarget1_Star1_Num") != 0 ||
					 getStageInfoAsInt("FruitTarget1_Star2_Num") != 0 ||
					 getStageInfoAsInt("FruitTarget1_Star3_Num") != 0))
				{
					drawLockTarget = true;
				}

				nowHaveNum = getFruitOwnNum(targetType);
				targetType += 4;
			}break;
			case 1:
			{
				targetType = getStageInfoAsInt("FruitTarget2");
				if(targetType == 0)
				{
					continue;
				}
				
				switch(enumType)
				{
				case Config.Pass_Stage_Enum.Pass_Stage_Star_1:{targetAskNum = getStageInfoAsInt("FruitTarget2_Star1_Num");}break;
				case Config.Pass_Stage_Enum.Pass_Stage_Star_2:{targetAskNum = getStageInfoAsInt("FruitTarget2_Star2_Num");}break;
				case Config.Pass_Stage_Enum.Pass_Stage_Star_3:{targetAskNum = getStageInfoAsInt("FruitTarget2_Star3_Num");}break;
				}
				
				if(targetAskNum == 0 &&
				   (getStageInfoAsInt("FruitTarget2_Star1_Num") != 0 ||
				 getStageInfoAsInt("FruitTarget2_Star2_Num") != 0 ||
				 getStageInfoAsInt("FruitTarget2_Star3_Num") != 0))
				{
					drawLockTarget = true;
				}
				
				nowHaveNum = getFruitOwnNum(targetType);
				targetType += 4;
			}break;
			case 2:
			{
				targetType = getStageInfoAsInt("SpecialTarget1");
				if(targetType == 0)
				{
					continue;
				}
				
				switch(enumType)
				{
				case Config.Pass_Stage_Enum.Pass_Stage_Star_1:{targetAskNum = getStageInfoAsInt("SpecialTarget1_Star1");}break;
				case Config.Pass_Stage_Enum.Pass_Stage_Star_2:{targetAskNum = getStageInfoAsInt("SpecialTarget1_Star2");}break;
				case Config.Pass_Stage_Enum.Pass_Stage_Star_3:{targetAskNum = getStageInfoAsInt("SpecialTarget1_Star3");}break;
				}
				
				if(targetAskNum == 0 &&
				   (getStageInfoAsInt("SpecialTarget1_Star1") != 0 ||
				 getStageInfoAsInt("SpecialTarget1_Star2") != 0 ||
				 getStageInfoAsInt("SpecialTarget1_Star3") != 0))
				{
					drawLockTarget = true;
				}
				
				nowHaveNum = m_blockManager.getRecordNum(targetType);
			}break;
			case 3:
			{
				targetType = getStageInfoAsInt("SpecialTarget2");
				if(targetType == 0)
				{
					continue;
				}
				
				switch(enumType)
				{
				case Config.Pass_Stage_Enum.Pass_Stage_Star_1:{targetAskNum = getStageInfoAsInt("SpecialTarget2_Star1");}break;
				case Config.Pass_Stage_Enum.Pass_Stage_Star_2:{targetAskNum = getStageInfoAsInt("SpecialTarget2_Star2");}break;
				case Config.Pass_Stage_Enum.Pass_Stage_Star_3:{targetAskNum = getStageInfoAsInt("SpecialTarget2_Star3");}break;
				}
				
				if(targetAskNum == 0 &&
				   (getStageInfoAsInt("SpecialTarget2_Star1") != 0 ||
				 getStageInfoAsInt("SpecialTarget2_Star2") != 0 ||
				 getStageInfoAsInt("SpecialTarget2_Star3") != 0))
				{
					drawLockTarget = true;
				}
				
				nowHaveNum = m_blockManager.getRecordNum(targetType);
			}break;
			}

			if(targetAskNum == 0)
			{
				if(drawLockTarget)
				{
					coverStageTarget(nowUseIndex);
					nowUseIndex++;
				}

				continue;
			}

			changeStageTarget(nowUseIndex, targetType, nowHaveNum, targetAskNum);
			nowUseIndex++;
		}
	}

	public void coverStageTarget(int Index)
	{
		GameObject targetObj = null;
		
		switch(Index)
		{
		case 1:{targetObj = StageTarget1;}break;
		case 2:{targetObj = StageTarget2;}break;
		case 3:{targetObj = StageTarget3;}break;
		case 4:{targetObj = StageTarget4;}break;
		default:{return;}
		}

		targetObj.SetActive (true);

		GameObject spriteChild = DataManager.Instance.FindChild(targetObj, "TargetSprite");
		
		if(spriteChild)
		{
			UISprite sprite = spriteChild.GetComponent<UISprite>();
			
			if(sprite)
			{
				sprite.spriteName = "UI_InGame_Special_Target";
				sprite.transform.localScale = new Vector3(54, 51, 0);
			}
		}

		GameObject labelChild = DataManager.Instance.FindChild(targetObj, "NumLabel");
		
		if(labelChild)
		{
			labelChild.SetActive(false);
		}

		GameObject checkSpriteChild = DataManager.Instance.FindChild(targetObj, "FinishSprite");
		
		if(checkSpriteChild)
		{
			checkSpriteChild.SetActive(false);
		}
	}

	public void changeStageTarget(int Index, int Type, int MinNum, int MaxNum)
	{
		string fileName = null;

		Config.Player_Info_Enum enumType = (Config.Player_Info_Enum)Type;

		bool needSmall = false;

		switch(enumType)
		{
		case Config.Player_Info_Enum.Player_Info_Score:{fileName = "UI_InGame_Target_Score";}break;
		case Config.Player_Info_Enum.Player_Info_Free_Fall_Num:{fileName = "UI_InGame_Target_Free_Fall";}break;
		case Config.Player_Info_Enum.Player_Info_Bombed_Block_Num:{fileName = "UI_InGame_Target_Total_Bomb";}break;
		case Config.Player_Info_Enum.Player_Info_Gold_Coin:{fileName = "UI_InGame_Target_Coin";}break;
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_1_Num:{fileName = "product_1"; needSmall = true;}break;
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_2_Num:{fileName = "product_2"; needSmall = true;}break;
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_3_Num:{fileName = "product_3"; needSmall = true;}break;
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_4_Num:{fileName = "product_4"; needSmall = true;}break;
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_5_Num:{fileName = "product_5"; needSmall = true;}break;
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_6_Num:{fileName = "product_6"; needSmall = true;}break;
		case Config.Player_Info_Enum.Player_Info_Fruit_1_Num:{fileName = "fruit_1"; needSmall = true;}break;
		case Config.Player_Info_Enum.Player_Info_Fruit_2_Num:{fileName = "fruit_2"; needSmall = true;}break;
		case Config.Player_Info_Enum.Player_Info_Fruit_3_Num:{fileName = "fruit_3"; needSmall = true;}break;
		case Config.Player_Info_Enum.Player_Info_Fruit_4_Num:{fileName = "fruit_4"; needSmall = true;}break;
		case Config.Player_Info_Enum.Player_Info_Fruit_5_Num:{fileName = "fruit_5"; needSmall = true;}break;
		case Config.Player_Info_Enum.Player_Info_Fruit_6_Num:{fileName = "fruit_6"; needSmall = true;}break;
		case Config.Player_Info_Enum.Player_Info_Use_Fruit_Bomb_Num:{fileName = "UI_InGame_Target_Fruit_Bomb";}break;
		case Config.Player_Info_Enum.Player_Info_Use_Small_Bomb_Num:{fileName = "UI_InGame_Target_Small_Bomb";}break;
		case Config.Player_Info_Enum.Player_Info_Use_Big_Bomb_Num:{fileName = "UI_InGame_Target_Super_Bomb";}break;
		default:{return;}
		}

		GameObject targetObj = null;

		switch(Index)
		{
		case 1:{targetObj = StageTarget1;}break;
		case 2:{targetObj = StageTarget2;}break;
		case 3:{targetObj = StageTarget3;}break;
		case 4:{targetObj = StageTarget4;}break;
		default:{return;}
		}

		targetObj.SetActive (true);

		GameObject spriteChild = DataManager.Instance.FindChild(targetObj, "TargetSprite");
		spriteChild.SetActive (true);

		if(spriteChild)
		{
			UISprite sprite = spriteChild.GetComponent<UISprite>();
			
			if(sprite)
			{
				sprite.spriteName = fileName;

				if(needSmall)
				{
					sprite.transform.localScale = new Vector3(42, 42, 0);
				}
				else
				{
					sprite.transform.localScale = new Vector3(54, 51, 0);
				}
			}
		}
		
		GameObject labelChild = DataManager.Instance.FindChild(targetObj, "NumLabel");
		labelChild.SetActive (true);

		if(labelChild)
		{
			UILabel label = labelChild.GetComponent<UILabel>();
			
			if(label)
			{
				string content = string.Format("{0}/{1}", MinNum, MaxNum);
				label.text = content;
			}
		}

		GameObject checkSpriteChild = DataManager.Instance.FindChild(targetObj, "FinishSprite");

		if(checkSpriteChild)
		{
			if(MinNum >= MaxNum)
			{
				checkSpriteChild.SetActive(true);
			}
			else
			{
				checkSpriteChild.SetActive(false);
			}
		}
	}

	public void changeStageTargetMainBk(int StarNum)
	{
		GameObject spriteChild = DataManager.Instance.FindChild(StageTargetMainBk, "TargetStarBtn_Bg");
		
		if(spriteChild)
		{
			UISprite sprite = spriteChild.GetComponent<UISprite>();

			if(sprite)
			{
				if(StarNum == (int)Config.Pass_Stage_Enum.Pass_Stage_Star_1)
				{
					sprite.spriteName = "UI_InGame_1_star_bk";
				}
				else if(StarNum == (int)Config.Pass_Stage_Enum.Pass_Stage_Star_2)
				{
					sprite.spriteName = "UI_InGame_2_star_bk";
				}
				else
				{
					sprite.spriteName = "UI_InGame_3_star_bk";
				}
			}
		}
	}

	public void changeStageTargetStarImage(int StarNum)
	{
		GameObject oneStarObj1 = DataManager.Instance.FindChild(StageTargetMainBk, "OneStarSprite");
		GameObject oneStarObj2 = DataManager.Instance.FindChild(StageTargetMainBk, "TwoStarSprite");
		GameObject oneStarObj3 = DataManager.Instance.FindChild(StageTargetMainBk, "ThreeStarSprite");
		
		if(!oneStarObj1 ||
		   !oneStarObj2 ||
		   !oneStarObj3)
		{
			return;
		}

		UISprite sprite1 = oneStarObj1.GetComponent<UISprite>();
		UISprite sprite2 = oneStarObj2.GetComponent<UISprite>();
		UISprite sprite3 = oneStarObj3.GetComponent<UISprite>();

		if(!sprite1 ||
		   !sprite2 ||
		   !sprite3)
		{
			return;
		}

		Config.Pass_Stage_Record_Enum enumType = (Config.Pass_Stage_Record_Enum)StarNum;

		switch(enumType)
		{
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Null:
		{
			sprite1.spriteName = "UI_InGame_oneStar_Gray";
			sprite2.spriteName = "UI_InGame_twoStar_Gray";
			sprite3.spriteName = "UI_InGame_threeStar_Gray";
		}break;
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_1:
		{
			sprite1.spriteName = "UI_InGame_oneStar_Yellow";
			sprite2.spriteName = "UI_InGame_twoStar_Gray";
			sprite3.spriteName = "UI_InGame_threeStar_Gray";
		}break;
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_2:
		{
			sprite1.spriteName = "UI_InGame_oneStar_Yellow";
			sprite2.spriteName = "UI_InGame_twoStar_Yellow";
			sprite3.spriteName = "UI_InGame_threeStar_Gray";
		}break;
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_3:
		{
			sprite1.spriteName = "UI_InGame_oneStar_Yellow";
			sprite2.spriteName = "UI_InGame_twoStar_Yellow";
			sprite3.spriteName = "UI_InGame_threeStar_Yellow";
		}break;
		}
	}

	public int getFruitOwnNum(int Type)
	{
		if(Type > 6 ||
		   Type < 1)
		{
			return 0;
		}
		
		int realIndex = Type + (int)Config.Player_Info_Enum.Player_Info_Fruit_1_Num - 1;
		return m_blockManager.getRecordNum (realIndex);
	}

	public void checkStagePass()
	{
		if(isChallengMode)
		{
			return;
		}

		if(stage_Pass_Record >= (int)Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_3)
		{
			updateTargets(stageSelectTag, false);
			return;
		}

		while(checkStarLevelUp())
		{
			stage_Pass_Record++;
		}

		Config.Pass_Stage_Record_Enum enumType = (Config.Pass_Stage_Record_Enum)stage_Pass_Record;
		switch(enumType)
		{
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Null:{stageSelectTag = (int)Config.Pass_Stage_Enum.Pass_Stage_Star_1;}break;
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_1:{stageSelectTag = (int)Config.Pass_Stage_Enum.Pass_Stage_Star_2;}break;
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_2:
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_3:{stageSelectTag = (int)Config.Pass_Stage_Enum.Pass_Stage_Star_3;}break;
		}

		updateTargets (stageSelectTag, false);
	}

	public bool checkStarLevelUp()
	{
		if(stage_Pass_Record >= (int)Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_3)
		{
			return false;
		}

		int fruitType1 = getStageInfoAsInt("FruitTarget1");
		int fruitNeedNum1 = 0;
		int fruitType2 = getStageInfoAsInt("FruitTarget2");
		int fruitNeedNum2 = 0;
		int specialType1 = getStageInfoAsInt("SpecialTarget1");
		int specialNeedNum1 = 0;
		int specialType2 = getStageInfoAsInt("SpecialTarget2");
		int specialNeedNum2 = 0;

		Config.Pass_Stage_Record_Enum enumType = (Config.Pass_Stage_Record_Enum)stage_Pass_Record;
		switch(enumType)
		{
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Null:
		{
			if(fruitType1 != 0)
			{
				fruitNeedNum1 = getStageInfoAsInt("FruitTarget1_Star1_Num");
			}
			
			if(fruitType2 != 0)
			{
				fruitNeedNum2 = getStageInfoAsInt("FruitTarget2_Star1_Num");
			}
			
			if(specialType1 != 0)
			{
				specialNeedNum1 = getStageInfoAsInt("SpecialTarget1_Star1");
			}
			
			if(specialType2 != 0)
			{
				specialNeedNum2 = getStageInfoAsInt("SpecialTarget2_Star1");
			}
		}break;
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_1:
		{
			if(fruitType1 != 0)
			{
				fruitNeedNum1 = getStageInfoAsInt("FruitTarget1_Star2_Num");
			}
			
			if(fruitType2 != 0)
			{
				fruitNeedNum2 = getStageInfoAsInt("FruitTarget2_Star2_Num");
			}
			
			if(specialType1 != 0)
			{
				specialNeedNum1 = getStageInfoAsInt("SpecialTarget1_Star2");
			}
			
			if(specialType2 != 0)
			{
				specialNeedNum2 = getStageInfoAsInt("SpecialTarget2_Star2");
			}
		}break;
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_2:
		{
			if(fruitType1 != 0)
			{
				fruitNeedNum1 = getStageInfoAsInt("FruitTarget1_Star3_Num");
			}
			
			if(fruitType2 != 0)
			{
				fruitNeedNum2 = getStageInfoAsInt("FruitTarget2_Star3_Num");
			}
			
			if(specialType1 != 0)
			{
				specialNeedNum1 = getStageInfoAsInt("SpecialTarget1_Star3");
			}
			
			if(specialType2 != 0)
			{
				specialNeedNum2 = getStageInfoAsInt("SpecialTarget2_Star3");
			}
		}break;
		default:
		{
			return false;
		}
		}

		bool levelUp = true;
		
		if(fruitType1 != 0)
		{
			int ownFruitNum = getFruitOwnNum(fruitType1);
			
			if(ownFruitNum < fruitNeedNum1)
			{
				levelUp = false;
			}
		}
		
		if(fruitType2 != 0 && levelUp)
		{
			int ownFruitNum = getFruitOwnNum(fruitType2);
			
			if(ownFruitNum < fruitNeedNum2)
			{
				levelUp = false;
			}
		}
		
		if(specialType1 != 0 && levelUp)
		{
			int nowNum = m_blockManager.getRecordNum(specialType1);
			
			if(nowNum < specialNeedNum1)
			{
				levelUp = false;
			}
		}
		
		if(specialType2 != 0 && levelUp)
		{
			int nowNum = m_blockManager.getRecordNum(specialType2);
			
			if(nowNum < specialNeedNum2)
			{
				levelUp = false;
			}
		}
		
		return levelUp;
	}

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.A ) )
		{
			stage_Pass_Record = 3;

			handleGameOver();
		}

		if( Input.GetKeyDown( KeyCode.B ) )
		{
			LocalDBManager.Instance.addOneOwnItem(1, 10);
			LocalDBManager.Instance.addOneOwnItem(2, 10);
			LocalDBManager.Instance.addOneOwnItem(3, 10);
			LocalDBManager.Instance.addOneOwnItem(4, 10);
		}
	}

	public void handleGameOver()
	{
		if(m_blockManager.gameOver)
		{
			return;
		}

		if(!isChallengMode)
		{
			if(stage_Pass_Record != (int)Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Null)
			{
				int bigStage = DataManager.Instance.BigStageNum;
				int smallStage = DataManager.Instance.SmallStageNum;
				int moveScore = m_blockManager.getRecordNum((int)Config.Player_Info_Enum.Player_Info_Free_Fall_Num);
				int bombScore = m_blockManager.getRecordNum((int)Config.Player_Info_Enum.Player_Info_Bombed_Block_Num);

				LocalDBManager.Instance.saveStageStarNum(bigStage, smallStage, stage_Pass_Record, moveScore, bombScore);
			}
		}
		else
		{
			if(isBeChalleng)
			{
				ChallengData challengData = NetWorkManager.Instance.getChallengDataByFBUid(challengFriendFBUid);

				if(challengData != null)
				{
					string myFBUid = NetWorkManager.Instance.myData.PD_FB_Uid;
					int targetScore = challengData.challengScore;
					int scoreType = challengData.challengScoreType;
					int bigStage = challengData.challengBigStage;
					int smallStage = challengData.challengSmallStage;
					int myScore = 0;

					if(scoreType == Config.CHALLENG_SCORE_TYPE_MOVE)
					{
						myScore = m_blockManager.getRecordNum((int)Config.Player_Info_Enum.Player_Info_Free_Fall_Num);
					}
					else
					{
						myScore = m_blockManager.getRecordNum((int)Config.Player_Info_Enum.Player_Info_Bombed_Block_Num);
					}

					if(targetScore > myScore)
					{
						beChallengWin = false;

						NetWorkManager.Instance.parse_sendMailToParse(myFBUid,
						                                              challengFriendFBUid,
						                                              Config.MESSAGE_TYPE_CHALLENG_GIFT,
						                                              scoreType.ToString(),
						                                              myScore.ToString(),
						                                              targetScore.ToString(),
						                                              Config.CHALLENG_WIN_REWARD_ITEM_ID.ToString(),
						                                              Config.CHALLENG_WIN_REWARD_ITEM_NUM.ToString(), 
						                                              bigStage.ToString(), 
						                                              smallStage.ToString());
					}
					else if(myScore > targetScore)
					{
						beChallengWin = true;

						NetWorkManager.Instance.parse_sendMailToParse(challengFriendFBUid,
						                                              myFBUid,
						                                              Config.MESSAGE_TYPE_CHALLENG_GIFT,
						                                              scoreType.ToString(),
						                                              myScore.ToString(),
						                                              targetScore.ToString(),
						                                              Config.CHALLENG_WIN_REWARD_ITEM_ID.ToString(),
						                                              Config.CHALLENG_WIN_REWARD_ITEM_NUM.ToString(), 
						                                              bigStage.ToString(), 
						                                              smallStage.ToString());
					}

					NetWorkManager.Instance.parse_deleteOtherChallengInfo(challengFriendFBUid);
				}
			}
			else
			{
				int scoreType = NetWorkManager.Instance.challengScoreType;
				int score = 0;

				if(scoreType == Config.CHALLENG_SCORE_TYPE_MOVE)
				{
					score = m_blockManager.getRecordNum((int)Config.Player_Info_Enum.Player_Info_Free_Fall_Num);
				}
				else
				{
					score = m_blockManager.getRecordNum((int)Config.Player_Info_Enum.Player_Info_Bombed_Block_Num);
				}

				int bigStage = DataManager.Instance.BigStageNum;
				int smallStage = DataManager.Instance.SmallStageNum;

				NetWorkManager.Instance.parse_SendChalleng(challengFriendFBUid,
				                                           bigStage,
				                                           smallStage,
				                                           scoreType,
				                                           score);
			}
		}

		bool showWinUI = false;

		if(isChallengMode)
		{
			if(!isBeChalleng)
			{
				showWinUI = true;
			}
			else
			{
				if(beChallengWin)
				{
					showWinUI = true;
				}
			}
		}
		else
		{
			if(!isTeachMode)
			{
				if(stage_Pass_Record == (int)Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Null)
				{
					LifeManager.Instance.useLife();
				}
				else
				{
					showWinUI = true;
				}
			}
		}

		if(!isTeachMode)
		{
			if(showWinUI)
			{
				startPlayResultProcess(Config.PLAY_RESULT_LAYER_SUCCESS);
			}
			else
			{
				startPlayResultProcess(Config.PLAY_RESULT_LAYER_FAIL);
			}
		}
		else
		{
			startPlayResultProcess(Config.PLAY_RESULT_LAYER_SUCCESS);
		}

		m_blockManager.gameOver = true;
		m_blockManager.resetShingingTimer ();
	}

	public void startPlayResultProcess(int Type)
	{
		GameResultMainObj.SetActive (true);

		if(Type == Config.PLAY_RESULT_LAYER_SUCCESS)
		{
			StartCoroutine (showFlag(true));
		}
		else
		{
			StartCoroutine (showFlag(false));
		}
	}

	private float m_showTime = 1.5f;
	private float m_splashFadingPeriod = 1.5f;

	IEnumerator showFlag(bool Win)
	{
		UISprite showSprite = null;

		if(Win)
		{
			showSprite = winFlagSprite;
		}
		else
		{
			showSprite = loseFlagSprite;
		}

		// show the logo
		float TmpAlpha = 0.0f;
		showSprite.gameObject.SetActive( true );

		while( TmpAlpha <= 1.0f )
		{
			TmpAlpha += 1.0f / m_splashFadingPeriod*Time.deltaTime;
			showSprite.color = new Color( 1,1,1,TmpAlpha );

			yield return null;
		}

		winFlagSprite.gameObject.SetActive (false);
		loseFlagSprite.gameObject.SetActive (false);

		if(isTeachMode)
		{
			CPDebug.Log("quitGame start");
			DataManager.Instance.m_InGameType = DataManager.InGameType.SmallStage;
			SceneChangeManager.Instance.LoadScene("InGame");
		}
		else if(isChallengMode &&
		        isBeChalleng)
		{
			string mainMsg = "";

			if(beChallengWin)
			{
				mainMsg = Localization.Localize("75");
			}
			else
			{
				mainMsg = Localization.Localize("76");
			}

			string sYes = Localization.Localize("46");

			SceneChangeManager.Instance.LoadScene("InGame");
			PopupManager.Instance.ShowMessageBox_Tip(mainMsg, sYes);

		}
		else if(isChallengMode &&
		        !isBeChalleng)
		{
			string msg = Localization.Localize("77");
			string ok = Localization.Localize("46");
			SceneChangeManager.Instance.LoadScene("InGame");
			PopupManager.Instance.ShowMessageBox_Tip(msg, ok);
		}
		else
		{
			StartCoroutine (__showPlayResultUI(Win));
		}

		// stay
		yield return new WaitForSeconds( m_showTime );
	}

	private float m_showResultPeriod = 1.0f;
	IEnumerator __showPlayResultUI(bool Win)
	{
		GameObject mainBk = getPlayResultObj (Win);
		
		if(!mainBk)
		{
			yield return null;
		}

		UIPanel panel = mainBk.GetComponent<UIPanel> ();

		if(!panel)
		{
			yield return null;
		}

		mainBk.SetActive (true);
		panel.alpha = 0;

		float TmpAlpha = 0.0f;

		while( TmpAlpha <= 1.0f )
		{
			TmpAlpha += 1.0f / m_showResultPeriod*Time.deltaTime;
			panel.alpha = TmpAlpha;
			
			yield return null;
		}

		showPlayResultUI (Win);
		yield return new WaitForSeconds( m_showTime );
	}

	public void showPlayResultUI(bool Win)
	{
		GameObject mainBk = getPlayResultObj (Win);

		if(!mainBk)
		{
			return;
		}

		GameObject blackBackChild = DataManager.Instance.FindChild(GameResultMainObj, "BlackBg");

		if(blackBackChild)
		{
			blackBackChild.SetActive(true);
		}

		fullPlayResultInfo (Win, mainBk);
	}

	public void fullPlayResultInfo(bool Win, GameObject Obj)
	{
		if(!Obj)
		{
			return;
		}

		int bigStage = DataManager.Instance.BigStageNum;
		int smallStage = DataManager.Instance.SmallStageNum;

		if(Win)
		{
			setPlayResultLabelText(Obj, "StarNumLabel", stage_Pass_Record.ToString());
			setPlayResultLabelText(Obj, "MoveScoreLabel", m_blockManager.getRecordNum((int)Config.Player_Info_Enum.Player_Info_Free_Fall_Num).ToString());
			setPlayResultLabelText(Obj, "BombScoreLabel", m_blockManager.getRecordNum((int)Config.Player_Info_Enum.Player_Info_Bombed_Block_Num).ToString());
			setPlayResultLabelText(Obj, "BestMoveScoreLabel", LocalDBManager.Instance.getBestMoveScore(bigStage, smallStage).ToString());
			setPlayResultLabelText(Obj, "BestBombScoreLabel", LocalDBManager.Instance.getBestBombScore(bigStage, smallStage).ToString());
		}
		else
		{
			setPlayResultLabelText(Obj, "MoveScoreLabel", m_blockManager.getRecordNum((int)Config.Player_Info_Enum.Player_Info_Free_Fall_Num).ToString());
			setPlayResultLabelText(Obj, "BombScoreLabel", m_blockManager.getRecordNum((int)Config.Player_Info_Enum.Player_Info_Bombed_Block_Num).ToString());
			setPlayResultLabelText(Obj, "BestMoveScoreLabel", LocalDBManager.Instance.getBestMoveScore(bigStage, smallStage).ToString());
			setPlayResultLabelText(Obj, "BestBombScoreLabel", LocalDBManager.Instance.getBestBombScore(bigStage, smallStage).ToString());
		}

		setPlayResultStarNum (Obj, stage_Pass_Record);
	}

	public void setPlayResultLabelText(GameObject Obj, string ChildName, string Text)
	{
		GameObject labelObj = DataManager.Instance.FindChild(Obj, ChildName);
		
		if(labelObj)
		{
			UILabel label = labelObj.GetComponent<UILabel>();
			
			if(label)
			{
				label.text = Text;
			}
		}
	}

	public void setPlayResultStarNum(GameObject Obj, int StarNum)
	{
		GameObject star1Obj = DataManager.Instance.FindChild(Obj, "Star1Sprite");
		GameObject star2Obj = DataManager.Instance.FindChild(Obj, "Star2Sprite");
		GameObject star3Obj = DataManager.Instance.FindChild(Obj, "Star3Sprite");

		if(!star1Obj || !star2Obj || !star3Obj)
		{
			return;
		}

		UISprite star1Sprite = star1Obj.GetComponent<UISprite> ();
		UISprite star2Sprite = star2Obj.GetComponent<UISprite> ();
		UISprite star3Sprite = star3Obj.GetComponent<UISprite> ();

		if(!star1Sprite || !star2Sprite || !star3Sprite)
		{
			return;
		}

		Config.Pass_Stage_Record_Enum enumType = (Config.Pass_Stage_Record_Enum)StarNum;

		switch(enumType)
		{
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Null:
		{
			star1Sprite.spriteName = "UI_Play_Result_Star_Empty";
			star2Sprite.spriteName = "UI_Play_Result_Star_Empty";
			star3Sprite.spriteName = "UI_Play_Result_Star_Empty";
		}break;
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_1:
		{
			star1Sprite.spriteName = "UI_Play_Result_Star_Full";
			star2Sprite.spriteName = "UI_Play_Result_Star_Empty";
			star3Sprite.spriteName = "UI_Play_Result_Star_Empty";
		}break;
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_2:
		{
			star1Sprite.spriteName = "UI_Play_Result_Star_Full";
			star2Sprite.spriteName = "UI_Play_Result_Star_Full";
			star3Sprite.spriteName = "UI_Play_Result_Star_Empty";
		}break;
		case Config.Pass_Stage_Record_Enum.Pass_Stage_Record_Star_3:
		{
			star1Sprite.spriteName = "UI_Play_Result_Star_Full";
			star2Sprite.spriteName = "UI_Play_Result_Star_Full";
			star3Sprite.spriteName = "UI_Play_Result_Star_Full";
		}break;
		}
	}
	
	public GameObject getPlayResultObj(bool Win)
	{
		string mode = LocalDBManager.Instance.GetLanguage ();
		
		if(mode == "tw")
		{
			if(Win)
			{
				GameObject uiChild = DataManager.Instance.FindChild(GameResultMainObj, "GameResultObj_Win");
				return uiChild;
			}
			else
			{
				GameObject uiChild = DataManager.Instance.FindChild(GameResultMainObj, "GameResultObj_Lose");
				return uiChild;
			}
		}
		else
		{
			if(Win)
			{
				GameObject uiChild = DataManager.Instance.FindChild(GameResultMainObj, "GameResultObj_Win_En");
				return uiChild;
			}
			else
			{
				GameObject uiChild = DataManager.Instance.FindChild(GameResultMainObj, "GameResultObj_Lose_En");
				return uiChild;
			}
		}
	}

	UISprite m_ShuffleBtnSprite;
	public void showShuffleUI(bool Show)
	{
		if(isTeachMode)
		{
			return;
		}

		m_ShuffleBtn.gameObject.SetActive(Show);

		GameObject spriteChild = DataManager.Instance.FindChild(m_ShuffleBtn.gameObject, "ShuttleBtn_Bg");
		
		if(spriteChild)
		{
			m_ShuffleBtnSprite = spriteChild.GetComponent<UISprite>();

			if(m_ShuffleBtnSprite)
			{
				StartCoroutine (__fadeoutShuffleBtn());
			}
		}
	}

	private float m_showShuttlePeriod = 2.0f;
	IEnumerator __fadeInShuffleBtn()
	{
		float TmpAlpha = 0.0f;
		
		while( TmpAlpha <= 1.0f )
		{
			TmpAlpha += 1.0f / m_showShuttlePeriod*Time.deltaTime;
			m_ShuffleBtnSprite.alpha = TmpAlpha;
			
			yield return null;
		}
		
		StartCoroutine (__fadeoutShuffleBtn());
		yield return new WaitForSeconds( m_showTime );
	}

	IEnumerator __fadeoutShuffleBtn()
	{
		float TmpAlpha = 1.0f;
		
		while( TmpAlpha >= 0 )
		{
			TmpAlpha -= 1.0f / m_showShuttlePeriod*Time.deltaTime;
			m_ShuffleBtnSprite.alpha = TmpAlpha;
			
			yield return null;
		}
		
		StartCoroutine (__fadeInShuffleBtn());
		yield return new WaitForSeconds( m_showTime );
	}

	public void shuffleBlocks()
	{
		m_blockManager.startRefillBlock ();
		m_ShuffleBtn.gameObject.SetActive(false);
	}

	public void quitGame()
	{
//		if(!isTeachMode &&
//		   !isChallengMode)
//		{
//			int key = DataManager.Instance.StageKey;
//			int moveLeftNum = CSVManager.Instance.StageInfoTable.readFieldAsInt(key, "TotalMove");
//
//			if(m_blockManager.moveLeftNum < moveLeftNum)
//			{
//				LifeManager.Instance.useLife();
//			}
//		}

		DataManager.Instance.m_InGameType = DataManager.InGameType.StageInfo;
		SceneChangeManager.Instance.LoadScene("InGame");
	}

	public void playAgain()
	{
		SceneChangeManager.Instance.LoadScene("GamePlay");
	}

	public void nextStage()
	{
		if((DataManager.Instance.BigStageNum == 1 && DataManager.Instance.SmallStageNum >= 23) ||
		   DataManager.Instance.SmallStageNum >= 20)
		{
			quitGame();
			return;
		}

		DataManager.Instance.SmallStageNum ++;

		string Key = string.Format("{0}:{1}" , DataManager.Instance.BigStageNum ,  DataManager.Instance.SmallStageNum );
		DataManager.Instance.StageKey = InGameSceneMain.Instance.StageInfoToKey[Key];

		SceneChangeManager.Instance.LoadScene("GamePlay");
	}

	public void addMessageById(int MessageId)
	{
		StopCoroutine("__waitForRemoveMessageUI");

		MessageUI.gameObject.SetActive (true);
		MessageUI.transform.localPosition = new Vector3 (0, 402, 1);

		string str = Localization.Localize(MessageId.ToString());

		string mode = LocalDBManager.Instance.GetLanguage ();
		string formatStr = "";
		
		if(mode == "tw")
		{
			formatStr = string.Format ("{0}:\n{1}", "Message", str);
		}
		else
		{
			formatStr = string.Format ("{0}:\n{1}", "Message", str);
		}

		GameObject labelChild = DataManager.Instance.FindChild(MessageUI, "MessageLabel");

		if(labelChild)
		{
			UILabel label = labelChild.GetComponent<UILabel>();

			if(label)
			{
				label.text = formatStr;
			}
		}

		StartCoroutine ("__waitForRemoveMessageUI");
	}

	public void addFixMessageById_Pos(int MessageId, Vector3 Pos)
	{
		StopCoroutine("__waitForRemoveMessageUI");
		
		MessageUI.gameObject.SetActive (true);
		MessageUI.transform.localPosition = Pos;
		
		string str = Localization.Localize(MessageId.ToString());

		string mode = LocalDBManager.Instance.GetLanguage ();
		string formatStr = "";
		
		if(mode == "tw")
		{
			formatStr = string.Format ("{0}:\n{1}", "Message", str);
		}
		else
		{
			formatStr = string.Format ("{0}:\n{1}", "Message", str);
		}

		GameObject labelChild = DataManager.Instance.FindChild(MessageUI, "MessageLabel");
		
		if(labelChild)
		{
			UILabel label = labelChild.GetComponent<UILabel>();
			
			if(label)
			{
				label.text = formatStr;
			}
		}
	}

	public void hideMessage()
	{
		StopCoroutine("__waitForRemoveMessageUI");

		if(MessageUI)
		{
			MessageUI.gameObject.SetActive(false);
		}
	}

	private int messageCount = 0;
	IEnumerator __waitForRemoveMessageUI()
	{
		while(true)
		{
			if(messageCount != 0)
			{
				hideMessage();
				StopCoroutine("__waitForRemoveMessageUI");
				messageCount = 0;

				yield break;
			}

			messageCount++;
			yield return new WaitForSeconds( 3.0f );
		}
	}

	public void checkItemLock()
	{
		int starNum_1 = LocalDBManager.Instance.getStageTotalStarNum (1);

		if(starNum_1 < Config.UNLOCK_GOLD_ITEM_STAR_NUM)
		{
			if(!LocalDBManager.Instance.isUnlock(Config.MESSAGE_INVITE_UNLOCK_GOLD_ITEM, (int)Config.Operate_Mode_Enum.Operate_Mode_Lignting))
			{
				m_blockManager.addLockItemType((int)Config.Operate_Mode_Enum.Operate_Mode_Lignting);
			}
		}

		int starNum_2 = LocalDBManager.Instance.getStageTotalStarNum (2);
		
		if(starNum_2 < Config.UNLOCK_GOLD_ITEM_STAR_NUM)
		{
			m_blockManager.addLockItemType((int)Config.Operate_Mode_Enum.Operate_Mode_Erase_Knife);
		}

		int starNum_3 = LocalDBManager.Instance.getStageTotalStarNum (3);
		
		if(starNum_3 < Config.UNLOCK_GOLD_ITEM_STAR_NUM)
		{
			if(!LocalDBManager.Instance.isUnlock(Config.MESSAGE_INVITE_UNLOCK_GOLD_ITEM, (int)Config.Operate_Mode_Enum.Operate_Mode_Shovel))
			{
				m_blockManager.addLockItemType((int)Config.Operate_Mode_Enum.Operate_Mode_Shovel);
			}
		}

		int starNum_4 = LocalDBManager.Instance.getStageTotalStarNum (4);
		
		if(starNum_4 < Config.UNLOCK_GOLD_ITEM_STAR_NUM)
		{
			m_blockManager.addLockItemType((int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Upgrade);
		}

		int starNum_5 = LocalDBManager.Instance.getStageTotalStarNum (5);
		
		if(starNum_5 < Config.UNLOCK_GOLD_ITEM_STAR_NUM)
		{
			if(!LocalDBManager.Instance.isUnlock(Config.MESSAGE_INVITE_UNLOCK_GOLD_ITEM, (int)Config.Operate_Mode_Enum.Operate_Mode_Bomber))
			{
				m_blockManager.addLockItemType((int)Config.Operate_Mode_Enum.Operate_Mode_Bomber);
			}
		}
	}

	public void checkTip()
	{
		CPDebug.Log ("checkTip11111111");

		int key = DataManager.Instance.StageKey;
		string tipFileName = CSVManager.Instance.StageInfoTable.readFieldAsString (key, "TeachTip");

		if(tipFileName != "0")
		{
			m_TipBtn.gameObject.SetActive(true);

			int bigStage = DataManager.Instance.BigStageNum;
			int smallStage = DataManager.Instance.SmallStageNum;

			if(!LocalDBManager.Instance.isRecordExist(bigStage,smallStage))
			{
				LocalDBManager.Instance.addRecord(bigStage, smallStage);
				showTip();
			}
		}
	}

	void showTip()
	{
		if(m_blockManager.gameOver)
		{
			return;
		}
		
		if(isShowTip)
		{
			return;
		}

		if(tipObj)
		{
			m_blockManager.enableAllTouchObj(false);
			tipObj.gameObject.SetActive(true);
			isShowTip = true;

			GameObject spriteChild = DataManager.Instance.FindChild(tipObj, "TipSprite");

			if(spriteChild)
			{
				UISprite sprite = spriteChild.GetComponent<UISprite>();

				if(sprite)
				{
					int bigStage = DataManager.Instance.BigStageNum;
					int smallStage = DataManager.Instance.SmallStageNum;

					string altasName = string.Format("Tip_{0}-{1}Altas", bigStage, smallStage);
					string spriteName = null;
					string mode = LocalDBManager.Instance.GetLanguage ();
					
					if(mode == "tw")
					{
						spriteName = string.Format("tip_{0}_{1}", bigStage, smallStage);
					}
					else
					{
						spriteName = string.Format("tip_{0}_{1}_En", bigStage, smallStage);
					}

					CPDebug.Log("altasName = " + altasName);
					CPDebug.Log("spriteName = " + spriteName);
					UIAtlas atlas = Resources.Load( string.Format("Tip/{0}" , altasName ) , typeof(UIAtlas) ) as UIAtlas;
					sprite.atlas = atlas;
					sprite.spriteName = spriteName;
				}
			}
		}
	}

	void closeTip()
	{
		if(tipObj)
		{
			tipObj.gameObject.SetActive(false);
			m_blockManager.enableAllTouchObj(true);
			isShowTip = false;
		}
	}

	public void openTipUIForGoldItem(int ItemType)
	{
		int bigStage = 0;
		int smallStage = 0;
		
		Config.Operate_Mode_Enum enumType = (Config.Operate_Mode_Enum)ItemType;
		switch(enumType)
		{
		case Config.Operate_Mode_Enum.Operate_Mode_Lignting:
		{
			bigStage = 1;
			smallStage = 19;
		}break;
		case Config.Operate_Mode_Enum.Operate_Mode_Shovel:
		{
			bigStage = 3;
			smallStage = 19;
		}break;
		case Config.Operate_Mode_Enum.Operate_Mode_Bomber:
		{
			bigStage = 5;
			smallStage = 19;
		}break;
		default:
		{
			return;
		}
		}

		if(tipObj)
		{
			m_blockManager.enableAllTouchObj(false);
			tipObj.gameObject.SetActive(true);
			isShowTip = true;
			
			GameObject spriteChild = DataManager.Instance.FindChild(tipObj, "TipSprite");
			
			if(spriteChild)
			{
				UISprite sprite = spriteChild.GetComponent<UISprite>();
				
				if(sprite)
				{
					string altasName = string.Format("Tip_{0}-{1}Altas", bigStage, smallStage);
					string spriteName = null;
					string mode = LocalDBManager.Instance.GetLanguage ();
					
					if(mode == "tw")
					{
						spriteName = string.Format("tip_{0}_{1}", bigStage, smallStage);
					}
					else
					{
						spriteName = string.Format("tip_{0}_{1}_En", bigStage, smallStage);
					}

					UIAtlas atlas = Resources.Load( string.Format("Tip/{0}" , altasName ) , typeof(UIAtlas) ) as UIAtlas;
					sprite.atlas = atlas;
					sprite.spriteName = spriteName;
				}
			}
		}
	}

	public void onTipClick()
	{
		CPDebug.Log ("onTipClick");
	}
}
