using UnityEngine;
using System.Collections;

public class SlotWarmHoleStart : SlotNormal {

	public int myEndSlotIndex = -1;

	// Use this for initialization
	void Start () {
		initSlot ();
	}

	override public void initSlot()
	{
		base.initSlot ();
		
		m_nSlotType = (int)Config.Slot_Enum.Slot_WarmHole_Start;
	}

	public void changeSpriteByIndex(int Index)
	{
		// 100,102,104,106,108,110,112,114,116,118
		if(Index < 100 ||
		   Index > 118 ||
		   Index % 2 == 1)
		{
			return;
		}

		int imageNum = ((Index - 100) / 2) + 1;

		string fileName = string.Format ("UI_InGame_SquareWarmHoleIn_{0}", imageNum);
		m_SlotBKSprite.spriteName = fileName;
	}

	override public void setBKISprite()
	{
		if(m_SlotBKSprite)
		{
			m_SlotBKSprite.spriteName = "UI_InGame_SquareWarmHoleOut";
		}
	}

	override public bool receiveBlock(BlockBase Block)
	{
		if(m_SlotBlock)
		{
			return false;
		}
		
		setBlock (Block);
		
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
					else
					{
						return false;
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

	public void pauseEndWarmHole(bool Pause)
	{
		SlotBase downSlot = m_SlotMotherManager.getSlotByIndex (myEndSlotIndex);

		if(downSlot)
		{
			if(downSlot.m_nSlotType != (int)Config.Slot_Enum.Slot_WarmHole_End)
			{
				return;
			}

			SlotWarmHoleEnd slotEnd = (SlotWarmHoleEnd)downSlot;

			if(slotEnd.myStartSlotIndex != m_nSlotIndex)
			{
				return;
			}

			if(!slotEnd.isPause && Pause)
			{
				slotEnd.isPause = true;
				m_SlotMotherManager.resetAllWarmHoleRecord();
			}
			else if(slotEnd.isPause && !Pause)
			{
				slotEnd.isPause = false;
				m_SlotMotherManager.resetAllWarmHoleRecord();
			}
		}
	}

	override public void updateSlotWork()
	{
		if(!m_SlotBlock)
		{
			return;
		}

		if(!m_SlotBlock.m_bIsCanMove)
		{
			return;
		}

		if(m_SlotBlock.m_nLastWarmStartIndex != m_nSlotIndex ||
		   m_SlotBlock.m_nLastWarmStartIndex == 0)
		{
			if(myEndSlotIndex == -1)
			{
				return;
			}

			SlotBase downSlot = m_SlotMotherManager.getSlotByIndex(myEndSlotIndex);

			if(downSlot)
			{
				if(downSlot.m_nSlotType != (int)Config.Slot_Enum.Slot_WarmHole_End)
				{
					return;
				}

				SlotWarmHoleEnd slotEnd = (SlotWarmHoleEnd)downSlot;

				if(slotEnd.myStartSlotIndex != m_nSlotIndex)
				{
					return;
				}

				if(slotEnd.isCanReceiveBlockByWarmHole())
				{
					m_SlotBlock.m_nLastWarmStartIndex = m_nSlotIndex;

					if(slotEnd.receiveBlock(m_SlotBlock))
					{
						pauseEndWarmHole(false);
						m_SlotBlock = null;
					}
				}
			}

		}
		else
		{
			pauseEndWarmHole(true);
		}
	}
}
