using UnityEngine;
using System.Collections;
using Pathfinding.Serialization.JsonFx;
using System.Collections.Generic;
public class FaceBookManager : ManagerComponent< FaceBookManager > {
	public delegate void IsLoginOK(string FBid);

	IsLoginOK m_loginCb = null;
	void Awake()
	{
		FB.Init( OnFBInit );
	}

	void OnFBInit()
	{
		CPDebug.Log("OnFBInit OK");
		// add Friend
		/*AddTestFriendData("1594618099");
		LocalDBManager.Instance.SetGameRecordObject("IYvwQoyFYP");
		LocalDBManager.Instance.SetGameScoreObject("yNHFLgFA93");
		NetWorkManager.Instance.myData = new MyPersonalData("100004021787606", "Sorrows Lee", "fghfhfc");
		NetWorkManager.Instance.parse_StartCheckData();*/
	}
	void AddTestFriendData(string p_fbUid)
	{
		PersonalData TmpData = new PersonalData(p_fbUid);
		TmpData.PD_FB_Name = "123";
		TmpData.PD_IsPlayThisGame = true;
		NetWorkManager.Instance.addFBFriendData( TmpData );
	}
	// login FB
	public void LoginFB( IsLoginOK p_loginCB  = null )
	{
		CPDebug.Log("LoginFB start");
		PopupManager.Instance.VisibleLoading(true);
		m_loginCb = p_loginCB;
		FB.Login("email,publish_actions", LoginCallback);           
	}

	void LoginCallback(FBResult result)                                                        
	{                                                                                          
		Debug.Log("LoginCallback result = " + result.Text );                                                          
		
		if (FB.IsLoggedIn)                                                                     
		{                                                                                      
			LocalDBManager.Instance.SetFBUid(FB.UserId);
			// get friend and personal data 
			//FB.API("/me?fields=installed,id,first_name,picture , friends.limit(500).fields(first_name,id,picture)", Facebook.HttpMethod.GET, APICallback); 
			//FB.API("/me?fields=id,first_name,picture", Facebook.HttpMethod.GET, APICallback); 

			FB.API("/me?fields=id,name,picture", Facebook.HttpMethod.GET, MyDataCallBack); 
			                                         
		}else
		{
			if( m_loginCb != null )
				m_loginCb(""); 
			PopupManager.Instance.VisibleLoading(false);
		}
	}
	void MyDataCallBack( FBResult result)
	{
		Debug.Log("MyDataCallBack result = " + result.Text);
		if (result.Error != null)                                                                                                  
		{ 
			m_loginCb(""); 
			PopupManager.Instance.VisibleLoading(false);
			return;                   
		}
		JsonParser Parser = new JsonParser();
		DataManager.Instance.m_TmpFbData = Parser.ParserData< FaceBookData >( result.Text );
		//DataManager.Instance.m_TmpFbData = Parser.ParserData< FaceBookData >( result.Text );

		FB.API("me/friends?fields=installed,name,id,picture", Facebook.HttpMethod.GET, APICallback); 

	}

	void APICallback(FBResult result)                                                                                              
	{                                                                                                                              
		Debug.Log("APICallback result = " + result.Text);                                                                                                
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			Debug.LogError(result.Error);                                                                                           
			// Let's just try again                                                                                                
			//FB.API("/me?fields=id,first_name,picture,friends.limit(100).fields(first_name,id,picture)", Facebook.HttpMethod.GET, APICallback); 
			m_loginCb(""); 
			PopupManager.Instance.VisibleLoading(false);
			return;                                                                                                                
		}


		JsonParser Parser = new JsonParser();
		DataManager.Instance.m_TmpFbFriendData = Parser.ParserData< FaceBookFriendData >( result.Text );

		AddFbData();

		if( m_loginCb != null )
			m_loginCb(FB.UserId); 

		EventManager.Instance.onEvent( EventManager.EventName.FBLoginOK );

	}                

	// add FB data to NetWorkManager
	void AddFbData()
	{
		NetWorkManager.Instance.myFriendData.Clear();

		List< FaceBookData> TmpList =  DataManager.Instance.m_TmpFbFriendData.data;

		for( int i = 0 ; i < TmpList.Count ; i ++ )
		{
			PersonalData TmpData = new PersonalData(TmpList[i].id);
			TmpData.PD_FB_Name = TmpList[i].name;
			TmpData.PD_FB_Image = TmpList[i].picture.data.url;
			TmpData.PD_IsPlayThisGame = TmpList[i].installed;
			NetWorkManager.Instance.addFBFriendData( TmpData );
			if( TmpData.PD_IsPlayThisGame )
				NetWorkManager.Instance.parse_getFriendScoreInfo(TmpData.PD_FB_Uid);
		}
		NetWorkManager.Instance.myData = new MyPersonalData(DataManager.Instance.m_TmpFbData.id, DataManager.Instance.m_TmpFbData.name, DataManager.Instance.m_TmpFbData.picture.data.url);
		NetWorkManager.Instance.fbIsLogin = true;

		NetWorkManager.Instance.parse_StartCheckData();
	}

	// logout FB
	public void LogoutFB()
	{
		CPDebug.Log("Logout FB");
		NetWorkManager.Instance.fbIsLogin = false;
		NetWorkManager.Instance.clearAllData();
		FB.Logout();
		EventManager.Instance.onEvent( EventManager.EventName.FBLogoutOK );
	}

	public string GetFBId()
	{
		string TmpFBUid = LocalDBManager.Instance.GetFBUid();
		return TmpFBUid;
	}

	public void SetFbImg( UITexture p_tex , string p_url )
	{
		StartCoroutine( loadFbImg(p_tex,p_url));
	}

	IEnumerator loadFbImg( UITexture p_tex , string p_url )
	{
		WWW obj = new WWW(p_url);
		yield return obj;
		if( p_tex != null )
			p_tex.mainTexture = obj.texture;
	}
	void Update()
	{
		if( Input.GetKeyDown(KeyCode.B ) )
		{
			LifeManager.Instance.useLife();
		}
	}

	public void postTextOnMyFB(string content)
	{
		if (FB.IsLoggedIn)                                                                     
		{
			var query = new Dictionary<string, string>();
			query["message"] = content;
			query["caption"] = "Fruit Kitchen";
			query["link"] = "https://www.facebook.com/fruitkitchenpuzzlegame";
			query["picture"] = "http://www.sourcekode.com.tw/sk2/save/icon_152.png";
			FB.API("/me/feed", Facebook.HttpMethod.POST, delegate(FBResult r) {

				if (r.Error == null)                                                                                                  
				{ 
					string msg = Localization.Localize("81");
					string ok = Localization.Localize("46");
					PopupManager.Instance.ShowMessageBox_Tip(msg, ok);
				}

			}, query);
		}
	}

}
