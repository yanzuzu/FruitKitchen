using UnityEngine;
using System.Collections;

public class BlockBase : MonoBehaviour {


	public int			m_nColorType;
	public int			m_nBlockType;
	public int			m_nBelongSlotIndex;
	public int			m_nLastWarmStartIndex;
	public int			m_nBlockIndex;
	public float		m_fScaleSize_x;
	public float		m_fScaleSize_y;
	public bool			m_bIsOnTable;
	public bool			m_bIsMainActionBlock;
	public bool			m_bIsPlayerCanOperate;
	public bool			m_bIsCanMove;
	public bool			m_bEraseByBomb;
	public UISprite		m_BlockBodySprite;
	public UISprite		m_ReleaseBlockSprite;
	public UISprite		m_FreeMoveNumSprite;
	public UISprite		m_SauceSprite;
	public UILabel		m_FreeMoveNumLabel;
	public UILabel		m_CountDownBombLabel;
	public static int 	blockIndex = 1;
	public GamePlayLayer	m_SlotMotherLayer;
	public BlockManager		m_SlotMotherManager;
	public GameObject		m_myGameObject;
	public UISprite		goldImage;
	public UISprite		holdBombSprite;
	public bool 		isNowMoving = false;
	public bool 		isNowShaking = false;

	void Awake()
	{
//		if(m_ReleaseBlockSprite)
//		{
//			m_ReleaseBlockSprite.gameObject.SetActive(false);
//		}
//		
//		if(goldImage)
//		{
//			goldImage.gameObject.SetActive(false);
//		}
//
//		if(holdBombSprite)
//		{
//			holdBombSprite.gameObject.SetActive(false);
//		}
//
//		if(m_FreeMoveNumSprite)
//		{
//			m_FreeMoveNumSprite.gameObject.SetActive(false);
//		}
//
//		if(m_FreeMoveNumLabel)
//		{
//			m_FreeMoveNumLabel.gameObject.SetActive(false);
//		}
//
//		if(m_CountDownBombLabel)
//		{
//			m_CountDownBombLabel.gameObject.SetActive(false);
//		}
//
//		if(m_SauceSprite)
//		{
//			m_SauceSprite.gameObject.SetActive(false);
//		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	virtual public void initBlock(int color)
	{
		m_nColorType = -1;
		m_nBlockType = -1;
		m_nBelongSlotIndex = -1;
		m_nLastWarmStartIndex = 0;
		m_nBlockIndex = -1;
		m_fScaleSize_x = 0.0f;
		m_fScaleSize_y = 0.0f;
		m_bIsOnTable = true;
		m_bIsMainActionBlock = true;
		m_bIsPlayerCanOperate = true;
		m_bIsCanMove = true;
		m_bEraseByBomb = false;

		m_nBlockIndex = blockIndex;
		blockIndex++;
	}

	public bool isBomb()
	{
		if(m_nBlockType == (int)Config.Block_Enum.Block_Big_Bomb ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Small_Bomb ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Bomb_CountDown ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Bomb_ConnectOnly ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
		{
			return true;
		}
		
		return false;
	}

	public void showReleaseBlockImage_knife()
	{
		if(m_ReleaseBlockSprite)
		{
			UISpriteAnimation spriteAnimation = m_ReleaseBlockSprite.gameObject.GetComponent< UISpriteAnimation >();

			m_ReleaseBlockSprite.spriteName = "eftSlice_1";
			spriteAnimation.namePrefix = "eftSlice_";
			m_ReleaseBlockSprite.gameObject.SetActive(true);

			StartCoroutine(closeReleaseBlockSprite());
		}
	}

	public int firstInFlag = 0;
	public IEnumerator closeReleaseBlockSprite()
	{
		while(true)
		{
			if(m_ReleaseBlockSprite)
			{
				if(firstInFlag == 1)
				{
					m_ReleaseBlockSprite.gameObject.SetActive(false);
					yield break;
				}
				else
				{
					firstInFlag = 1;
				}
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	virtual public void showReleaseBlockImage()
	{
		if(m_ReleaseBlockSprite)
		{
			UISpriteAnimation spriteAnimation = m_ReleaseBlockSprite.gameObject.GetComponent< UISpriteAnimation >();

			Config.Block_Color_Enum enumType = (Config.Block_Color_Enum)(m_nColorType - 1);

			switch(enumType)
			{
			case Config.Block_Color_Enum.Block_Color_1:
			{
				m_ReleaseBlockSprite.spriteName = "eft_FruitExplosion_A1";
				spriteAnimation.namePrefix = "eft_FruitExplosion_A";
			}break;
			case Config.Block_Color_Enum.Block_Color_2:
			{
				m_ReleaseBlockSprite.spriteName = "eft_FruitExplosion_C1";
				spriteAnimation.namePrefix = "eft_FruitExplosion_C";
			}break;
			case Config.Block_Color_Enum.Block_Color_3:
			{
				m_ReleaseBlockSprite.spriteName = "eft_FruitExplosion_E1";
				spriteAnimation.namePrefix = "eft_FruitExplosion_E";
			}break;
			case Config.Block_Color_Enum.Block_Color_4:
			{
				m_ReleaseBlockSprite.spriteName = "eft_FruitExplosion_D1";
				spriteAnimation.namePrefix = "eft_FruitExplosion_D";
			}break;
			case Config.Block_Color_Enum.Block_Color_5:
			{
				m_ReleaseBlockSprite.spriteName = "eft_FruitExplosion_B1";
				spriteAnimation.namePrefix = "eft_FruitExplosion_B";
			}break;
			case Config.Block_Color_Enum.Block_Color_6:
			{
				m_ReleaseBlockSprite.spriteName = "eft_FruitExplosion_F1";
				spriteAnimation.namePrefix = "eft_FruitExplosion_F";
			}break;
			}

			m_ReleaseBlockSprite.gameObject.SetActive(true);
		}
	}

	public void stopMoving()
	{
		if(isNowMoving)
		{
			Go.killAllTweensWithTarget(gameObject);
			iTween.Stop(m_myGameObject);
			isNowMoving = false;
		}
	}

	public void resetSize()
	{
		if(m_BlockBodySprite)
		{
			m_BlockBodySprite.gameObject.transform.localScale = new Vector3(50, 50, 1);
		}
	}
	
	public void beSelect()
	{
		if(m_BlockBodySprite)
		{
			m_BlockBodySprite.gameObject.transform.localScale = new Vector3(70, 70, 1);
		}
	}

	public bool isNeedNoResetBlock()
	{
		if(m_nBlockType == (int)Config.Block_Enum.Block_Score ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Produce ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Small_Bomb ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Big_Bomb ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Wood ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Grass ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Bomb_CountDown ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Bomb_ConnectOnly ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Sauce ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Milk ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Field_Item ||
		   m_nBlockType == (int)Config.Block_Enum.Block_Bomb_Fruit)
		{
			return true;
		}

		return false;
	}

	virtual public void setBlockColor(int color)
	{

	}

	virtual public bool copyBlockDetail(BlockBase NewBlock)
	{
		return true;
	}

	virtual public void blockWillRemove()
	{
		
	}
}
