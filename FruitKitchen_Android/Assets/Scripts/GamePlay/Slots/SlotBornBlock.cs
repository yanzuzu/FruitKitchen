using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotBornBlock : SlotBase {

	public int					 		m_nBornSlotType;
	public bool							m_bIsBorningBlock;
	List< int > preSetBornBlocks_ForTeachMode = new List<int>();

	// Use this for initialization
	void Start () {
		initSlot ();
	}

	override public void initSlot()
	{
		base.initSlot ();
		
		m_nSlotType = (int)Config.Slot_Enum.Slot_Born_Block_All;
		m_nBornSlotType = (int)Config.Born_Slot_Enum.Born_Slot_All;
		m_bIsBorningBlock = false;
	}

	public BlockBase bornBlock(int Type, int Color)
	{
		BlockBase block = null;

		Config.Block_Enum enumType = (Config.Block_Enum)Type;
		switch(enumType)
		{
		case Config.Block_Enum.Block_Normal:
		{
			int color = (int)Config.Block_Color_Enum.Block_Color_Null;

			if(Color == (int)Config.Block_Color_Enum.Block_Color_Null)
			{
				color = m_SlotMotherManager.getRandomExistColor();
			}
			else
			{
				color = Color;
			}

			block = BlockCreater.Instance.getBlockByType(m_SlotMotherManager.m_BlockObj.gameObject, m_SlotMotherManager.m_myPanel, (int)Config.Block_Enum.Block_Normal, color);

		}break;
		case Config.Block_Enum.Block_Produce:
		{
			block = BlockCreater.Instance.getBlockByType(m_SlotMotherManager.m_BlockObj.gameObject, m_SlotMotherManager.m_myPanel, (int)Config.Block_Enum.Block_Produce, (int)Config.Block_Color_Enum.Block_Color_Null);
		}break;
		default:
		{
			return null;
		}
		}

		if(block)
		{
			block.m_bIsMainActionBlock = true;
		}

		return block;
	}

	public void addPreBlocksForTeachMode(int BlockType)
	{
		preSetBornBlocks_ForTeachMode.Add (BlockType);
	}

	override public void setBKISprite()
	{
		if(m_SlotBKSprite)
		{
			m_SlotBKSprite.transform.localScale = new Vector3( 73 , 40 , 0 );

			float x = m_SlotBKSprite.transform.localPosition.x;
			float y = m_SlotBKSprite.transform.localPosition.y;
			float z = m_SlotBKSprite.transform.localPosition.z;
			
			y -= 10;

			m_SlotBKSprite.transform.localPosition = new Vector3(x,y,z);
		}

		Config.Born_Slot_Enum enumType = (Config.Born_Slot_Enum)m_nBornSlotType;
		switch(enumType)
		{
		case Config.Born_Slot_Enum.Born_Slot_All:
		{
			if(m_SlotBKSprite)
			{
				m_SlotBKSprite.spriteName = "UI_InGame_iconEntranceFruit_Knife";
			}
		}break;
		case Config.Born_Slot_Enum.Born_Slot_ProductOnly:
		{
			if(m_SlotBKSprite)
			{
				m_SlotBKSprite.spriteName = "UI_InGame_iconEntranceKnife";
			}
		}break;
		case Config.Born_Slot_Enum.Born_Slot_NormalOnly:
		{
			if(m_SlotBKSprite)
			{
				m_SlotBKSprite.spriteName = "UI_InGame_iconEntranceFruit";
			}
		}break;
		default:{}break;
		}
	}

	override public void updateSlotWork()
	{
		SlotBase downSlot = m_SlotMotherManager.getSlot (m_nSlotIndex, Config.DIRECT_DOWN);

		if(downSlot)
		{
			if(downSlot.isCanReceiveBlock(false))
			{
				if(m_nSlotIndex == 90)
				{
					CPDebug.Log("updateSlotWork111111111");
				}

				Hashtable TmpTable = new Hashtable();
				TmpTable.Add("x", 0.02f);
				TmpTable.Add("time", 0.3f);
				iTween.ShakePosition(m_SlotBKSprite.gameObject,TmpTable);

				BlockBase block = null;

				if(preSetBornBlocks_ForTeachMode.Count > 0)
				{
					int color = (int)Config.Block_Color_Enum.Block_Color_Null;

					int blockType = preSetBornBlocks_ForTeachMode[0];

					Config.Teach_Mode_Id_Meaning_Enum enumType = (Config.Teach_Mode_Id_Meaning_Enum)blockType;
					switch(enumType)
					{
					case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Banana:
					case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Strawberries:
					case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Kiwi:
					case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Grapes:
					case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Mango:
					case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Muskmelon:
					{
						color = blockType;
					}break;
					}

					if(color != (int)Config.Block_Color_Enum.Block_Color_Null)
					{
						block = bornBlock((int)Config.Block_Enum.Block_Normal, color);
					}

					preSetBornBlocks_ForTeachMode.RemoveAt(0);
				}
				else
				{
					if(m_nBornSlotType == (int)Config.Born_Slot_Enum.Born_Slot_All)
					{
						int key = DataManager.Instance.StageKey;
						int knifeMaxNum = 6;//CSVManager.Instance.StageInfoTable.readFieldAsInt(key, "KnifeMaxNum");

						if(m_SlotMotherManager.getBlockNums((int)Config.Block_Enum.Block_Produce) >= knifeMaxNum)
						{
							block = SlotCreater.Instance.getFieldItemWithGoldItem();
						}
						else
						{
							System.Random rnd = new System.Random();
							int random = ((rnd.Next(1000000) + 1) % 100) * 100;

							int rate = 600;//CSVManager.Instance.StageInfoTable.readFieldAsInt(key, "NormalKnifeRate") * 100;

							if(random > rate)
							{
								block = SlotCreater.Instance.getFieldItemWithGoldItem();
							}
							else
							{
								block = bornBlock((int)Config.Block_Enum.Block_Produce, (int)Config.Block_Color_Enum.Block_Color_Null);
							}
						}
					}
					else if(m_nBornSlotType == (int)Config.Born_Slot_Enum.Born_Slot_ProductOnly)
					{
						int key = DataManager.Instance.StageKey;
						int knifeMaxNum = 6;//CSVManager.Instance.StageInfoTable.readFieldAsInt(key, "KnifeMaxNum");

						if(m_SlotMotherManager.getBlockNums((int)Config.Block_Enum.Block_Produce) >= knifeMaxNum)
						{
							block = bornBlock((int)Config.Block_Enum.Block_Normal, (int)Config.Block_Color_Enum.Block_Color_Null);
						}
						else
						{
							System.Random rnd = new System.Random();
							int random = rnd.Next(100) + 1;

							int rate = 25;

							if(random <= rate)
							{
								block = bornBlock((int)Config.Block_Enum.Block_Produce, (int)Config.Block_Color_Enum.Block_Color_Null);
							}
							else
							{
								block = bornBlock((int)Config.Block_Enum.Block_Normal, (int)Config.Block_Color_Enum.Block_Color_Null);
							}
						}
					}
					else if(m_nBornSlotType == (int)Config.Born_Slot_Enum.Born_Slot_NormalOnly)
					{
						block = SlotCreater.Instance.getFieldItemWithGoldItem();
					}
				}

				if(block)
				{
					m_bIsBorningBlock = true;

					if(block.m_BlockBodySprite)
					{
						block.transform.localPosition = m_myGameObject.transform.localPosition;
					}

					downSlot.receiveBlock(block);
				}
			}
			else
			{
				m_bIsBorningBlock = false;
			}
		}
	}
}
