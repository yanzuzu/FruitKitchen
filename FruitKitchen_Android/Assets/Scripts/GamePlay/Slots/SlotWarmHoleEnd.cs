using UnityEngine;
using System.Collections;

public class SlotWarmHoleEnd : SlotNormal {

	public int myStartSlotIndex = -1;
	public bool isPause;

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

		m_nSlotType = (int)Config.Slot_Enum.Slot_WarmHole_End;
		isPause = false;
	}

	public void changeSpriteByIndex(int Index)
	{
		// 101,103,105,107,109,111,113,115,117,119
		if(Index < 101 ||
		   Index > 119 ||
		   Index % 2 == 0)
		{
			return;
		}
		
		int imageNum = ((Index - 101) / 2) + 1;
		
		string fileName = string.Format ("UI_InGame_SquareWarmHoleOut_{0}", imageNum);
		m_SlotBKSprite.spriteName = fileName;
	}

	public bool isCanReceiveBlockByWarmHole()
	{
		if(m_SlotBlock)
		{
			return false;
		}

		return true;
	}

	override public void setBKISprite()
	{
		if(m_SlotBKSprite)
		{
			m_SlotBKSprite.spriteName = "UI_InGame_SquareWarmHoleIn";
		}
	}

	override public bool receiveBlock(BlockBase Block)
	{
		if(m_SlotBlock)
		{
			return false;
		}

		if(!Block)
		{
			return false;
		}

		setBlockDirect (Block);

		if(Block.m_nBlockType == (int)Config.Block_Enum.Block_Score)
		{
			BlockScore scoreBlock = (BlockScore)Block;
			scoreBlock.increaseMoveNum();
		}

		return true;
	}

	override public bool isCanReceiveBlock(bool FromLR)
	{
		if(!isPause)
		{
			return false;
		}
		else
		{
			if(m_SlotBlock)
			{
				return false;
			}

			return true;
		}
	}
}
