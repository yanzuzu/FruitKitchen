using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotBase : MonoBehaviour {
	
	public int				m_nSlotIndex;
	public bool				m_bSlotReadyEraseBlock;
	public bool				m_bSlotEraseNeedPerformace;
	public bool				m_bSlotIsShining;
	public bool				m_bIsShowingTurnBtns;
	public bool				slotBlockHasBeReset = false;
	public bool				slotFadeOutFinish = false;
	public Rect				m_SlotRect;
	public int				m_nSlotType;
	public GamePlayLayer	m_SlotMotherLayer;
	public BlockManager		m_SlotMotherManager;
	public BlockBase		m_SlotBlock;
	public GameObject		m_myGameObject;
	public GameObject		touchObj;
	public GameObject		effectNumObj;
	
	public UISprite			m_SlotBKSprite;
	public UISprite			m_FarBombSprite_H;
	public UISprite			m_FarBombSprite_V;

	public UIButton			m_UpBtn;
	public UIButton			m_DownBtn;
	public UIButton			m_LeftBtn;
	public UIButton			m_RightBtn;

	public UIButton			m_HorizontalBtn;
	public UIButton			m_VerticalBtn;
	public UIButton			m_MiddleBtn;

	void Awake()
	{
		if(m_FarBombSprite_H)
		{
			m_FarBombSprite_H.gameObject.SetActive(false);
		}

		if(m_FarBombSprite_V)
		{
			m_FarBombSprite_V.gameObject.SetActive(false);
		}

		if(m_HorizontalBtn)
		{
			UIButtonMessage message = m_HorizontalBtn.gameObject.GetComponent< UIButtonMessage >();
			message.target = this.gameObject;
			message.functionName = "setBombHorizontal";
		}

		if(m_VerticalBtn)
		{
			UIButtonMessage message = m_VerticalBtn.gameObject.GetComponent< UIButtonMessage >();
			message.target = this.gameObject;
			message.functionName = "setBombVertical";
		}

		if(m_MiddleBtn)
		{
			UIButtonMessage message = m_MiddleBtn.gameObject.GetComponent< UIButtonMessage >();
			message.target = this.gameObject;
			message.functionName = "setBombMiddle";
		}

		if(touchObj)
		{
			touchObj.gameObject.SetActive(false);
		}

		showBombDirectBtns (false, true);
	}

	// Use this for initialization
	void Start () {

		// init
		initSlot ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	virtual public void initSlot()
	{
	//	m_nSlotIndex = -1;
	//	m_SlotBlock = null;
	//	m_SlotBKSprite = null;
	//	m_nSlotType = Config.Slot_Enum.Slot_Normal;
		m_bSlotReadyEraseBlock = false;
		m_bSlotEraseNeedPerformace = true;
		m_bSlotIsShining = false;
		m_bIsShowingTurnBtns = false;

	//	m_SlotRect.center = new Vector2(0,0);
	//	m_SlotRect.width = 0;
	//	m_SlotRect.height = 0;
	}
	
	public void setBlock(BlockBase block)
	{
		block.stopMoving ();

		m_SlotBlock = block;
		m_SlotBlock.m_nBelongSlotIndex = m_nSlotIndex;
		m_SlotBlock.m_SlotMotherManager = m_SlotMotherManager;

		if(m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Score)
		{
			BlockScore scoreBlock = (BlockScore)m_SlotBlock;
			scoreBlock.increaseMoveNum();
		}

		iTween.MoveTo(block.gameObject,iTween.Hash("position",new Vector3( gameObject.transform.localPosition.x , gameObject.transform.localPosition.y ,0 )
		                                           ,"time",Config.BLOCK_MOVE_TIME,
		                                           "islocal",true,
		                                           "easetype",iTween.EaseType.linear,
		                                           "oncomplete", "blockMoveFinish",
		                                           "oncompletetarget", this.gameObject));

		m_SlotBlock.isNowMoving = true;
	}

	public void bePress()
	{
		if(m_nSlotIndex != 0)
		{
			m_SlotMotherManager.nowTouchSlotIndex = m_nSlotIndex;
			m_SlotMotherManager.resetShingingTimer ();
		}

	}

	public void blockMoveFinish()                            
	{
		if(m_SlotBlock)
		{
			if(m_SlotMotherManager.ifCanDoOperate)
			{
				checkToBeAbility();
			}

			detectNearBlock();

			m_SlotBlock.isNowMoving = false;
			m_SlotBlock.transform.localPosition = gameObject.transform.localPosition;
		}
	}

	public void setBlockDirect(BlockBase block)
	{
		block.stopMoving ();

		m_SlotBlock = block;
		m_SlotBlock.m_nBelongSlotIndex = m_nSlotIndex;
		m_SlotBlock.m_SlotMotherManager = m_SlotMotherManager;
		
		if(m_SlotBlock.m_BlockBodySprite)
		{
			m_SlotBlock.transform.localPosition = m_myGameObject.transform.localPosition;
		}

		detectNearBlock ();
	}

	public void combineBomb(int Type, BlockBombBase.Special_Bomb_Info TargetBombInfo)
	{
		if(!m_SlotBlock)
		{
			return;
		}

		BlockBase targetBlock = null;

		if(Type == (int)Config.Block_Enum.Block_Score ||
		   Type == (int)Config.Block_Enum.Block_Bomb_Fruit)
		{
			targetBlock = BlockCreater.Instance.getBlockByType(m_SlotMotherManager.m_BlockObj, m_SlotMotherManager.m_myPanel, Type, m_SlotBlock.m_nColorType);
		}
		else
		{
			targetBlock = BlockCreater.Instance.getBlockByType(m_SlotMotherManager.m_BlockObj, m_SlotMotherManager.m_myPanel, Type, (int)Config.Block_Color_Enum.Block_Color_Null);
		}

		if(!targetBlock)
		{
			return;
		}

		if(targetBlock.isBomb())
		{
			BlockBombBase bombBlock = (BlockBombBase)targetBlock;
			bombBlock.specialBombInfo = TargetBombInfo;
		}

		if(!m_SlotBlock.copyBlockDetail(targetBlock))
		{
			return;
		}

		Destroy (m_SlotBlock.m_myGameObject);
		setBlockDirect (targetBlock);

		if(Type == (int)Config.Block_Enum.Block_Bomb_Fruit)
		{
			m_SlotBlock.m_nBlockType = targetBlock.m_nBlockType;
			m_SlotBlock.m_nColorType = targetBlock.m_nColorType;
		}

		if(targetBlock.isBomb())
		{
			BlockBombBase bombBlock = (BlockBombBase)targetBlock;
			bombBlock.startTransBlock();
		}

		m_bSlotReadyEraseBlock = false;

		if(Type == (int)Config.Block_Enum.Block_Small_Bomb)
		{
			SoundManager.Instance.PlaySE("Sound_Be_Bomb_s", false);
		}
		else if(Type == (int)Config.Block_Enum.Block_Big_Bomb)
		{
			SoundManager.Instance.PlaySE("Sound_Be_Bomb_b", false);
		}
	}

	public void eraseWithoutTriggerSpecial()
	{
		if(!m_SlotBlock)
		{
			return;
		}

		if(!m_SlotBlock.m_BlockBodySprite)
		{
			return;
		}

		m_SlotBlock.m_bIsOnTable = false;

		m_SlotMotherManager.recoverBlock (m_SlotBlock);
		m_SlotBlock = null;
	}

	public bool changeBlockTypeToScore(int Color)
	{
		if(!m_SlotBlock)
		{
			return false;
		}

		m_SlotBlock.m_nColorType = Color;

		return changeBlockType ((int)Config.Block_Enum.Block_Score);
	}

	public void testChange(string name)
	{
		if(m_SlotBKSprite)
		{
			m_SlotBKSprite.spriteName = name;
		}
	}

	public void detectNearBlock()
	{
		List< SlotBase > List = new List<SlotBase>();

		for(int i = Config.DIRECT_UP; i <= Config.DIRECT_RIGHT; i++)
		{
			SlotBase slot = m_SlotMotherManager.getSlot(m_nSlotIndex, i);

			if(!slot || !slot.m_SlotBlock)
			{
				continue;
			}

			List.Add(slot);
		}

		SlotBase slotLeft = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_LEFT);

		if(slotLeft)
		{
			SlotBase slotLeftUp = m_SlotMotherManager.getSlot(slotLeft.m_nSlotIndex, Config.DIRECT_UP);

			if(slotLeftUp)
			{
				List.Add(slotLeftUp);
			}

			SlotBase slotLeftDown = m_SlotMotherManager.getSlot(slotLeft.m_nSlotIndex, Config.DIRECT_DOWN);
			
			if(slotLeftDown)
			{
				List.Add(slotLeftDown);
			}
		}

		SlotBase slotRight = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_RIGHT);
		
		if(slotRight)
		{
			SlotBase slotRightUp = m_SlotMotherManager.getSlot(slotRight.m_nSlotIndex, Config.DIRECT_UP);
			
			if(slotRightUp)
			{
				List.Add(slotRightUp);
			}
			
			SlotBase slotRightDown = m_SlotMotherManager.getSlot(slotRight.m_nSlotIndex, Config.DIRECT_DOWN);
			
			if(slotRightDown)
			{
				List.Add(slotRightDown);
			}
		}

		for(int i = 0; i < List.Count; i++)
		{
			SlotBase slot = List[i];

			if(!slot)
			{
				continue;
			}

			detectSauceBlock(slot);
			detectMilkBlock(slot);
		}
	}

	public void detectSauceBlock(SlotBase slot)
	{
		if(!slot || !slot.m_SlotBlock)
		{
			return;
		}

		if(!m_SlotBlock)
		{
			return;
		}

		if(m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Score)
		{
			return;
		}

		BlockScore scoreBlock = (BlockScore)m_SlotBlock;

		if(scoreBlock.isTrash)
		{
			return;
		}

		if(slot.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Sauce)
		{
			return;
		}

		scoreBlock.setIsTrash ();
	}

	public void detectMilkBlock(SlotBase slot)
	{
		if(!slot || !slot.m_SlotBlock)
		{
			return;
		}

		if(!m_SlotBlock)
		{
			return;
		}
		
		if(m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Score)
		{
			return;
		}
		
		BlockScore scoreBlock = (BlockScore)m_SlotBlock;
		
		if(scoreBlock.isTrash)
		{
			return;
		}
		
		if(slot.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Milk)
		{
			return;
		}
		
		scoreBlock.setIsDoubleFreeFall ();
	}

	public bool blockEraseDirect(bool NeedPerformace, bool DoAfterErase)
	{
		bool checkPass = false;

		if(m_bSlotReadyEraseBlock)
		{
			if (m_bSlotEraseNeedPerformace && !NeedPerformace) 
			{
				checkPass = true;
			} 
			else 
			{
				checkPass = false;
			}
		}
		else
		{
			checkPass = true;
		}

		if(!checkPass)
		{
			return false;
		}

		m_bSlotReadyEraseBlock = true;
		m_bSlotEraseNeedPerformace = NeedPerformace;
		releaseBlock (NeedPerformace, DoAfterErase, false);

		return true;
	}

	public bool releaseBlock(bool NeedPerformace, bool DoAfterErase, bool PlayReleaseEffect)
	{
		if(!m_SlotBlock)
		{
			return false;
		}

		if (!m_SlotBlock.m_BlockBodySprite)
		{
			return false;
		}

		if(!m_bSlotReadyEraseBlock)
		{
			return false;
		}

		m_SlotBlock.m_bIsOnTable = false;
		m_SlotMotherManager.earnScoreByType (m_SlotBlock.m_nBlockType, false);

		if(m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Normal)
		{
			BlockNormal normalBlock = (BlockNormal)m_SlotBlock;

			if(normalBlock.isGoldBlock)
			{
				m_SlotMotherManager.addOwnCoins(1);
				SoundManager.Instance.PlaySE("Sound_Block_Delete_With_Gold", false);
			}
		}

		if(m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
		{
			BlockBomb_Fruit fruitBomb = (BlockBomb_Fruit)m_SlotBlock;
			fruitBomb.block_blockDoBomb(true);
		}

		if(PlayReleaseEffect)
		{
			m_SlotBlock.showReleaseBlockImage();
		}

		if(NeedPerformace)
		{
			if(DoAfterErase)
			{
				checkAroundBlockAfterReleaseBlock(true);
			}

			m_bSlotReadyEraseBlock = false;
			m_bSlotEraseNeedPerformace = true;
			m_SlotBlock.isNowShaking = true;

			Hashtable TmpTable = new Hashtable();
			TmpTable.Add("x", 0.02f);
			TmpTable.Add("time", 0.2f);
			iTween.ShakePosition(m_SlotBlock.m_myGameObject,TmpTable);

			StartCoroutine(__afterShakeDo(m_SlotBlock.m_nBlockIndex));
		}
		else
		{
			m_SlotMotherManager.recoverBlock(m_SlotBlock);
			m_SlotBlock = null;

			if(DoAfterErase)
			{
				checkAroundBlockAfterReleaseBlock(true);
			}

			m_bSlotReadyEraseBlock = false;
			m_bSlotEraseNeedPerformace = true;
		}

		return true;
	}

	int firstShakeOff = 0;
	IEnumerator __afterShakeDo(int BlockIndex)
	{
		while(true)
		{
			if(m_SlotBlock && firstShakeOff == 1)
			{
				if(BlockIndex != m_SlotBlock.m_nBlockIndex)
				{
					firstShakeOff = 0;
					yield break;
				}

				m_SlotMotherManager.recoverBlock(m_SlotBlock);
				m_SlotBlock = null;
				firstShakeOff = 0;

				yield break;
			}
			else
			{
				firstShakeOff = 1;
			}

			yield return new WaitForSeconds(0.2f);
		}
	}

	void afterShakeDo()
	{
		if(m_SlotBlock)
		{
			m_SlotMotherManager.recoverBlock(m_SlotBlock);
			m_SlotBlock = null;
		}
	}

	public bool isBornSlot()
	{
		if(m_nSlotType == (int)Config.Slot_Enum.Slot_Born_Block_All ||
		   m_nSlotType == (int)Config.Slot_Enum.Slot_Born_Block_Product ||
		   m_nSlotType == (int)Config.Slot_Enum.Slot_Born_Block_Normal)
		{
			return true;
		}

		return false;
	}

	public bool eraseByBomb()
	{
		if(!m_SlotBlock)
		{
			return false;
		}

		if(m_SlotBlock.m_bEraseByBomb)
		{
			return false;
		}

		m_SlotBlock.m_bEraseByBomb = true;

		blockEraseDirect (true, false);
		return true;
	}

	public bool changeBlockType(int Type)
	{
		if(!m_SlotBlock)
		{
			return false;
		}

		if(m_SlotBlock.m_nBlockType == Type)
		{
			return false;
		}

		if(m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Normal)
		{
			BlockNormal normalBlock = (BlockNormal)m_SlotBlock;

			if(normalBlock.isGoldBlock)
			{
				m_SlotMotherManager.addOwnCoins(1);
			}
		}

		if(m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Normal)
		{
			Config.Block_Color_Enum enumType = (Config.Block_Color_Enum)m_SlotBlock.m_nColorType - 1;
			switch(enumType)
			{
			case Config.Block_Color_Enum.Block_Color_1:
			{
				m_SlotMotherManager.addRecordNum((int)Config.Player_Info_Enum.Player_Info_Erase_Fruit_1_Num, 1);
			}break;
			case Config.Block_Color_Enum.Block_Color_2:
			{
				m_SlotMotherManager.addRecordNum((int)Config.Player_Info_Enum.Player_Info_Erase_Fruit_2_Num, 1);
			}break;
			case Config.Block_Color_Enum.Block_Color_3:
			{
				m_SlotMotherManager.addRecordNum((int)Config.Player_Info_Enum.Player_Info_Erase_Fruit_3_Num, 1);
			}break;
			case Config.Block_Color_Enum.Block_Color_4:
			{
				m_SlotMotherManager.addRecordNum((int)Config.Player_Info_Enum.Player_Info_Erase_Fruit_4_Num, 1);
			}break;
			case Config.Block_Color_Enum.Block_Color_5:
			{
				m_SlotMotherManager.addRecordNum((int)Config.Player_Info_Enum.Player_Info_Erase_Fruit_5_Num, 1);
			}break;
			case Config.Block_Color_Enum.Block_Color_6:
			{
				m_SlotMotherManager.addRecordNum((int)Config.Player_Info_Enum.Player_Info_Erase_Fruit_6_Num, 1);
			}break;
			}
		}

		BlockBase targetBlock = null;

		if(Type == (int)Config.Block_Enum.Block_Score ||
		   Type == (int)Config.Block_Enum.Block_Bomb_Fruit)
		{
			targetBlock = BlockCreater.Instance.getBlockByType(m_SlotMotherManager.m_BlockObj, m_SlotMotherManager.m_myPanel, Type, m_SlotBlock.m_nColorType);
		}
		else
		{
			targetBlock = BlockCreater.Instance.getBlockByType(m_SlotMotherManager.m_BlockObj, m_SlotMotherManager.m_myPanel, Type, (int)Config.Block_Color_Enum.Block_Color_Null);
		}

		if(!targetBlock)
		{
			return false;
		}

		if(!m_SlotBlock.copyBlockDetail(targetBlock))
		{
			return false;
		}

		Destroy (m_SlotBlock.m_myGameObject);

		setBlockDirect (targetBlock);

		if(Type == (int)Config.Block_Enum.Block_Bomb_Fruit)
		{
			m_SlotBlock.m_nBlockType = targetBlock.m_nBlockType;
			m_SlotBlock.m_nColorType = targetBlock.m_nColorType;
		}

		m_bSlotReadyEraseBlock = false;

		if(Type == (int)Config.Block_Enum.Block_Small_Bomb)
		{
			SoundManager.Instance.PlaySE("Sound_Be_Bomb_s", false);
		}
		else if(Type == (int)Config.Block_Enum.Block_Big_Bomb)
		{
			SoundManager.Instance.PlaySE("Sound_Be_Bomb_b", false);
		}

		return true;
	}

	public void checkAroundBlockAfterReleaseBlock(bool NeedCheck)
	{
		if(!m_bSlotReadyEraseBlock && NeedCheck)
		{
			return;
		}

		for(int i = Config.DIRECT_UP; i <= Config.DIRECT_RIGHT; i++)
		{
			SlotBase slot = m_SlotMotherManager.getSlot(m_nSlotIndex, i);

			if(!slot || !slot.m_SlotBlock)
			{
				continue;
			}

			if(slot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Wood)
			{
				BlockWood woodBlock = (BlockWood)slot.m_SlotBlock;

				if(woodBlock.m_nResistBombNums > 1)
				{
					woodBlock.brokenBlock();
				}
				else
				{
					slot.m_bSlotReadyEraseBlock = true;
					slot.releaseBlock(false, false, false);
				}
			}
			else if(slot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Field_Item)
			{
				slot.m_bSlotReadyEraseBlock = true;
				slot.releaseBlock(true, false, false);
			}
		}
	}

	public void setBombHorizontal()
	{
		if(!m_SlotBlock)
		{
			return;
		}

		if(!m_SlotBlock.isBomb())
		{
			return;
		}

		BlockBombBase bombBlock = (BlockBombBase)m_SlotBlock;

		if(bombBlock.specialBombInfo.isHorizontalBomb)
		{
			return;
		}

		if(m_SlotMotherManager.isTeachMode)
		{
			TeachModeStep step = m_SlotMotherManager.m_GamePlayLayer.getNowTeachStep();

			if(step != null)
			{
				bool isAllow = step.isAllow(0, Config.TEACH_MODE_ALLOW_ACIOTN_PRESS_BOMB_HORIZONTAL_BTN);

				if(!isAllow)
				{
					return;
				}

				m_SlotMotherManager.m_GamePlayLayer.doNextTeachStep();
			}
		}

		bombBlock.setHorizontalBomb (true);

		m_SlotMotherManager.finishOperateMode (false);

		m_SlotMotherManager.addOwnCoins (-Config.NEED_COIN_BOMB_DIRECT);

		showBombDirectBtns (false);

		SoundManager.Instance.PlaySE("Sound_ChangeBombProperty", false);
	}

	public void setBombVertical()
	{
		if(!m_SlotBlock)
		{
			return;
		}
		
		if(!m_SlotBlock.isBomb())
		{
			return;
		}
		
		BlockBombBase bombBlock = (BlockBombBase)m_SlotBlock;
		
		if(bombBlock.specialBombInfo.isVerticalBomb)
		{
			return;
		}

		if(m_SlotMotherManager.isTeachMode)
		{
			return;
		}
		
		bombBlock.setVerticalBomb (true);
		
		m_SlotMotherManager.finishOperateMode (false);
		
		m_SlotMotherManager.addOwnCoins (-Config.NEED_COIN_BOMB_DIRECT);
		
		showBombDirectBtns (false);

		SoundManager.Instance.PlaySE("Sound_ChangeBombProperty", false);
	}

	public void setBombMiddle()
	{
		if(!m_SlotBlock)
		{
			return;
		}
		
		if(!m_SlotBlock.isBomb())
		{
			return;
		}
		
		BlockBombBase bombBlock = (BlockBombBase)m_SlotBlock;
		
		if(bombBlock.specialBombInfo.isHoldBomb)
		{
			return;
		}

		if(m_SlotMotherManager.isTeachMode)
		{
			return;
		}
		
		bombBlock.setHoldBomb (true);
		
		m_SlotMotherManager.finishOperateMode (false);
		
		m_SlotMotherManager.addOwnCoins (-Config.NEED_COIN_BOMB_DIRECT);
		
		showBombDirectBtns (false);

		SoundManager.Instance.PlaySE("Sound_ChangeBombProperty", false);
	}

	public void showBombDirectBtns(bool show, bool IsInitCall = false)
	{
		if(show)
		{
			if(!m_SlotBlock)
			{
				return;
			}
			
			if(!m_SlotBlock.isBomb())
			{
				return;
			}
			
			BlockBombBase bombBlock = (BlockBombBase)m_SlotBlock;

			if(!bombBlock.specialBombInfo.isHoldBomb)
			{
				showMiddleBtn (show);
			}
			
			if(!bombBlock.specialBombInfo.isHorizontalBomb)
			{
				showHorizontalBtn (show);
			}
			
			if(!bombBlock.specialBombInfo.isVerticalBomb)
			{
				showVerticalBtn (show);
			}

			if(!IsInitCall)
			{
				m_SlotMotherManager.enableAllTouchObj(false);
			}
		}
		else
		{
			showHorizontalBtn (show);
			showVerticalBtn (show);
			showMiddleBtn (show);

			if(!IsInitCall)
			{
				m_SlotMotherManager.enableAllTouchObj(true);
			}
		}
	}

	public void showHorizontalBtn(bool show)
	{
		if(!m_HorizontalBtn)
		{
			return;
		}

		m_HorizontalBtn.gameObject.SetActive(show);
	}

	public void showVerticalBtn(bool show)
	{
		if(!m_VerticalBtn)
		{
			return;
		}
		
		m_VerticalBtn.gameObject.SetActive(show);
	}

	public void showMiddleBtn(bool show)
	{
		if(!m_MiddleBtn)
		{
			return;
		}
		
		m_MiddleBtn.gameObject.SetActive(show);
	}

	private bool startShowResetPerformace = false;
	public void startResetPerformace(bool NeedRemoveBlock)
	{
		if(!m_SlotBlock)
		{
			return;
		}

		startShowResetPerformace = true;
		StartCoroutine (__startResetPerformace());
	}

	private float m_showTime = 1.0f;
	private float m_showPeriod = 1.5f;
	IEnumerator __startResetPerformace()
	{
		if(startShowResetPerformace)
		{
			float TmpAlpha = 0;
			
			while( TmpAlpha <= 1.0f )
			{
				TmpAlpha += 1.0f / m_showPeriod*Time.deltaTime;

				if(m_SlotBlock &&
				   m_SlotBlock.m_BlockBodySprite)
				{
					m_SlotBlock.m_BlockBodySprite.alpha = TmpAlpha;
				}
				
				yield return null;
			}

			endResetPerformace();
			yield break;
		}

		yield return new WaitForSeconds( m_showTime );
	}
	public void endResetPerformace()
	{
		startShowResetPerformace = false;
		slotBlockHasBeReset = false;

		if(m_SlotBlock &&
		   m_SlotBlock.m_BlockBodySprite)
		{
			m_SlotBlock.m_BlockBodySprite.alpha = 1;
		}
	}

	private bool isStartRemovePerformace = false;
	public void startRemovePerformace(bool NeedRemoveBlock)
	{
		if(!m_SlotBlock)
		{
			return;
		}
		
		isStartRemovePerformace = true;
		slotBlockHasBeReset = true;
		slotFadeOutFinish = false;
		StartCoroutine (__startRemovePerformace(NeedRemoveBlock));
	}

	IEnumerator __startRemovePerformace(bool NeedRemoveBlock)
	{
		if(isStartRemovePerformace)
		{
			float TmpAlpha = 1.0f;
			
			while( TmpAlpha >= 0 )
			{
				TmpAlpha -= 1.0f / m_showPeriod*Time.deltaTime;

				if(m_SlotBlock &&
				   m_SlotBlock.m_BlockBodySprite)
				{
					m_SlotBlock.m_BlockBodySprite.alpha = TmpAlpha;
				}

				yield return null;
			}
			
			endRemovePerformace(NeedRemoveBlock);
			yield break;
		}
		
		yield return new WaitForSeconds( m_showTime );
	}

	public void endRemovePerformace(bool NeedRemoveBlock)
	{
		isStartRemovePerformace = false;
		slotFadeOutFinish = true;

		if(NeedRemoveBlock)
		{
			m_SlotMotherManager.recoverBlock (m_SlotBlock);
			m_SlotBlock = null;
		}
	}

	private bool slotIsShinging = false;
	public void startShinging()
	{
		if(!m_SlotBlock)
		{
			return;
		}

		slotIsShinging = true;

		StartCoroutine ("__fadeoutBlock");
	}

	public void stopShinging()
	{
	//	if(slotIsShinging)
		{
			if(m_SlotBlock &&
			   m_SlotBlock.m_BlockBodySprite)
			{
				slotIsShinging = false;
				m_SlotBlock.m_BlockBodySprite.alpha = 1.0f;
			}

			StopCoroutine("__fadeoutBlock");
			StopCoroutine("__fadeInBlock");
		}
	}

	private float m_shingingPeriod = 1.0f;
	IEnumerator __fadeInBlock()
	{
		if(slotIsShinging)
		{
			float TmpAlpha = 0.0f;
			
			while( TmpAlpha <= 1.0f )
			{
				if(!slotIsShinging)
				{
					m_SlotBlock.m_BlockBodySprite.alpha = 1.0f;
					yield break;
				}

				TmpAlpha += 1.0f / m_shingingPeriod*Time.deltaTime;

				if(m_SlotBlock &&
				   m_SlotBlock.m_BlockBodySprite)
				{
					m_SlotBlock.m_BlockBodySprite.alpha = TmpAlpha;
				}
				
				yield return null;
			}
			
			StartCoroutine ("__fadeoutBlock");
		}

		yield return new WaitForSeconds( m_showTime );
	}
	
	IEnumerator __fadeoutBlock()
	{
		if(slotIsShinging)
		{
			float TmpAlpha = 1.0f;
			
			while( TmpAlpha >= 0 )
			{
				if(!slotIsShinging)
				{
					m_SlotBlock.m_BlockBodySprite.alpha = 1.0f;
					yield break;
				}

				TmpAlpha -= 1.0f / m_shingingPeriod*Time.deltaTime;

				if(m_SlotBlock &&
				   m_SlotBlock.m_BlockBodySprite)
				{
					m_SlotBlock.m_BlockBodySprite.alpha = TmpAlpha;
				}
				
				yield return null;
			}
			
			StartCoroutine ("__fadeInBlock");
		}

		yield return new WaitForSeconds( m_showTime );
	}

	public void showNumberEffect(string ImageFileName, int Num)
	{
		if(effectNumObj)
		{
			effectNumObj.SetActive(true);
		}

		GameObject spriteChild = DataManager.Instance.FindChild(effectNumObj, "EffectNumBk");

		if(spriteChild)
		{
			UISprite sprite = spriteChild.GetComponent<UISprite>();

			if(sprite)
			{
				sprite.spriteName = ImageFileName;
			}
		}

		GameObject labelChild = DataManager.Instance.FindChild(effectNumObj, "EffectNumLabel");
		
		if(labelChild)
		{
			UILabel label = labelChild.GetComponent<UILabel>();
			
			if(label)
			{
				label.text = string.Format("+ {0}", Num.ToString());
			}
		}

		iTween.Stop (effectNumObj);
		effectNumObj.transform.localPosition = m_SlotBKSprite.transform.localPosition;

		Hashtable TmpTable = new Hashtable();
		TmpTable.Add("position",new Vector3( m_SlotBKSprite.transform.localPosition.x ,
		                                    m_SlotBKSprite.transform.localPosition.y + 100,
		                                    0 ));
		TmpTable.Add("time",1.0f);
		TmpTable.Add("islocal" , true );
		TmpTable.Add ("oncomplete","effectNumMoveFinish");
		TmpTable.Add ("oncompletetarget",this.gameObject);
		iTween.MoveTo(effectNumObj,TmpTable);
	}

	void effectNumMoveFinish()
	{
		effectNumObj.SetActive(false);
	}

	virtual public void setBKISprite()
	{
		
	}

	virtual public bool ifCanBeErase()
	{
		return false;
	}

	virtual public bool isCanMoveBlockOut()
	{
		return false;
	}

	virtual public bool checkToBeAbility()
	{
		return false;
	}

	virtual public void setMoveEnergy(int Dir, bool Energy)
	{
	}

	virtual public void disbuteMoveEnergy(int Dir, int MoveDir)
	{
	}

	virtual public bool isHasMoveEnergy(int Dir)
	{
		return false;
	}

	virtual public bool checkToBeAbility_ForFall()
	{
		return false;
	}

	virtual public bool checkToEraseBlock()
	{
		return false;
	}

	virtual public bool checkToBeBomb()
	{
		return false;
	}

	virtual public bool ifCanBeEraseInThisColor(int ColorType, int ExceptIndex)
	{
		return false;
	}

	virtual public bool isCanReceiveBlock(bool FromLR)
	{
		return false;
	}

	virtual public bool receiveBlock(BlockBase Block)
	{
		return false;
	}

	virtual public void updateSlotWork()
	{
	}

}
