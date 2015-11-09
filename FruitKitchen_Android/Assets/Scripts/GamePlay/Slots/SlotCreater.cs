using UnityEngine;
using System.Collections;

public class SlotCreater : MonoBehaviour {

	public static SlotCreater Instance;

	public GamePlayLayer	m_SlotMotherLayer;
	public BlockManager		m_SlotMotherManager;


	void Awake()
	{
		Instance = this;
	}

	public SlotBase getSlotByType(GameObject gameObject, UIGrid grid, int type)
	{
		if(!gameObject ||
		   !grid)
		{
			return null;
		}

		GameObject TmpObj = Instantiate( gameObject.gameObject ) as GameObject;
//		TmpObj.transform.parent = grid.transform;
//		TmpObj.transform.localScale = Vector3.one;
		TmpObj.SetActive( true  );

		Config.Slot_Enum slotType = (Config.Slot_Enum)type;
		switch(slotType)
		{
		case Config.Slot_Enum.Slot_Block:
		{
			SlotBlock slot = TmpObj.GetComponent< SlotBlock >();
			slot.enabled = true;
			slot.m_myGameObject = TmpObj;
			return slot;
		}
		case Config.Slot_Enum.Slot_Normal:
		case Config.Slot_Enum.Slot_Normal_NO_Block:
		case Config.Slot_Enum.Slot_Normal_With_CountDown_Bomb:
		case Config.Slot_Enum.Slot_Normal_With_ConnectOnly_Bomb:
		case Config.Slot_Enum.Slot_Normal_With_WoodBlock:
		case Config.Slot_Enum.Slot_Normal_With_WoodBlock_Break:
		case Config.Slot_Enum.Slot_Normal_With_GrassBlock:
		case Config.Slot_Enum.Slot_Normal_With_SauceBlock:
		case Config.Slot_Enum.Slot_Normal_With_MilkBlock:
		case Config.Slot_Enum.Slot_Normal_With_FieldItem:
		{
			SlotNormal slot = TmpObj.GetComponent< SlotNormal >();
			slot.enabled = true;
			slot.m_myGameObject = TmpObj;
			return slot;
		}
		case Config.Slot_Enum.Slot_Born_Block_All:
		{
			SlotBornBlock slot = TmpObj.GetComponent< SlotBornBlock >();
			slot.enabled = true;
			slot.m_myGameObject = TmpObj;
			slot.m_nBornSlotType = (int)Config.Born_Slot_Enum.Born_Slot_All;
			return slot;
		}
		case Config.Slot_Enum.Slot_Born_Block_Product:
		{
			SlotBornBlock slot = TmpObj.GetComponent< SlotBornBlock >();
			slot.enabled = true;
			slot.m_myGameObject = TmpObj;
	//		slot.m_nBornSlotType = (int)Config.Born_Slot_Enum.Born_Slot_ProductOnly;
			return slot;
		}
		case Config.Slot_Enum.Slot_Born_Block_Normal:
		{
			SlotBornBlock slot = TmpObj.GetComponent< SlotBornBlock >();
			slot.enabled = true;
			slot.m_myGameObject = TmpObj;
			slot.m_nBornSlotType = (int)Config.Born_Slot_Enum.Born_Slot_NormalOnly;
			return slot;
		}
		case Config.Slot_Enum.Slot_Score:
		{
			SlotScore slot = TmpObj.GetComponent< SlotScore >();
			slot.enabled = true;
			slot.m_myGameObject = TmpObj;
			return slot;
		}
		case Config.Slot_Enum.Slot_WarmHole_Start:
		{
			SlotWarmHoleStart slot = TmpObj.GetComponent< SlotWarmHoleStart >();
			slot.enabled = true;
			slot.m_myGameObject = TmpObj;
			return slot;
		}
		case Config.Slot_Enum.Slot_WarmHole_End:
		{
			SlotWarmHoleEnd slot = TmpObj.GetComponent< SlotWarmHoleEnd >();
			slot.enabled = true;
			slot.m_myGameObject = TmpObj;
			return slot;
		}
		case Config.Slot_Enum.Slot_Born_Field_Item:
		{
			SlotBornFieldItemBlock slot = TmpObj.GetComponent< SlotBornFieldItemBlock >();
			slot.enabled = true;
			slot.m_myGameObject = TmpObj;
			return slot;
		}
		case Config.Slot_Enum.Slot_Born_Product_Block:
		{
			SlotBornProductBlock slot = TmpObj.GetComponent< SlotBornProductBlock >();
			slot.enabled = true;
			slot.m_myGameObject = TmpObj;
			return slot;
		}

		}

		return null;
	}

