﻿using UnityEngine;
using System.Collections;

public class BlockMilk : BlockBase {

	override public void initBlock(int color)
	{
		base.initBlock (color);
		
		m_nColorType = (int)Config.Block_Color_Enum.Block_Color_Null;
		m_nBlockType = (int)Config.Block_Enum.Block_Milk;
		m_bIsPlayerCanOperate = false;
		m_bIsCanMove = false;

		if(m_BlockBodySprite)
		{
			m_BlockBodySprite.transform.localScale = new Vector3( 54 , 54 , 0 );
			m_BlockBodySprite.spriteName = "milk";
		}
	}
}
