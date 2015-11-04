using UnityEngine;
using System.Collections;

public class BlockBomb_b : BlockBombBase {

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
		m_nBlockType = (int)Config.Block_Enum.Block_Big_Bomb;
		
		startTransBlock ();
	}

	override public void startTransBlock()
	{
		if(!m_BlockBodySprite)
		{
			return;
		}
		
		m_BlockBodySprite.spriteName = "big_Bomb_Normal_1";
		
		UISpriteAnimation spriteAnimation = m_BlockBodySprite.gameObject.GetComponent< UISpriteAnimation >();
		
		if (!spriteAnimation) 
		{
			spriteAnimation = m_BlockBodySprite.gameObject.AddComponent< UISpriteAnimation > ();
		}
			
		if(spriteAnimation)
		{
			if(specialBombInfo.isHorizontalBomb &&
			   specialBombInfo.isVerticalBomb)
			{
				spriteAnimation.namePrefix = "big_Bomb_Double_";
			}
			else if(specialBombInfo.isHorizontalBomb &&
			        !specialBombInfo.isVerticalBomb)
			{
				spriteAnimation.namePrefix = "big_Bomb_Horizontal_";
			}
			else if(!specialBombInfo.isHorizontalBomb &&
			        specialBombInfo.isVerticalBomb)
			{
				spriteAnimation.namePrefix = "big_Bomb_Vertical_";
			}
			else
			{
				spriteAnimation.namePrefix = "big_Bomb_Normal_";
			}
			
			spriteAnimation.framesPerSecond = 15;
			spriteAnimation.loop = true;
		}

		if(specialBombInfo.isHoldBomb)
		{
			setHoldBombImage();
		}
	}

	override public void showBombUPBaseImage()
	{
		if(m_ReleaseBlockSprite)
		{
			UISpriteAnimation spriteAnimation = m_ReleaseBlockSprite.gameObject.GetComponent< UISpriteAnimation >();
			spriteAnimation.SetScale(400, 400, 1);

			m_ReleaseBlockSprite.spriteName = "eftExplosion_C1";
			spriteAnimation.namePrefix = "eftExplosion_C";
			
			m_ReleaseBlockSprite.gameObject.SetActive(true);

			firstInFlag = 0;
			StartCoroutine(closeReleaseBlockSprite());
		}	
	}
}