	public BlockBase getFieldItemWithGoldItem()
	{
		int key = DataManager.Instance.StageKey;

		if(key == 0)
		{
			return null;
		}

		System.Random rnd = new System.Random();
		int random = rnd.Next(10000) + 1;

		int lineBombRate = (int)(float.Parse(CSVManager.Instance.StageInfoTable.readFieldAsString(key, "Item_LineBonb")) * 100);
		lineBombRate = 0;
		int bombUpRate = (int)(float.Parse(CSVManager.Instance.StageInfoTable.readFieldAsString(key, "Item_BombUpgrade")) * 100);
		int knifeEraseRate = (int)(float.Parse(CSVManager.Instance.StageInfoTable.readFieldAsString(key, "Item_KnifeErase")) * 100);
		int warmHoleRate = (int)(float.Parse(CSVManager.Instance.StageInfoTable.readFieldAsString(key, "Item_WarmHole")) * 100);
		warmHoleRate = 0;
		int doubleBombRate = (int)(float.Parse(CSVManager.Instance.StageInfoTable.readFieldAsString(key, "Item_DoubtBomb")) * 100);

		int lightRate = 0;
		if(!m_SlotMotherManager.isLockItem((int)Config.Operate_Mode_Enum.Operate_Mode_Lignting))
		{
			lightRate = 20;
		}

		int shovelRate = 0;
		if(!m_SlotMotherManager.isLockItem((int)Config.Operate_Mode_Enum.Operate_Mode_Shovel))
		{
			shovelRate = 20;
		}

		int bomberRate = 0;
		if(!m_SlotMotherManager.isLockItem((int)Config.Operate_Mode_Enum.Operate_Mode_Bomber))
		{
			bomberRate = 10;
		}

		int lineBombNum = lineBombRate;
		int bombUpRateNum = lineBombNum + bombUpRate;
		int knifeEraseNum = bombUpRateNum + knifeEraseRate;
		int warmHoleNum = knifeEraseNum + warmHoleRate;
		int doubleBombNum = warmHoleNum + doubleBombRate;
		int lightNum = doubleBombNum + lightRate;
		int shovelNum = lightNum + shovelRate;
		int bomberNum = shovelNum + bomberRate;

		BlockBase block = null;

		if(random < lineBombNum)
		{
			block = bornFieldItemBlock(Config.CARD_ITEM_BOMB_CONNECT);
		}
		else if(random > lineBombNum &&
		        random <= bombUpRateNum)
		{
			block = bornFieldItemBlock(Config.CARD_ITEM_BOMB_UP);
		}
		else if(random > bombUpRateNum &&
		        random <= knifeEraseNum)
		{
			block = bornFieldItemBlock(Config.CARD_ITEM_KNIFE_ERASE);
		}
		else if(random > knifeEraseNum &&
		        random <= warmHoleNum)
		{
			block = bornFieldItemBlock(Config.CARD_ITEM_WARM_HOLE);
		}
		else if(random > warmHoleNum &&
		        random <= doubleBombNum)
		{
			block = bornFieldItemBlock(Config.CARD_ITEM_BOMB_DOUBLE);
		}
		else if(random > doubleBombNum &&
		        random <= lightNum)
		{
			block = bornFieldItemBlock(Config.GOLD_ITEM_LIGHTING);
		}
		else if(random > lightNum &&
		        random <= shovelNum)
		{
			block = bornFieldItemBlock(Config.GOLD_ITEM_SHOVEL);
		}
		else if(random > shovelNum &&
		        random <= bomberNum)
		{
			block = bornFieldItemBlock(Config.GOLD_ITEM_BOMBER);
		}
		else
		{
			int color = m_SlotMotherManager.getRandomExistColor ();
			block = BlockCreater.Instance.getBlockByType(m_SlotMotherManager.m_BlockObj.gameObject, m_SlotMotherManager.m_myPanel, (int)Config.Block_Enum.Block_Normal, color);
		}

		return block;
	}

