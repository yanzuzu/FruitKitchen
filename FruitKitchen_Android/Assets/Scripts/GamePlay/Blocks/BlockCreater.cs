using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockCreater : MonoBehaviour {

	public static BlockCreater Instance;

	public GamePlayLayer	m_SlotMotherLayer;
	public BlockManager		m_SlotMotherManager;
	public GameObject		m_SampleObject;
	List< GameObject > gameObjects = new List< GameObject >();

	void Awake()
	{
		Instance = this;
	}


	public void initGameObjects(GameObject gameObject)
	{
		m_SampleObject = gameObject;

		for(int i = 0; i < Config.BLOCK_INIT_CREATE_NUM; i++)
		{
			GameObject TmpObj = Instantiate( gameObject.gameObject ) as GameObject;
			gameObjects.Add(TmpObj);
		}

		StartCoroutine (__bornGameObject());
	}

	IEnumerator __bornGameObject()
	{
		while(true)
		{
			yield return new WaitForSeconds (0.5f);

			if(gameObjects.Count < Config.BLOCK_INIT_CREATE_NUM)
			{
				GameObject TmpObj = Instantiate( m_SampleObject ) as GameObject;
				gameObjects.Add(TmpObj);
			}
		}
	}

	GameObject getOneObject()
	{
		if(gameObjects.Count <= 0)
		{
			return null;
		}

		GameObject TmpObj = gameObjects[0];
		gameObjects.RemoveAt (0);

		return TmpObj;
	}

	public BlockBase getBlockByType(GameObject gameObject, UIPanel Panel, int type, int color)
	{
		GameObject TmpObj = getOneObject ();

		if(TmpObj == null)
		{
			CPDebug.Log("getBlockByType null");
			TmpObj = Instantiate( gameObject.gameObject ) as GameObject;
		}

		TmpObj.transform.localScale = Vector3.one;
		TmpObj.SetActive( true  );

		Config.Block_Enum blockType = (Config.Block_Enum)type;
		switch(blockType)
		{
		case Config.Block_Enum.Block_Normal:
		{
			BlockNormal block = TmpObj.GetComponent< BlockNormal >();
			block.enabled = true;
			block.m_SlotMotherManager = m_SlotMotherManager;
			block.gameObject.transform.parent = Panel.transform;
			block.gameObject.transform.localScale = Vector3.one;
			block.initBlock(color);
			block.m_myGameObject = TmpObj;
			return block;
		}
		case Config.Block_Enum.Block_Score:
		{
			BlockScore block = TmpObj.GetComponent< BlockScore >();
			block.enabled = true;
			block.m_SlotMotherManager = m_SlotMotherManager;
			block.gameObject.transform.parent = Panel.transform;
			block.gameObject.transform.localScale = Vector3.one;
			block.initBlock(color);
			block.m_myGameObject = TmpObj;
			return block;
		}
		case Config.Block_Enum.Block_Produce:
		{
			BlockProduct block = TmpObj.GetComponent< BlockProduct >();
			block.enabled = true;
			block.m_SlotMotherManager = m_SlotMotherManager;
			block.gameObject.transform.parent = Panel.transform;
			block.gameObject.transform.localScale = Vector3.one;
			block.initBlock(1);
			block.m_myGameObject = TmpObj;
			return block;
		}
		case Config.Block_Enum.Block_Small_Bomb:
		{
			BlockBomb_s block = TmpObj.GetComponent< BlockBomb_s >();
			block.enabled = true;
			block.m_SlotMotherManager = m_SlotMotherManager;
			block.gameObject.transform.parent = Panel.transform;
			block.gameObject.transform.localScale = Vector3.one;
			block.initBlock(1);
			block.m_myGameObject = TmpObj;
			return block;
		}
		case Config.Block_Enum.Block_Big_Bomb:
		{
			BlockBomb_b block = TmpObj.GetComponent< BlockBomb_b >();
			block.enabled = true;
			block.m_SlotMotherManager = m_SlotMotherManager;
			block.gameObject.transform.parent = Panel.transform;
			block.gameObject.transform.localScale = Vector3.one;
			block.initBlock(1);
			block.m_myGameObject = TmpObj;
			return block;
		}
		case Config.Block_Enum.Block_Bomb_CountDown:
		{
			BlockBomb_CountDown block = TmpObj.GetComponent< BlockBomb_CountDown >();
			block.enabled = true;
			block.m_SlotMotherManager = m_SlotMotherManager;
			block.gameObject.transform.parent = Panel.transform;
			block.gameObject.transform.localScale = Vector3.one;
			block.initBlock(1);
			block.m_myGameObject = TmpObj;
			return block;
		}
		case Config.Block_Enum.Block_Bomb_ConnectOnly:
		{
			BlockBomb_ConnectOnly block = TmpObj.GetComponent< BlockBomb_ConnectOnly >();
			block.enabled = true;
			block.m_SlotMotherManager = m_SlotMotherManager;
			block.gameObject.transform.parent = Panel.transform;
			block.gameObject.transform.localScale = Vector3.one;
			block.initBlock(1);
			block.m_myGameObject = TmpObj;
			return block;
		}
		case Config.Block_Enum.Block_Wood:
		{
			BlockWood block = TmpObj.GetComponent< BlockWood >();
			block.enabled = true;
			block.m_SlotMotherManager = m_SlotMotherManager;
			block.gameObject.transform.parent = Panel.transform;
			block.gameObject.transform.localScale = Vector3.one;
			block.m_myGameObject = TmpObj;
			block.initBlock(1);
			return block;
		}
		case Config.Block_Enum.Block_Grass:
		{
			BlockGrass block = TmpObj.GetComponent< BlockGrass >();
			block.enabled = true;
			block.m_SlotMotherManager = m_SlotMotherManager;
			block.gameObject.transform.parent = Panel.transform;
			block.gameObject.transform.localScale = Vector3.one;
			block.initBlock(1);
			block.m_myGameObject = TmpObj;
			return block;
		}
		case Config.Block_Enum.Block_Sauce:
		{
			BlockSauce block = TmpObj.GetComponent< BlockSauce >();
			block.enabled = true;
			block.m_SlotMotherManager = m_SlotMotherManager;
			block.gameObject.transform.parent = Panel.transform;
			block.gameObject.transform.localScale = Vector3.one;
			block.initBlock(1);
			block.m_myGameObject = TmpObj;
			return block;
		}
		case Config.Block_Enum.Block_Milk:
		{
			BlockMilk block = TmpObj.GetComponent< BlockMilk >();
			block.enabled = true;
			block.m_SlotMotherManager = m_SlotMotherManager;
			block.gameObject.transform.parent = Panel.transform;
			block.gameObject.transform.localScale = Vector3.one;
			block.initBlock(1);
			block.m_myGameObject = TmpObj;
			return block;
		}
		case Config.Block_Enum.Block_Field_Item:
		{
			BlockFieldItem block = TmpObj.GetComponent< BlockFieldItem >();
			block.enabled = true;
			block.m_SlotMotherManager = m_SlotMotherManager;
			block.gameObject.transform.parent = Panel.transform;
			block.gameObject.transform.localScale = Vector3.one;
			block.initBlock(color);
			block.m_myGameObject = TmpObj;
			return block;
		}
		case Config.Block_Enum.Block_Bomb_Fruit:
		{
			BlockBomb_Fruit block = TmpObj.GetComponent< BlockBomb_Fruit >();
			block.enabled = true;
			block.m_SlotMotherManager = m_SlotMotherManager;
			block.gameObject.transform.parent = Panel.transform;
			block.gameObject.transform.localScale = Vector3.one;
			block.initBlock(color);
			block.m_myGameObject = TmpObj;
			return block;
		}
		}

		return null;
	}
	
}
