using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class fieldItemRecord
{
	public int fieldItemType = 0;
	public int fieldItemNum = 0;

	public fieldItemRecord(int Type)
	{
		fieldItemType = Type;
		fieldItemNum = Config.PLAYER_DEFAULT_OWN_ITEM_NUM;
	}
	
	public void addFieldItemNum(int Num)
	{
		fieldItemNum += Num;

		if(fieldItemNum < 0)
		{
			fieldItemNum = 0;
		}
	}
}

public class warmHoleCreate : MonoBehaviour
{
	public int	m_nWarmHoleCreateIndex;
	public int	m_nWarmHoleCreateNum;

	public warmHoleCreate(int Index, int Num)
	{
		m_nWarmHoleCreateIndex = Index;
		m_nWarmHoleCreateNum = Num;
	}
}

public class BlockManager : MonoBehaviour {
	[SerializeField]
	public GameObject m_SlotObj;

	[SerializeField]
	public GameObject m_BlockObj;

	[SerializeField]
	public UIGrid m_SlotGridObj;

	[SerializeField]
	private Camera m_UICamera;

	[SerializeField]
	public UIPanel m_myPanel;

	struct myRecordNum
	{
		public int     freeFallNum;            
		public int     bombedNum;              
		public int     fruit1Num;              
		public int     fruit2Num;              
		public int     fruit3Num;              
		public int     fruit4Num;              
		public int     fruit5Num;              
		public int     fruit6Num;              
		public int     eraseFruit1Num;         
		public int     eraseFruit2Num;         
		public int     eraseFruit3Num;         
		public int     eraseFruit4Num;         
		public int     eraseFruit5Num;         
		public int     eraseFruit6Num;         
		public int     useFruitBombNum;        
		public int     useSmallBombNum;        
		public int     useBigBombNum;          
	};
	
	struct systemRecordNum
	{
		public int     born_Coin_Num;         
		public int     born_Knife_Num;        
		public int     born_Bomb_Up;          
		public int     born_Knife_Erase;      
		public int     born_Warm_Hole;         
		public int     born_Bomb_Twice;        
		public int     born_Bomb_Connect;      
		public int     born_Lignting;          
		public int     born_Shovel;            
		public int     born_Bomber;            
		public int     born_Atomic;            
	};

	// member list
	[SerializeField]
	public GamePlayLayer m_GamePlayLayer;

	public bool gameOver = false;
	public bool ifCanDoOperate = false;
	public bool ifStartFindCanErase = false;
	int operateMode = (int)Config.Operate_Mode_Enum.Operate_Mode_Null;
	int operateParam1 = -1;
	int operateParam2 = -1;
	int switchSlotIndex1 = -1;
	int switchSlotIndex2 = -1;
	int tempMainIndex = -1;
	public int nowTouchSlotIndex = -1;
	int ownCoins = 0;
	int refillBlockStep = (int)Config.Refill_Block_Step_Enum.Refill_Block_Null;
	int errorCreateStageNum = Config.MAX_CREATE_STAGE_NUM;
	public int moveLeftNum = 0;
	int checkOperateCount = Config.CHECK_BLOCK_NUM;
	public bool isTeachMode = false;
	bool checkEraseStart = false;
	BlockBase tempBlock1 = null;
	BlockBase tempBlock2 = null;
	myRecordNum recordNum = new myRecordNum();
	
	List< int > lockItemRecords = new List< int >();
	List< int > existBlocks = new List< int >();

	Dictionary< int , SlotBase > AllSlotMap = new Dictionary<int, SlotBase>();

	List< string > canEraseSlot = new List< string >();
	List< fieldItemRecord > fieldItemRecords = new List< fieldItemRecord >();


	DateTime testTime;
	// Use this for initialization
	void Start () 
	{
	//	initBlocks ();
	//	initBlocksForTeachMode (DataManager.Instance.BigStageNum, DataManager.Instance.SmallStageNum);
		SlotCreater.Instance.m_SlotMotherManager = this;
		BlockCreater.Instance.m_SlotMotherManager = this;

		ownCoins = Config.PLAYER_DEFAULT_OWN_COINS;

		initFieldItemInfo ();

		int keyNum = DataManager.Instance.StageKey;
		moveLeftNum = CSVManager.Instance.StageInfoTable.readFieldAsInt(keyNum, "TotalMove");
		m_GamePlayLayer.updatePlayInfo ((int)Config.Player_Info_Enum.Player_Info_Left_Move_Num, moveLeftNum);

		int BananaExist = CSVManager.Instance.StageInfoTable.readFieldAsInt(keyNum, "BananaExist"); 
		if (BananaExist == 1) existBlocks.Add (1);
		int StrawberriesExist = CSVManager.Instance.StageInfoTable.readFieldAsInt(keyNum, "StrawberriesExist"); 
		if (StrawberriesExist == 1) existBlocks.Add (2);
		int KiwiExist = CSVManager.Instance.StageInfoTable.readFieldAsInt(keyNum, "KiwiExist"); 
		if (KiwiExist == 1) existBlocks.Add (3);
		int GrapesExist = CSVManager.Instance.StageInfoTable.readFieldAsInt(keyNum, "GrapesExist"); 
		if (GrapesExist == 1) existBlocks.Add (4);
		int MangoExist = CSVManager.Instance.StageInfoTable.readFieldAsInt(keyNum, "MangoExist"); 
		if (MangoExist == 1) existBlocks.Add (5);
		int MuskmelonExist = CSVManager.Instance.StageInfoTable.readFieldAsInt(keyNum, "MuskmelonExist"); 
		if (MuskmelonExist == 1) existBlocks.Add (6);

	//	InvokeRepeating("updateBlockMove", 0, Config.BLOCK_UPDATE_TIME);
	//	InvokeRepeating("eraseUpdate", 0, (Config.BLOCK_UPDATE_TIME * (float)Config.CHECK_BLOCK_NUM));

		StartCoroutine (__myUpdate());
		StartCoroutine (__myErase());
		InvokeRepeating("startFindCanErase", 0, 5.0f);
	//	StartCoroutine (__deleteBlock());
	}

	IEnumerator __myUpdate()
	{
		while(true)
		{
			yield return new WaitForSeconds(Config.BLOCK_UPDATE_TIME);
			updateBlockMove();
		}
	}

	IEnumerator __enterEraseMode()
	{
		yield return new WaitForSeconds (Config.BLOCK_UPDATE_TIME);
		checkEraseStart = true;
	}

	IEnumerator __myErase()
	{
		while(true)
		{
			yield return new WaitForSeconds(Config.ERASE_UPDATE_TIME);

			if(checkEraseStart)
			{
				eraseUpdate();
			}
		}
	}

/*	void Update()
	{
		if(Time.frameCount % 5 == 0)
		{
			updateBlockMove();
		}

		if (Time.frameCount % (5 * Config.CHECK_BLOCK_NUM) == 0)
		{
			eraseUpdate();
		}
	}*/
	
