using UnityEngine;
using System.Collections;

public class BlockFieldItem : BlockBase {

	public int fieldItemType = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	override public void initBlock(int color)
	{
		base.initBlock (color);
		
		m_nColorType = (int)Config.Block_Color_Enum.Block_Color_Null;
		m_nBlockType = (int)Config.Block_Enum.Block_Field_Item;

		string sFileName = null;

		switch(color)
		{
		case Config.CARD_ITEM_BOMB_CONNECT:
		{
			fieldItemType = (int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Connect;
			sFileName = "Item_Connect_Bomb";
		}break;
		case Config.CARD_ITEM_BOMB_UP:
		{
			fieldItemType = (int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Upgrade;
			sFileName = "Item_BombUpgrade";
		}break;
		case Config.CARD_ITEM_KNIFE_ERASE:
		{
			fieldItemType = (int)Config.Operate_Mode_Enum.Operate_Mode_Erase_Knife;
			sFileName = "Item_Erase_Knife";
		}break;
		case Config.CARD_ITEM_WARM_HOLE:
		{
			fieldItemType = (int)Config.Operate_Mode_Enum.Operate_Mode_Wormhole;
			sFileName = "Item_WarmHole";
		}break;
		case Config.CARD_ITEM_BOMB_DOUBLE:
		{
			fieldItemType = (int)Config.Operate_Mode_Enum.Operate_Mode_Bomb_Twice;
			sFileName = "Item_Double_Bomb";
		}break;
		case Config.GOLD_ITEM_LIGHTING:
		{
			fieldItemType = (int)Config.Operate_Mode_Enum.Operate_Mode_Lignting;
			sFileName = "Item_Lighting";
		}break;
		case Config.GOLD_ITEM_SHOVEL:
		{
			fieldItemType = (int)Config.Operate_Mode_Enum.Operate_Mode_Shovel;
			sFileName = "Item_Shovel";
		}break;
		case Config.GOLD_ITEM_BOMBER:
		{
			fieldItemType = (int)Config.Operate_Mode_Enum.Operate_Mode_Bomber;
			sFileName = "Item_Bomber";
		}break;
		case Config.GOLD_ITEM_ATOMIC:
		{
			fieldItemType = (int)Config.Operate_Mode_Enum.Operate_Mode_Atomic;
			sFileName = "Item_Atomic";
		}break;
		}
		
		if(m_BlockBodySprite)
		{
			m_BlockBodySprite.transform.localScale = new Vector3( 54 , 54 , 0 );
			m_BlockBodySprite.spriteName = sFileName;
		}
	}

	override public void blockWillRemove()
	{
		base.blockWillRemove();
		m_SlotMotherManager.addFieldItemNum(fieldItemType, 1);

		SoundManager.Instance.PlaySE("Sound_Get_Item", false);
	}
}
