using UnityEngine;
using System.Collections;

public class SlotBornFieldItemBlock : SlotBornBlock {

	public bool stopBornBlock = true;
	public int bornBlockCount;

	void Awake()
	{
		stopBornBlock = true;
		bornBlockCount = 0;
	}

	// Use this for initialization
	void Start () {
		initSlot ();
	}	
	
	// Update is called once per frame
	void Update () {
	
	}

	override public void initSlot()
	{
		base.initSlot ();
		
		m_nSlotType = (int)Config.Slot_Enum.Slot_Born_Field_Item;
		m_nBornSlotType = (int)Config.Born_Slot_Enum.Born_Slot_All;
		m_bIsBorningBlock = false;
		stopBornBlock = true;
	}

	override public void setBKISprite()
	{
		if(m_SlotBKSprite)
		{
			m_SlotBKSprite.spriteName = "UI_InGame_Entrance_2";
		}
	}

	override public void updateSlotWork()
	{
		if(stopBornBlock)
		{
			return;
		}

		SlotBase downSlot = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_DOWN);
		
		if(downSlot)
		{
			if(downSlot.isCanReceiveBlock(false) && 
			   bornBlockCount < Config.BORN_FIELD_ITEM_SLOT_MAX_PER_TIME)
			{
				Hashtable TmpTable = new Hashtable();
				TmpTable.Add("x", 0.02f);
				TmpTable.Add("time", 0.3f);
				iTween.ShakePosition(m_SlotBKSprite.gameObject,TmpTable);

				m_bIsBorningBlock = true;

				BlockBase block = bornARandomBlock();

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

		if(!stopBornBlock && m_SlotMotherManager.ifCanDoOperate)
		{
			stopBornBlock = true;
			bornBlockCount = 0;
		}
	}

	public void startBornFieldItem()
	{
		stopBornBlock = false;
	}

	public BlockBase bornARandomBlock()
	{
		int fieldItemType = getRandomFieldItemType ();

		BlockBase block = BlockCreater.Instance.getBlockByType(m_SlotMotherManager.m_BlockObj, m_SlotMotherManager.m_myPanel, (int)Config.Block_Enum.Block_Field_Item, fieldItemType);
		block.m_bIsMainActionBlock = true;

		if(block.m_BlockBodySprite)
		{
			block.transform.localPosition = m_myGameObject.transform.localPosition;
		}

		return block;
	}

	public int getRandomFieldItemType()
	{
		System.Random rnd = new System.Random();
		int randomIndex = rnd.Next(100) % 3;
		
		if(randomIndex == 0)
		{
			return Config.CARD_ITEM_KNIFE_ERASE;
		}
		else if(randomIndex == 1)
		{
			return Config.CARD_ITEM_BOMB_DOUBLE;
		}
		else
		{
			return Config.CARD_ITEM_BOMB_UP;
		}
	}
}
