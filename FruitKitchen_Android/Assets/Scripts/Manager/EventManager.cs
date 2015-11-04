using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : ManagerComponent< EventManager > {
	public enum EventName
	{
		LoadScene = 1,
		FriendListDataUpdate = 2,
		MessageListUpdate = 3,

		FBLoginOK = 4,
		FBLogoutOK = 5,

		IAP_BuyOK = 6,
	}


	public delegate void OnEventCallBack( params object [] param );
	private Dictionary<EventName , List<OnEventCallBack> > EventMap = new Dictionary<EventName, List<OnEventCallBack>>(); 
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void onEvent(EventName EventNameValue , params object []  param )
	{

		if( EventMap.ContainsKey( EventNameValue ) == false ) return;
		foreach( EventName EventKey in EventMap.Keys )
		{
			if( EventKey != EventNameValue ) continue;
			List<OnEventCallBack> CBList = EventMap[EventKey];
			for( int i = CBList.Count - 1 ; i >= 0 ; i -- )
			{
				OnEventCallBack CB = CBList[i];
				if( CB.Target.Equals(null) )
				{
					CBList.RemoveAt(i);
					continue;
				}

				try
				{
					CB(param);
				}catch( System.IO.IOException e )
				{
					CBList.RemoveAt(i);
					CPDebug.LogError("Event Error = " + e );
				}
			}

		}	
	}

	public void registerEvent(EventName EventNameValue, OnEventCallBack CB)
	{
		if( EventMap.ContainsKey( EventNameValue ) == false )
			EventMap[EventNameValue] = new List<OnEventCallBack>();
		EventMap[EventNameValue].Add(CB);
	}

}
