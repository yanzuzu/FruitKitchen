using UnityEngine;
using System.Collections;

public class PigTest : MonoBehaviour {
	bool enabled = false;
	// Use this for initialization

	void Start()
	{
		StartCoroutine("test",12);

	}

	IEnumerator test(object value )
	{
		while( true )
		{
			CPDebug.Log("11111111111111~~~" + value );
			yield return null;
		}
	}
	void Update()
	{
		if( Input.GetKeyDown( KeyCode.A ))
		{
			StopCoroutine("test");
		}
	}
	/*
	void Awake() {
		enabled = false;                  
		FB.Init(SetInit, OnHideUnity);  
	}

	private void SetInit()                                                                       
	{                                                                                            
		Debug.Log("SetInit");                                                                  
		enabled = true; // "enabled" is a property inherited from MonoBehaviour                  
		if (FB.IsLoggedIn)                                                                       
		{                                                                                        
			Debug.Log("Already logged in");                                                    
			OnLoggedIn();                                                                        
		}  
	}                                                                                            
	
	private void OnHideUnity(bool isGameShown)                                                   
	{                                                                                            
		Debug.Log("OnHideUnity");                                                              
		if (!isGameShown)                                                                        
		{                                                                                        
			// pause the game - we will need to hide                                             
			Time.timeScale = 0;                                                                  
		}                                                                                        
		else                                                                                     
		{                                                                                        
			// start the game back up - we're getting focus again                                
			Time.timeScale = 1;                                                                  
		}                                                                                        
	} 

	void OnGUI()
	{
		if (!FB.IsLoggedIn)                                                                                              
		{                                                                                                                

			if (GUI.Button(new Rect(179 , 11, 287, 160), "login Button" ))                                      
			{                                                                                                            
				FB.Login("email,publish_actions", LoginCallback);                                                        
			}                                                                                                            
		}    

	}

	void LoginCallback(FBResult result)                                                        
	{                                                                                          
		Debug.Log("LoginCallback");                                                          
		
		if (FB.IsLoggedIn)                                                                     
		{                                                                                      
			OnLoggedIn();                                                                      
		}                                                                                      
	}

	public string FriendSelectorTitle = "";
	public string FriendSelectorMessage = "Derp";
	public string FriendSelectorFilters = "[\"all\",\"app_users\",\"app_non_users\"]";
	public string FriendSelectorData = "{}";
	public string FriendSelectorExcludeIds = "";
	public string FriendSelectorMax = "";

	void OnLoggedIn()                                                                          
	{                                                                                          
		Debug.Log("Logged in. ID: " + FB.UserId); 
		// Reqest player info and profile picture                                                                           
		//FB.API("/me?fields=id,first_name,friends.fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);  
		FB.API("/me?fields=email,friends.limit(10).fields(first_name,id)",Facebook.HttpMethod.GET, APICallback); 

		// include the exclude ids
		/*string[] excludeIds = (FriendSelectorExcludeIds == "") ? null : FriendSelectorExcludeIds.Split(',');
		
		FB.AppRequest(
			FriendSelectorMessage,
			null,
			FriendSelectorFilters,
			excludeIds,
			100,
			FriendSelectorData,
			FriendSelectorTitle,
			APICallback
			);

	}                      
	void APICallback(FBResult result)                                                                                              
	{                                                                                                                              
		Debug.Log("APICallback");                                                                                                
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			Debug.LogError(result.Error);                                                                                           
			// Let's just try again                                                                                                
			FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);     
			return;                                                                                                                
		}
		Debug.Log("result = " + result.Text );                                       
	}    */            
}