	public BlockBase getFieldItemWithOutGoldItem()
	{
		// test
		int key = DataManager.Instance.StageKey;
		
		if(key == 0)
		{
			return null;
		}
		
		System.Random rnd = new System.Random();
		int random = rnd.Next(10000) + 1;
		
		int lineBombRate = (int)(float.Parse(CSVManager.Instance.StageInfoTable.readFieldAsString(key, "Item_LineBonb")) * 100);
		lineBombRate = 0;
		int bombUpRate = (int)(float.Parse(CSVManager.Instance.StageInfoTable.readFieldAsString(key, "Item_BombUpgrade")) * 100);
		int knifeEraseRate = (int)(float.Parse(CSVManager.Instance.StageInfoTable.readFieldAsString(key, "Item_KnifeErase")) * 100);
		int warmHoleRate = (int)(float.Parse(CSVManager.Instance.StageInfoTable.readFieldAsString(key, "Item_WarmHole")) * 100);
		warmHoleRate = 0;
		int doubleBombRate = (int)(float.Parse(CSVManager.Instance.StageInfoTable.readFieldAsString(key, "Item_DoubtBomb")) * 100);
		
		int lineBombNum = 0 + lineBombRate;
		int bombUpRateNum = lineBombNum + bombUpRate;
		int knifeEraseNum = bombUpRateNum + knifeEraseRate;
		int warmHoleNum = knifeEraseNum + warmHoleRate;
		int doubleBombNum = warmHoleNum + doubleBombRate;
		
		BlockBase block = null;

		if(random <= lineBombNum)
		{
			block = bornFieldItemBlock(Config.CARD_ITEM_BOMB_CONNECT);
		}
		else if(random > lineBombNum &&
		        random <= bombUpRateNum)
		{
			block = bornFieldItemBlock(Config.CARD_ITEM_BOMB_UP);
		}
		else if(random > bombUpRateNum &&
		        random <= knifeEraseNum)
		{
			block = bornFieldItemBlock(Config.CARD_ITEM_KNIFE_ERASE);
		}
		else if(random > knifeEraseNum &&
		        random <= warmHoleNum)
		{
			block = bornFieldItemBlock(Config.CARD_ITEM_WARM_HOLE);
		}
		else if(random > warmHoleNum &&
		        random <= doubleBombNum)
		{
			block = bornFieldItemBlock(Config.CARD_ITEM_BOMB_DOUBLE);
		}
		else
		{
			int color = m_SlotMotherManager.getRandomExistColor ();
			block = BlockCreater.Instance.getBlockByType(m_SlotMotherManager.m_BlockObj.gameObject, m_SlotMotherManager.m_myPanel, (int)Config.Block_Enum.Block_Normal, color);
		}
		
		return block;
	}

	public BlockBase bornFieldItemBlock(int Type)
	{
		BlockBase block = BlockCreater.Instance.getBlockByType (m_SlotMotherManager.m_BlockObj, m_SlotMotherManager.m_myPanel, (int)Config.Block_Enum.Block_Field_Item, Type);
		block.m_bIsMainActionBlock = true;

		return block;
	}
}
