using UnityEngine;
using System.Collections;

public class BlockWood : BlockBase {

	public int m_nResistBombNums = 2;

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
		m_nBlockType = (int)Config.Block_Enum.Block_Wood;
		m_bIsPlayerCanOperate = false;
		m_bIsCanMove = false;

		if(m_BlockBodySprite)
		{
			m_BlockBodySprite.spriteName = "wood_1";
			m_BlockBodySprite.transform.localScale = new Vector3( 54 , 54 , 1 );
		}
	}

	public void brokenBlock()
	{
		m_nResistBombNums--;

		if (m_BlockBodySprite)
		{
			m_BlockBodySprite.spriteName = "wood_2";

			SoundManager.Instance.PlaySE("Sound_Wood_Broken", false);
		}
	}

	override public void blockWillRemove()
	{
		base.blockWillRemove();
		SoundManager.Instance.PlaySE("Sound_Wood_Broken", false);
	}
}
