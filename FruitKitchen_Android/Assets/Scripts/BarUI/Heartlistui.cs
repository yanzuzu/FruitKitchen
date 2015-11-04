using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Heartlistui : MonoBehaviour {
	[SerializeField]
	List<GameObject> m_heartList;
	private int m_lifeNum;
	// Use this for initialization
	void Start () {
		StartCoroutine( UpdateHeart());

	}

	IEnumerator UpdateHeart()
	{
		while( true )
		{
			// get Time txt
			m_lifeNum = LocalDBManager.Instance.GetLifeNum();
			for( int i = 0  ; i < m_heartList.Count ; i ++ )
			{
				if( i >= m_lifeNum )
					m_heartList[i].SetActive( false );
				else
					m_heartList[i].SetActive( true );
			}
			yield return new WaitForSeconds( 1 );
		}
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void AddHeart()
	{
		int nowHeart = LifeManager.Instance.leftPlayNum;

		if(nowHeart < Config.MAX_PLAY_NUM)
		{
			PopupManager.Instance.ShowStoreShopUI();
		}
	}

	void OnApplicationFocus()
	{
		CPDebug.Log("OnApplicationFocus start");
		LifeManager.Instance.updateLifeNumByDB();
		//UpdateHeart();
	}
}
