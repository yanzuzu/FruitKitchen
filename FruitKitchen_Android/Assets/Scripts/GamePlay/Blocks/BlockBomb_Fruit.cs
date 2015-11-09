using UnityEngine;
using System.Collections;

public class BlockBomb_Fruit : BlockBombBase {

	override public void initBlock(int color)
	{
		base.initBlock (color);
		
		m_nColorType = color;
		m_nBlockType = (int)Config.Block_Enum.Block_Bomb_Fruit;
		isCanTouchBomb = false;
		isCanConnectBomb = false;
		isCanSetSpecialBomb = false;
		
		if(m_BlockBodySprite)
		{
			string fileName = string.Format ("fruit_Bomb_{0}", m_nColorType);
			m_BlockBodySprite.transform.localScale = new Vector3( 54 , 54 , 0 );
			m_BlockBodySprite.spriteName = fileName;
		}
	}

	override public void showBombUPBaseImage()
	{
		if(m_ReleaseBlockSprite)
		{
			UISpriteAnimation spriteAnimation = m_ReleaseBlockSprite.gameObject.GetComponent< UISpriteAnimation >();
			spriteAnimation.SetScale(200, 200, 1);
			
			m_ReleaseBlockSprite.spriteName = "eftExplosion_C1";
			spriteAnimation.namePrefix = "eftExplosion_C";
			
			m_ReleaseBlockSprite.gameObject.SetActive(true);

			firstInFlag = 0;
			StartCoroutine(closeReleaseBlockSprite());
		}	
	}
}
