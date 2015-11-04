using UnityEngine;
using System.Collections;

public class BlockBomb_CountDown : BlockBombBase {

	public int		m_nCountDownNum;

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
		m_nBlockType = (int)Config.Block_Enum.Block_Bomb_CountDown;
		m_bIsPlayerCanOperate = false;
		m_bIsCanMove = false;
		m_nCountDownNum = 1;
		
		if(m_BlockBodySprite)
		{
			m_BlockBodySprite.transform.localScale = new Vector3( 54 , 54 , 0 );
			m_BlockBodySprite.spriteName = "countDownBomb";
		}

		if(m_CountDownBombLabel)
		{
			m_CountDownBombLabel.gameObject.SetActive(true);
		}
	}

	public void bombCountDown()
	{
		m_nCountDownNum--;
		setCountDownNum (m_nCountDownNum);
	}

	public void setCountDownNum(int CountDownNum)
	{
		if(CountDownNum < 0)
		{
			return;
		}

		m_nCountDownNum = CountDownNum;

		if(m_nCountDownNum <= 0)
		{
			block_blockDoBomb(true);
		}
		else
		{
			m_CountDownBombLabel.text = m_nCountDownNum.ToString();
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
		}	
	}
}
