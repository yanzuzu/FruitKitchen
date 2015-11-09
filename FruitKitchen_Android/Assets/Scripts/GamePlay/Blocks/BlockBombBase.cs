using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockBombBase : BlockBase {

	public struct Special_Bomb_Info
	{
		public bool    isVerticalBomb;   
		public bool    isHorizontalBomb;  
		public bool    isHoldBomb;        
		public int     bombNum;            
	};

	public Special_Bomb_Info specialBombInfo;
	public List< string > connectBombs = new List< string >();
	public bool isBombThisTurn = false;
	public bool isCanTouchBomb = true;
	public bool isCanConnectBomb = true;
	public bool isCanSetSpecialBomb = true;

	void Awake()
	{

	}

	public void block_blockDoBomb(bool NeedEraseSelf)
	{
		if(!m_SlotMotherManager)
		{
			return;
		}

		SlotBase slot = m_SlotMotherManager.getSlotByIndex (m_nBelongSlotIndex);

		if(!slot)
		{
			return;
		}

		SlotNormal mySlot = (SlotNormal)slot;

		int widthRange = 3;
		int heightRange = 3;

		if(m_nBlockType == (int)Config.Block_Enum.Block_Big_Bomb)
		{
			widthRange = 5;
			heightRange = 5;
		}

		if(specialBombInfo.isHorizontalBomb &&
		   specialBombInfo.isVerticalBomb)
		{
			int newWidth = Config.WIDTH_NUM * 2;
			int newHeight = heightRange - 2;

			mySlot.bombInRange(newWidth, newHeight, NeedEraseSelf);

			newWidth = widthRange - 2;
			newHeight = Config.HEIGHT_NUM * 2;

			mySlot.bombInRange(newWidth, newHeight, NeedEraseSelf);
		}
		else if(specialBombInfo.isHorizontalBomb)
		{
			widthRange = Config.WIDTH_NUM * 2;
			heightRange = heightRange - 2;

			mySlot.bombInRange(widthRange, heightRange, NeedEraseSelf);
			
		}
		else if(specialBombInfo.isVerticalBomb)
		{
			heightRange = Config.HEIGHT_NUM * 2;
			widthRange = widthRange - 2;

			mySlot.bombInRange(widthRange, heightRange, NeedEraseSelf);
		}
		else
		{
			mySlot.bombInRange(widthRange, heightRange, NeedEraseSelf);
		}

		for(int i = 0; i < connectBombs.Count; i++)
		{
			int blockId = int.Parse(connectBombs[i]);
			BlockBase block = m_SlotMotherManager.getBlockById(blockId);

			if(!block)
			{
				continue;
			}

			BlockBombBase bombBlock = (BlockBombBase)block;

			if(!bombBlock.isBombThisTurn &&
			   bombBlock.m_bIsOnTable)
			{
				bombBlock.block_blockDoBomb(true);
			}
		}

		showBombUPBaseImage ();
		SoundManager.Instance.PlaySE("Sound_Bomb_Up", false);
	}

	public void addBombNum(int Num)
	{
		specialBombInfo.bombNum += Num;

		if(specialBombInfo.bombNum < 0)
		{
			specialBombInfo.bombNum = 0;

			if(m_CountDownBombLabel)
			{
				m_CountDownBombLabel.gameObject.SetActive(false);
			}
		}
		else
		{
			if(m_CountDownBombLabel)
			{
				if(specialBombInfo.bombNum == 1)
				{
					m_CountDownBombLabel.gameObject.SetActive(false);
				}
				else
				{
					m_CountDownBombLabel.gameObject.SetActive(true);
					m_CountDownBombLabel.transform.localPosition = new Vector3(-2.3f, -1.5f, 1);
					m_CountDownBombLabel.color = Color.white;
					m_CountDownBombLabel.text = specialBombInfo.bombNum.ToString();
				}
			}
		}

		if(specialBombInfo.bombNum > Config.SPECIAL_BOMB_MAX_NUM)
		{
			specialBombInfo.bombNum = Config.SPECIAL_BOMB_MAX_NUM;
		}
	}

	public void setVerticalBomb(bool CanDo)
	{
		if(specialBombInfo.isVerticalBomb == CanDo)
		{
			return;
		}

		specialBombInfo.isVerticalBomb = CanDo;

		if(CanDo)
		{
			startTransBlock();
		}
	}

	public void setHorizontalBomb(bool CanDo)
	{
		if(specialBombInfo.isHorizontalBomb == CanDo)
		{
			return;
		}
		
		specialBombInfo.isHorizontalBomb = CanDo;
		
		if(CanDo)
		{
			startTransBlock();
		}
	}

	public void setHoldBomb(bool CanDo)
	{
		if(specialBombInfo.isHoldBomb == CanDo)
		{
			return;
		}
		
		specialBombInfo.isHoldBomb = CanDo;
		
		if(CanDo)
		{
			setHoldBombImage();
			m_bIsCanMove = false;
		}
	}

	public void setHoldBombImage()
	{
		if(holdBombSprite)
		{
			holdBombSprite.gameObject.SetActive(true);
		}
	}

	public void setBombNum(int Num)
	{
		specialBombInfo.bombNum = Num;

		if(Num > 1)
		{
			if(m_CountDownBombLabel)
			{
				m_CountDownBombLabel.gameObject.SetActive(true);
			}

			m_CountDownBombLabel.transform.localPosition = new Vector3(-2.3f, -1.5f, 1);
			m_CountDownBombLabel.color = Color.white;
			m_CountDownBombLabel.text = Num.ToString();
		}
	}

	override public bool copyBlockDetail(BlockBase NewBlock)
	{
		if(!NewBlock)
		{
			return false;
		}

		if(!NewBlock.isBomb())
		{
			return false;
		}

		BlockBombBase transBlock = (BlockBombBase)NewBlock;

		transBlock.setHoldBomb (transBlock.specialBombInfo.isHoldBomb | specialBombInfo.isHoldBomb);
		transBlock.setVerticalBomb (transBlock.specialBombInfo.isVerticalBomb | specialBombInfo.isVerticalBomb);
		transBlock.setHorizontalBomb (transBlock.specialBombInfo.isHorizontalBomb | specialBombInfo.isHorizontalBomb);

		int maxBombNum = specialBombInfo.bombNum > transBlock.specialBombInfo.bombNum ? specialBombInfo.bombNum : transBlock.specialBombInfo.bombNum;
		transBlock.setBombNum (maxBombNum);

		for(int i = 0; i < connectBombs.Count; i++)
		{
			string sId = connectBombs[i];

			if(sId.Length <= 0)
			{
				continue;
			}

			transBlock.connectBombs.Add(sId);
		}

		transBlock.m_nBlockIndex = m_nBlockIndex;

		return true;
	}

	override public void showReleaseBlockImage()
	{
	}

	virtual public void startTransBlock()
	{

	}

	virtual public void showBombUPBaseImage()
	{
		
	}
}
