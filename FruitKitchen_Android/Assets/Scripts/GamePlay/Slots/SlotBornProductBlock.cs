using UnityEngine;
using System.Collections;

public class SlotBornProductBlock : SlotBornFieldItemBlock {
	
	// Use this for initialization
	void Awake()
	{
		stopBornBlock = true;
	}

	void Start () {
		initSlot ();
	}

	override public void initSlot()
	{
		base.initSlot ();
		
		m_nSlotType = (int)Config.Slot_Enum.Slot_Born_Product_Block;
		m_nBornSlotType = (int)Config.Born_Slot_Enum.Born_Slot_All;
		stopBornBlock = true;
	}

	override public void setBKISprite()
	{
		if(m_SlotBKSprite)
		{
			m_SlotBKSprite.spriteName = "UI_InGame_Entrance_Knife";
		}
	}

	override public void updateSlotWork()
	{
		if(stopBornBlock)
		{
			return;
		}

		int key = DataManager.Instance.StageKey;
		int knifeMaxNum = CSVManager.Instance.StageInfoTable.readFieldAsInt(key, "KnifeMaxNum");

		if(m_SlotMotherManager.getBlockNums((int)Config.Block_Enum.Block_Produce) < knifeMaxNum)
		{
			SlotBase downSlot = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_DOWN);
			
			if(downSlot)
			{
				if(downSlot.isCanReceiveBlock(false))
				{
					Hashtable TmpTable = new Hashtable();
					TmpTable.Add("x", 0.02f);
					TmpTable.Add("time", 0.3f);
					iTween.ShakePosition(m_SlotBKSprite.gameObject,TmpTable);
					
					m_bIsBorningBlock = true;
					
					BlockBase block = bornProductBlock();
					
					if(block.m_BlockBodySprite)
					{
						block.transform.localPosition = m_myGameObject.transform.localPosition;
					}
					
					downSlot.receiveBlock(block);
				}
				else
				{
					m_bIsBorningBlock = false;
				}
			}
		}
		else
		{
			m_bIsBorningBlock = false;
		}
		
		if(!stopBornBlock && m_SlotMotherManager.ifCanDoOperate)
		{
			stopBornBlock = true;
		}
	}

	BlockBase bornProductBlock()
	{
		BlockBase block = BlockCreater.Instance.getBlockByType(m_SlotMotherManager.m_BlockObj, m_SlotMotherManager.m_myPanel, (int)Config.Block_Enum.Block_Produce, (int)Config.Block_Color_Enum.Block_Color_Null);
		block.m_bIsMainActionBlock = true;
		
		if(block.m_BlockBodySprite)
		{
			block.transform.localPosition = m_myGameObject.transform.localPosition;
		}
		
		return block;
	}
}
