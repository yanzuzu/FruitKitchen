using UnityEngine;
using System.Collections;

public class BlockNormal : BlockBase {

	public bool			isGoldBlock;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setGoldBlock()
	{
		if(isGoldBlock)
		{
			return;
		}

		isGoldBlock = true;

		if(goldImage)
		{
			goldImage.gameObject.SetActive(true);
		}
	}

	override public void initBlock(int color)
	{
		base.initBlock (color);

		setBlockColor (color);

		if(!m_SlotMotherManager.isTeachMode)
		{
			int key = DataManager.Instance.StageKey;

			int goldRate = CSVManager.Instance.StageInfoTable.readFieldAsInt(key, "FruitGoldRate");

			System.Random rnd = new System.Random();
			int nowRate = rnd.Next(1, 100);

			if(nowRate <= goldRate)
			{
				setGoldBlock();
			}
			else
			{
				if(goldImage)
				{
		//			goldImage.gameObject.SetActive(false);
				}
			}
		}
	}

	override public void setBlockColor(int color)
	{
		if(m_nColorType == color)
		{
			return;
		}

		m_nColorType = color;
		m_nBlockType = (int)Config.Block_Enum.Block_Normal;

		string fileName = string.Format ("fruit_{0}", m_nColorType);

		if(m_BlockBodySprite)
		{
			m_BlockBodySprite.spriteName = fileName;
		}
	}
}
