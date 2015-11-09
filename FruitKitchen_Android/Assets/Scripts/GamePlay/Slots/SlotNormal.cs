using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotNormal : SlotBase {

	public int	m_nTest;
	public bool hasMoveLeftEnergy = false;
	public bool hasMoveRightEnergy = false;

	List< string > tempArray = new List< string >();
	List< SlotBase > tempSlotArray = new List< SlotBase >();
	List< SlotBase > normalBlock = new List< SlotBase >();
	List< SlotBase > otherBomb = new List< SlotBase >();

	// Use this for initialization
	void Start () {
		initSlot ();

//		InvokeRepeating("afterShakeDo", 0, 0.2f);
	}

	override public void initSlot()
	{
		base.initSlot ();
		
		m_nSlotType = (int)Config.Slot_Enum.Slot_Normal;

		if(touchObj)
		{
			touchObj.gameObject.SetActive(true);

			BoxCollider box = touchObj.GetComponent<BoxCollider>();
			
			if(box)
			{
				box.enabled = true;
			}
			
			UIButtonMessage message = touchObj.GetComponent<UIButtonMessage>();
			
			if(message)
			{
				message.enabled = true;
			}
		}
	}

	bool checkIfTheSameColor(int StartIndex, bool AtMid, int Dir,List< string > Array)
	{
		SlotBase slotBase = m_SlotMotherManager.getSlotByIndex (StartIndex);

		if(!slotBase)
		{
			return false;
		}

		if(!slotBase.m_SlotBlock)
		{
			return false;
		}

		if(slotBase.m_SlotBlock.m_nColorType == (int)Config.Block_Color_Enum.Block_Color_Null)
		{
			return false;
		}

		SlotBase slot1 = null;
		SlotBase slot2 = null;

		if(AtMid)
		{
			switch(Dir)
			{
			case Config.DIRECT_DOWN:
			case Config.DIRECT_UP:
			{
				slot1 = m_SlotMotherManager.getSlot(slotBase.m_nSlotIndex, Config.DIRECT_DOWN);
				slot2 = m_SlotMotherManager.getSlot(slotBase.m_nSlotIndex, Config.DIRECT_UP);
			}break;
			case Config.DIRECT_LEFT:
			case Config.DIRECT_RIGHT:
			{
				slot1 = m_SlotMotherManager.getSlot(slotBase.m_nSlotIndex, Config.DIRECT_LEFT);
				slot2 = m_SlotMotherManager.getSlot(slotBase.m_nSlotIndex, Config.DIRECT_RIGHT);
			}break;
			}

			if(!slot1 ||
			   !slot2)
			{
				return false;
			}

			if(!slot1.m_SlotBlock ||
			   !slot2.m_SlotBlock)
			{
				return false;
			}

			if(slotBase.m_SlotBlock.m_nColorType == slot1.m_SlotBlock.m_nColorType &&
			   slotBase.m_SlotBlock.m_nColorType == slot2.m_SlotBlock.m_nColorType)
			{
				string index1 = slot1.m_nSlotIndex.ToString();
				string index2 = slot2.m_nSlotIndex.ToString();

				Array.Add(index1);
				Array.Add(index2);

				return true;
			}
		}
		else
		{
			switch(Dir)
			{
			case Config.DIRECT_DOWN:
			{
				slot1 = m_SlotMotherManager.getSlot(slotBase.m_nSlotIndex, Config.DIRECT_DOWN);

				if(!slot1)
				{
					return false;
				}

				slot2 = m_SlotMotherManager.getSlot(slot1.m_nSlotIndex, Config.DIRECT_DOWN);

				if(!slot2)
				{
					return false;
				}
			}break;
			case Config.DIRECT_UP:
			{
				slot1 = m_SlotMotherManager.getSlot(slotBase.m_nSlotIndex, Config.DIRECT_UP);
				
				if(!slot1)
				{
					return false;
				}
				
				slot2 = m_SlotMotherManager.getSlot(slot1.m_nSlotIndex, Config.DIRECT_UP);
				
				if(!slot2)
				{
					return false;
				}
			}break;
			case Config.DIRECT_LEFT:
			{
				slot1 = m_SlotMotherManager.getSlot(slotBase.m_nSlotIndex, Config.DIRECT_LEFT);
				
				if(!slot1)
				{
					return false;
				}
				
				slot2 = m_SlotMotherManager.getSlot(slot1.m_nSlotIndex, Config.DIRECT_LEFT);
				
				if(!slot2)
				{
					return false;
				}
			}break;
			case Config.DIRECT_RIGHT:
			{
				slot1 = m_SlotMotherManager.getSlot(slotBase.m_nSlotIndex, Config.DIRECT_RIGHT);
				
				if(!slot1)
				{
					return false;
				}
				
				slot2 = m_SlotMotherManager.getSlot(slot1.m_nSlotIndex, Config.DIRECT_RIGHT);
				
				if(!slot2)
				{
					return false;
				}
			}break;
			default:
			{
				return false;
			}
			}

			if(!slot1 ||
			   !slot2)
			{
				return false;
			}
			
			if(!slot1.m_SlotBlock ||
			   !slot2.m_SlotBlock)
			{
				return false;
			}
			
			if(slotBase.m_SlotBlock.m_nColorType == slot1.m_SlotBlock.m_nColorType &&
			   slotBase.m_SlotBlock.m_nColorType == slot2.m_SlotBlock.m_nColorType)
			{
				string index1 = slot1.m_nSlotIndex.ToString();
				string index2 = slot2.m_nSlotIndex.ToString();
					
				Array.Add(index1);
				Array.Add(index2);
				
				return true;
			}
		}

		return false;
	}

	bool checkIfTheSameColorInThisColor(bool AtMid, int ColorType, int Dir, int ExceptIndex)
	{
		if(!m_SlotBlock)
		{
			return false;
		}

		if(!m_SlotBlock.m_BlockBodySprite)
		{
			return false;
		}

		if(m_SlotBlock.m_nColorType == (int)Config.Block_Color_Enum.Block_Color_Null)
		{
			return false;
		}

		SlotBase slot1 = null;
		SlotBase slot2 = null;

		if(AtMid)
		{
			switch(Dir)
			{
			case Config.DIRECT_UP:
			case Config.DIRECT_DOWN:
			{
				slot1 = m_SlotMotherManager.getSlot(m_nSlotIndex, Config.DIRECT_DOWN);
				slot2 = m_SlotMotherManager.getSlot(m_nSlotIndex, Config.DIRECT_UP);
			}break;
			case Config.DIRECT_LEFT:
			case Config.DIRECT_RIGHT:
			{
				slot1 = m_SlotMotherManager.getSlot(m_nSlotIndex, Config.DIRECT_LEFT);
				slot2 = m_SlotMotherManager.getSlot(m_nSlotIndex, Config.DIRECT_RIGHT);
			}break;
			default:
			{
				return false;
			}
			}

			if(!slot1 || !slot2)
			{
				return false;
			}

			if(!slot1.m_SlotBlock || !slot2.m_SlotBlock)
			{
				return false;
			}

			if(slot1.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal &&
			   slot2.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal)
			{
				return false;
			}

			if(!slot1.m_SlotBlock.m_BlockBodySprite || !slot2.m_SlotBlock.m_BlockBodySprite)
			{
				return false;
			}

			if(slot1.m_nSlotIndex == ExceptIndex ||
			   slot2.m_nSlotIndex == ExceptIndex)
			{
				return false;
			}

			if(ColorType == slot1.m_SlotBlock.m_nColorType &&
			   ColorType == slot2.m_SlotBlock.m_nColorType)
			{
				m_SlotMotherManager.addCanEraseSlot(slot1.m_nSlotIndex);
				m_SlotMotherManager.addCanEraseSlot(slot2.m_nSlotIndex);

				return true;
			}
		}
		else
		{
			switch(Dir)
			{
			case Config.DIRECT_DOWN:
			{
				slot1 = m_SlotMotherManager.getSlot(m_nSlotIndex, Config.DIRECT_DOWN);

				if(!slot1)
				{
					return false;
				}

				slot2 = m_SlotMotherManager.getSlot(slot1.m_nSlotIndex, Config.DIRECT_DOWN);

				if(!slot2)
				{
					return false;
				}
			}break;
			case Config.DIRECT_UP:
			{
				slot1 = m_SlotMotherManager.getSlot(m_nSlotIndex, Config.DIRECT_UP);
				
				if(!slot1)
				{
					return false;
				}
				
				slot2 = m_SlotMotherManager.getSlot(slot1.m_nSlotIndex, Config.DIRECT_UP);
				
				if(!slot2)
				{
					return false;
				}
			}break;
			case Config.DIRECT_LEFT:
			{
				slot1 = m_SlotMotherManager.getSlot(m_nSlotIndex, Config.DIRECT_LEFT);
				
				if(!slot1)
				{
					return false;
				}
				
				slot2 = m_SlotMotherManager.getSlot(slot1.m_nSlotIndex, Config.DIRECT_LEFT);
				
				if(!slot2)
				{
					return false;
				}
			}break;
			case Config.DIRECT_RIGHT:
			{
				slot1 = m_SlotMotherManager.getSlot(m_nSlotIndex, Config.DIRECT_RIGHT);
				
				if(!slot1)
				{
					return false;
				}
				
				slot2 = m_SlotMotherManager.getSlot(slot1.m_nSlotIndex, Config.DIRECT_RIGHT);
				
				if(!slot2)
				{
					return false;
				}
			}break;
			default:
			{
				return false;
			}
			}

			if(!slot1.m_SlotBlock || !slot2.m_SlotBlock)
			{
				return false;
			}
			
			if(slot1.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal &&
			   slot2.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal)
			{
				return false;
			}
			
			if(!slot1.m_SlotBlock.m_BlockBodySprite || !slot2.m_SlotBlock.m_BlockBodySprite)
			{
				return false;
			}
			
			if(slot1.m_nSlotIndex == ExceptIndex ||
			   slot2.m_nSlotIndex == ExceptIndex)
			{
				return false;
			}
			
			if(ColorType == slot1.m_SlotBlock.m_nColorType &&
			   ColorType == slot2.m_SlotBlock.m_nColorType)
			{
				m_SlotMotherManager.addCanEraseSlot(slot1.m_nSlotIndex);
				m_SlotMotherManager.addCanEraseSlot(slot2.m_nSlotIndex);
				
				return true;
			}
		}

		return false;
	}

	override public void setBKISprite()
	{
		if(m_SlotBKSprite)
		{
			m_SlotBKSprite.spriteName = "UI_InGame_Square_1";
		}
	}

	override public bool ifCanBeErase()
	{
		if(!m_SlotBlock)
		{
			return false;
		}

		if(m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal)
		{
			return false;
		}

		bool ifCanErase = false;

		tempArray.Clear ();

		ifCanErase = checkIfTheSameColor (m_nSlotIndex, true, Config.DIRECT_LEFT, tempArray);

		if(ifCanErase)
		{
			return true;
		}

		ifCanErase = checkIfTheSameColor (m_nSlotIndex, true, Config.DIRECT_UP, tempArray);
		
		if(ifCanErase)
		{
			return true;
		}

		ifCanErase = checkIfTheSameColor (m_nSlotIndex, false, Config.DIRECT_LEFT, tempArray);
		
		if(ifCanErase)
		{
			return true;
		}

		ifCanErase = checkIfTheSameColor (m_nSlotIndex, false, Config.DIRECT_RIGHT, tempArray);
		
		if(ifCanErase)
		{
			return true;
		}

		ifCanErase = checkIfTheSameColor (m_nSlotIndex, false, Config.DIRECT_UP, tempArray);
		
		if(ifCanErase)
		{
			return true;
		}

		ifCanErase = checkIfTheSameColor (m_nSlotIndex, false, Config.DIRECT_DOWN, tempArray);
		
		if(ifCanErase)
		{
			return true;
		}

		return false;
	}

	override public bool isCanMoveBlockOut()
	{
		if(!m_SlotBlock)
		{
			return false;
		}

		return true;
	}
	
	override public void setMoveEnergy(int Dir, bool Energy)
	{
		if(Dir == (int)Config.DIRECT_LEFT)
		{
			hasMoveLeftEnergy = Energy;
		}
		else if(Dir == (int)Config.DIRECT_RIGHT)
		{
			hasMoveRightEnergy = Energy;
		}
	}

	override public void disbuteMoveEnergy(int Dir, int MoveDir)
	{
		if(isHasMoveEnergy(MoveDir))
		{
			SlotBase slot1 = m_SlotMotherManager.getSlot(m_nSlotIndex, Dir);

			while(slot1)
			{
				bool needBreak = false;

				if(!slot1.m_SlotBlock ||
				   !slot1.m_SlotBlock.m_bIsCanMove ||
				   slot1.isHasMoveEnergy(MoveDir))
				{
					needBreak = true;
				}

				if(needBreak)
				{
					break;
				}

				slot1.setMoveEnergy(MoveDir, true);

				if(Dir == Config.DIRECT_DOWN)
				{
					SlotBase slotLeft = m_SlotMotherManager.getSlot(m_nSlotIndex, Config.DIRECT_LEFT);

					if(slotLeft &&
					   slotLeft.m_SlotBlock &&
					   slotLeft.m_SlotBlock.m_bIsCanMove &&
					   !slotLeft.isHasMoveEnergy(Config.DIRECT_LEFT))
					{
						slotLeft.setMoveEnergy(Config.DIRECT_LEFT, true);
						slotLeft.disbuteMoveEnergy(Config.DIRECT_LEFT, Config.DIRECT_LEFT);
					}

					SlotBase slotRight = m_SlotMotherManager.getSlot(m_nSlotIndex, Config.DIRECT_RIGHT);
					
					if(slotRight &&
					   slotRight.m_SlotBlock &&
					   slotRight.m_SlotBlock.m_bIsCanMove &&
					   !slotRight.isHasMoveEnergy(Config.DIRECT_RIGHT))
					{
						slotRight.setMoveEnergy(Config.DIRECT_RIGHT, true);
						slotRight.disbuteMoveEnergy(Config.DIRECT_RIGHT, Config.DIRECT_RIGHT);
					}

					SlotBase slotLeft2 = m_SlotMotherManager.getSlot(slot1.m_nSlotIndex, Config.DIRECT_LEFT);
					
					if(slotLeft2 &&
					   slotLeft2.m_SlotBlock &&
					   slotLeft2.m_SlotBlock.m_bIsCanMove &&
					   !slotLeft2.isHasMoveEnergy(Config.DIRECT_LEFT))
					{
						slotLeft2.setMoveEnergy(Config.DIRECT_LEFT, true);
						slotLeft2.disbuteMoveEnergy(Config.DIRECT_LEFT, Config.DIRECT_LEFT);
						slotLeft2.disbuteMoveEnergy(Config.DIRECT_DOWN, Config.DIRECT_LEFT);
					}

					SlotBase slotRight2 = m_SlotMotherManager.getSlot(slot1.m_nSlotIndex, Config.DIRECT_RIGHT);
					
					if(slotRight2 &&
					   slotRight2.m_SlotBlock &&
					   slotRight2.m_SlotBlock.m_bIsCanMove &&
					   !slotRight2.isHasMoveEnergy(Config.DIRECT_RIGHT))
					{
						slotRight2.setMoveEnergy(Config.DIRECT_RIGHT, true);
						slotRight2.disbuteMoveEnergy(Config.DIRECT_RIGHT, Config.DIRECT_RIGHT);
						slotRight2.disbuteMoveEnergy(Config.DIRECT_DOWN, Config.DIRECT_RIGHT);
					}
				}
				else if(Dir == Config.DIRECT_LEFT ||
				        Dir == Config.DIRECT_RIGHT)
				{
					SlotBase slotDown = m_SlotMotherManager.getSlot(slot1.m_nSlotIndex, Config.DIRECT_DOWN);

					if(slotDown &&
					   slotDown.m_SlotBlock &&
					   slotDown.m_SlotBlock.m_bIsCanMove &&
					   !slotDown.isHasMoveEnergy(Config.DIRECT_LEFT))
					{
						slotDown.setMoveEnergy(Config.DIRECT_LEFT, true);
						slotDown.disbuteMoveEnergy(Config.DIRECT_DOWN, Config.DIRECT_LEFT);
						slotDown.disbuteMoveEnergy(Config.DIRECT_LEFT, Config.DIRECT_LEFT);
					}

					if(slotDown &&
					   slotDown.m_SlotBlock &&
					   slotDown.m_SlotBlock.m_bIsCanMove &&
					   !slotDown.isHasMoveEnergy(Config.DIRECT_RIGHT))
					{
						slotDown.setMoveEnergy(Config.DIRECT_RIGHT, true);
						slotDown.disbuteMoveEnergy(Config.DIRECT_DOWN, Config.DIRECT_RIGHT);
						slotDown.disbuteMoveEnergy(Config.DIRECT_RIGHT, Config.DIRECT_RIGHT);
					}
				}

				slot1 = m_SlotMotherManager.getSlot(slot1.m_nSlotIndex, Dir);
			}
		}
	}

	override public bool isHasMoveEnergy(int Dir)
	{
		if(Dir == (int)Config.DIRECT_LEFT)
		{
			return hasMoveLeftEnergy;
		}
		else if(Dir == (int)Config.DIRECT_RIGHT)
		{
			return hasMoveRightEnergy;
		}
		else if(Dir == (int)Config.DIRECT_DOWN)
		{
			return true;
		}
		
		return false;
	}

	override public bool checkToBeAbility()
	{
		if(checkAbility_MoveFree3())
		{
			return true;
		}
		
		if(checkAbility_MoveFree1())
		{
			return true;
		}
		
		if(checkAbility_MoveAround1())
		{
			return true;
		}
		
		return false;
	}

	override public bool checkToEraseBlock()
	{
		if(!m_SlotBlock)
		{
			return false;
		}

		if(!m_SlotBlock.m_BlockBodySprite)
		{
			return false;
		}

		if(m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal &&
		   m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Bomb_Fruit)
		{
			return false;
		}

		bool ifCanErase = false;

		tempArray.Clear ();

		ifCanErase = checkIfTheSameColor (m_nSlotIndex, true, Config.DIRECT_LEFT, tempArray);

		if(ifCanErase)
		{
			m_bSlotReadyEraseBlock = true;
			return true;
		}

		ifCanErase = checkIfTheSameColor (m_nSlotIndex, true, Config.DIRECT_UP, tempArray);
		
		if(ifCanErase)
		{
			m_bSlotReadyEraseBlock = true;
			return true;
		}

		for(int i = Config.DIRECT_UP; i <= Config.DIRECT_RIGHT; i++)
		{
			ifCanErase = checkIfTheSameColor (m_nSlotIndex, false, i, tempArray);
			
			if(ifCanErase)
			{
				m_bSlotReadyEraseBlock = true;
				return true;
			}
		}

		return false;
	}

	override public void updateSlotWork()
	{
		if(m_SlotBlock &&
		   m_SlotBlock.m_bIsOnTable &&
		   !m_SlotBlock.isNowMoving)
		{
			m_SlotBlock.m_nBelongSlotIndex = m_nSlotIndex;

			if(m_SlotBlock.isBomb())
			{
				BlockBombBase bombBlock = (BlockBombBase)m_SlotBlock;

				if(bombBlock.specialBombInfo.isHoldBomb ||
				   !bombBlock.m_bIsCanMove)
				{
					return;
				}
			}
			else if(!m_SlotBlock.m_bIsCanMove)
			{
				return;
			}

			bool handleFinish = pushBlockToOtherSlot(Config.DIRECT_DOWN);

			if(!handleFinish)
			{
				if(isHasMoveEnergy(Config.DIRECT_LEFT))
				{
					handleFinish = pushBlockToOtherSlot(Config.DIRECT_LEFT);
				}

				if(!handleFinish && isHasMoveEnergy(Config.DIRECT_RIGHT))
				{
					handleFinish = pushBlockToOtherSlot(Config.DIRECT_RIGHT);
				}
			}
		}
	}

	override public bool receiveBlock(BlockBase Block)
	{
		if(m_SlotBlock)
		{
			return false;
		}

		setBlock (Block);
	//	setBlockDirect (Block);

		return true;
	}

	override public bool isCanReceiveBlock(bool FromLR)
	{
		if(m_SlotBlock)
		{
			return false;
		}

		if(FromLR)
		{
			SlotBase upSlot = m_SlotMotherManager.getSlot(m_nSlotIndex, Config.DIRECT_UP);

			while(upSlot)
			{
				if(upSlot.m_nSlotType == (int)Config.Slot_Enum.Slot_Block)
				{
					return true;
				}

				if(upSlot.m_nSlotType == (int)Config.Slot_Enum.Slot_Score)
				{
					return true;
				}

				if(upSlot.m_SlotBlock &&
				   upSlot.m_SlotBlock.isBomb())
				{
					BlockBombBase bombBlock = (BlockBombBase)upSlot.m_SlotBlock;

					if(bombBlock.specialBombInfo.isHoldBomb ||
					   !bombBlock.m_bIsCanMove)
					{
						return true;
					}
				}
				else if(upSlot.m_SlotBlock && !upSlot.m_SlotBlock.m_bIsCanMove)
				{
					return true;
				}

				if(upSlot.m_nSlotType == (int)Config.Slot_Enum.Slot_WarmHole_Start)
				{
					return true;
				}

				if(upSlot.m_nSlotType == (int)Config.Slot_Enum.Slot_WarmHole_End)
				{
					SlotWarmHoleEnd slotEnd = (SlotWarmHoleEnd)upSlot;

					if(slotEnd && slotEnd.isPause)
					{
						return true;
					}
					else if(slotEnd.m_SlotBlock)
					{
						return false;
					}
					else
					{
						return true;
					}
				}

				if(upSlot.isBornSlot())
				{
					return false;
				}

				if(upSlot.m_nSlotType == (int)Config.Slot_Enum.Slot_Born_Field_Item)
				{
					SlotBornFieldItemBlock targetSlot = (SlotBornFieldItemBlock)upSlot;

					if(!targetSlot.stopBornBlock)
					{
						return false;
					}
					else
					{
						return true;
					}
				}

				if(upSlot.m_nSlotType == (int)Config.Slot_Enum.Slot_Born_Product_Block)
				{
					SlotBornProductBlock targetSlot = (SlotBornProductBlock)upSlot;
					
					if(!targetSlot.stopBornBlock)
					{
						return false;
					}
					else
					{
						return true;
					}
				}

				if(upSlot.m_SlotBlock)
				{
					return false;
				}

				upSlot = m_SlotMotherManager.getSlot(upSlot.m_nSlotIndex, Config.DIRECT_UP);
			}
		}

		return true;
	}

	public bool pushBlockToOtherSlot(int Dir)
	{
		if(Dir == Config.DIRECT_NO_DIR)
		{
			return false;
		}

		SlotBase slot = m_SlotMotherManager.getSlot (m_nSlotIndex, Dir);

		if(!slot)
		{
			return false;
		}

		bool fromLR = (Dir == Config.DIRECT_DOWN) ? false : true;

		if(slot.isCanReceiveBlock(fromLR))
		{
			if(slot.receiveBlock(m_SlotBlock))
			{
				if(slot.m_SlotBlock)
				{
					slot.m_SlotBlock.m_bIsMainActionBlock = true;
				}
				m_SlotBlock = null;
				return true;
			}
		}

		return false;
	}

	public bool checkToEraseBlock_For_switch()
	{
		if(!m_SlotBlock)
		{
			return false;
		}

		if(!m_SlotBlock.m_BlockBodySprite)
		{
			return false;
		}

		if(m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal &&
		   m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Bomb_Fruit)
		{
			return false;
		}

		tempArray.Clear ();

		bool ifCanErase = false;

		ifCanErase = checkIfTheSameColor (m_nSlotIndex, true, Config.DIRECT_LEFT, tempArray);

		if(ifCanErase)
		{
			for(int i = 0; i < tempArray.Count; i++)
			{
				int index = int.Parse(tempArray[i]);
				SlotBase slot = m_SlotMotherManager.getSlotByIndex(index);

				if(!slot)
				{
					continue;
				}

				slot.m_bSlotReadyEraseBlock = true;
			}

			m_bSlotReadyEraseBlock = true;
			return true;
		}

		ifCanErase = checkIfTheSameColor (m_nSlotIndex, true, Config.DIRECT_UP, tempArray);
		
		if(ifCanErase)
		{
			for(int i = 0; i < tempArray.Count; i++)
			{
				int index = int.Parse(tempArray[i]);
				SlotBase slot = m_SlotMotherManager.getSlotByIndex(index);
				
				if(!slot)
				{
					continue;
				}
				
				slot.m_bSlotReadyEraseBlock = true;
			}
			
			m_bSlotReadyEraseBlock = true;
			return true;
		}

		for(int k = Config.DIRECT_UP; k <= Config.DIRECT_RIGHT; k++)
		{
			ifCanErase = checkIfTheSameColor (m_nSlotIndex, false, k, tempArray);
			
			if(ifCanErase)
			{
				for(int i = 0; i < tempArray.Count; i++)
				{
					int index = int.Parse(tempArray[i]);
					SlotBase slot = m_SlotMotherManager.getSlotByIndex(index);
					
					if(!slot)
					{
						continue;
					}
					
					slot.m_bSlotReadyEraseBlock = true;
				}
				
				m_bSlotReadyEraseBlock = true;
				return true;
			}
		}

		return false;
	}

	override public bool checkToBeBomb()
	{
		if(!m_SlotBlock)
		{
			return false;
		}

		if(m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal &&
		   m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Bomb_Fruit)
		{
			return false;
		}

		SlotBase slot1 = null;
		SlotBase slot2 = null;
		SlotBase slot3 = null;

		for(int dir = Config.DIRECT_UP; dir <= Config.DIRECT_RIGHT; dir++)
		{
			bool toUp = true;

			slot1 = m_SlotMotherManager.getSlot(m_nSlotIndex, dir);

			if(!slot1)
			{
				continue;
			}

			if(!slot1.m_SlotBlock)
			{
				continue;
			}

			if(slot1.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal &&
			   slot1.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Bomb_Fruit)
			{
				continue;
			}

			int reverseDir = m_SlotMotherManager.getReverseDir(dir);
			slot2 = m_SlotMotherManager.getSlot(slot1.m_nSlotIndex, dir);

			if(!slot2 || !slot2.m_SlotBlock)
			{
				toUp = false;

				slot2 = m_SlotMotherManager.getSlot(m_nSlotIndex, reverseDir);

				if(!slot2)
				{
					continue;
				}
				
				if(!slot2.m_SlotBlock)
				{
					continue;
				}
				
				if(slot2.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal &&
				   slot2.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Bomb_Fruit)
				{
					continue;
				}
			}

			if(toUp)
			{
				slot3 = m_SlotMotherManager.getSlot(m_nSlotIndex, reverseDir);
			}
			else
			{
				slot3 = m_SlotMotherManager.getSlot(slot2.m_nSlotIndex, reverseDir);
			}

			if(!slot3)
			{
				continue;
			}
			
			if(!slot3.m_SlotBlock)
			{
				continue;
			}
			
			if(slot3.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal &&
			   slot3.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Bomb_Fruit)
			{
				continue;
			}

			if(slot1.m_SlotBlock.m_nColorType != m_SlotBlock.m_nColorType ||
			   slot2.m_SlotBlock.m_nColorType != m_SlotBlock.m_nColorType ||
			   slot3.m_SlotBlock.m_nColorType != m_SlotBlock.m_nColorType)
			{
				continue;
			}

			tempSlotArray.Clear();

			SlotBase willBeFruitBombSlot = null;

			bool findBomb = false;

			if(m_SlotBlock.m_bIsMainActionBlock && !findBomb)
			{
				findBomb = true;
				willBeFruitBombSlot = this;
			}
			else
			{
				tempSlotArray.Add(this);
			}

			if(slot1.m_SlotBlock.m_bIsMainActionBlock && !findBomb)
			{
				findBomb = true;
				willBeFruitBombSlot = slot1;
			}
			else
			{
				tempSlotArray.Add(slot1);
			}

			if(slot2.m_SlotBlock.m_bIsMainActionBlock && !findBomb)
			{
				findBomb = true;
				willBeFruitBombSlot = slot2;
			}
			else
			{
				tempSlotArray.Add(slot2);
			}

			if(slot3.m_SlotBlock.m_bIsMainActionBlock && !findBomb)
			{
				findBomb = true;
				willBeFruitBombSlot = slot3;
			}
			else
			{
				tempSlotArray.Add(slot3);
			}

			for(int k = 0; k < tempSlotArray.Count; k++)
			{
				SlotBase slot = tempSlotArray[k];

				if(!slot)
				{
					continue;
				}

				slot.blockEraseDirect(false, true);
			}

			if(willBeFruitBombSlot)
			{
				if(willBeFruitBombSlot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
				{
					BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)willBeFruitBombSlot.m_SlotBlock;
					fruitBlock.block_blockDoBomb(false);
				}
				else
				{
					willBeFruitBombSlot.changeBlockType((int)Config.Block_Enum.Block_Bomb_Fruit);
					SoundManager.Instance.PlaySE("Sound_To_Fruit_Bomb", false);
				}

				willBeFruitBombSlot.checkAroundBlockAfterReleaseBlock(false);
			}

			return true;
		}

		return false;
	}

	override public bool ifCanBeEraseInThisColor(int ColorType, int ExceptIndex)
	{
		if(ColorType == (int)Config.Block_Color_Enum.Block_Color_Null)
		{
			return false;
		}

		if(!m_SlotBlock)
		{
			return false;
		}

		if(!m_SlotBlock.m_BlockBodySprite)
		{
			return false;
		}

		if(m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal)
		{
			return false;
		}

		bool isCanErase = false;

		m_SlotMotherManager.clearCanEraseSlot ();
		isCanErase = checkIfTheSameColorInThisColor (true, ColorType, Config.DIRECT_LEFT, ExceptIndex);

		if(isCanErase)
		{
			m_SlotMotherManager.addCanEraseSlot(ExceptIndex);
			return true;
		}

		m_SlotMotherManager.clearCanEraseSlot ();
		isCanErase = checkIfTheSameColorInThisColor (true, ColorType, Config.DIRECT_UP, ExceptIndex);
		
		if(isCanErase)
		{
			m_SlotMotherManager.addCanEraseSlot(ExceptIndex);
			return true;
		}
	
		m_SlotMotherManager.clearCanEraseSlot ();
		isCanErase = checkIfTheSameColorInThisColor (false, ColorType, Config.DIRECT_LEFT, ExceptIndex);
		
		if(isCanErase)
		{
			m_SlotMotherManager.addCanEraseSlot(ExceptIndex);
			return true;
		}

		m_SlotMotherManager.clearCanEraseSlot ();
		isCanErase = checkIfTheSameColorInThisColor (false, ColorType, Config.DIRECT_RIGHT, ExceptIndex);
		
		if(isCanErase)
		{
			m_SlotMotherManager.addCanEraseSlot(ExceptIndex);
			return true;
		}

		m_SlotMotherManager.clearCanEraseSlot ();
		isCanErase = checkIfTheSameColorInThisColor (false, ColorType, Config.DIRECT_UP, ExceptIndex);
		
		if(isCanErase)
		{
			m_SlotMotherManager.addCanEraseSlot(ExceptIndex);
			return true;
		}

		m_SlotMotherManager.clearCanEraseSlot ();
		isCanErase = checkIfTheSameColorInThisColor (false, ColorType, Config.DIRECT_DOWN, ExceptIndex);
		
		if(isCanErase)
		{
			m_SlotMotherManager.addCanEraseSlot(ExceptIndex);
			return true;
		}

		return false;
	}
	
	bool checkAbility_MoveFree3()
	{
		tempArray.Clear ();

		for(int i = Config.DIRECT_UP; i <= Config.DIRECT_RIGHT; i++)
		{
			checkIfTheSameColor(m_nSlotIndex, false, i, tempArray);

			if(tempArray.Count == 6)
			{
				break;
			}
		}

		if(tempArray.Count == 6)
		{
			bool isFindBigBomb = false;
			SlotBase willBeBigBombSlot = null;

			tempSlotArray.Clear();

			for(int i = 0; i < tempArray.Count; i++)
			{
				int index = int.Parse(tempArray[i]);
				SlotBase slot = m_SlotMotherManager.getSlotByIndex(index);

				if(!slot)
				{
					continue;
				}

				if(!slot.m_SlotBlock)
				{
					continue;
				}

				if(slot.m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
				{
					willBeBigBombSlot = this;
					isFindBigBomb = true;
				}
				else
				{
					tempSlotArray.Add(slot);
				}
			}

			for(int k = 0; k < tempSlotArray.Count; k++)
			{
				SlotBase slot = tempSlotArray[k];

				if(!slot)
				{
					continue;
				}

				slot.blockEraseDirect(false, true);
			}

			if(willBeBigBombSlot)
			{
				if(willBeBigBombSlot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
				{
					BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)willBeBigBombSlot.m_SlotBlock;
					fruitBlock.block_blockDoBomb(false);
				}

				willBeBigBombSlot.changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
			}

			if(m_SlotBlock)
			{
				if(m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
				{
					if(m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
					{
						BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)m_SlotBlock;
						fruitBlock.block_blockDoBomb(false);
					}

					changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
					isFindBigBomb = true;
				}
				else
				{
					blockEraseDirect(false, true);
				}
			}

			return true;
		}

		return false;
	}

	bool checkAbility_MoveFree1()
	{
		tempArray.Clear ();

		if(checkIfTheSameColor(m_nSlotIndex, false, Config.DIRECT_LEFT, tempArray))
		{
			checkIfTheSameColor(m_nSlotIndex, false, Config.DIRECT_RIGHT, tempArray);

			if(tempArray.Count == 4)
			{
				checkAndEraseBlocks(tempArray, true);
				return true;
			}
		}

		tempArray.Clear ();
		
		if(checkIfTheSameColor(m_nSlotIndex, false, Config.DIRECT_DOWN, tempArray))
		{
			checkIfTheSameColor(m_nSlotIndex, false, Config.DIRECT_UP, tempArray);
			
			if(tempArray.Count == 4)
			{
				checkAndEraseBlocks(tempArray, true);
				return true;
			}
		}

		return false;
	}

	bool checkAbility_MoveAround1()
	{
		tempArray.Clear ();

		for(int i = Config.DIRECT_UP; i <= Config.DIRECT_RIGHT; i++)
		{
			checkIfTheSameColor(m_nSlotIndex, false, i, tempArray);

			if(tempArray.Count == 4)
			{
				break;
			}
		}

		if(tempArray.Count == 4)
		{
			checkAndEraseBlocks(tempArray, false);
			return true;
		}

		tempArray.Clear ();

		SlotBase slotLeft = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_LEFT);
		SlotBase slotRight = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_RIGHT);

		if(slotLeft && slotRight)
		{
			if(slotLeft.m_SlotBlock && slotRight.m_SlotBlock)
			{
				if(slotLeft.m_SlotBlock.m_nColorType != (int)Config.Block_Color_Enum.Block_Color_Null &&
				   slotRight.m_SlotBlock.m_nColorType != (int)Config.Block_Color_Enum.Block_Color_Null)
				{
					if(slotLeft.m_SlotBlock.m_nColorType == m_SlotBlock.m_nColorType &&
					   slotRight.m_SlotBlock.m_nColorType == m_SlotBlock.m_nColorType)
					{
						string sIndex1 = slotLeft.m_nSlotIndex.ToString();
						string sIndex2 = slotRight.m_nSlotIndex.ToString();

						tempArray.Add(sIndex1);
						tempArray.Add(sIndex2);
					}

					bool find = false;

					if(checkIfTheSameColor(m_nSlotIndex, false, Config.DIRECT_DOWN, tempArray))
					{
						find = true;
					}

					if(!find)
					{
						if(checkIfTheSameColor(m_nSlotIndex, false, Config.DIRECT_UP, tempArray))
						{
							find = true;
						}
					}

					if(tempArray.Count == 4)
					{
						checkAndEraseBlocks(tempArray, false);
						return true;
					}
				}
			}
		}

		tempArray.Clear ();
		
		SlotBase slotUp = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_UP);
		SlotBase slotDown = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_DOWN);
		
		if(slotUp && slotDown)
		{
			if(slotUp.m_SlotBlock && slotDown.m_SlotBlock)
			{
				if(slotUp.m_SlotBlock.m_nColorType != (int)Config.Block_Color_Enum.Block_Color_Null &&
				   slotDown.m_SlotBlock.m_nColorType != (int)Config.Block_Color_Enum.Block_Color_Null)
				{
					if(slotUp.m_SlotBlock.m_nColorType == m_SlotBlock.m_nColorType &&
					   slotDown.m_SlotBlock.m_nColorType == m_SlotBlock.m_nColorType)
					{
						string sIndex1 = slotUp.m_nSlotIndex.ToString();
						string sIndex2 = slotDown.m_nSlotIndex.ToString();
						
						tempArray.Add(sIndex1);
						tempArray.Add(sIndex2);
					}
					
					bool find = false;
					
					if(checkIfTheSameColor(m_nSlotIndex, false, Config.DIRECT_LEFT, tempArray))
					{
						find = true;
					}
					
					if(!find)
					{
						if(checkIfTheSameColor(m_nSlotIndex, false, Config.DIRECT_RIGHT, tempArray))
						{
							find = true;
						}
					}
					
					if(tempArray.Count == 4)
					{
						checkAndEraseBlocks(tempArray, false);
						return true;
					}
				}
			}
		}

		return false;
	}

	override public bool checkToBeAbility_ForFall()
	{
		if(checkAbility_MoveFree3_For_Fall())
		{
			return true;
		}
		
		if(checkAbility_MoveFree1_For_Fall())
		{
			return true;
		}
		
		if(checkAbility_MoveAround1())
		{
			return true;
		}
		
		return false;
	}

	bool checkAbility_MoveFree3_For_Fall()
	{
		tempArray.Clear ();

		int myIndex = m_nSlotIndex;

		if(checkIfTheSameColor(myIndex, false, Config.DIRECT_RIGHT, tempArray))
		{
			SlotBase slotTemp = m_SlotMotherManager.getSlotByIndex(myIndex + 2);

			if(slotTemp)
			{
				if(slotTemp.m_SlotBlock)
				{
					checkIfTheSameColor(slotTemp.m_nSlotIndex, false, Config.DIRECT_RIGHT, tempArray);
					checkIfTheSameColor(slotTemp.m_nSlotIndex, false, Config.DIRECT_DOWN, tempArray);

					bool enough = false;
					if(tempArray.Count == 6)
					{
						enough = true;
					}

					if(!enough)
					{
						checkIfTheSameColor(slotTemp.m_nSlotIndex, false, Config.DIRECT_UP, tempArray);
					}

					if(tempArray.Count == 6)
					{
						checkAndEraseBlocks(tempArray, true);
						return true;
					}
				}
			}
		}

		tempArray.Clear ();

		if(checkIfTheSameColor(myIndex, false, Config.DIRECT_UP, tempArray))
		{
			SlotBase slotTemp = m_SlotMotherManager.getSlotByIndex(myIndex + (2 * Config.WIDTH_NUM));

			if(slotTemp)
			{
				if(slotTemp.m_SlotBlock)
				{
					checkIfTheSameColor(slotTemp.m_nSlotIndex, false, Config.DIRECT_RIGHT, tempArray);
					checkIfTheSameColor(slotTemp.m_nSlotIndex, false, Config.DIRECT_LEFT, tempArray);

					bool enough = false;
					if(tempArray.Count == 6)
					{
						enough = true;
					}

					if(!enough)
					{
						checkIfTheSameColor(slotTemp.m_nSlotIndex, false, Config.DIRECT_UP, tempArray);
					}

					if(tempArray.Count == 6)
					{
						checkAndEraseBlocks(tempArray, true);
						return true;
					}
				}
			}
		}

		return false;
	}

	bool checkAbility_MoveFree1_For_Fall()
	{
		SlotBase slot1 = null;
		SlotBase slot2 = null;
		SlotBase slot3 = null;
		SlotBase slot4 = null;

		slot1 = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_RIGHT);

		if(slot1)
		{
			slot2 = m_SlotMotherManager.getSlot (slot1.m_nSlotIndex, Config.DIRECT_RIGHT);

			if(slot2)
			{
				slot3 = m_SlotMotherManager.getSlot (slot2.m_nSlotIndex, Config.DIRECT_RIGHT);

				if(slot3)
				{
					slot4 = m_SlotMotherManager.getSlot (slot3.m_nSlotIndex, Config.DIRECT_RIGHT);
				}
			}
		}

		if(slot1 && slot2 && slot3 && slot4)
		{
			if(slot1.m_SlotBlock && slot2.m_SlotBlock && slot3.m_SlotBlock && slot4.m_SlotBlock)
			{
				if(slot1.m_SlotBlock.m_nColorType != (int)Config.Block_Color_Enum.Block_Color_Null &&
				   slot2.m_SlotBlock.m_nColorType != (int)Config.Block_Color_Enum.Block_Color_Null &&
				   slot3.m_SlotBlock.m_nColorType != (int)Config.Block_Color_Enum.Block_Color_Null &&
				   slot4.m_SlotBlock.m_nColorType != (int)Config.Block_Color_Enum.Block_Color_Null)
				{
					if(slot1.m_SlotBlock.m_nColorType == m_SlotBlock.m_nColorType &&
					   slot2.m_SlotBlock.m_nColorType == m_SlotBlock.m_nColorType &&
					   slot3.m_SlotBlock.m_nColorType == m_SlotBlock.m_nColorType &&
					   slot4.m_SlotBlock.m_nColorType == m_SlotBlock.m_nColorType)
					{
						bool isFindBigBomb = false;

						if(slot1.m_SlotBlock)
						{
							if(slot1.m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
							{
								if(slot1.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
								{
									BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)slot1.m_SlotBlock;
									fruitBlock.block_blockDoBomb(false);
								}

								slot1.changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
								isFindBigBomb = true;

								slot1.checkAroundBlockAfterReleaseBlock(false);
							}
							else
							{
								slot1.blockEraseDirect(false, true);
							}
						}

						if(slot2.m_SlotBlock)
						{
							if(slot2.m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
							{
								if(slot2.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
								{
									BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)slot2.m_SlotBlock;
									fruitBlock.block_blockDoBomb(false);
								}
								
								slot2.changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
								isFindBigBomb = true;
								
								slot2.checkAroundBlockAfterReleaseBlock(false);
							}
							else
							{
								slot2.blockEraseDirect(false, true);
							}
						}

						if(slot3.m_SlotBlock)
						{
							if(slot3.m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
							{
								if(slot3.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
								{
									BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)slot3.m_SlotBlock;
									fruitBlock.block_blockDoBomb(false);
								}
								
								slot3.changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
								isFindBigBomb = true;
								
								slot3.checkAroundBlockAfterReleaseBlock(false);
							}
							else
							{
								slot3.blockEraseDirect(false, true);
							}
						}

						if(slot4.m_SlotBlock)
						{
							if(slot4.m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
							{
								if(slot4.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
								{
									BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)slot4.m_SlotBlock;
									fruitBlock.block_blockDoBomb(false);
								}
								
								slot4.changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
								isFindBigBomb = true;
								
								slot4.checkAroundBlockAfterReleaseBlock(false);
							}
							else
							{
								slot4.blockEraseDirect(false, true);
							}
						}

						if(m_SlotBlock)
						{
							if(m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
							{
								if(m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
								{
									BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)m_SlotBlock;
									fruitBlock.block_blockDoBomb(false);
								}
								
								changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
								isFindBigBomb = true;
								
								checkAroundBlockAfterReleaseBlock(false);
							}
							else
							{
								blockEraseDirect(false, true);
							}
						}

						return true;
					}
				}
			}
		}



		SlotBase slot5 = null;
		SlotBase slot6 = null;
		SlotBase slot7 = null;
		SlotBase slot8 = null;
		
		slot5 = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_UP);
		
		if(slot5)
		{
			slot6 = m_SlotMotherManager.getSlot (slot5.m_nSlotIndex, Config.DIRECT_UP);
			
			if(slot6)
			{
				slot7 = m_SlotMotherManager.getSlot (slot6.m_nSlotIndex, Config.DIRECT_UP);
				
				if(slot7)
				{
					slot8 = m_SlotMotherManager.getSlot (slot7.m_nSlotIndex, Config.DIRECT_UP);
				}
			}
		}
		
		if(slot5 && slot6 && slot7 && slot8)
		{
			if(slot5.m_SlotBlock && slot6.m_SlotBlock && slot7.m_SlotBlock && slot8.m_SlotBlock)
			{
				if(slot5.m_SlotBlock.m_nColorType != (int)Config.Block_Color_Enum.Block_Color_Null &&
				   slot6.m_SlotBlock.m_nColorType != (int)Config.Block_Color_Enum.Block_Color_Null &&
				   slot7.m_SlotBlock.m_nColorType != (int)Config.Block_Color_Enum.Block_Color_Null &&
				   slot8.m_SlotBlock.m_nColorType != (int)Config.Block_Color_Enum.Block_Color_Null)
				{
					if(slot5.m_SlotBlock.m_nColorType == m_SlotBlock.m_nColorType &&
					   slot6.m_SlotBlock.m_nColorType == m_SlotBlock.m_nColorType &&
					   slot7.m_SlotBlock.m_nColorType == m_SlotBlock.m_nColorType &&
					   slot8.m_SlotBlock.m_nColorType == m_SlotBlock.m_nColorType)
					{
						bool isFindBigBomb = false;
						
						if(slot5.m_SlotBlock)
						{
							if(slot5.m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
							{
								if(slot5.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
								{
									BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)slot5.m_SlotBlock;
									fruitBlock.block_blockDoBomb(false);
								}
								
								slot5.changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
								isFindBigBomb = true;
								
								slot5.checkAroundBlockAfterReleaseBlock(false);
							}
							else
							{
								slot5.blockEraseDirect(false, true);
							}
						}
						
						if(slot6.m_SlotBlock)
						{
							if(slot6.m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
							{
								if(slot6.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
								{
									BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)slot6.m_SlotBlock;
									fruitBlock.block_blockDoBomb(false);
								}
								
								slot6.changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
								isFindBigBomb = true;
								
								slot6.checkAroundBlockAfterReleaseBlock(false);
							}
							else
							{
								slot6.blockEraseDirect(false, true);
							}
						}
						
						if(slot7.m_SlotBlock)
						{
							if(slot7.m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
							{
								if(slot7.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
								{
									BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)slot7.m_SlotBlock;
									fruitBlock.block_blockDoBomb(false);
								}
								
								slot7.changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
								isFindBigBomb = true;
								
								slot7.checkAroundBlockAfterReleaseBlock(false);
							}
							else
							{
								slot7.blockEraseDirect(false, true);
							}
						}
						
						if(slot8.m_SlotBlock)
						{
							if(slot8.m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
							{
								if(slot8.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
								{
									BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)slot8.m_SlotBlock;
									fruitBlock.block_blockDoBomb(false);
								}
								
								slot8.changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
								isFindBigBomb = true;
								
								slot8.checkAroundBlockAfterReleaseBlock(false);
							}
							else
							{
								slot8.blockEraseDirect(false, true);
							}
						}
						
						if(m_SlotBlock)
						{
							if(m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
							{
								if(m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
								{
									BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)m_SlotBlock;
									fruitBlock.block_blockDoBomb(false);
								}
								
								changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
								isFindBigBomb = true;
								
								checkAroundBlockAfterReleaseBlock(false);
							}
							else
							{
								blockEraseDirect(false, true);
							}
						}
						
						return true;
					}
				}
			}
		}

		return false;
	}

	void checkAndEraseBlocks(List< string > array, bool IsBigBomb)
	{
		bool isFindBigBomb = false;
		
		for(int i = 0; i < array.Count; i++)
		{
			int index = int.Parse(array[i]);
			
			SlotBase slot = m_SlotMotherManager.getSlotByIndex(index);
			
			if(!slot)
			{
				continue;
			}
			
			if(!slot.m_SlotBlock)
			{
				continue;
			}
			
			if(slot.m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
			{
				if(slot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
				{
					BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)slot.m_SlotBlock;
					fruitBlock.block_blockDoBomb(false);
				}

				if(IsBigBomb)
				{
					slot.changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
				}
				else
				{
					slot.changeBlockType((int)Config.Block_Enum.Block_Small_Bomb);
				}

				isFindBigBomb = true;
				
				slot.checkAroundBlockAfterReleaseBlock(false);
			}
			else
			{
				slot.blockEraseDirect(false, true);
			}
		}
		
		if(m_SlotBlock)
		{
			if(m_SlotBlock.m_bIsMainActionBlock && !isFindBigBomb)
			{
				if(m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
				{
					BlockBomb_Fruit fruitBlock = (BlockBomb_Fruit)m_SlotBlock;
					fruitBlock.block_blockDoBomb(false);
				}
				
				if(IsBigBomb)
				{
					changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
				}
				else
				{
					changeBlockType((int)Config.Block_Enum.Block_Small_Bomb);
				}

				isFindBigBomb = true;
				
				checkAroundBlockAfterReleaseBlock(false);
			}
			else
			{
				blockEraseDirect(false, true);
			}
		}
	}

	public void bombInRange(int WidthRange, int HeightRange, bool NeedEraseSelf)
	{
		if(WidthRange == 0 ||
		   HeightRange == 0)
		{
			return;
		}

		SlotBase targetSlot = m_SlotMotherManager.getSlotByIndex (m_nSlotIndex);

		if(!targetSlot)
		{
			return;
		}

		normalBlock.Clear ();
		otherBomb.Clear ();

		bool isBigBomb = false;
		
		if(targetSlot.m_SlotBlock)
		{
			if(targetSlot.m_SlotBlock.isBomb())
			{
				if(targetSlot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Big_Bomb)
				{
					isBigBomb = true;
				}

				BlockBombBase bombBlock = (BlockBombBase)targetSlot.m_SlotBlock;

				if(!bombBlock.isBombThisTurn)
				{
					bombBlock.addBombNum(-1);

					if(NeedEraseSelf)
					{
						if(bombBlock.specialBombInfo.bombNum <= 0)
						{
							addSlotAfterCheckTheSame(normalBlock, this);
						}
					}

					bombBlock.isBombThisTurn = true;
				}
			}
		}

		int widthNum = (WidthRange - 1) / 2;
		int heightNum = (HeightRange - 1) / 2;

		if(WidthRange != HeightRange)
		{
			int playAnimateWidth = 0;
			bool isHorizontal = false;
			
			if(WidthRange > HeightRange)
			{
				playAnimateWidth = HeightRange;
				isHorizontal = true;
			}
			else
			{
				playAnimateWidth = WidthRange;
			}
			
			if(playAnimateWidth == 0)
			{
				playAnimateWidth = 1;
			}

			doFarBombAnimation(isHorizontal, playAnimateWidth, isBigBomb);
		}

		getDirBlocks (m_nSlotIndex, Config.DIRECT_UP, heightNum, normalBlock, otherBomb);
		getDirBlocks (m_nSlotIndex, Config.DIRECT_DOWN, heightNum, normalBlock, otherBomb);

		SlotBase leftSlot = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_LEFT);
		SlotBase rightSlot = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_RIGHT);

		for(int width = 0; width < widthNum; width++)
		{
			if(leftSlot)
			{
				if(isCanBeBomb(leftSlot))
				{
					if(leftSlot.m_SlotBlock &&
					   leftSlot.m_SlotBlock.isBomb())
					{
						addSlotAfterCheckTheSame(otherBomb, leftSlot);
					}
					else
					{
						addSlotAfterCheckTheSame(normalBlock, leftSlot);
					}
				}

				getDirBlocks(leftSlot.m_nSlotIndex, Config.DIRECT_UP, heightNum, normalBlock, otherBomb);
				getDirBlocks(leftSlot.m_nSlotIndex, Config.DIRECT_DOWN, heightNum, normalBlock, otherBomb);

				leftSlot = m_SlotMotherManager.getSlot (leftSlot.m_nSlotIndex, Config.DIRECT_LEFT);
			}

			if(rightSlot)
			{
				if(isCanBeBomb(rightSlot))
				{
					if(rightSlot.m_SlotBlock &&
					   rightSlot.m_SlotBlock.isBomb())
					{
						addSlotAfterCheckTheSame(otherBomb, rightSlot);
					}
					else
					{
						addSlotAfterCheckTheSame(normalBlock, rightSlot);
					}
				}
				
				getDirBlocks(rightSlot.m_nSlotIndex, Config.DIRECT_UP, heightNum, normalBlock, otherBomb);
				getDirBlocks(rightSlot.m_nSlotIndex, Config.DIRECT_DOWN, heightNum, normalBlock, otherBomb);
				
				rightSlot = m_SlotMotherManager.getSlot (rightSlot.m_nSlotIndex, Config.DIRECT_RIGHT);
			}
		}

		getDirBlocks (m_nSlotIndex, Config.DIRECT_LEFT, widthNum, normalBlock, otherBomb);
		getDirBlocks (m_nSlotIndex, Config.DIRECT_RIGHT, widthNum, normalBlock, otherBomb);
		
		SlotBase upSlot = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_UP);
		SlotBase downSlot = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_DOWN);
		
		for(int height = 0; height < heightNum; height++)
		{
			if(upSlot)
			{
				if(isCanBeBomb(upSlot))
				{
					if(upSlot.m_SlotBlock &&
					   upSlot.m_SlotBlock.isBomb())
					{
						addSlotAfterCheckTheSame(otherBomb, upSlot);
					}
					else
					{
						addSlotAfterCheckTheSame(normalBlock, upSlot);
					}
				}
				
				getDirBlocks(upSlot.m_nSlotIndex, Config.DIRECT_LEFT, widthNum, normalBlock, otherBomb);
				getDirBlocks(upSlot.m_nSlotIndex, Config.DIRECT_RIGHT, widthNum, normalBlock, otherBomb);
				
				upSlot = m_SlotMotherManager.getSlot (upSlot.m_nSlotIndex, Config.DIRECT_UP);
			}
			
			if(downSlot)
			{
				if(isCanBeBomb(downSlot))
				{
					if(downSlot.m_SlotBlock &&
					   downSlot.m_SlotBlock.isBomb())
					{
						addSlotAfterCheckTheSame(otherBomb, downSlot);
					}
					else
					{
						addSlotAfterCheckTheSame(normalBlock, downSlot);
					}
				}
				
				getDirBlocks(downSlot.m_nSlotIndex, Config.DIRECT_LEFT, widthNum, normalBlock, otherBomb);
				getDirBlocks(downSlot.m_nSlotIndex, Config.DIRECT_RIGHT, widthNum, normalBlock, otherBomb);
				
				downSlot = m_SlotMotherManager.getSlot (downSlot.m_nSlotIndex, Config.DIRECT_DOWN);
			}
		}

		int eraseNum = 0;
		for(int i = 0; i < normalBlock.Count; i++)
		{
			SlotBase slot = normalBlock[i];

			if(!slot)
			{
				continue;
			}

			if(slot.m_nSlotType == (int)Config.Slot_Enum.Slot_Born_Field_Item)
			{
				SlotBornFieldItemBlock fieldItemSlot = (SlotBornFieldItemBlock)slot;
				fieldItemSlot.startBornFieldItem();
			}
			else if(slot.m_nSlotType == (int)Config.Slot_Enum.Slot_Born_Product_Block)
			{
				SlotBornProductBlock fieldItemSlot = (SlotBornProductBlock)slot;
				fieldItemSlot.startBornFieldItem();
			}

			if(!slot.m_SlotBlock)
			{
				continue;
			}

			if(slot.eraseByBomb())
			{
				eraseNum++;
				m_SlotMotherManager.addRecordNum((int)Config.Player_Info_Enum.Player_Info_Bombed_Block_Num, 1);
			}
		}

		if(eraseNum > 0)
		{
			showNumberEffect("UI_InGame_Target_Total_Bomb", eraseNum);
		}

		for(int i = 0; i < otherBomb.Count; i++)
		{
			SlotBase slot = otherBomb[i];
			
			if(!slot)
			{
				continue;
			}

			if(!slot.m_SlotBlock)
			{
				continue;
			}

			if(slot.m_nSlotIndex == m_nSlotIndex)
			{
				continue;
			}

			BlockBombBase bombBlock = (BlockBombBase)slot.m_SlotBlock;

			if(bombBlock.isBombThisTurn)
			{
				continue;
			}

			bombBlock.block_blockDoBomb(true);
		}
	}

	public void addSlotAfterCheckTheSame(List< SlotBase > Array, SlotBase Slot)
	{
		for(int i = 0; i < Array.Count; i++)
		{
			SlotBase mySlot = Array[i];

			if(!mySlot)
			{
				continue;
			}

			if(mySlot.m_nSlotIndex == Slot.m_nSlotIndex)
			{
				return;
			}
		}

		Array.Add (Slot);
	}

	public void getDirBlocks(int StartIndex, int Dir, int SerachNum, List< SlotBase > NormalArray, List< SlotBase > BombArray)
	{
		if(SerachNum == 0)
		{
			return;
		}

		SlotBase targetSlot = m_SlotMotherManager.getSlotByIndex (StartIndex);

		if(!targetSlot)
		{
			return;
		}

		SlotBase nextSlot = m_SlotMotherManager.getSlot (StartIndex, Dir);

		while(nextSlot)
		{
			if(isCanBeBomb(nextSlot))
			{
				if(nextSlot.m_SlotBlock &&
				   nextSlot.m_SlotBlock.isBomb())
				{
					addSlotAfterCheckTheSame(BombArray, nextSlot);
				}
				else
				{
					addSlotAfterCheckTheSame(NormalArray, nextSlot);
				}
			}

			SerachNum--;

			if(SerachNum <= 0)
			{
				break;
			}

			nextSlot = m_SlotMotherManager.getSlot (nextSlot.m_nSlotIndex, Dir);
		}
	}

	public bool isCanBeBomb(SlotBase TargetSlot)
	{
		if(!TargetSlot)
		{
			return false;
		}

		if(TargetSlot.m_nSlotType == (int)Config.Slot_Enum.Slot_Born_Field_Item ||
		   TargetSlot.m_nSlotType == (int)Config.Slot_Enum.Slot_Born_Product_Block)
		{
			return true;
		}
		
		if(!TargetSlot.m_SlotBlock)
		{
			return false;
		}
		
		if(TargetSlot.m_nSlotType != (int)Config.Slot_Enum.Slot_Normal &&
		   TargetSlot.m_nSlotType != (int)Config.Slot_Enum.Slot_WarmHole_Start &&
		   TargetSlot.m_nSlotType != (int)Config.Slot_Enum.Slot_WarmHole_End)
		{
			return false;
		}

		if(TargetSlot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Score ||
		   TargetSlot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Wood ||
		   TargetSlot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Milk)
		{
			if(TargetSlot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Score)
			{
				BlockScore scoreBlock = (BlockScore)TargetSlot.m_SlotBlock;

				if(scoreBlock.isTrash)
				{
					return true;
				}
			}
			
			return false;
		}
		
		return true;
	}

	public void doFarBombAnimation(bool Horizontal, int Width, bool IsBig)
	{
		if(Horizontal && !m_FarBombSprite_H)
		{
			return;
		}
		else if(!Horizontal && !m_FarBombSprite_V)
		{
			return;
		}

		UISprite bombSprite = null;

		if(Horizontal)
		{
			bombSprite = m_FarBombSprite_H;
		}
		else
		{
			bombSprite = m_FarBombSprite_V;
		}

		bombSprite.gameObject.SetActive (true);

		if(IsBig)
		{
			bombSprite.spriteName = "eftExplosion_B1";
			bombSprite.transform.localScale = new Vector3(150, 694, 1);
		}
		else
		{
			bombSprite.spriteName = "eftExplosion_A1";
			bombSprite.transform.localScale = new Vector3(39, 694, 1);
		}

		if(Horizontal)
		{
			bombSprite.gameObject.transform.Rotate(0, 0, 270);
		}

//		bombSprite.transform.localScale = new Vector3 (100, 694, 1);
		bombSprite.transform.localScale = new Vector3(bombSprite.transform.localScale.x * 2, bombSprite.transform.localScale.y * 2, bombSprite.transform.localScale.z * 2);

		Hashtable TmpTable = new Hashtable();
		if(Horizontal)
		{
			if(IsBig)
			{
				TmpTable.Add("y", 0.07f);
			}
			else
			{
				TmpTable.Add("y", 0.04f);
			}
		}
		else
		{
			if(IsBig)
			{
				TmpTable.Add("x", 0.07f);
			}
			else
			{
				TmpTable.Add("x", 0.04f);
			}
		}

		TmpTable.Add("time", 0.7f);
		TmpTable.Add ("oncomplete","afterFarBomb");
		TmpTable.Add ("oncompletetarget",this.gameObject);
		iTween.ShakePosition(bombSprite.gameObject,TmpTable);
	}

	void afterFarBomb()
	{
		if(m_FarBombSprite_H)
		{
			m_FarBombSprite_H.gameObject.SetActive (false);
		}

		if(m_FarBombSprite_V)
		{
			m_FarBombSprite_V.gameObject.SetActive (false);
		}
	}
}
