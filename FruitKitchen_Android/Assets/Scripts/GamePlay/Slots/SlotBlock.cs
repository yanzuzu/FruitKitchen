using UnityEngine;
using System.Collections;

public class SlotBlock : SlotBase {

	// Use this for initialization
	void Start () {
		initSlot ();
	}

	override public void initSlot()
	{
		base.initSlot ();
		
		m_nSlotType = (int)Config.Slot_Enum.Slot_Block;
	}

	override public void setBKISprite()
	{
		if(m_SlotBKSprite)
		{
			m_SlotBKSprite.gameObject.SetActive(false);
		}
	}
}
