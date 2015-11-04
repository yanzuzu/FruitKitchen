using UnityEngine;
using System.Collections;

public class SlotScore : SlotBase {

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
		
		m_nSlotType = (int)Config.Slot_Enum.Slot_Score;
	}

	override public void setBKISprite()
	{
		if(m_SlotBKSprite)
		{
			m_SlotBKSprite.spriteName = "UI_InGame_Goal";

			float x = m_SlotBKSprite.transform.localPosition.x;
			float y = m_SlotBKSprite.transform.localPosition.y;
			float z = m_SlotBKSprite.transform.localPosition.z;

			y += 35;

			m_SlotBKSprite.transform.localPosition = new Vector3(x,y,z);
		}
	}

	override public bool isCanReceiveBlock(bool FromLR)
	{
		if(FromLR)
		{
			return false;
		}

		return true;
	}

	override public bool receiveBlock(BlockBase Block)
	{
		if(Block.m_nBlockType != (int)Config.Block_Enum.Block_Score)
		{
			return false;
		}

		BlockScore scoreBlock = (BlockScore)Block;

		if(scoreBlock.isTrash)
		{
			return false;
		}
		
		m_SlotBlock = Block;

		int num = m_SlotBlock.m_nColorType - (int)Config.Block_Color_Enum.Block_Color_Num - 1;
		m_SlotMotherManager.finishTargetBlock (num);

		int freeFallNum = scoreBlock.moveNum;

		if(freeFallNum < 0)
		{
			freeFallNum = 0;
		}

		if(scoreBlock.isDoubleFreeFall)
		{
			freeFallNum *= 2;
		}

		m_SlotMotherManager.addRecordNum ((int)Config.Player_Info_Enum.Player_Info_Free_Fall_Num, freeFallNum);

		SoundManager.Instance.PlaySE("Sound_Block_Drop", false);

		if(freeFallNum > 0)
		{
			showNumberEffect("UI_InGame_Target_Free_Fall", freeFallNum);
		}

		blockEraseDirect (false, false);
		
		return true;
	}
}
