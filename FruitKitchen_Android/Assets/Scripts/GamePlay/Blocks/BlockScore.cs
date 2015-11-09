using UnityEngine;
using System.Collections;

public class BlockScore : BlockBase {

	public bool isTrash = false;
	public bool isDoubleFreeFall = false;
	public int moveNum = 0;


	public void setIsTrash()
	{
		if(isTrash)
		{
			return;
		}

		isTrash = true;
		isDoubleFreeFall = false;

		if(m_SauceSprite)
		{
			m_SauceSprite.gameObject.SetActive(true);
		}
	}

	public void setIsDoubleFreeFall()
	{
		if(isTrash || isDoubleFreeFall)
		{
			return;
		}

		isDoubleFreeFall = true;

		int colorIndex = m_nColorType - (int)Config.Block_Color_Enum.Block_Color_Num - 1;
		string fileName = string.Format ("productMilk_{0}", colorIndex);
		m_BlockBodySprite.spriteName = fileName;
	}

	override public void initBlock(int color)
	{
		base.initBlock (color);
		
		m_nColorType = color + (int)Config.Block_Color_Enum.Block_Color_Num + 1;
		m_nBlockType = (int)Config.Block_Enum.Block_Score;
		
		if(m_BlockBodySprite)
		{
			string fileName = string.Format ("product_{0}", color);
			m_BlockBodySprite.transform.localScale = new Vector3( 54 , 54 , 0 );
			m_BlockBodySprite.spriteName = fileName;
		}

		if(m_FreeMoveNumLabel)
		{
			m_FreeMoveNumLabel.gameObject.SetActive(true);
			m_FreeMoveNumLabel.transform.localPosition = new Vector3(19,-14,0);

			m_FreeMoveNumLabel.text = moveNum.ToString();
		}

		if(m_FreeMoveNumSprite)
		{
			m_FreeMoveNumSprite.gameObject.SetActive(true);
			m_FreeMoveNumSprite.transform.localPosition = new Vector3(15,-13,0);
		}

		CPDebug.Log ("BlockScore initBlock");
	}

	public void increaseMoveNum()
	{
		if(isTrash)
		{
			return;
		}

		moveNum++;
		m_FreeMoveNumLabel.text = moveNum.ToString();
	}
}
