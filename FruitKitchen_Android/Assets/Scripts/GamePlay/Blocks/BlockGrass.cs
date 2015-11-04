using UnityEngine;
using System.Collections;

public class BlockGrass : BlockBase {

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
		m_nBlockType = (int)Config.Block_Enum.Block_Grass;
		m_bIsPlayerCanOperate = false;
		m_bIsCanMove = false;
		
		if(m_BlockBodySprite)
		{
			m_BlockBodySprite.transform.localScale = new Vector3( 54 , 54 , 0 );
			m_BlockBodySprite.spriteName = "grass_1";
		}
	}

	override public void blockWillRemove()
	{
		base.blockWillRemove();
		SoundManager.Instance.PlaySE("Sound_Grass_Broken", false);
	}
}