	void initFieldItemInfo()
	{
		fieldItemRecord record1 = new fieldItemRecord((int)Config.Operate_Mode_Enum.Operate_Mode_Wormhole);
		fieldItemRecords.Add (record1);
		fieldItemRecord record2 = new fieldItemRecord((int)Config.Operate_Mode_Enum.Operate_Mode_Erase_Knife);
		fieldItemRecords.Add (record2);
		fieldItemRecord record3 = new fieldItemRecord((int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Twice);
		fieldItemRecords.Add (record3);
		fieldItemRecord record4 = new fieldItemRecord((int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Connect);
		fieldItemRecords.Add (record4);
		fieldItemRecord record5 = new fieldItemRecord((int)Config.Operate_Mode_Enum.Operate_Mode_Lignting);
		fieldItemRecords.Add (record5);
		fieldItemRecord record6 = new fieldItemRecord((int)Config.Operate_Mode_Enum.Operate_Mode_Shovel);
		fieldItemRecords.Add (record6);
		fieldItemRecord record7 = new fieldItemRecord((int)Config.Operate_Mode_Enum.Operate_Mode_Bomber);
		fieldItemRecords.Add (record7);
		fieldItemRecord record8 = new fieldItemRecord((int)Config.Operate_Mode_Enum.Operate_Mode_Atomic);
		fieldItemRecords.Add (record8);
		fieldItemRecord record9 = new fieldItemRecord((int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Upgrade);
		fieldItemRecords.Add (record9);
	}

	public void initBlocksForTeachMode(int BigStage, int SmallStage)
	{
		if(BigStage <= 0 ||
		   SmallStage <= 0)
		{
			return;
		}

		List< int > stageList = new List<int>();
		
		TextAsset TmpTxt = Resources.Load( string.Format("TxtData/Stage/Stage_{0}/stage_{0}_{1}" , DataManager.Instance.BigStageNum , DataManager.Instance.SmallStageNum ) ) as TextAsset;
		
		string [] SplitResult = TmpTxt.text.Split('\n');
		for ( int i = SplitResult.Length - 1 ; i >= 0 ; i -- )
		{
			if( SplitResult[i].IndexOf('\r') != -1)
			{
				int lenth = SplitResult[i].Length;
				
				if(lenth == 1)
				{
					continue;
				}
			}
			
			if(SplitResult[i].IndexOf('\n') != -1)
			{
				continue;
			}
			
			string [] TmpStr = SplitResult[i].Split(',');
			for( int j = 0 ; j < TmpStr.Length ; j ++ )
			{
				if(TmpStr[j].Length <= 0)
				{
					continue;
				}
				
				stageList.Add(int.Parse(TmpStr[j]));
			}
		}

		for(int i = Config.HEIGHT_NUM - 1; i >= 0; i--)
		{
			for(int j = 0; j < Config.WIDTH_NUM; j++)
			{
				int index = (i * Config.WIDTH_NUM) + j;
				int blockType = stageList[index];

				int slotType = 0;
				int blockColor = 0;
				bool isGoldBlock = false;

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
					slotType = (int)Config.Slot_Enum.Slot_Normal;
					blockColor = blockType;
				}break;
				case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Knife:
				{
					slotType = (int)Config.Slot_Enum.Slot_Normal;
				}break;
				case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Banana_With_Gold:
				case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Strawberries_With_Gold:
				case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Kiwi_With_Gold:
				case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Grapes_With_Gold:
				case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Mango_With_Gold:
				case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Muskmelon_With_Gold:
				{
					slotType = (int)Config.Slot_Enum.Slot_Normal;
					blockColor = blockType - 100;
					isGoldBlock = true;
				}break;
				case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Born:
				{
					slotType = (int)Config.Slot_Enum.Slot_Born_Block_All;
				}break;
				case Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Receive:
				{
					slotType = (int)Config.Slot_Enum.Slot_Score;
				}break;
				default:
				{
					slotType = (int)Config.Slot_Enum.Slot_Block;
				}break;
				}

				SlotBase slot = SlotCreater.Instance.getSlotByType( m_SlotObj.gameObject, m_SlotGridObj, slotType);

				int pos_x = -270 + j * 65;
				int pos_y = -330 + i * 65;
				
				slot.m_SlotRect.center = new Vector3(pos_x,pos_y,0);
				slot.m_SlotRect.width = Config.SLOT_SIZE_WIDTH;
				slot.m_SlotRect.height = Config.SLOT_SIZE_HEIGHT;
				
				slot.m_myGameObject.transform.parent = m_myPanel.transform;
				slot.m_myGameObject.transform.localScale = Vector3.one;
				slot.m_myGameObject.transform.localPosition = new Vector3(pos_x, pos_y, 0);
				
				slot.m_nSlotIndex = index;
				slot.m_SlotMotherManager = this;
				slot.setBKISprite();

				if(slotType == (int)Config.Slot_Enum.Slot_Normal)
				{
					if(blockType == (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Knife)
					{
						BlockBase block = BlockCreater.Instance.getBlockByType(m_BlockObj.gameObject, m_myPanel, (int)Config.Block_Enum.Block_Produce, blockColor);
						slot.setBlock(block);
					}
					else
					{
						BlockBase block = BlockCreater.Instance.getBlockByType(m_BlockObj.gameObject, m_myPanel, (int)Config.Block_Enum.Block_Normal, blockColor);
						slot.setBlock(block);

						if(isGoldBlock)
						{
							BlockNormal normalBlock = (BlockNormal)block;
							normalBlock.setGoldBlock();
						}
					}
				}
			
				AllSlotMap[ index ] = slot;
			//	allSlots.Add(slot);
			}
		}

		if(BigStage == 1 && SmallStage == 1)
		{
			addBornBlockPreBlock(77, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Strawberries);
			addBornBlockPreBlock(77, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Kiwi);
			addBornBlockPreBlock(77, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Muskmelon);
			addBornBlockPreBlock(77, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Muskmelon);

			addBornBlockPreBlock(78, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Grapes);
			addBornBlockPreBlock(78, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Mango);
			addBornBlockPreBlock(78, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Banana);
			addBornBlockPreBlock(78, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Mango);

			addBornBlockPreBlock(79, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Kiwi);
			addBornBlockPreBlock(79, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Muskmelon);
		}
		else if(BigStage == 1 && SmallStage == 2)
		{
			addBornBlockPreBlock(74, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Banana);
			addBornBlockPreBlock(74, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Mango);

			addBornBlockPreBlock(75, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Strawberries);
			addBornBlockPreBlock(75, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Muskmelon);
			addBornBlockPreBlock(75, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Kiwi);
			addBornBlockPreBlock(75, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Kiwi);

			addBornBlockPreBlock(76, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Mango);
			addBornBlockPreBlock(76, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Banana);
			addBornBlockPreBlock(76, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Grapes);

			addBornBlockPreBlock(77, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Muskmelon);
			addBornBlockPreBlock(77, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Strawberries);
			addBornBlockPreBlock(77, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Strawberries);
			addBornBlockPreBlock(77, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Kiwi);

			addBornBlockPreBlock(78, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Banana);

			addBornBlockPreBlock(79, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Strawberries);
			addBornBlockPreBlock(79, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Muskmelon);

			addBornBlockPreBlock(80, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Banana);
		}
		else if(BigStage == 1 && SmallStage == 3)
		{
			addBornBlockPreBlock(74, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Banana);

			addBornBlockPreBlock(75, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Kiwi);

			addBornBlockPreBlock(76, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Muskmelon);
			addBornBlockPreBlock(76, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Grapes);
			addBornBlockPreBlock(76, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Strawberries);

			addBornBlockPreBlock(77, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Kiwi);
			addBornBlockPreBlock(77, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Muskmelon);

			addBornBlockPreBlock(78, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Banana);
			addBornBlockPreBlock(78, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Grapes);

			addBornBlockPreBlock(79, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Muskmelon);
			
			addBornBlockPreBlock(80, (int)Config.Teach_Mode_Id_Meaning_Enum.Teach_Mode_Fruit_Mango);
		}
	}

	void addBornBlockPreBlock(int SlotIndex, int BlockType)
	{
		SlotBase slot = getSlotByIndex (SlotIndex);

		if(slot.m_nSlotType == (int)Config.Slot_Enum.Slot_Born_Block_All)
		{
			SlotBornBlock bornSlot = (SlotBornBlock)slot;
			bornSlot.addPreBlocksForTeachMode(BlockType);
		}
	}

	public void initBlocks(int BigStage, int SmallStage)
	{
		List< int > stageList = new List<int>();

		TextAsset TmpTxt = Resources.Load( string.Format("TxtData/Stage/Stage_{0}/stage_{0}_{1}" , BigStage, SmallStage ) ) as TextAsset;

		string [] SplitResult = TmpTxt.text.Split('\n');
		for ( int i = SplitResult.Length - 1 ; i >= 0 ; i -- )
		{
			if( SplitResult[i].IndexOf('\r') != -1)
			{
				int lenth = SplitResult[i].Length;

				if(lenth == 1)
				{
					continue;
				}
			}

			if(SplitResult[i].IndexOf('\n') != -1)
			{
				continue;
			}

			string [] TmpStr = SplitResult[i].Split(',');
			for( int j = 0 ; j < TmpStr.Length ; j ++ )
			{
				if(TmpStr[j].Length <= 0)
				{
					continue;
				}

				stageList.Add(int.Parse(TmpStr[j]));
			}
		}

		BlockCreater.Instance.initGameObjects (m_BlockObj.gameObject);


		List< warmHoleCreate > warmHoleTemp = new List<warmHoleCreate>();

		for(int i = Config.HEIGHT_NUM - 1; i >= 0; i--)
		{
			for(int j = 0; j < Config.WIDTH_NUM; j++)
			{
				int index = (i * Config.WIDTH_NUM) + j;
				int slotType = stageList[index];

				int tempWarmHoleIndex = -1;
				int blockTypeInWarmHole = 0;
				bool isBlockInWarmHole = false;
				int countDownSeconds = 0;
				bool isGoldBlock = false;
				int fieldItemId = 0;
				
				if(slotType >= Config.WARM_HOLE_START &&
				   slotType <= Config.WARM_HOLE_END)
				{
					tempWarmHoleIndex = slotType;

					if(slotType % 2 == 0)
					{
						slotType = (int)Config.Slot_Enum.Slot_WarmHole_Start;
					}
					else
					{
						slotType = (int)Config.Slot_Enum.Slot_WarmHole_End;
					}
				}
				else if(slotType >= Config.WOOD_WARM_HOLE_START &&
				        slotType <= Config.WOOD_WARM_HOLE_END)
				{
					isBlockInWarmHole = true;
					blockTypeInWarmHole = (int)Config.Slot_Enum.Slot_Normal_With_WoodBlock;

					tempWarmHoleIndex = slotType - 20;

					if(slotType % 2 == 0)
					{
						slotType = (int)Config.Slot_Enum.Slot_WarmHole_Start;
					}
					else
					{
						slotType = (int)Config.Slot_Enum.Slot_WarmHole_End;
					}
				}
				else if(slotType >= Config.GLASS_WARM_HOLE_START &&
				        slotType <= Config.GLASS_WARM_HOLE_END)
				{
					isBlockInWarmHole = true;
					blockTypeInWarmHole = (int)Config.Slot_Enum.Slot_Normal_With_GrassBlock;

					tempWarmHoleIndex = slotType - 40;

					if(slotType % 2 == 0)
					{
						slotType = (int)Config.Slot_Enum.Slot_WarmHole_Start;
					}
					else
					{
						slotType = (int)Config.Slot_Enum.Slot_WarmHole_End;
					}
				}
				else if(slotType >= Config.H_WOOD_WARM_HOLE_START &&
				        slotType <= Config.H_WOOD_WARM_HOLE_END)
				{
					isBlockInWarmHole = true;
					blockTypeInWarmHole = (int)Config.Slot_Enum.Slot_Normal_With_WoodBlock_Break;

					tempWarmHoleIndex = slotType - 60;

					if(slotType % 2 == 0)
					{
						slotType = (int)Config.Slot_Enum.Slot_WarmHole_Start;
					}
					else
					{
						slotType = (int)Config.Slot_Enum.Slot_WarmHole_End;
					}
				}
				else if(slotType >= Config.COUNT_DOWN_BOMB_START && slotType <= Config.COUNT_DOWN_BOMB_END)
				{
					countDownSeconds = slotType - Config.COUNT_DOWN_BOMB_START + 1;
					slotType = (int)Config.Slot_Enum.Slot_Normal_With_CountDown_Bomb;
				}
				else if(slotType == Config.GOLD_BLOCK)
				{
					isGoldBlock = true;
					slotType = (int)Config.Slot_Enum.Slot_Normal;
				}
				else if(slotType >= Config.FIELD_ITEM_START &&
				        slotType <= Config.FIELD_ITEM_END)
				{
					fieldItemId = slotType;
					slotType = (int)Config.Slot_Enum.Slot_Normal_With_FieldItem;
				}

				SlotBase slot = SlotCreater.Instance.getSlotByType( m_SlotObj.gameObject, m_SlotGridObj, slotType);

				if(slot)
				{
					int pos_x = -270 + j * 65;
					int pos_y = -330 + i * 65;

					slot.m_SlotRect.center = new Vector3(pos_x,pos_y,0);
					slot.m_SlotRect.width = Config.SLOT_SIZE_WIDTH;
					slot.m_SlotRect.height = Config.SLOT_SIZE_HEIGHT;

					slot.m_myGameObject.transform.parent = m_myPanel.transform;
					slot.m_myGameObject.transform.localScale = Vector3.one;
					slot.m_myGameObject.transform.localPosition = new Vector3(pos_x, pos_y, 0);

					slot.m_nSlotIndex = index;
					slot.m_SlotMotherManager = this;
					slot.setBKISprite();
					AllSlotMap[ index ] = slot;
			//		allSlots.Add(slot);

					if(slotType == (int)Config.Slot_Enum.Slot_Normal)
					{
						int color = getRandomExistColor();
						BlockBase block = BlockCreater.Instance.getBlockByType(m_BlockObj.gameObject, m_myPanel, (int)Config.Block_Enum.Block_Normal, color);
						slot.setBlockDirect(block);

						if(isGoldBlock)
						{
							BlockNormal normalBlock = (BlockNormal)block;
							normalBlock.setGoldBlock();
						}
					}
					else if(slotType == (int)Config.Slot_Enum.Slot_Normal_With_CountDown_Bomb)
					{
						BlockBase block = BlockCreater.Instance.getBlockByType(m_BlockObj.gameObject, m_myPanel, (int)Config.Block_Enum.Block_Bomb_CountDown, 1);	
						slot.setBlockDirect(block);

						BlockBomb_CountDown bombBlock = (BlockBomb_CountDown)block;
						bombBlock.setCountDownNum(countDownSeconds);
					}
					else if(slotType == (int)Config.Slot_Enum.Slot_Normal_With_ConnectOnly_Bomb)
					{
						
					}
					else if(slotType == (int)Config.Slot_Enum.Slot_Normal_With_WoodBlock)
					{
						BlockBase block = BlockCreater.Instance.getBlockByType(m_BlockObj.gameObject, m_myPanel, (int)Config.Block_Enum.Block_Wood, 1);
						slot.setBlockDirect(block);
					}
					else if(slotType == (int)Config.Slot_Enum.Slot_Normal_With_WoodBlock_Break)
					{
						BlockBase block = BlockCreater.Instance.getBlockByType(m_BlockObj.gameObject, m_myPanel, (int)Config.Block_Enum.Block_Wood, 1);
						slot.setBlockDirect(block);

						BlockWood woodBlock = (BlockWood)block;
						woodBlock.brokenBlock();
					}
					else if(slotType == (int)Config.Slot_Enum.Slot_Normal_With_GrassBlock)
					{
						BlockBase block = BlockCreater.Instance.getBlockByType(m_BlockObj.gameObject, m_myPanel, (int)Config.Block_Enum.Block_Grass, 1);
						slot.setBlockDirect(block);
					}
					else if(slotType == (int)Config.Slot_Enum.Slot_Normal_With_SauceBlock)
					{
						BlockBase block = BlockCreater.Instance.getBlockByType(m_BlockObj.gameObject, m_myPanel, (int)Config.Block_Enum.Block_Sauce, 1);
						slot.setBlockDirect(block);
					}
					else if(slotType == (int)Config.Slot_Enum.Slot_Normal_With_MilkBlock)
					{
						BlockBase block = BlockCreater.Instance.getBlockByType(m_BlockObj.gameObject, m_myPanel, (int)Config.Block_Enum.Block_Milk, 1);
						slot.setBlockDirect(block);
					}
					else if(slotType == (int)Config.Slot_Enum.Slot_Normal_With_FieldItem)
					{
						BlockBase block = BlockCreater.Instance.getBlockByType(m_BlockObj.gameObject, m_myPanel, (int)Config.Block_Enum.Block_Field_Item, fieldItemId);
						slot.setBlockDirect(block);

						BlockFieldItem itemBlock = (BlockFieldItem)block;

						if(itemBlock)
						{
							if(itemBlock.fieldItemType == (int)Config.Operate_Mode_Enum.Operate_Mode_Lignting ||
							   itemBlock.fieldItemType == (int)Config.Operate_Mode_Enum.Operate_Mode_Shovel ||
							   itemBlock.fieldItemType == (int)Config.Operate_Mode_Enum.Operate_Mode_Bomber ||
							   itemBlock.fieldItemType == (int)Config.Operate_Mode_Enum.Operate_Mode_Atomic)
							{
								itemBlock.m_bIsPlayerCanOperate = false;
							}
						}
					}

					if(tempWarmHoleIndex != -1)
					{
						if(tempWarmHoleIndex % 2 == 0)
						{
							SlotWarmHoleStart slotStart = (SlotWarmHoleStart)slot;
							slotStart.changeSpriteByIndex(tempWarmHoleIndex);

							for(int k = 0; k < warmHoleTemp.Count; k++)
							{
								warmHoleCreate temp = warmHoleTemp[k];

								if(temp.m_nWarmHoleCreateNum == tempWarmHoleIndex + 1)
								{
									slotStart.myEndSlotIndex = temp.m_nWarmHoleCreateIndex;

									SlotBase slotTemp = getSlotByIndex(temp.m_nWarmHoleCreateIndex);

									if(slotTemp)
									{
										SlotWarmHoleEnd slotEnd = (SlotWarmHoleEnd)slotTemp;
										slotEnd.myStartSlotIndex = index;
										
										slotEnd.changeSpriteByIndex(temp.m_nWarmHoleCreateNum);
									}

									break;
								}
							}
						}
						else
						{
							SlotWarmHoleEnd slotEnd = (SlotWarmHoleEnd)slot;
							slotEnd.changeSpriteByIndex(tempWarmHoleIndex);
							
							for(int k = 0; k < warmHoleTemp.Count; k++)
							{
								warmHoleCreate temp = warmHoleTemp[k];
								
								if(temp.m_nWarmHoleCreateNum == tempWarmHoleIndex - 1)
								{
									slotEnd.myStartSlotIndex = temp.m_nWarmHoleCreateIndex;
									
									SlotBase slotTemp = getSlotByIndex(temp.m_nWarmHoleCreateIndex);
									
									if(slotTemp)
									{
										SlotWarmHoleStart slotStart = (SlotWarmHoleStart)slotTemp;
										slotStart.myEndSlotIndex = index;
										
										slotStart.changeSpriteByIndex(temp.m_nWarmHoleCreateNum);
									}
									
									break;
								}
							}
						}

						warmHoleCreate temp2 = new warmHoleCreate(index, tempWarmHoleIndex);
						warmHoleTemp.Add(temp2);

						if(isBlockInWarmHole)
						{
							BlockBase block = null;

							if(blockTypeInWarmHole == (int)Config.Slot_Enum.Slot_Normal_With_WoodBlock)
							{
								block = BlockCreater.Instance.getBlockByType(m_BlockObj, m_myPanel, (int)Config.Block_Enum.Block_Wood, 1);
							}
							else if(blockTypeInWarmHole == (int)Config.Slot_Enum.Slot_Normal_With_GrassBlock)
							{
								block = BlockCreater.Instance.getBlockByType(m_BlockObj, m_myPanel, (int)Config.Block_Enum.Block_Grass, 1);
							}
							else if(blockTypeInWarmHole == (int)Config.Slot_Enum.Slot_Normal_With_WoodBlock_Break)
							{
								block = BlockCreater.Instance.getBlockByType(m_BlockObj, m_myPanel, (int)Config.Block_Enum.Block_Wood, 1);

								BlockWood woodBlock = (BlockWood)block;
								woodBlock.brokenBlock();
							}

							if(block)
							{
								slot.setBlock(block);
							}
						}
					}

					while(slot.ifCanBeErase())
					{
						int color = getRandomExistColor();
						slot.m_SlotBlock.setBlockColor(color);
					}
				}
			}
		}
	}

	void startFindCanErase()
	{
		if(!isTeachMode &&
		   ifStartFindCanErase &&
		   ifCanDoOperate)
		{
			findCanErase();
		}
	}

	void updateBlockMove()
	{
		if(refillBlockStep != (int)Config.Refill_Block_Step_Enum.Refill_Block_Null)
		{
			if(refillBlockStep == (int)Config.Refill_Block_Step_Enum.Refill_Block_Player_Press_Reset)
			{
				bool isAllFinish = true;

				for(int j = 0; j < Config.WIDTH_NUM; j++)
				{
					for(int i = 1; i <= Config.HEIGHT_NUM; i++)
					{
						int index = (i - 1) * Config.WIDTH_NUM + j;
						SlotBase slot = getSlotByIndex(index);

						if(!slot)
						{
							continue;
						}

						if(!slot.slotBlockHasBeReset)
						{
							continue;
						}

						if(!slot.slotFadeOutFinish)
						{
							isAllFinish = false;
							break;
						}
					}
				}

				if(isAllFinish)
				{
					refillBlockStep = (int)Config.Refill_Block_Step_Enum.Refill_Block_PrepareAddNewBlock;
				}
			}
			else if(refillBlockStep == (int)Config.Refill_Block_Step_Enum.Refill_Block_PrepareAddNewBlock)
			{
				for(int j = 0; j < Config.WIDTH_NUM; j++)
				{
					for(int i = 1; i <= Config.HEIGHT_NUM; i++)
					{
						int index = (i - 1) * Config.WIDTH_NUM + j;
						SlotBase slot = getSlotByIndex(index);
						
						if(!slot)
						{
							continue;
						}

						if(slot.m_SlotBlock)
						{
							continue;
						}

						if(slot.slotBlockHasBeReset)
						{
							int color = getRandomExistColor();
							BlockBase block = BlockCreater.Instance.getBlockByType(m_BlockObj.gameObject, m_myPanel, (int)Config.Block_Enum.Block_Normal, color);

							slot.setBlockDirect(block);
						}
					}
				}

				if(!checkIfNextMove() && errorCreateStageNum > 0)
				{
					for(int j = 0; j < Config.WIDTH_NUM; j++)
					{
						for(int i = 1; i <= Config.HEIGHT_NUM; i++)
						{
							int index = (i - 1) * Config.WIDTH_NUM + j;
							SlotBase slot = getSlotByIndex(index);
							
							if(!slot)
							{
								continue;
							}

							if(slot.m_nSlotType != (int)Config.Slot_Enum.Slot_Normal)
							{
								continue;
							}

							if(slot.m_SlotBlock)
							{
								if(slot.m_SlotBlock.isNeedNoResetBlock())
								{
									continue;
								}

								recoverBlock(slot.m_SlotBlock);
								slot.m_SlotBlock = null;
								slot.slotBlockHasBeReset = true;
							}
						}
					}

					errorCreateStageNum--;
					return;
				}

				errorCreateStageNum = Config.MAX_CREATE_STAGE_NUM;

				for(int j = 0; j < Config.WIDTH_NUM; j++)
				{
					for(int i = 1; i <= Config.HEIGHT_NUM; i++)
					{
						int index = (i - 1) * Config.WIDTH_NUM + j;
						SlotBase slot = getSlotByIndex(index);
						
						if(slot && 
							slot.m_SlotBlock)
						{
							if(slot.m_SlotBlock.isNeedNoResetBlock())
							{
								slot.startResetPerformace(false);
							}
							else
							{
								slot.startResetPerformace(true);
							}
						}
					}
				}

				if(errorCreateStageNum == 0)
				{

				}

				refillBlockStep = (int)Config.Refill_Block_Step_Enum.Refill_Block_Null;
			}
		}
		else
		{
			initMoveEnergy ();


			for(int j = 0; j < Config.WIDTH_NUM; j++)
			{
				for(int i = 1; i <= Config.HEIGHT_NUM; i++)
				{
					int index = (i - 1) * Config.WIDTH_NUM + j;
					
					SlotBase slot = getSlotByIndex(index);
					
					if(slot)
					{
						slot.updateSlotWork();
					}
				}
			}
		
			clearMoveEnergy();

			if(!ifCanDoOperate)
			{
				checkOperateCount--;
				
				if(checkOperateCount <= 0)
				{
					stopAllShiningBlocks();

					ifCanDoOperate = checkIfCanDoOperate();

					if(ifCanDoOperate)
					{
						ifCanDoOperate = checkIfBornSlotStop();
					}

					if(ifCanDoOperate)
					{
						StartCoroutine(__enterEraseMode());
					}

					checkOperateCount = Config.CHECK_BLOCK_NUM;
				}
			}
		}
	}

	public void findCanErase()
	{
		if(gameOver)
		{
			return;
		}

		if(!ifCanDoOperate)
		{
			resetShingingTimer();
			return;
		}

		checkIfNextMove ();

		if(canEraseSlot.Count > 0)
		{
			ifStartFindCanErase = false;

			for(int i = 0; i < canEraseSlot.Count; i++)
			{
				string sIndex = canEraseSlot[i];

				if(sIndex == null)
				{
					continue;
				}

				int slotIndex = int.Parse(sIndex);

				SlotBase slot = getSlotByIndex(slotIndex);

				if(!slot || !slot.m_SlotBlock)
				{
					continue;
				}

				slot.startShinging();
			}
		}
	}

	public void resetShingingTimer()
	{
		if(isTeachMode)
		{
			return;
		}

		stopAllShiningBlocks ();
		clearCanEraseSlot ();
	}

	public void stopAllShiningBlocks()
	{
		for(int j = 0; j < Config.WIDTH_NUM; j++)
		{
			for(int i = 1; i <= Config.HEIGHT_NUM; i++)
			{
				int index = (i - 1) * Config.WIDTH_NUM + j;
				
				SlotBase slot = getSlotByIndex(index);
				
				if(slot)
				{
					slot.stopShinging();
				}
			}
		}
	}

	void initMoveEnergy()
	{
		for(int i = Config.HEIGHT_NUM; i > 0; i--)
		{
			for(int j = 0; j < Config.WIDTH_NUM; j++)
			{
				int index = (i - 1) * Config.WIDTH_NUM + j;

				SlotBase slot = getSlotByIndex(index);

				if(!slot)
				{
					continue;
				}

				if(slot.m_nSlotType == (int)Config.Slot_Enum.Slot_Born_Block_All ||
				   slot.m_nSlotType == (int)Config.Slot_Enum.Slot_Born_Field_Item)
				{
					if(slot.m_nSlotType == (int)Config.Slot_Enum.Slot_Born_Field_Item)
					{
						SlotBornFieldItemBlock fieldItemSlot = (SlotBornFieldItemBlock)slot;

						if(fieldItemSlot.stopBornBlock)
						{
							continue;
						}
					}

					SlotBase slotDown = getSlot(slot.m_nSlotIndex, Config.DIRECT_DOWN);

					if(slotDown &&
					   slotDown.m_SlotBlock &&
					   slotDown.m_SlotBlock.m_bIsCanMove)
					{
						slotDown.setMoveEnergy(Config.DIRECT_LEFT, true);
						slotDown.setMoveEnergy(Config.DIRECT_RIGHT, true);
						slotDown.disbuteMoveEnergy(Config.DIRECT_LEFT, Config.DIRECT_LEFT);
						slotDown.disbuteMoveEnergy(Config.DIRECT_RIGHT, Config.DIRECT_RIGHT);
						slotDown.disbuteMoveEnergy(Config.DIRECT_DOWN, Config.DIRECT_LEFT);
						slotDown.disbuteMoveEnergy(Config.DIRECT_DOWN, Config.DIRECT_RIGHT);
					}
				}
				else if(slot.m_nSlotType == (int)Config.Slot_Enum.Slot_WarmHole_End)
				{
					if(slot &&
					   slot.m_SlotBlock &&
					   slot.m_SlotBlock.m_bIsCanMove)
					{
						slot.setMoveEnergy(Config.DIRECT_LEFT, true);
						slot.setMoveEnergy(Config.DIRECT_RIGHT, true);
						slot.disbuteMoveEnergy(Config.DIRECT_LEFT, Config.DIRECT_LEFT);
						slot.disbuteMoveEnergy(Config.DIRECT_RIGHT, Config.DIRECT_RIGHT);
						slot.disbuteMoveEnergy(Config.DIRECT_DOWN, Config.DIRECT_LEFT);
						slot.disbuteMoveEnergy(Config.DIRECT_DOWN, Config.DIRECT_RIGHT);
					}
				}
			}
		}
	}

	void clearMoveEnergy()
	{
		for(int i = Config.HEIGHT_NUM; i > 0; i--)
		{
			for(int j = 0; j < Config.WIDTH_NUM; j++)
			{
				int index = (i - 1) * Config.WIDTH_NUM + j;
				
				SlotBase slot = getSlotByIndex(index);

				if(slot)
				{
					slot.setMoveEnergy(Config.DIRECT_RIGHT, false);
					slot.setMoveEnergy(Config.DIRECT_LEFT, false);
				}
			}
		}
	}

	void eraseUpdate()
	{
		if(checkEraseStart)
		{
			checkAllEraseBlock();
		}
	}
	
	void stopCheckEraseMode()
	{
		checkEraseStart = false;
	}

	public void moveBlock(int Index, int Dir, bool IsSystemMove)
	{
		if(gameOver)
		{
			return;
		}

		if(!ifCanDoOperate)
		{
			return;
		}

		if(!IsSystemMove)
		{
			if(operateMode != (int)Config.Operate_Mode_Enum.Operate_Mode_Null)
			{
				if(operateMode != (int)Config.Operate_Mode_Enum.Operate_Mode_Around_Move)
				{

				}

				return;
			}
		}

		SlotBase slot1 = getSlotByIndex (Index);
		SlotBase slot2 = getSlot (Index, Dir);

		if(!slot1 || !slot2)
		{
			return;
		}

		if(!slot1.m_SlotBlock || !slot2.m_SlotBlock)
		{
			return;
		}

		if(slot1.m_SlotBlock.isNowMoving || slot2.m_SlotBlock.isNowMoving)
		{
			return;
		}

		if(!slot1.m_SlotBlock.m_bIsOnTable || !slot2.m_SlotBlock.m_bIsOnTable)
		{
			return;
		}

		if(!slot1.m_SlotBlock.m_bIsCanMove || !slot2.m_SlotBlock.m_bIsCanMove)
		{
			return;
		}

		if(!slot1.isCanMoveBlockOut() || !slot2.isCanMoveBlockOut())
		{
			return;
		}

		ifCanDoOperate = false;

		switchSlotIndex1 = slot1.m_nSlotIndex;
		switchSlotIndex2 = slot2.m_nSlotIndex;

		changeTwoSlotBlockEx (switchSlotIndex1, switchSlotIndex2, Index);
	}

	void changeTwoSlotBlockEx(int SlotIndex1, int SlotIndex2, int MainIndex)
	{
		SlotBase slot1 = getSlotByIndex (SlotIndex1);
		SlotBase slot2 = getSlotByIndex (SlotIndex2);

		if(!slot1 || !slot2)
		{
			return;
		}

		if(!slot1.m_SlotBlock || !slot2.m_SlotBlock)
		{
			return;
		}

		slot1.m_SlotBlock.m_bIsMainActionBlock = true;
		slot2.m_SlotBlock.m_bIsMainActionBlock = true;

		tempBlock1 = slot1.m_SlotBlock;
		tempBlock2 = slot2.m_SlotBlock;

		tempMainIndex = MainIndex;

		Hashtable TmpTable = new Hashtable();
		TmpTable.Add("position",new Vector3( slot2.m_SlotBlock.gameObject.transform.localPosition.x ,
		                                    slot2.m_SlotBlock.gameObject.transform.localPosition.y ,
		                                    slot2.m_SlotBlock.gameObject.transform.localPosition.z ));
		TmpTable.Add("time",0.08f);
		TmpTable.Add("islocal" , true );
		TmpTable.Add ("oncomplete","moveSlotCallBack1");
		TmpTable.Add ("oncompletetarget",this.gameObject);
		iTween.MoveTo(slot1.m_SlotBlock.gameObject,TmpTable);

		Hashtable TmpTable2 = new Hashtable();
		TmpTable2.Add("position",new Vector3( slot1.m_SlotBlock.gameObject.transform.localPosition.x ,
		                                    slot1.m_SlotBlock.gameObject.transform.localPosition.y ,
		                                    slot1.m_SlotBlock.gameObject.transform.localPosition.z ));
		TmpTable2.Add("time",0.08f);
		TmpTable2.Add("islocal" , true );
		TmpTable2.Add ("oncomplete","moveSlotCallBack2");
		TmpTable2.Add ("oncompletetarget",this.gameObject);
		iTween.MoveTo(slot2.m_SlotBlock.gameObject,TmpTable2);
	}

	void moveSlotCallBack1()
	{
		if(switchSlotIndex1 == -1 ||
		   tempBlock1 == null)
		{
			return;
		}

		SlotBase slot1 = getSlotByIndex (switchSlotIndex1);

		if(!slot1)
		{
			return;
		}

		slot1.m_SlotBlock = tempBlock2;
		slot1.m_SlotBlock.m_nBelongSlotIndex = switchSlotIndex1;

		reportSwitchBlockFinishEx ();
	}

	void moveSlotCallBack2()
	{
		if(switchSlotIndex2 == -1 ||
		   tempBlock2 == null)
		{
			return;
		}
		
		SlotBase slot2 = getSlotByIndex (switchSlotIndex2);
		
		if(!slot2)
		{
			return;
		}
		
		slot2.m_SlotBlock = tempBlock1;
		slot2.m_SlotBlock.m_nBelongSlotIndex = switchSlotIndex2;
		
		reportSwitchBlockFinishEx ();
	}

	int reportNum = 0;
	bool realNeedBack = true;
	bool isSecondReturn = false;
	void reportSwitchBlockFinishEx()
	{
		reportNum++;

		if(reportNum == 2)
		{
			reportNum = 0;
			realNeedBack = true;

			if(isSecondReturn)
			{
				switchSlotIndex1 = -1;
				switchSlotIndex2 = -1;
				
				tempBlock1 = null;
				tempBlock2 = null;

				isSecondReturn = false;
				return;
			}

			SlotBase mainSlot = getSlotByIndex(tempMainIndex);
			SlotBase slot2 = null;

			if(tempMainIndex == switchSlotIndex1)
			{
				slot2 = getSlotByIndex(switchSlotIndex2);
			}
			else
			{
				slot2 = getSlotByIndex(switchSlotIndex1);
			}

			int targetColor = -1;
			BlockBombBase.Special_Bomb_Info specialInfo;

			if(mainSlot && slot2 &&
			   mainSlot.m_SlotBlock && slot2.m_SlotBlock)
			{
				if(mainSlot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Produce)
				{
					if(slot2.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Normal)
					{
						SoundManager.Instance.PlaySE("Sound_Slice_Fruit", false);

						targetColor = slot2.m_SlotBlock.m_nColorType;

						if(mainSlot.m_nSlotIndex == tempMainIndex)
						{
							mainSlot.blockEraseDirect(false, false);

							slot2.changeBlockTypeToScore(targetColor);

							slot2.m_SlotBlock.showReleaseBlockImage_knife();
						}
						else
						{
							slot2.blockEraseDirect(false, false);

							mainSlot.changeBlockTypeToScore(targetColor);

							mainSlot.m_SlotBlock.showReleaseBlockImage_knife();
						}

						realNeedBack = false;
					}
				}
				else if(mainSlot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Normal)
				{
					if(slot2.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Produce)
					{
						SoundManager.Instance.PlaySE("Sound_Slice_Fruit", false);

						targetColor = mainSlot.m_SlotBlock.m_nColorType;
						
						if(mainSlot.m_nSlotIndex == tempMainIndex)
						{
							mainSlot.blockEraseDirect(false, false);
							
							slot2.changeBlockTypeToScore(targetColor);

							slot2.m_SlotBlock.showReleaseBlockImage_knife();
						}
						else
						{
							slot2.blockEraseDirect(false, false);
							
							mainSlot.changeBlockTypeToScore(targetColor);

							mainSlot.m_SlotBlock.showReleaseBlockImage_knife();
						}
						
						realNeedBack = false;
					}
				}
				else if(mainSlot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Small_Bomb)
				{
					if(slot2.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Small_Bomb)
					{
						if(mainSlot.m_nSlotIndex == tempMainIndex)
						{
							BlockBombBase bombBlock = (BlockBombBase)mainSlot.m_SlotBlock;
							specialInfo = bombBlock.specialBombInfo;

							slot2.combineBomb((int)Config.Block_Enum.Block_Big_Bomb, specialInfo);

							mainSlot.blockEraseDirect(false, true);
						}
						else
						{
							BlockBombBase bombBlock = (BlockBombBase)slot2.m_SlotBlock;
							specialInfo = bombBlock.specialBombInfo;

							mainSlot.combineBomb((int)Config.Block_Enum.Block_Big_Bomb, specialInfo);

							slot2.blockEraseDirect(false, true);
						}
						
						realNeedBack = false;
					}
				}
				else if(mainSlot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
				{
					if(slot2.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
					{
						if(mainSlot.m_nSlotIndex == tempMainIndex)
						{
							BlockBombBase bombBlock = (BlockBombBase)mainSlot.m_SlotBlock;
							specialInfo = bombBlock.specialBombInfo;

							mainSlot.eraseWithoutTriggerSpecial();
							slot2.combineBomb((int)Config.Block_Enum.Block_Small_Bomb, specialInfo);
						}
						else
						{
							BlockBombBase bombBlock = (BlockBombBase)slot2.m_SlotBlock;
							specialInfo = bombBlock.specialBombInfo;

							slot2.eraseWithoutTriggerSpecial();
							mainSlot.combineBomb((int)Config.Block_Enum.Block_Small_Bomb, specialInfo);
						}
						
						realNeedBack = false;
					}
				}
			}

			if(realNeedBack)
			{
				bool checkSlot1 = false;
				bool checkSlot2 = false;

				checkSlot1 = mainSlot.checkToBeAbility();
				checkSlot2 = slot2.checkToBeAbility();

				if(checkSlot1 ||
				   checkSlot2)
				{
					realNeedBack = false;
				}
				else
				{
					checkSlot1 = mainSlot.checkToBeBomb();
					checkSlot2 = slot2.checkToBeBomb();

					if(checkSlot1 ||
					   checkSlot2)
					{
						realNeedBack = false;
					}
					else
					{
						SlotNormal normalMainSlot = (SlotNormal)mainSlot;
						SlotNormal normalSlot2 = (SlotNormal)slot2;

						checkSlot1 = normalMainSlot.checkToEraseBlock_For_switch();
						checkSlot2 = normalSlot2.checkToEraseBlock_For_switch();

						if(checkSlot1 ||
						   checkSlot2)
						{
							realNeedBack = false;
							checkAllEraseBlock_erase();
						}
					}
				}
			}

			if(realNeedBack)
			{
				isSecondReturn = true;
				changeTwoSlotBlockEx(switchSlotIndex1, switchSlotIndex2, switchSlotIndex1);

				SoundManager.Instance.PlaySE("Sound_Move_Error", false);
			}
			else
			{
				decreaseLeftMoveNum();

				SlotBase mySlot1 = getSlotByIndex(switchSlotIndex1);

				if(mySlot1 && mySlot1.m_SlotBlock && mySlot1.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Score)
				{
					BlockScore scoreBlock = (BlockScore)mySlot1.m_SlotBlock;
					scoreBlock.increaseMoveNum();
				}

				mySlot1.detectNearBlock();

				SlotBase mySlot2 = getSlotByIndex(switchSlotIndex2);
				
				if(mySlot2 && mySlot2.m_SlotBlock && mySlot2.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Score)
				{
					BlockScore scoreBlock = (BlockScore)mySlot2.m_SlotBlock;
					scoreBlock.increaseMoveNum();
				}
				
				mySlot2.detectNearBlock();
			}
		}
	}

	public void earnScoreByType(int BlockType, bool IsDouble)
	{
		Config.Block_Enum enumType = (Config.Block_Enum)BlockType;

		switch(enumType)
		{
		case Config.Block_Enum.Block_Normal:
		case Config.Block_Enum.Block_Small_Bomb:
		case Config.Block_Enum.Block_Big_Bomb:
		case Config.Block_Enum.Block_Produce:
		{
			int score = Config.SCORE_ERASE_BLOCK;
			
			if(IsDouble)
			{
				score *= 2;
			}

			addRecordNum((int)Config.Player_Info_Enum.Player_Info_Score, score);
		}break;
		case Config.Block_Enum.Block_Score:
		{
			int score = Config.SCORE_ERASE_SCORE_BLOCK;
			
			if(IsDouble)
			{
				score *= 2;
			}

			addRecordNum((int)Config.Player_Info_Enum.Player_Info_Score, score);
		}break;
		}
	}

	public void checkAllEraseBlock()
	{
		if(!ifCanDoOperate)
		{
			return;
		}

		if(!checkIfCanDoOperate())
		{
			return;
		}

		if(checkAllEraseBlock_check())
		{
			checkAllEraseBlock_erase();
			ifCanDoOperate = false;

			resetAllMainActionBlocks();
			resetAllBombBlockBomedRecord();

			errorCreateStageNum = Config.MAX_CREATE_STAGE_NUM;
		}
		else
		{
			stopCheckEraseMode();

			resetAllMainActionBlocks();
			resetAllBombBlockBomedRecord();

			updatePlayLayerStageInfo();

			ifCanDoOperate = true;
			ifStartFindCanErase = true;

			checkCantMove();

			if(moveLeftNum <= 0)
			{
				enableAllTouchObj(false);

				if(!isTeachMode)
				{
					updatePlayLayerStageInfo();

					m_GamePlayLayer.handleGameOver();
				}
			}
		}
	}

	void checkCantMove()
	{
		if(gameOver)
		{
			return;
		}

		if(refillBlockStep != (int)Config.Refill_Block_Step_Enum.Refill_Block_Null)
		{
			return;
		}

		if(!checkIfCanDoOperate())
		{
			return;
		}

		if(checkIfNextMove())
		{
			m_GamePlayLayer.showShuffleUI(false);
		}
		else
		{
			m_GamePlayLayer.showShuffleUI(true);
		}
	}

	public void startRefillBlock()
	{
		if(refillBlockStep != (int)Config.Refill_Block_Step_Enum.Refill_Block_Null)
		{
			return;
		}

		ifCanDoOperate = false;

		for(int j = 0; j < Config.WIDTH_NUM; j++)
		{
			for(int i = 1; i <= Config.HEIGHT_NUM; i++)
			{
				int index = (i - 1) * Config.WIDTH_NUM + j;
				
				SlotBase slot = getSlotByIndex(index);
				
				if(!slot)
				{
					continue;
				}

				if(!slot.m_SlotBlock)
				{
					continue;
				}

				iTween.Stop(slot.m_SlotBlock.gameObject);

				if(slot.m_SlotBlock.isNeedNoResetBlock())
				{
					slot.startRemovePerformace(false);
				}
				else
				{
					slot.startRemovePerformace(true);
				}
			}
		}

		resetShingingTimer ();
		refillBlockStep = (int)Config.Refill_Block_Step_Enum.Refill_Block_Player_Press_Reset;
	}

	void checkAllEraseBlock_erase()
	{
		bool playSound = false;

		for(int i = 0; i < AllSlotMap.Count; i++)
		{
			SlotBase slot = AllSlotMap[i];

			if(!slot)
			{
				continue;
			}

			bool success = slot.releaseBlock(true, true, true);

			if(success)
			{
				playSound = true;
			}
		}

		if(playSound)
		{
			SoundManager.Instance.PlaySE("Sound_Block_Delete", false);
		}
	}

	public bool checkAllEraseBlock_check()
	{
		bool needErase = false;

		for(int j = 0; j < Config.WIDTH_NUM; j++)
		{
			for(int i = 1; i <= Config.HEIGHT_NUM; i++)
			{
				int index = (i - 1) * Config.WIDTH_NUM + j;
				SlotBase slot = AllSlotMap[index];
				
				if(slot)
				{
					if(!slot.m_SlotBlock)
					{
						continue;
					}

					if(slot.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal &&
					   slot.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Bomb_Fruit)
					{
						continue;
					}

					bool temp = false;

					temp = slot.checkToBeAbility_ForFall();

					if(temp)
					{
						needErase = true;
					}
					else
					{
						temp = slot.checkToBeBomb();

						if(temp)
						{
							needErase = true;
						}
						else
						{
							temp = slot.checkToEraseBlock();

							if(temp)
							{
								needErase = true;
							}
						}
					}
				}
			}
		}

		return needErase;
	}

	void resetAllMainActionBlocks()
	{
		for(int i = 0; i < AllSlotMap.Count; i++)
		{
			SlotBase slot = AllSlotMap[i];

			if(!slot)
			{
				continue;
			}

			if(!slot.m_SlotBlock)
			{
				continue;
			}

			slot.m_SlotBlock.m_bIsMainActionBlock = false;
		}
	}

	void resetAllBombBlockBomedRecord()
	{
		for(int i = 0; i < AllSlotMap.Count; i++)
		{
			SlotBase slot = AllSlotMap[i];
			
			if(!slot)
			{
				continue;
			}
			
			if(!slot.m_SlotBlock)
			{
				continue;
			}

			if(!slot.m_SlotBlock.isBomb())
			{
				continue;
			}

			BlockBombBase bombBlock = (BlockBombBase)slot.m_SlotBlock;
			bombBlock.isBombThisTurn = false;
		}
	}

	public bool checkIfCanDoOperate()
	{
		bool doSomeThing = false;

		for(int j = 0; j < Config.WIDTH_NUM; j++)
		{
			for(int i = 0; i < Config.HEIGHT_NUM; i++)
			{
				int index = (i - 1) * Config.WIDTH_NUM + j;

				SlotBase slot = getSlotByIndex(index);

				if(!slot)
				{
					continue;
				}

				if(!slot.m_SlotBlock)
				{
					continue;
				}

				if(!slot.m_SlotBlock.m_BlockBodySprite)
				{
					continue;
				}

				if(slot.m_SlotBlock.isNowMoving)
				{
					doSomeThing = true;
					break;
				}

				if(slot.m_SlotBlock.isNowShaking)
				{
					doSomeThing = true;
					break;
				}
			}
		}

		return !doSomeThing;
	}

	public bool checkIfNextMove()
	{
		for(int j = 0; j < Config.WIDTH_NUM; j++)
		{
			for(int i = 0; i < Config.HEIGHT_NUM; i++)
			{
				int index = (i - 1) * Config.WIDTH_NUM + j;
				
				SlotBase slot = getSlotByIndex(index);
				
				if(!slot)
				{
					continue;
				}
				
				if(!slot.m_SlotBlock)
				{
					continue;
				}

				SlotBase slot1 = getSlot(slot.m_nSlotIndex, Config.DIRECT_RIGHT);

				if(slot1 && slot1.m_SlotBlock)
				{
					if(slot1.ifCanBeEraseInThisColor(slot.m_SlotBlock.m_nColorType, slot.m_nSlotIndex))
					{
						return true;
					}
				}

				SlotBase slot2 = getSlot(slot.m_nSlotIndex, Config.DIRECT_UP);
				
				if(slot2 && slot2.m_SlotBlock)
				{
					if(slot2.ifCanBeEraseInThisColor(slot.m_SlotBlock.m_nColorType, slot.m_nSlotIndex))
					{
						return true;
					}
				}

				SlotBase slot3 = getSlot(slot.m_nSlotIndex, Config.DIRECT_DOWN);
				
				if(slot3 && slot3.m_SlotBlock)
				{
					if(slot3.ifCanBeEraseInThisColor(slot.m_SlotBlock.m_nColorType, slot.m_nSlotIndex))
					{
						return true;
					}
				}

				SlotBase slot4 = getSlot(slot.m_nSlotIndex, Config.DIRECT_LEFT);
				
				if(slot4 && slot4.m_SlotBlock)
				{
					if(slot4.ifCanBeEraseInThisColor(slot.m_SlotBlock.m_nColorType, slot.m_nSlotIndex))
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	public bool checkIfBornSlotStop()
	{
		for(int j = 0; j < Config.WIDTH_NUM; j++)
		{
			for(int i = 0; i < Config.HEIGHT_NUM; i++)
			{
				int index = (i - 1) * Config.WIDTH_NUM + j;
				
				SlotBase slot = getSlotByIndex(index);
				
				if(!slot)
				{
					continue;
				}

				if(slot.m_nSlotType != (int)Config.Slot_Enum.Slot_Born_Block_All &&
				   slot.m_nSlotType != (int)Config.Slot_Enum.Slot_Born_Field_Item)
				{
					continue;
				}

				SlotBornBlock slotBorn = (SlotBornBlock)slot;
				bool isBorning = slotBorn.m_bIsBorningBlock;

				if(isBorning)
				{
					return false;
				}
			}
		}

		return true;
	}

	public void enterOperateMode(int Mode)
	{
		if(gameOver)
		{
			return;
		}

		if(operateMode != (int)Config.Operate_Mode_Enum.Operate_Mode_Null)
		{
			return;
		}

		if(!isTeachMode)
		{
			if(Mode == (int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Direct ||
			   Mode == (int)Config.Operate_Mode_Enum.Operate_Mode_Free_Move)
			{
				int needCoin = 0;

				Config.Operate_Mode_Enum enumType = (Config.Operate_Mode_Enum)Mode;

				switch(enumType)
				{
				case Config.Operate_Mode_Enum.Operate_Mode_Free_Move:
				{
					needCoin = Config.NEED_COIN_FREE_MOVE;
				}break;
				case Config.Operate_Mode_Enum.Operate_Mode_Bomb_Direct:
				{
					needCoin = Config.NEED_COIN_BOMB_DIRECT;
				}break;
				default:
				{

				}break;
				}

				int nowCoin = getOwnCoins();

				if(nowCoin < needCoin)
				{
					return;
				}
			}
			else
			{
				int leftItemNum = getFieldItemNum(Mode);

				if(leftItemNum <= 0)
				{
					if(!m_GamePlayLayer.isChallengMode)
					{
						if(Mode != (int)Config.Operate_Mode_Enum.Operate_Mode_Lignting &&
						   Mode != (int)Config.Operate_Mode_Enum.Operate_Mode_Shovel &&
						   Mode != (int)Config.Operate_Mode_Enum.Operate_Mode_Bomber &&
						   Mode != (int)Config.Operate_Mode_Enum.Operate_Mode_Atomic)
						{
							PopupManager.Instance.ShowStoreShopUI();
						}
						else
						{
							m_GamePlayLayer.openTipUIForGoldItem(Mode);
						}
					}

					return;
				}
			}
		}

		operateMode = Mode;

		Config.Operate_Mode_Enum enumType2 = (Config.Operate_Mode_Enum)Mode;
		switch(enumType2)
		{
		case Config.Operate_Mode_Enum.Operate_Mode_Free_Move:
		{
			if(!isTeachMode)
			{
				m_GamePlayLayer.addMessageById(1);
			}
		}break; 
		case Config.Operate_Mode_Enum.Operate_Mode_Bomb_Direct:
		{
			if(!isTeachMode)
			{
				m_GamePlayLayer.addMessageById(4);
			}
		}break; 
		case Config.Operate_Mode_Enum.Operate_Mode_Bomb_Twice:
		{
			m_GamePlayLayer.addMessageById(4);
		}break;
		case Config.Operate_Mode_Enum.Operate_Mode_Bomb_Upgrade:
		{
			bool bFind = false;

			for(int i = 0; i < AllSlotMap.Count; i++)
			{
				SlotBase slot = AllSlotMap[i];

				if(!slot)
				{
					continue;
				}

				if(!slot.m_SlotBlock)
				{
					continue;
				}

				if(slot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit ||
				   slot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Small_Bomb)
				{
					bFind = true;
					break;
				}
			}

			if(bFind)
			{
				m_GamePlayLayer.addMessageById(14);
			}
			else
			{
				finishOperateMode(false);
				m_GamePlayLayer.addMessageById(7);
			}
		}break;
		case Config.Operate_Mode_Enum.Operate_Mode_Lignting:
		{
			m_GamePlayLayer.addMessageById(6);
		}break;
		case Config.Operate_Mode_Enum.Operate_Mode_Shovel:
		{
			bool bGold = false;

			for(int i = 0; i < AllSlotMap.Count; i++)
			{
				SlotBase targetSlot = AllSlotMap[i];

				if(!targetSlot || !targetSlot.m_SlotBlock)
				{
					continue;
				}

				if(targetSlot.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal)
				{
					continue;
				}

				BlockNormal normalBlock = (BlockNormal)targetSlot.m_SlotBlock;

				if(!normalBlock.isGoldBlock)
				{
					continue;
				}

				targetSlot.m_bSlotReadyEraseBlock = true;
				targetSlot.releaseBlock(true, false, false);

				bGold = true;
			}

			if(bGold)
			{
				ifCanDoOperate = false;
			}
			else
			{
				m_GamePlayLayer.addMessageById(9);
			}

			finishOperateMode(bGold);
		}break;
		case Config.Operate_Mode_Enum.Operate_Mode_Bomber:
		{
			bool bBomb = false;
			
			for(int i = 0; i < AllSlotMap.Count; i++)
			{
				SlotBase targetSlot = AllSlotMap[i];
				
				if(!targetSlot || !targetSlot.m_SlotBlock)
				{
					continue;
				}
				
				if(!targetSlot.m_SlotBlock.isBomb())
				{
					continue;
				}
				
				BlockBombBase bombBlock = (BlockBombBase)targetSlot.m_SlotBlock;
				bombBlock.block_blockDoBomb(true);

				bBomb = true;
			}
			
			if(bBomb)
			{
				ifCanDoOperate = false;
			}
			else
			{
				m_GamePlayLayer.addMessageById(10);
			}
			
			finishOperateMode(bBomb);
		}break;
		case Config.Operate_Mode_Enum.Operate_Mode_Erase_Knife:
		{
			bool find = false;
			
			for(int i = 0; i < AllSlotMap.Count; i++)
			{
				SlotBase targetSlot = AllSlotMap[i];
				
				if(!targetSlot || !targetSlot.m_SlotBlock)
				{
					continue;
				}
				
				if(targetSlot.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Produce)
				{
					continue;
				}
				
				targetSlot.m_bSlotReadyEraseBlock = true;
				targetSlot.releaseBlock(true, false, false);

				find = true;
			}
			
			if(find)
			{
				ifCanDoOperate = false;
			}
			else
			{
				m_GamePlayLayer.addMessageById(11);
			}

			SoundManager.Instance.PlaySE("Sound_Delete_Knife", false);
			
			finishOperateMode(find);
		}break;
		}
	}

	public SlotBase getSlotByIndex(int index)
	{
		if( AllSlotMap.ContainsKey( index ) ) return AllSlotMap[index];

		return null;

		for(int i = 0; i < AllSlotMap.Count; i++)
		{
			SlotBase slot = AllSlotMap[i];

			if(!slot)
			{
				continue;
			}

			if(slot.m_nSlotIndex == index)
			{
				return slot;
			}
		}

		return null;
	}

	public SlotBase getSlot(int index, int dir)
	{
		SlotBase slot = getSlotByIndex (index);

		if(!slot)
		{
			return null;
		}

		int targetIndex = 0;

		switch(dir)
		{
		case Config.DIRECT_UP:
		{
			targetIndex = index + Config.WIDTH_NUM;
		}break;
		case Config.DIRECT_DOWN:
		{
			targetIndex = index - Config.WIDTH_NUM;
		}break;
		case Config.DIRECT_LEFT:
		{
			if(index % Config.WIDTH_NUM == 0)
			{
				return null;
			}

			targetIndex = index - 1;
		}break;
		case Config.DIRECT_RIGHT:
		{
			if((index + 1) % Config.WIDTH_NUM == 0)
			{
				return null;
			}

			targetIndex = index + 1;
		}break;
		default:
		{
			return null;
		}
		}

		return getSlotByIndex (targetIndex);
	}

	public BlockBase getBlockById(int BlockId)
	{
		for(int i = 0; i < AllSlotMap.Count; i++)
		{
			SlotBase slot = AllSlotMap[i];

			if(!slot)
			{
				continue;
			}

			if(!slot.m_SlotBlock)
			{
				continue;
			}

			if(slot.m_SlotBlock.m_nBlockIndex == BlockId)
			{
				return slot.m_SlotBlock;
			}
		}

		return null;
	}

	public int getReverseDir(int Dir)
	{
		if(Dir != Config.DIRECT_UP &&
		   Dir != Config.DIRECT_DOWN &&
		   Dir != Config.DIRECT_LEFT &&
		   Dir != Config.DIRECT_RIGHT)
		{
			return Config.DIRECT_NO_DIR;
		}
		
		switch(Dir)
		{
		case Config.DIRECT_UP:{return Config.DIRECT_DOWN;};
		case Config.DIRECT_DOWN:{return Config.DIRECT_UP;};
		case Config.DIRECT_LEFT:{return Config.DIRECT_RIGHT;};
		case Config.DIRECT_RIGHT:{return Config.DIRECT_LEFT;};
		}
		
		return Config.DIRECT_NO_DIR;
	}

	public void addCanEraseSlot(int SlotIndex)
	{
		string sIndex = SlotIndex.ToString ();

		for(int i = 0; i < canEraseSlot.Count; i++)
		{
			int slotIndex = int.Parse(canEraseSlot[i]);

			if(slotIndex == SlotIndex)
			{
				return;
			}
		}

		canEraseSlot.Add (sIndex);
	}

	public void clearCanEraseSlot()
	{
		canEraseSlot.Clear ();
	}

	List< GameObject > destoryGameObject = new List< GameObject >();
	public void recoverBlock(BlockBase Block)
	{
		if(!Block)
		{
			return;
		}

		Block.blockWillRemove ();
	/*	destoryGameObject.Add (Block.m_myGameObject);
		Block.m_myGameObject.SetActive (false);*/
		Destroy (Block.m_myGameObject);
	}

	IEnumerator __deleteBlock()
	{
		while(true)
		{
			if(destoryGameObject.Count > 0)
			{
				CPDebug.Log("delete obj");
				GameObject obj = destoryGameObject[0];
				Destroy(obj);

				destoryGameObject.RemoveAt(0);
			}

			yield return new WaitForSeconds(1.0f);
		}
	}

	public int getBlockNums(int Type)
	{
		int count = 0;

		for(int i = 0; i < AllSlotMap.Count; i++)
		{
			SlotBase slot = AllSlotMap[i];

			if(!slot)
			{
				continue;
			}

			if(!slot.m_SlotBlock)
			{
				continue;
			}

			if(slot.m_SlotBlock.m_nBlockType == Type)
			{
				count++;
			}
		}

		return count;
	}

	public void enableAllTouchObj(bool Enable)
	{
		for(int i = 0; i < AllSlotMap.Count; i++)
		{
			SlotBase slot = AllSlotMap[i];

			if(slot == null)
			{
				continue;
			}

			if(slot.touchObj == null)
			{
				continue;
			}

			if(Enable)
			{
				if(slot.m_nSlotType == (int)Config.Slot_Enum.Slot_Normal)
				{
					slot.touchObj.SetActive(Enable);

					BoxCollider box = slot.touchObj.GetComponent<BoxCollider>();

					if(box)
					{
						box.enabled = true;
					}
					
					UIButtonMessage message = slot.touchObj.GetComponent<UIButtonMessage>();
					
					if(message)
					{
						message.enabled = true;
					}
				}
			}
			else
			{
				slot.touchObj.gameObject.SetActive(Enable);

				BoxCollider box = slot.touchObj.GetComponent<BoxCollider>();

				if(box)
				{
					box.enabled = false;
				}

				UIButtonMessage message = slot.touchObj.GetComponent<UIButtonMessage>();
				
				if(message)
				{
					message.enabled = false;
				}
			}
		}

	}

	public int getRandomExistColor()
	{
		int count = existBlocks.Count;

		System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
		int random = rnd.Next(count);

		return existBlocks[random];
	}
	
	void OnFingerDown(FingerDownEvent e) 
	{ 
		nowTouchSlotIndex = -1;
	}

	void OnTap(TapGesture gesture)
	{ 
		if(isTeachMode)
		{
			TeachModeStep step = m_GamePlayLayer.getNowTeachStep();
			
			if(step != null)
			{
				bool isAllow = step.isAllow(nowTouchSlotIndex, Config.TEACH_MODE_ALLOW_ACIOTN_SPACE_TOUCH);
				
				if(!isAllow)
				{
					if(!ifCanDoOperate)
					{
						return;
					}

					isAllow = step.isAllow(nowTouchSlotIndex, Config.TEACH_MODE_ALLOW_ACIOTN_TOUCH);
					
					if(!isAllow)
					{
						return;
					}
				}
				
				m_GamePlayLayer.doNextTeachStep();
			}
		}
		
		touchEnd (gesture.Position);
	}

	void OnSwipe(SwipeGesture gesture) 
	{ 
		if(nowTouchSlotIndex == -1)
		{
			return;
		}

		if(!ifCanDoOperate)
		{
			return;
		}
		
		int direct = (int)gesture.Direction;
		
		switch(direct)
		{
		case 4:		// up
		{
			if(isTeachMode)
			{
				TeachModeStep step = m_GamePlayLayer.getNowTeachStep();
				
				if(step != null)
				{
					bool isAllow = step.isAllow(nowTouchSlotIndex, Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_UP);
					
					if(!isAllow)
					{
						nowTouchSlotIndex = -1;
						return;
					}
					
					m_GamePlayLayer.doNextTeachStep();
				}
			}

			moveBlock(nowTouchSlotIndex, Config.DIRECT_UP, false);
		}break;
		case 8:		// down
		{
			if(isTeachMode)
			{
				TeachModeStep step = m_GamePlayLayer.getNowTeachStep();
				
				if(step != null)
				{
					bool isAllow = step.isAllow(nowTouchSlotIndex, Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_DOWN);
					
					if(!isAllow)
					{
						nowTouchSlotIndex = -1;
						return;
					}
					
					m_GamePlayLayer.doNextTeachStep();
				}
			}

			moveBlock(nowTouchSlotIndex, Config.DIRECT_DOWN, false);
		}break;
		case 2:		// left
		{
			if(isTeachMode)
			{
				TeachModeStep step = m_GamePlayLayer.getNowTeachStep();
				
				if(step != null)
				{
					bool isAllow = step.isAllow(nowTouchSlotIndex, Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_LEFT);
					
					if(!isAllow)
					{
						nowTouchSlotIndex = -1;
						return;
					}
					
					m_GamePlayLayer.doNextTeachStep();
				}
			}

			moveBlock(nowTouchSlotIndex, Config.DIRECT_LEFT, false);
		}break;
		case 1:		// right
		{
			if(isTeachMode)
			{
				TeachModeStep step = m_GamePlayLayer.getNowTeachStep();
				
				if(step != null)
				{
					bool isAllow = step.isAllow(nowTouchSlotIndex, Config.TEACH_MODE_ALLOW_ACIOTN_SWIPE_RIGHT);
					
					if(!isAllow)
					{
						nowTouchSlotIndex = -1;
						return;
					}
					
					m_GamePlayLayer.doNextTeachStep();
				}
			}

			moveBlock(nowTouchSlotIndex, Config.DIRECT_RIGHT, false);
		}break;
		default:
		{
			
		}break;
		}

		m_GamePlayLayer.showFieldButtons (false);
		nowTouchSlotIndex = -1;
	}

	void touchEnd(Vector2 Point)
	{
		if(gameOver)
		{
			return;
		}

		if(!ifCanDoOperate)
		{
			nowTouchSlotIndex = -1;
			return;
		}

		if(operateMode == (int)Config.Operate_Mode_Enum.Operate_Mode_Null)
		{
			int index = nowTouchSlotIndex;//isTouchtSlot (Point);

			if(index == -1)
			{
				return;
			}

			startTargetBomb(index);

		//	ifCanDoOperate = false;
		}
		else
		{
			Config.Operate_Mode_Enum enumType = (Config.Operate_Mode_Enum)operateMode;

			switch(enumType)
			{
			case Config.Operate_Mode_Enum.Operate_Mode_Free_Move:
			{
				bool doNothing = false;

				int index = nowTouchSlotIndex;//isTouchtSlot (Point);
				
				if(index == -1)
				{
					return;
				}

				SlotBase slot = getSlotByIndex(index);

				if(!slot)
				{
					doNothing = true;
				}

				if(slot && !slot.m_SlotBlock)
				{
					doNothing = true;
				}

				if(!slot.m_SlotBlock.m_bIsPlayerCanOperate)
				{
					doNothing = true;
				}

				if(!slot.m_SlotBlock.m_bIsCanMove)
				{
					doNothing = true;
				}

				if(doNothing)
				{
					SlotBase slotPre = getSlotByIndex(operateParam1);

					if(slotPre && slotPre.m_SlotBlock)
					{
						slotPre.m_SlotBlock.resetSize();
					}

					finishOperateMode(false);
					return;
				}

				if(operateParam1 == -1 && operateParam2 == -1)
				{
					operateParam1 = index;

					slot.m_SlotBlock.beSelect();
					return;
				}
				else if(operateParam1 == -1 || operateParam2 == -1)
				{
					operateParam2 = index;

					if(operateParam1 == operateParam2)
					{
						SlotBase slotPre = getSlotByIndex(operateParam1);
						
						if(slotPre && slotPre.m_SlotBlock)
						{
							slotPre.m_SlotBlock.resetSize();
						}

						finishOperateMode(false);
						return;
					}

					SlotBase slot1 = getSlotByIndex(operateParam1);
					SlotBase slot2 = getSlotByIndex(operateParam2);

					if(!slot1 || !slot2 || !slot1.m_SlotBlock || !slot2.m_SlotBlock)
					{
						finishOperateMode(false);
						return;
					}

					slot1.m_SlotBlock.m_bIsMainActionBlock = true;
					slot2.m_SlotBlock.m_bIsMainActionBlock = true;

					switchTwoSlotBlocks(operateParam1, operateParam2);

					ifCanDoOperate = false;

					finishOperateMode(true);
					return;
				}
				else
				{
					finishOperateMode(false);
					return;
				}
			}
			case Config.Operate_Mode_Enum.Operate_Mode_Bomb_Direct:
			{
				if(operateParam1 != -1)
				{
//					hideAllBombDirectBtn();
//					finishOperateMode(false);

					return;
				}

				bool doNothing = false;
				
				int index = nowTouchSlotIndex;//isTouchtSlot (Point);
				
				if(index == -1)
				{
					return;
				}

				if(isTeachMode)
				{
					if(operateParam1 == -1)
					{
						index = 40;
						doNothing = false;
					}
				}
				
				SlotBase slot = getSlotByIndex(index);
				
				if(!slot)
				{
					doNothing = true;
				}
				
				if(slot && !slot.m_SlotBlock)
				{
					doNothing = true;
				}
				
				if(!slot.m_SlotBlock.m_bIsPlayerCanOperate)
				{
					doNothing = true;
				}
				
				if(!slot.m_SlotBlock.isBomb())
				{
					m_GamePlayLayer.addMessageById(7);
					doNothing = true;
				}

				BlockBombBase bombBlock = null;

				if(!doNothing)
				{
					bombBlock = (BlockBombBase)slot.m_SlotBlock;
				}

				if(!bombBlock)
				{
					doNothing = true;
				}

				if(!doNothing && bombBlock)
				{
					if(!bombBlock.isCanSetSpecialBomb)
					{
						m_GamePlayLayer.addMessageById(5);
						doNothing = true;
					}
				}

				if(doNothing)
				{
					finishOperateMode(false);
					return;
				}

				operateParam1 = slot.m_nSlotIndex;

				slot.showBombDirectBtns(true);
			}break;
			case Config.Operate_Mode_Enum.Operate_Mode_Bomb_Upgrade:
			{
				bool doNothing = false;
				
				int index = nowTouchSlotIndex;//isTouchtSlot (Point);
				
				if(index == -1)
				{
					return;
				}
				
				SlotBase slot = getSlotByIndex(index);
				
				if(!slot)
				{
					doNothing = true;
				}
				
				if(slot && !slot.m_SlotBlock)
				{
					doNothing = true;
				}
				
				if(!slot.m_SlotBlock.isBomb())
				{
					doNothing = true;
				}

				if(slot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Big_Bomb)
				{
					doNothing = true;
				}

				if(doNothing)
				{
					finishOperateMode(false);
					return;
				}

				if(slot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
				{
					slot.changeBlockType((int)Config.Block_Enum.Block_Small_Bomb);
				}
				else if(slot.m_SlotBlock.m_nBlockType == (int)Config.Block_Enum.Block_Small_Bomb)
				{
					slot.changeBlockType((int)Config.Block_Enum.Block_Big_Bomb);
				}

				ifCanDoOperate = false;

				finishOperateMode(true);

				SoundManager.Instance.PlaySE("Sound_Be_Bomb_s", false);
			}break;
			case Config.Operate_Mode_Enum.Operate_Mode_Bomb_Twice:
			{
				bool doNothing = false;
				
				int index = nowTouchSlotIndex;//isTouchtSlot (Point);
				
				if(index == -1)
				{
					return;
				}
				
				SlotBase slot = getSlotByIndex(index);
				
				if(!slot)
				{
					doNothing = true;
				}
				
				if(slot && !slot.m_SlotBlock)
				{
					doNothing = true;
				}
				
				if(!slot.m_SlotBlock.m_bIsPlayerCanOperate)
				{
					doNothing = true;
				}
				
				if(!slot.m_SlotBlock.isBomb())
				{
					m_GamePlayLayer.addMessageById(5);
					doNothing = true;
				}
				
				BlockBombBase bombBlock = null;
				
				if(!doNothing)
				{
					bombBlock = (BlockBombBase)slot.m_SlotBlock;
					
					if(bombBlock.specialBombInfo.bombNum >= Config.SPECIAL_BOMB_MAX_NUM)
					{
						doNothing = true;	
					}
				}
				
				if(!doNothing && bombBlock)
				{
					if(!bombBlock.isCanSetSpecialBomb)
					{
						m_GamePlayLayer.addMessageById(5);
						doNothing = true;
					}
				}
				
				if(doNothing)
				{
					finishOperateMode(false);
					return;
				}
				
				bombBlock.setBombNum(Config.SPECIAL_BOMB_MAX_NUM);
				finishOperateMode(true);
			}break;
			case Config.Operate_Mode_Enum.Operate_Mode_Lignting:
			{
				bool doNothing = false;
				
				int index = nowTouchSlotIndex;//isTouchtSlot (Point);
				
				if(index == -1)
				{
					return;
				}
				
				SlotBase slot = getSlotByIndex(index);
				
				if(!slot)
				{
					doNothing = true;
				}
				
				if(slot && !slot.m_SlotBlock)
				{
					doNothing = true;
				}

				if(slot.m_SlotBlock)
				{
					if(slot.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal)
					{
						doNothing = true;
					}
				}
				
				if(doNothing)
				{
					finishOperateMode(false);
					return;
				}

				int targetColor = slot.m_SlotBlock.m_nColorType;

				bool find = false;

				for(int i = 0; i < AllSlotMap.Count; i++)
				{
					SlotBase targetSlot = AllSlotMap[i];

					if(!targetSlot || !targetSlot.m_SlotBlock)
					{
						continue;
					}

					if(targetSlot.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Normal)
					{
						continue;
					}

					if(targetSlot.m_SlotBlock.m_nColorType != (int)Config.Block_Color_Enum.Block_Color_Null &&
					   targetSlot.m_SlotBlock.m_nColorType == targetColor)
					{
						targetSlot.m_bSlotReadyEraseBlock = true;
						targetSlot.releaseBlock(true, false, false);

						find = true;
					}
				}

				if(find)
				{
					ifCanDoOperate = false;
					SoundManager.Instance.PlaySE("Sound_Item_Lighting", false);
				}

				finishOperateMode(find);
			}break;
			}
		}
	}

	bool switchTwoSlotBlocks(int SlotIndex1, int SlotIndex2)
	{
		SlotBase slot1 = getSlotByIndex (SlotIndex1);
		SlotBase slot2 = getSlotByIndex (SlotIndex2);

		if(!slot1 || !slot2)
		{
			return false;
		}

		if(!slot1.m_SlotBlock || !slot2.m_SlotBlock)
		{
			return false;
		}

		slot1.m_SlotBlock.resetSize ();
		slot2.m_SlotBlock.resetSize ();

		BlockBase tempBlock = slot1.m_SlotBlock;

		slot1.setBlock (slot2.m_SlotBlock);
		slot2.setBlock (tempBlock);

		slot1.m_SlotBlock.m_bIsMainActionBlock = true;
		slot2.m_SlotBlock.m_bIsMainActionBlock = true;

		return true;
	}

	public void finishOperateMode(bool NeedDecreaseNum)
	{
		if(!isTeachMode)
		{
			if(NeedDecreaseNum)
			{
				bool isDecreaseCoin = false;

				int needCoin = 0;

				Config.Operate_Mode_Enum enumType = (Config.Operate_Mode_Enum)operateMode;

				switch(enumType)
				{
				case Config.Operate_Mode_Enum.Operate_Mode_Bomb_Direct:
				{
					isDecreaseCoin = true;
					needCoin = Config.NEED_COIN_BOMB_DIRECT;
				}break;
				case Config.Operate_Mode_Enum.Operate_Mode_Free_Move:
				{
					isDecreaseCoin = true;
					needCoin = Config.NEED_COIN_FREE_MOVE;
				}break;
				}

				if(isDecreaseCoin)
				{
					addOwnCoins(-needCoin);
				}
				else
				{
					addFieldItemNum(operateMode, -1);
				}
			}
		}

		operateMode = (int)Config.Operate_Mode_Enum.Operate_Mode_Null;
		operateParam1 = -1;
		operateParam2 = -1;
	}

	void startTargetBomb(int Index)
	{
		if(Index >= AllSlotMap.Count ||
		   Index < 0)
		{
			return;
		}

		SlotBase slot = getSlotByIndex(Index);

		if(!slot)
		{
			return;
		}

		if(!slot.m_SlotBlock)
		{
			return;
		}

		if(!slot.m_SlotBlock.m_bIsPlayerCanOperate)
		{
			return;
		}

		if(!slot.m_SlotBlock.isBomb())
		{
			return;
		}

		BlockBombBase blockBomb = (BlockBombBase)slot.m_SlotBlock;

		if(!blockBomb.isCanTouchBomb)
		{
			return;
		}

		blockBomb.block_blockDoBomb (true);
		ifCanDoOperate = false;
		decreaseLeftMoveNum ();
	}

	public void hideAllBombDirectBtn()
	{
		for(int i = 0; i < AllSlotMap.Count; i++)
		{
			SlotBase slot = AllSlotMap[i];

			if(!slot)
			{
				continue;
			}

			if(!slot.m_SlotBlock)
			{
				continue;
			}

			slot.showHorizontalBtn(false);
		}
	}

	int isTouchtSlot(Vector2 point)
	{
		for(int i = 0; i < AllSlotMap.Count; i++)
		{
			SlotBase slot = AllSlotMap[i];

			if(!slot)
			{
				continue;
			}

			Vector2 transPoint = m_UICamera.WorldToScreenPoint(slot.gameObject.transform.position);

			Rect rect = new Rect();
			rect.width = slot.m_SlotRect.width;
			rect.height = slot.m_SlotRect.height;
			rect.center = new Vector3(transPoint.x, transPoint.y, 0);
		
			if (rect.Contains(point))
			{
				return slot.m_nSlotIndex;
			}
		}
		
		return -1;
	}

	Vector2 transToScreenPoint(Vector3 WorldPosition)
	{
		Vector2 transPoint = m_UICamera.WorldToScreenPoint(WorldPosition);
		return transPoint;
	}

	public void addOwnCoins(int Num)
	{
		ownCoins += Num;

		if(ownCoins < 0)
		{
			ownCoins = 0;
		}

		m_GamePlayLayer.updateOwnCoins ();
	}

	public int getOwnCoins()
	{
		return ownCoins;
	}

	public void addFieldItemNum(int Type, int Num)
	{
		for(int i = 0; i < fieldItemRecords.Count; i++)
		{
			fieldItemRecord record = fieldItemRecords[i];

//			if(!record)
//			{
//				continue;
//			}

			if(record.fieldItemType == Type)
			{
				record.addFieldItemNum(Num);
			}
		}
	}

	public int getFieldItemNum(int Type)
	{
		for(int i = 0; i < fieldItemRecords.Count; i++)
		{
			fieldItemRecord record = fieldItemRecords[i];

//			if(!record)
//			{
//				continue;
//			}

			if(record.fieldItemType == Type)
			{
				return record.fieldItemNum;
			}
		}

		return 0;
	}

	public void increaseLeftMoveNum(int Num)
	{
		if(Num <= 0)
		{
			return;
		}

		moveLeftNum += Num;
		m_GamePlayLayer.updatePlayInfo ((int)Config.Player_Info_Enum.Player_Info_Left_Move_Num, moveLeftNum);
	}

	public void decreaseLeftMoveNum()
	{
		moveLeftNum--;
		checkCountDownBomb ();

		m_GamePlayLayer.updatePlayInfo ((int)Config.Player_Info_Enum.Player_Info_Left_Move_Num, moveLeftNum);
	}

	public void finishTargetBlock(int ColorType)
	{
		int finishType = 0;

		Config.Block_Color_Enum enumType = (Config.Block_Color_Enum)(ColorType - 1);

		switch(enumType)
		{
		case Config.Block_Color_Enum.Block_Color_1:
		{
			finishType = (int)Config.Player_Info_Enum.Player_Info_Fruit_1_Num;
		}break;
		case Config.Block_Color_Enum.Block_Color_2:
		{
			finishType = (int)Config.Player_Info_Enum.Player_Info_Fruit_2_Num;
		}break;
		case Config.Block_Color_Enum.Block_Color_3:
		{
			finishType = (int)Config.Player_Info_Enum.Player_Info_Fruit_3_Num;
		}break;
		case Config.Block_Color_Enum.Block_Color_4:
		{
			finishType = (int)Config.Player_Info_Enum.Player_Info_Fruit_4_Num;
		}break;
		case Config.Block_Color_Enum.Block_Color_5:
		{
			finishType = (int)Config.Player_Info_Enum.Player_Info_Fruit_5_Num;
		}break;
		case Config.Block_Color_Enum.Block_Color_6:
		{
			finishType = (int)Config.Player_Info_Enum.Player_Info_Fruit_6_Num;
		}break;
		}

		addRecordNum (finishType, 1);
	}

	public void checkCountDownBomb()
	{
		for(int i = 0; i < AllSlotMap.Count; i++)
		{
			SlotBase slot = AllSlotMap[i];

			if(!slot)
			{
				continue;
			}

			if(!slot.m_SlotBlock)
			{
				continue;
			}

			if(slot.m_SlotBlock.m_nBlockType != (int)Config.Block_Enum.Block_Bomb_CountDown)
			{
				continue;
			}

			BlockBomb_CountDown bombBlock = (BlockBomb_CountDown)slot.m_SlotBlock;
			bombBlock.bombCountDown();
		}
	}

	public void resetAllWarmHoleRecord()
	{
		for(int i = 0; i < AllSlotMap.Count; i++)
		{
			SlotBase slot = AllSlotMap[i];
			
			if(!slot)
			{
				continue;
			}
			
			if(!slot.m_SlotBlock)
			{
				continue;
			}

			slot.m_SlotBlock.m_nLastWarmStartIndex = 0;
		}
	}

	public void addRecordNum(int Type, int Num)
	{
		int showNum = 0;
		bool needUpdateInfo = false;
		
		Config.Player_Info_Enum enumType = (Config.Player_Info_Enum)Type;
		switch(enumType)
		{
		case Config.Player_Info_Enum.Player_Info_Free_Fall_Num:
		{
			recordNum.freeFallNum += Num;
			showNum = recordNum.freeFallNum;
			
			needUpdateInfo = true;
		}break;
		case Config.Player_Info_Enum.Player_Info_Bombed_Block_Num:
		{
			recordNum.bombedNum += Num;
			showNum = recordNum.bombedNum;
			
			needUpdateInfo = true;
		}break;
		case Config.Player_Info_Enum.Player_Info_Fruit_1_Num:
		{
			recordNum.fruit1Num += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Fruit_2_Num:
		{
			recordNum.fruit2Num += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Fruit_3_Num:
		{
			recordNum.fruit3Num += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Fruit_4_Num:
		{
			recordNum.fruit4Num += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Fruit_5_Num:
		{
			recordNum.fruit5Num += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Fruit_6_Num:
		{
			recordNum.fruit6Num += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_1_Num:
		{
			recordNum.eraseFruit1Num += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_2_Num:
		{
			recordNum.eraseFruit2Num += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_3_Num:
		{
			recordNum.eraseFruit3Num += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_4_Num:
		{
			recordNum.eraseFruit4Num += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_5_Num:
		{
			recordNum.eraseFruit5Num += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_6_Num:
		{
			recordNum.eraseFruit6Num += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Use_Fruit_Bomb_Num:
		{
			recordNum.useFruitBombNum += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Use_Small_Bomb_Num:
		{
			recordNum.useSmallBombNum += Num;
		}break;
		case Config.Player_Info_Enum.Player_Info_Use_Big_Bomb_Num:
		{
			recordNum.useBigBombNum += Num;
		}break;
		default:
		{
		}break;
		}
		
		if(needUpdateInfo)
		{
			m_GamePlayLayer.updatePlayInfo(Type, showNum);
		}
	}

	public int getRecordNum(int Type)
	{
		Config.Player_Info_Enum enumType = (Config.Player_Info_Enum)Type;

		switch(enumType)
		{
		case Config.Player_Info_Enum.Player_Info_Left_Move_Num:
		{
			return moveLeftNum;
		}
		case Config.Player_Info_Enum.Player_Info_Free_Fall_Num:
		{
			return recordNum.freeFallNum;
		}
		case Config.Player_Info_Enum.Player_Info_Bombed_Block_Num:
		{
			return recordNum.bombedNum;
		}
		case Config.Player_Info_Enum.Player_Info_Gold_Coin:
		{
			return ownCoins;
		}
		case Config.Player_Info_Enum.Player_Info_Fruit_1_Num:
		{
			return recordNum.fruit1Num;
		}
		case Config.Player_Info_Enum.Player_Info_Fruit_2_Num:
		{
			return recordNum.fruit2Num;
		}
		case Config.Player_Info_Enum.Player_Info_Fruit_3_Num:
		{
			return recordNum.fruit3Num;
		}
		case Config.Player_Info_Enum.Player_Info_Fruit_4_Num:
		{
			return recordNum.fruit4Num;
		}
		case Config.Player_Info_Enum.Player_Info_Fruit_5_Num:
		{
			return recordNum.fruit5Num;
		}
		case Config.Player_Info_Enum.Player_Info_Fruit_6_Num:
		{
			return recordNum.fruit6Num;
		}
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_1_Num:
		{
			return recordNum.eraseFruit1Num;
		}
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_2_Num:
		{
			return recordNum.eraseFruit2Num;
		}
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_3_Num:
		{
			return recordNum.eraseFruit3Num;
		}
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_4_Num:
		{
			return recordNum.eraseFruit4Num;
		}
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_5_Num:
		{
			return recordNum.eraseFruit5Num;
		}
		case Config.Player_Info_Enum.Player_Info_Erase_Fruit_6_Num:
		{
			return recordNum.eraseFruit6Num;
		}
		case Config.Player_Info_Enum.Player_Info_Use_Fruit_Bomb_Num:
		{
			return recordNum.useFruitBombNum;
		}
		case Config.Player_Info_Enum.Player_Info_Use_Small_Bomb_Num:
		{
			return recordNum.useSmallBombNum;
		}
		case Config.Player_Info_Enum.Player_Info_Use_Big_Bomb_Num:
		{
			return recordNum.useBigBombNum;
		}
		default:
		{
		}break;
		}

		return 0;
	}

	public void updatePlayLayerStageInfo()
	{
		m_GamePlayLayer.checkStagePass ();
	}

	public void addLockItemType(int Type)
	{
		for(int i = 0; i < lockItemRecords.Count; i++)
		{
			int type = lockItemRecords[i];

			if(type == Type)
			{
				return;
			}
		} 

		lockItemRecords.Add (Type);
	}

	public bool isLockItem(int Type)
	{
		for(int i = 0; i < lockItemRecords.Count; i++)
		{
			int type = lockItemRecords[i];
			
			if(type == Type)
			{
				return true;
			}
		} 

		return false;
	}
}
