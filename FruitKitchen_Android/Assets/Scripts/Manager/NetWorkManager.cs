using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Parse;
using System.Threading.Tasks;

public class UnlockFormatData
{
	public int unlockType = 0;
	public int unlockParam = 0;
	public List< string > unlockFriendArray;

	public UnlockFormatData(int Type, int Param)
	{
		unlockType = Type;
		unlockParam = Param;
		unlockFriendArray = new List< string >();
	}

	public void addHelpFriend(string FBId)
	{
		for(int i = 0; i < unlockFriendArray.Count; i++)
		{
			string fbId = unlockFriendArray[i];

			if(fbId == FBId)
			{
				return;
			}
		}

		unlockFriendArray.Add (FBId);
	}
}

public class FriendUnlockData
{
	public int unlockType = 0;
	public int unlockParam = 0;
	public string unlockFriends;

	public FriendUnlockData(string String)
	{
		string [] TmpStr = String.Split(',');

		if(TmpStr.Length == 4)
		{
			unlockFriends = TmpStr[1];
			unlockType = int.Parse(TmpStr[2]);
			unlockParam = int.Parse(TmpStr[3]);
		}
		else
		{
			unlockType = -1;
			unlockParam = -1;
			unlockFriends = null;
		}
	}
}

public class DailyGiftData
{
	public string dailyGiftlimmitTime;
	public string dailyGiftTargetFBUid;
	public string dailyGiftOringeStr;

	public DailyGiftData(string GiftlimmitTime, string GiftTargetFBUid)
	{
		dailyGiftlimmitTime = GiftlimmitTime;
		dailyGiftTargetFBUid = GiftTargetFBUid;
	}
}

public class MailData
{
	public int mailType = -1;
	public string mailComFrom;
	public string mailOringeStr;
	public string mailParam1,mailParam2,mailParam3,mailParam4,mailParam5,mailParam6,mailParam7;
	
	public MailData(int MailType, string MailComFrom)
	{
		mailType = MailType;
		mailComFrom = MailComFrom;
	}
}

public class ChallengData
{
	public int challengBigStage = -1;
	public int challengSmallStage = -1;
	public int challengScore = -1;
	public int challengScoreType = -1;
	public string challengLimmitTime;
	public string challengFBUid;
	public string parse_oringe_string;

	public ChallengData(string LimmitTime, string ChallengFBUid, int BigStage, int SmallStage, int ScoreType, int Score)
	{
		challengLimmitTime = LimmitTime;
		challengFBUid = ChallengFBUid;
		challengBigStage = BigStage;
		challengSmallStage = SmallStage;
		challengScoreType = ScoreType;
		challengScore = Score;
	}

	public string getCombineString()
	{
		string combineStr = string.Format("{0},{1},{2},{3},{4},{5}" , challengLimmitTime, challengFBUid, challengBigStage, challengSmallStage, challengScoreType, challengScore);
		return combineStr;
	}

	public void setParse_oringe_string(string String)
	{
		parse_oringe_string = String;
	}
}

public class PersonalData
{
	public string PD_FB_Uid;
	public string PD_FB_Name;
	public string PD_FB_Image;
	public bool PD_IsPlayThisGame;
	public ParseObject PD_ParseScoreData = null;

	public PersonalData(string FBId)
	{
		PD_FB_Uid = FBId;
		PD_IsPlayThisGame = false;
	}

	public PersonalData(string FBId, string FBName, string FBImage)
	{
		PD_FB_Uid = FBId;
		PD_FB_Name = FBName;
		PD_FB_Image = FBImage;

		PD_IsPlayThisGame = false;
	}

	public int getStageStar(int BigStage, int SmallStage)
	{
		if(PD_ParseScoreData == null)
		{
			return -1;
		}

		string sStageName = string.Format ("Stage_{0}_{1}", BigStage, SmallStage);
		string sScore = PD_ParseScoreData [sStageName].ToString();

		if(sScore.Length <= 0)
		{
			return -1;
		}

		string [] TmpStr = sScore.Split(';');

		if(TmpStr.Length != 3)
		{
			return -1;
		}

		return int.Parse (TmpStr[0]);
	}

	public int getStageBestMoveScore(int BigStage, int SmallStage)
	{
		if(PD_ParseScoreData == null)
		{
			return -1;
		}
		
		string sStageName = string.Format ("Stage{0}_{1}", BigStage, SmallStage);

		if(!PD_ParseScoreData.ContainsKey(sStageName))
		{
			return -1;
		}

		string sScore = PD_ParseScoreData [sStageName].ToString();
		
		if(sScore.Length <= 0)
		{
			return -1;
		}
		
		string [] TmpStr = sScore.Split(';');
		
		if(TmpStr.Length != 3)
		{
			return -1;
		}
		
		return int.Parse (TmpStr[1]);
	}

	public int getStageBestBombScore(int BigStage, int SmallStage)
	{
		if(PD_ParseScoreData == null)
		{
			return -1;
		}
		
		string sStageName = string.Format ("Stage{0}_{1}", BigStage, SmallStage);

		if(!PD_ParseScoreData.ContainsKey(sStageName))
		{
			return -1;
		}

		string sScore = PD_ParseScoreData [sStageName].ToString();
		
		if(sScore.Length <= 0)
		{
			return -1;
		}
		
		string [] TmpStr = sScore.Split(';');
		
		if(TmpStr.Length != 3)
		{
			return -1;
		}
		
		return int.Parse (TmpStr[2]);
	}
}

public class MyPersonalData
{
	public string PD_FB_Uid;
	public string PD_FB_Name;
	public string PD_FB_Image;
	public bool PD_IsPlayThisGame;
	public ParseObject PD_ParseScoreData = null;
	
	public MyPersonalData(string FBId)
	{
		PD_FB_Uid = FBId;
		PD_IsPlayThisGame = false;
	}
	
	public MyPersonalData(string FBId, string FBName, string FBImage)
	{
		PD_FB_Uid = FBId;
		PD_FB_Name = FBName;
		PD_FB_Image = FBImage;
		
		PD_IsPlayThisGame = false;
	}

	public int getStageStar(int BigStage, int SmallStage)
	{
		return LocalDBManager.Instance.getStageStarNum (BigStage, SmallStage);
	}

	public int getStageBestMoveScore(int BigStage, int SmallStage)
	{
		return LocalDBManager.Instance.getBestMoveScore (BigStage, SmallStage);
	}

	public int getStageBestBombScore(int BigStage, int SmallStage)
	{
		return LocalDBManager.Instance.getBestBombScore (BigStage, SmallStage);
	}
}

public class NetWorkManager : ManagerComponent< NetWorkManager > 
{
	public int challengScoreType = -1;

	public bool internetConnect = true;
	public bool fbIsLogin = false;
	public bool isPlayChallengMode = false;
	public bool isBeChalleng = false;
	public bool firstDataReceive = false;
	public bool gameScoreFinish = false;
	public bool gameRecordFinish = false;

	public string challengFriendFBUid = null;
	public string realVersion = null;

	public List< PersonalData > myFriendData = new List< PersonalData >();
	public List< ChallengData > myChallengData = new List< ChallengData >();
	public List< ChallengData > otherChallengMeData = new List< ChallengData >();
	public List< MailData > mailData = new List< MailData >();
	public List< DailyGiftData > dailyGiftData = new List< DailyGiftData >();
	public List< string > unSendInviteArray = new List< string >();
	public List< FriendUnlockData > unlockInfoArray = new List< FriendUnlockData >();
	public List< UnlockFormatData > unlockFormatInfoArray = new List< UnlockFormatData >();

	public MyPersonalData myData;

	public void initNetWorkManager()
	{
		gameRecordFinish = false;
		gameScoreFinish = false;

		CPDebug.Log (" initNetWorkManager 111111111111");
		StartCoroutine(UpdateReceiveParseInfo());
		StartCoroutine(UpdateReceiveNowVersion());

		bool isDisconnect = (Application.internetReachability == NetworkReachability.NotReachable );
	}

	IEnumerator UpdateReceiveParseInfo()
	{
		while(true)
		{
			yield return new WaitForSeconds(Config.SYNCHRONOUS_GAME_RECORD_TIME);
			parse_synchronousGameRecordDataFromParse();
		}
	}

	IEnumerator UpdateReceiveNowVersion()
	{
		while(true)
		{
			yield return new WaitForSeconds(Config.SYNCHRONOUS_GAME_RECORD_TIME);
			parse_GetNowVersion();
		}
	}

	public void parse_GetNowVersion()
	{
		StartCoroutine( __parse_GetNowVersion());
	}

	IEnumerator __parse_GetNowVersion()
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ))
		{
			yield return null;
		}

		ParseQuery<ParseObject> query = ParseObject.GetQuery("GameSetting").WhereExists ("GameVersion");

		Task< ParseObject > t =  query.FirstAsync();

		while( true )
		{
			if(t.IsCompleted)
			{
				ParseObject obj = t.Result;
				realVersion = obj["GameVersion"].ToString();

				yield break;
			}
			yield return null;
		}
	}

	public void initMyData(string FBId, string FBName, string FBImage)
	{
		if(myData != null)
		{
			return;
		}

		myData = new MyPersonalData (FBId, FBName, FBImage);
	}

	public void addFBFriendData(PersonalData Friend)
	{
		if(Friend == null)
		{
			return;
		}

		for(int i = 0; i < myFriendData.Count; i++)
		{
			PersonalData data = myFriendData[i];

			if(data == null)
			{
				continue;
			}

			if(data.PD_FB_Uid == Friend.PD_FB_Uid)
			{
				myFriendData[i] = Friend;
				return;
			}
		}

		myFriendData.Add (Friend);
	}

	public PersonalData getFriendData(string FBUid)
	{
		for(int i = 0; i < myFriendData.Count; i++)
		{
			PersonalData data = myFriendData[i];
			
			if(data == null)
			{
				continue;
			}
			
			if(data.PD_FB_Uid == FBUid)
			{
				return data;
			}
		}

		return null;
	}

	public void clearAllData()
	{
		if(myData != null)
		{
			myData = null;
		}

		myFriendData.Clear ();
	}

	public void parse_StartCheckData()
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ) ||
		   !fbIsLogin ||
		   myData == null)
		{
			return;
		}

		if(firstDataReceive)
		{
			return;
		}

		firstDataReceive = true;

		string recordFacebookId = LocalDBManager.Instance.GetFBUid ();

		if(recordFacebookId.Length > 0)
		{
			if(recordFacebookId != myData.PD_FB_Uid)
			{
				LocalDBManager.Instance.clearParseTable();
				LocalDBManager.Instance.updateRecordStage(1, 4);
				LocalDBManager.Instance.clearStageScoreRecordTable();
				LocalDBManager.Instance.clearUnlockInfoTable();
				LocalDBManager.Instance.SetFBUid(myData.PD_FB_Uid);
			}
		}
		else
		{
			LocalDBManager.Instance.SetFBUid(myData.PD_FB_Uid);
		}
		parse_HandleGameScoreData ();
		parse_HandleGameRecordData ();
	}

	public void parse_HandleGameScoreData()
	{
		StartCoroutine( __parse_HandleGameScoreData());
	}

	IEnumerator __parse_HandleGameScoreData()
	{
		string parseObjectId = LocalDBManager.Instance.GetGameScoreObject ();

		if(parseObjectId.Length > 0)
		{
			//ParseQuery<ParseObject> query = ParseObject.GetQuery("GameScore").WhereContains("objectId", parseObjectId);
			ParseQuery<ParseObject> query = ParseObject.GetQuery("GameScore");
			Task< ParseObject > t = query.GetAsync(parseObjectId);
			//Task< ParseObject > t =  query.FirstAsync();

			while( true )
			{
				if(t.IsCompleted)
				{
					if(t.Exception == null)
					{
						ParseObject gameScore = t.Result;
						
						if(gameScore != null)
						{
							checkAndRecoverGameScore(gameScore);
							
							gameScoreFinish = true;
							CPDebug.Log("gameScore1 finish");
							
							if(gameRecordFinish)
							{
								// remove Black
								PopupManager.Instance.VisibleLoading(false);
								CPDebug.Log("Parse Init Finish");

								initNetWorkManager();
							}
						}
					}

					yield break;
				}
				yield return null;
			}
		}
		else
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("GameScore").WhereEqualTo("FacebookID", myData.PD_FB_Uid);

			Task< ParseObject > t =  query.FirstAsync();

			while( true )
			{
				if(t.IsCompleted)
				{
					if(t.Exception == null)
					{
						ParseObject obj = t.Result;
						
						if(obj != null)
						{
							LocalDBManager.Instance.SetGameScoreObject(obj.ObjectId);
							checkAndRecoverGameScore(obj);
							
							gameScoreFinish = true;
							CPDebug.Log("gameScore2 finish");
							
							if(gameRecordFinish)
							{
								// remove Black
								PopupManager.Instance.VisibleLoading(false);
								CPDebug.Log("Parse Init Finish");

								initNetWorkManager();
							}
						}
					}
					else
					{
						parse_InitGameScoreInfo();
					}
					
					yield break;
				}
				yield return null;
			}
		}
	}

	public void parse_HandleGameRecordData()
	{
		StartCoroutine( __parse_HandleGameRecordData());
	}

	IEnumerator __parse_HandleGameRecordData()
	{
		string parseObjectId = LocalDBManager.Instance.GetGameRecordObject ();
		
		if (parseObjectId.Length > 0)
		{
			//ParseQuery<ParseObject> query = ParseObject.GetQuery("GameRecord").WhereContains("objectId", parseObjectId);;
			ParseQuery<ParseObject> query = ParseObject.GetQuery("GameRecord");
			Task< ParseObject > t =  query.GetAsync(parseObjectId);
			//Task< ParseObject > t =  query.FirstAsync();

			while( true )
			{
				if(t.IsCompleted)
				{
					if(t.Exception == null)
					{
						ParseObject gameScore = t.Result;

						if(gameScore != null)
						{
							parse_synchronousGameRecordDataFromParse();
						}
					}

					yield break;
				}
				yield return null;
			}
		}
		else
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("GameRecord").WhereEqualTo("FacebookID", myData.PD_FB_Uid);

			Task< ParseObject > t =  query.FirstAsync();

			while( true )
			{
				if(t.IsCompleted)
				{
					if(t.Exception == null)
					{
						ParseObject obj = t.Result;
						
						if(obj != null)
						{
							LocalDBManager.Instance.SetGameRecordObject(obj.ObjectId);
							parse_synchronousGameRecordDataFromParse();
						}
					}
					else
					{
						parse_InitGameRecordInfo();
					}
					yield break;
				}
				yield return null;
			}
		}
	}

	public void parse_InitGameScoreInfo()
	{
		StartCoroutine( __parse_InitGameScoreInfo());
	}

	IEnumerator __parse_InitGameScoreInfo()
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ) ||
		   !fbIsLogin ||
		   myData == null)
		{
			yield return null;
		}

		ParseObject gameScore = new ParseObject("GameScore");
		gameScore["FacebookID"] = myData.PD_FB_Uid;
		gameScore["FacebookName"] = myData.PD_FB_Name;

		parse_SaveDBScoreDataToParse (gameScore);

		Task t =  gameScore.SaveAsync();

		while( true )
		{
			if(t.IsCompleted)
			{
				LocalDBManager.Instance.SetGameScoreObject(gameScore.ObjectId);
				
				gameScoreFinish = true;
				CPDebug.Log("gameScore3 finish");
				
				if(gameRecordFinish)
				{
					// remove Black
					PopupManager.Instance.VisibleLoading(false);
					CPDebug.Log("Parse Init Finish");

					initNetWorkManager();
				}
				
				yield break;
			}
			yield return null;
		}
	}

	public void parse_InitGameRecordInfo()
	{
		StartCoroutine( __parse_InitGameRecordInfo());
	}

	IEnumerator __parse_InitGameRecordInfo()
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ) ||
		   !fbIsLogin ||
		   myData == null)
		{
			yield return null;
		}
		
		ParseObject gameRecord = new ParseObject("GameRecord");
		gameRecord["FacebookID"] = myData.PD_FB_Uid;
		gameRecord["FacebookName"] = myData.PD_FB_Name;

		Task t =  gameRecord.SaveAsync();
		
		while( true )
		{
			if(t.IsCompleted)
			{
				LocalDBManager.Instance.SetGameRecordObject(gameRecord.ObjectId);
				
				gameRecordFinish = true;
				CPDebug.Log("gameRecord1 finish");
				
				if(gameScoreFinish)
				{
					// remove Black
					PopupManager.Instance.VisibleLoading(false);
					CPDebug.Log("Parse Init Finish");

					initNetWorkManager();
				}
				
				yield break;
			}
			yield return null;
		}
	}

	public void parse_SaveDBScoreDataToParse(ParseObject Obj)
	{
		if(Obj == null)
		{
			return;
		}

		string recordBigStage = LocalDBManager.Instance.getRecordBigStage ();
		string recordSmallStage = LocalDBManager.Instance.getRecordSmallStage ();

		Obj ["BigStageRecord"] = recordBigStage;
		Obj ["SmallStageRecord"] = recordSmallStage;

		List< StageRecordData > stageRecordData = new List< StageRecordData >();

		LocalDBManager.Instance.getAllStageScoreRecord (stageRecordData);

		if(stageRecordData.Count > 0)
		{
			for(int i = 0; i < stageRecordData.Count; i++)
			{
				StageRecordData data = stageRecordData[i];

				if(data == null)
				{
					continue;
				}

				string sColName = string.Format("Stage{0}_{1}", data.BigStage, data.SmallStage);
				string sRecordScore = string.Format("{0};{1};{2}", data.BestStar, data.BestMoveScore, data.BestBombScore);

				Obj[sColName] = sRecordScore;
			}
		}
	}

	public void parse_SaveParseScoreDataToDB(ParseObject Obj)
	{
		if(Obj == null)
		{
			return;
		}

		int recordBigStage = int.Parse(Obj["BigStageRecord"].ToString());
		int recordSmallStage = int.Parse(Obj["SmallStageRecord"].ToString());

		LocalDBManager.Instance.updateRecordStage (recordBigStage, recordSmallStage);

		List< StageRecordData > stageRecordData = new List< StageRecordData >();

		for(int i = 1; i <= 5; i++)
		{
			for(int j = 1; j <= 23; j++)
			{
				string sColName = string.Format("Stage{0}_{1}", i, j);
				string sScoreFormat = Obj[sColName].ToString();

				if(sScoreFormat.Length > 0)
				{
					string [] TmpStr = sScoreFormat.Split(';');

					if(TmpStr.Length != 3)
					{
						continue;
					}

					int bestStar = int.Parse(TmpStr[0]);
					int bestMove = int.Parse(TmpStr[1]);
					int bestBomb = int.Parse(TmpStr[2]);

					StageRecordData data = new StageRecordData(i, j, bestStar, bestMove, bestBomb);
					stageRecordData.Add(data);
				}
			}
		}

		if(stageRecordData.Count > 0)
		{
			LocalDBManager.Instance.saveAllStageScoreRecord(stageRecordData);
		}
	}

	public void parse_UpdateOneStage(int BigStage, int SmallStage, int BestStar, int BestMoveScore, int BestBombScore)
	{
		StartCoroutine( __parse_UpdateOneStage(BigStage, SmallStage, BestStar, BestMoveScore, BestBombScore));
	}

	IEnumerator __parse_UpdateOneStage(int BigStage, int SmallStage, int BestStar, int BestMoveScore, int BestBombScore)
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ) ||
		   !fbIsLogin ||
		   myData == null)
		{
			yield return null;
		}

		string parseObjectId = LocalDBManager.Instance.GetGameScoreObject ();

		if(parseObjectId.Length > 0)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("GameScore");

			Task< ParseObject > t =  query.FirstAsync();

			while( true )
			{
				if(t.IsCompleted)
				{
					ParseObject gameScore = t.Result;
					
					string sColName = string.Format("Stage{0}_{1}", BigStage, SmallStage);
					string sRecordScore = string.Format("{0};{1};{2}", BestStar, BestMoveScore, BestBombScore);
					
					gameScore[sColName] = sRecordScore;
					
					gameScore.SaveAsync();
					
					yield break;
				}
				yield return null;
			}
		}
	}

	public void parse_UpdateStage(int BigStage, int SmallStage)
	{
		StartCoroutine( __parse_UpdateStage(BigStage, SmallStage));
	}

	IEnumerator __parse_UpdateStage(int BigStage, int SmallStage)
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ) ||
		   !fbIsLogin ||
		   myData == null)
		{
			yield return null;
		}

		string parseObjectId = LocalDBManager.Instance.GetGameScoreObject ();
		
		if(parseObjectId.Length > 0)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("GameScore");

			Task< ParseObject > t =  query.FirstAsync();

			while( true )
			{
				if(t.IsCompleted)
				{
					ParseObject gameScore = t.Result;
					
					gameScore["BigStageRecord"] = BigStage.ToString();
					gameScore["SmallStageRecord"] = SmallStage.ToString();
					
					gameScore.SaveAsync();
					
					yield break;
				}
				yield return null;
			}
		}
	}

	public void parse_HandleChallengData(ParseObject Obj)
	{
		if(Obj == null)
		{
			return;
		}

		parse_AnalyzeChallengData (Obj, true);
		parse_AnalyzeChallengData (Obj, false);
		transOtherChallengDataToMailData ();
	}

	public void parse_AnalyzeChallengData(ParseObject Obj, bool IsMyChalleng)
	{
		if(Obj == null)
		{
			return;
		}

		if(IsMyChalleng)
		{
			if( Obj.ContainsKey("myChalleng") == false ) 
			{
				return;
			}

			IList<object> handleArray = Obj.Get<IList<object>>("myChalleng");
			myChallengData.Clear();

			if(handleArray.Count <= 0)
			{
				return;
			}
			
			List< string > eraseArray = new List< string >();
			
			for(int i = 0; i < handleArray.Count; i++)
			{
				string challengStr = handleArray[i].ToString();
				
				if(challengStr.Length <= 0)
				{
					continue;
				}
				
				string [] TmpStr = challengStr.Split(',');
				
				if(TmpStr.Length != 6)
				{
					eraseArray.Add(challengStr);
					continue;
				}
				
				string sLimmitTime = TmpStr[0];

				if(checkIfChallengTimeUp(sLimmitTime))
				{
					eraseArray.Add(challengStr);
					continue;
				}
				
				string sFBUid = TmpStr[1];
				int bigStage = int.Parse(TmpStr[2]);
				int smallStage = int.Parse(TmpStr[3]);
				int scoreType = int.Parse(TmpStr[4]);
				int score = int.Parse(TmpStr[5]);
				
				ChallengData data = new ChallengData(sLimmitTime, sFBUid, bigStage, smallStage, scoreType, score);
				data.setParse_oringe_string(challengStr);

				myChallengData.Add(data);
			}
			
			if(eraseArray.Count > 0)
			{
				for(int i = 0; i < eraseArray.Count; i++)
				{
					string challengStr = eraseArray[i];
					
					if(challengStr.Length <= 0)
					{
						continue;
					}
					
					removeStringFromList(handleArray, challengStr);
				}
				
				if(IsMyChalleng)
				{
					Obj["myChalleng"] = handleArray;
				}
				else
				{
					Obj["otherChalleng"] = handleArray;
				}
				
				Obj.SaveAsync ();
			}
		}
		else
		{
			if( Obj.ContainsKey( "otherChalleng" ) == false )
			{
				return;
			}

			IList<object> handleArray = Obj.Get<List<object>>("otherChalleng");
			otherChallengMeData.Clear();

			if(handleArray.Count <= 0)
			{
				return;
			}
			
			List< string > eraseArray = new List< string >();
			
			for(int i = 0; i < handleArray.Count; i++)
			{
				string challengStr = handleArray[i].ToString();
				
				if(challengStr.Length <= 0)
				{
					continue;
				}
				
				string [] TmpStr = challengStr.Split(',');
				
				if(TmpStr.Length != 6)
				{
					eraseArray.Add(challengStr);
					continue;
				}
				
				string sLimmitTime = TmpStr[0];
				
				if(checkIfChallengTimeUp(sLimmitTime))
				{
					eraseArray.Add(challengStr);
					continue;
				}
				
				string sFBUid = TmpStr[1];
				int bigStage = int.Parse(TmpStr[2]);
				int smallStage = int.Parse(TmpStr[3]);
				int scoreType = int.Parse(TmpStr[4]);
				int score = int.Parse(TmpStr[5]);
				
				ChallengData data = new ChallengData(sLimmitTime, sFBUid, bigStage, smallStage, scoreType, score);
				data.setParse_oringe_string(challengStr);

				otherChallengMeData.Add(data);
			}
			
			if(eraseArray.Count > 0)
			{
				for(int i = 0; i < eraseArray.Count; i++)
				{
					string challengStr = eraseArray[i];
					
					if(challengStr.Length <= 0)
					{
						continue;
					}
					
					removeStringFromList(handleArray, challengStr);
				}
				
				if(IsMyChalleng)
				{
					Obj["myChalleng"] = handleArray;
				}
				else
				{
					Obj["otherChalleng"] = handleArray;
				}
				
				Obj.SaveAsync ();
			}
		}
	}

	public void removeStringFromList(IList<object> Array, string String)
	{
		if(Array.Count <= 0 ||
		   String.Length <= 0)
		{
			return;
		}

		for(int i = 0; i < Array.Count; i++)
		{
			string arrayString = Array[i].ToString();

			if(arrayString.Length <= 0)
			{
				continue;
			}

			if(arrayString == String)
			{
				Array.RemoveAt(i);
				break;
			}
		}
	}

	public void parse_HandleMailData(ParseObject Obj)
	{
		if(Obj == null)
		{
			return;
		}

		mailData.Clear ();

		transOtherChallengDataToMailData ();

		if( Obj.ContainsKey("mailBox") == false )
		{
			return;
		}

		IList<object> handleArray = Obj.Get<List<object>>("mailBox");

		if(handleArray.Count <= 0)
		{
			return;
		}

		List< string > eraseArray = new List< string >();

		for(int i = 0; i < handleArray.Count; i++)
		{
			string mailStr = handleArray[i].ToString();
			
			if(mailStr.Length <= 0)
			{
				continue;
			}

			string [] TmpStr = mailStr.Split(',');

			if(TmpStr.Length != 10)
			{
				eraseArray.Add(mailStr);
				continue;
			}

			string sMailComeFrom = TmpStr[1];
			int mailType = int.Parse(TmpStr[2]);
			string sMailParam1 = TmpStr[3];
			string sMailParam2 = TmpStr[4];
			string sMailParam3 = TmpStr[5];
			string sMailParam4 = TmpStr[6];
			string sMailParam5 = TmpStr[7];
			string sMailParam6 = TmpStr[8];
			string sMailParam7 = TmpStr[9];

			MailData mail = new MailData(mailType, sMailComeFrom);
			mail.mailParam1 = sMailParam1;
			mail.mailParam2 = sMailParam2;
			mail.mailParam3 = sMailParam3;
			mail.mailParam4 = sMailParam4;
			mail.mailParam5 = sMailParam5;
			mail.mailParam6 = sMailParam6;
			mail.mailParam7 = sMailParam7;
			mail.mailOringeStr = mailStr;

			mailData.Add(mail);
		}

		if(eraseArray.Count > 0)
		{
			for(int i = 0; i < eraseArray.Count; i++)
			{
				string mailStr = eraseArray[i];
				
				if(mailStr.Length <= 0)
				{
					continue;
				}
				
				removeStringFromList(handleArray, mailStr);
			}

			Obj["mailBox"] = handleArray;
			Obj.SaveAsync ();
		}
	}

	public void parse_HandleDailyGift(ParseObject Obj)
	{
		if(Obj == null)
		{
			return;
		}
		
		dailyGiftData.Clear ();
		if( Obj.ContainsKey("dailyGift") == false )
		{
			return;
		}

		IList<object> handleArray = Obj.Get<List<object>>("dailyGift");
		
		if(handleArray.Count <= 0)
		{
			return;
		}
		
		List< string > eraseArray = new List< string >();
		
		for(int i = 0; i < handleArray.Count; i++)
		{
			string dailyGiftStr = handleArray[i].ToString();
			
			if(dailyGiftStr.Length <= 0)
			{
				continue;
			}
			
			string [] TmpStr = dailyGiftStr.Split(',');
			
			if(TmpStr.Length != 2)
			{
				eraseArray.Add(dailyGiftStr);
				continue;
			}

			string sLimmitTime = TmpStr[0];

			if(checkIfChallengTimeUp(sLimmitTime))
			{
				eraseArray.Add(dailyGiftStr);
				continue;
			}
			
			string sFBUid = TmpStr[1];
			
			DailyGiftData data = new DailyGiftData(sLimmitTime, sFBUid);
			data.dailyGiftOringeStr = dailyGiftStr;

			dailyGiftData.Add(data);
		}
		
		if(eraseArray.Count > 0)
		{
			for(int i = 0; i < eraseArray.Count; i++)
			{
				string dailyGiftStr = eraseArray[i];
				
				if(dailyGiftStr.Length <= 0)
				{
					continue;
				}
				
				removeStringFromList(handleArray, dailyGiftStr);
			}
			
			Obj["dailyGift"] = handleArray;
			Obj.SaveAsync ();
		}
	}

	public void parse_HandleMyInviteHelp(ParseObject Obj)
	{
		if(Obj == null)
		{
			return;
		}

		unlockInfoArray.Clear ();
		if( Obj.ContainsKey("unlockInfo") == false )
		{
			return;
		}
		IList<object> handleArray = Obj.Get<List<object>>("unlockInfo");
		
		if(handleArray.Count <= 0)
		{
			return;
		}

		for(int i = 0; i < handleArray.Count; i++)
		{
			string unlockStr = handleArray[i].ToString();
			
			if(unlockStr.Length <= 0)
			{
				continue;
			}
			
			string [] TmpStr = unlockStr.Split(',');
			
			if(TmpStr.Length != 4)
			{
				continue;
			}
			
			FriendUnlockData unlockData = new FriendUnlockData(unlockStr);

			unlockInfoArray.Add(unlockData);
		}

		transUnlockInfoToDB ();
	}

	public void parse_SendChalleng(string TargetFriend, int BigStage, int SmallStage, int ScoreType, int Score)
	{
		if(myChallengData.Count >= Config.CHALLENG_MAX_NUM)
		{
			return;
		}

		if(TargetFriend.Length <= 0)
		{
			return;
		}

		StartCoroutine (__parse_SendChalleng(TargetFriend, BigStage, SmallStage, ScoreType, Score));
	}

	IEnumerator __parse_SendChalleng(string TargetFriend, int BigStage, int SmallStage, int ScoreType, int Score)
	{
		string sLimmitStr = getTimeIntervalSinceNow (Config.CHALLENG_LIMMIT_TIME);

		string sChallengMyStr = string.Format ("{0},{1},{2},{3},{4},{5}", sLimmitStr, TargetFriend, BigStage, SmallStage, ScoreType, Score);
		string sChallengOtherStr = string.Format ("{0},{1},{2},{3},{4},{5}", sLimmitStr, myData.PD_FB_Uid, BigStage, SmallStage, ScoreType, Score);

		string parseObjectId = LocalDBManager.Instance.GetGameRecordObject ();
		
		if(parseObjectId.Length > 0)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("GameRecord");
			Task< ParseObject > t = query.GetAsync(parseObjectId);

			while( true )
			{
				if(t.IsCompleted)
				{
					if(t.Exception == null)
					{
						ParseObject gameRecord = t.Result;
						
						if(gameRecord != null)
						{
							gameRecord.AddUniqueToList("myChalleng", sChallengMyStr);

							Task t2 = gameRecord.SaveAsync();

							while( true )
							{
								if(t2.IsCompleted)
								{
									if(t2.Exception == null)
									{
										ChallengData data = new ChallengData(sLimmitStr, TargetFriend, BigStage, SmallStage, ScoreType, Score);
										data.setParse_oringe_string(sChallengMyStr);
										
										myChallengData.Add(data);

										ParseQuery<ParseObject> query2 = ParseObject.GetQuery("GameRecord").WhereEqualTo("FacebookID", TargetFriend);
										query2.FirstAsync().ContinueWith(t3 =>
										                                 {
											if(t3.IsCompleted)
											{
												ParseObject obj = t3.Result;
												
												if(obj != null)
												{
													obj.AddUniqueToList("otherChalleng", sChallengOtherStr);

													obj.SaveAsync();
												}
											}
										});
									}
									
									yield break;
								}
								yield return null;
							}
						}
					}
					
					yield break;
				}
				yield return null;
			}
		}
	}

	public void parse_checkChalleng()
	{
		if(myChallengData.Count == 0)
		{
			return;
		}

		List< string > eraseArray = new List< string >();

		for(int i = 0; i < myChallengData.Count; i++)
		{
			ChallengData data = myChallengData[i];

			if(data == null)
			{
				continue;
			}

			string limmitTime = data.challengLimmitTime;

			if(checkIfChallengTimeUp(limmitTime))
			{
				string combineString = data.getCombineString();
				eraseArray.Add(combineString);

				myChallengData.RemoveAt(i);
				continue;
			}
		}

		if(eraseArray.Count > 0)
		{
			string parseObjectId = LocalDBManager.Instance.GetGameRecordObject();

			if(parseObjectId.Length > 0)
			{
				ParseQuery<ParseObject> query = ParseObject.GetQuery("GameRecord");
				query.GetAsync(parseObjectId).ContinueWith(t =>
				                                           {
					if(t.IsCompleted)
					{
						ParseObject gameRecord = t.Result;
						
						if(gameRecord != null)
						{
							if( gameRecord.ContainsKey("myChalleng") == false )
							{
								return;
							}

							IList<object> handleArray = gameRecord.Get<List<object>>("myChalleng");

							if(handleArray.Count > 0)
							{
								for(int i = 0; i < eraseArray.Count; i++)
								{
									string challengStr = eraseArray[i];
									
									if(challengStr.Length <= 0)
									{
										continue;
									}
									
									removeStringFromList(handleArray, challengStr);
								}

								gameRecord["myChalleng"] = handleArray;
								gameRecord.SaveAsync();
							}
						}
					}
				});
			}
		}
	}

	public bool parse_checkCanChalleng()
	{
		if(myChallengData.Count >= Config.CHALLENG_MAX_NUM)
		{
			return false;
		}

		return true;
	}

	public bool checkCanChallengToSomeOne(string TargetFBUid)
	{
		if(myChallengData.Count <= 0)
		{
			return true;
		}

		for(int i = 0; i < myChallengData.Count; i++)
		{
			ChallengData data = myChallengData[i];

			if(data == null)
			{
				continue;
			}

			if(data.challengFBUid == TargetFBUid)
			{
				return false;
			}
		}

		return true;
	}

	public bool checkHasChalleng(string TargetFriend)
	{
		if(TargetFriend.Length <= 0)
		{
			return false;
		}

		for(int i = 0; i < myChallengData.Count; i++)
		{
			ChallengData data = myChallengData[i];

			if(data == null)
			{
				continue;
			}

			if(data.challengFBUid == TargetFriend)
			{
				return true;
			}
		}

		return false;
	}

	public int getChallengLeftNum()
	{
		int leftNum = Config.CHALLENG_MAX_NUM - myChallengData.Count;
		
		if(leftNum < 0)
		{
			leftNum = 0;
		}
		
		return leftNum;
	}

	public string getChallengUntilTime(string FBUid)
	{
		for(int i = 0; i < myChallengData.Count; i++)
		{
			ChallengData data = myChallengData[i];

			if(data == null)
			{
				continue;
			}

			if(data.challengFBUid == FBUid)
			{

				string sLimmitTime = data.challengLimmitTime;

				DateTime fromDate = Convert.ToDateTime(sLimmitTime);
				DateTime nowDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				
				TimeSpan ts = fromDate - nowDate;
				int seconds = (int)ts.TotalSeconds;

				if(seconds <= 0)
				{
					return "";
				}

				if(seconds >= 24 * 60 * 60)
				{
					int realDay = seconds / (24 * 60 * 60);
					return string.Format("{0}{1}", realDay, Localization.Localize("53"));
				}
				else if(seconds >= 60 * 60)
				{
					int realHour = seconds / (60 * 60);
					return string.Format("{0}{1}", realHour, Localization.Localize("54"));
				}
				else
				{
					int realMin = seconds / 60;
					return string.Format("{0}{1}", realMin, Localization.Localize("55"));
				}
			}
		}

		return "";
	}

	public string getLimmitUnitTime(string FutureTime)
	{
		string sLimmitTime = FutureTime;
		
		DateTime fromDate = Convert.ToDateTime(sLimmitTime);
		DateTime nowDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
		
		TimeSpan ts = fromDate - nowDate;
		int seconds = (int)ts.TotalSeconds;
		
		if(seconds <= 0)
		{
			return null;
		}
		
		if(seconds >= 24 * 60 * 60)
		{
			int realDay = seconds / (24 * 60 * 60);
			return string.Format("{0}{1}", realDay, Localization.Localize("53"));
		}
		else if(seconds >= 60 * 60)
		{
			int realHour = seconds / (60 * 60);
			return string.Format("{0}{1}", realHour, Localization.Localize("54"));
		}
		else
		{
			int realMin = seconds / 60;
			return string.Format("{0}{1}", realMin, Localization.Localize("55"));
		}
	}

	public void parse_sendDailyGift(string ToFBUid)
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ) ||
		   !fbIsLogin ||
		   myData == null)
		{
			return;
		}

		StartCoroutine (__parse_sendDailyGift (ToFBUid));
	}

	IEnumerator __parse_sendDailyGift(string ToFBUid)
	{
		string myFBUid = myData.PD_FB_Uid;
		string parseObjectId = LocalDBManager.Instance.GetGameRecordObject ();
		
		if(parseObjectId.Length > 0)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("GameRecord");
			Task< ParseObject > t =  query.GetAsync(parseObjectId);
			
			while( true )
			{
				if(t.IsCompleted)
				{
					if(t.Exception == null)
					{
						ParseObject gameRecord = t.Result;
						
						if(gameRecord != null)
						{
							string limmitDate = getTimeIntervalSinceNow(Config.DAILY_GIFT_RESET_TIME);
							string dailyGiftStr = string.Format("{0},{1}", limmitDate, ToFBUid);

							gameRecord.AddUniqueToList("dailyGift", dailyGiftStr);
							Task t2 = gameRecord.SaveAsync();

							while( true )
							{
								if(t2.IsCompleted)
								{
									if(t2.Exception == null)
									{
										if(getDailyGiftDataByFBUid(ToFBUid) == null)
										{
											DailyGiftData data = new DailyGiftData(limmitDate, ToFBUid);
											data.dailyGiftOringeStr = dailyGiftStr;
											
											dailyGiftData.Add(data);
											parse_sendMailToParse(myFBUid, ToFBUid, Config.MESSAGE_TYPE_GIFT, Config.DAILY_GIFT_ITEM_ID.ToString(), Config.DAILY_GIFT_ITEM_NUM.ToString(), "", "", "", "", "");
										}
									}

									yield break;
								}

								yield return null;
							}				
						}
					}
					
					yield break;
				}
				yield return null;
			}
		}
	}

	public void parse_startGainMailItemProcess(string MailOringeStr)
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ) ||
		   !fbIsLogin ||
		   myData == null)
		{
			return;
		}

		StartCoroutine (__parse_startGainMailItemProcess(MailOringeStr));
	}

	IEnumerator __parse_startGainMailItemProcess(string MailOringeStr)
	{
		string parseObjectId = LocalDBManager.Instance.GetGameRecordObject();
		
		if(parseObjectId.Length > 0)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("GameRecord");

			Task< ParseObject > t =  query.GetAsync(parseObjectId);
			//Task< ParseObject > t =  query.FirstAsync();
			
			while( true )
			{
				if(t.IsCompleted)
				{
					if(t.Exception == null)
					{
						ParseObject gameRecord = t.Result;
						
						if(gameRecord != null)
						{
							if( gameRecord.ContainsKey("mailBox") == false )
							{
								yield break;	
							}

							MailData data = getMailDataByOringeStr (MailOringeStr);
							
							if(data == null)
							{
								yield break;
							}

							IList<object> handleArray = gameRecord.Get<List<object>>("mailBox");
							removeStringFromList(handleArray, MailOringeStr);
							
							gameRecord["mailBox"] = handleArray;

							Task t2 =  gameRecord.SaveAsync();

							while( true )
							{
								if(t2.IsCompleted)
								{
									if(t2.Exception == null)
									{
										int itemId = 0;
										int itemNum = 0;
										
										if(data.mailType == Config.MESSAGE_TYPE_CHALLENG_GIFT)
										{
											itemId = int.Parse(data.mailParam4);
											itemNum = int.Parse(data.mailParam5);
										}
										else if(data.mailType == Config.MESSAGE_TYPE_GIFT)
										{
											itemId = int.Parse(data.mailParam1);
											itemNum = int.Parse(data.mailParam2);
										}
										
										removeMailData(MailOringeStr);
										
										if(itemId != 0 &&
										   itemNum != 0)
										{
											LocalDBManager.Instance.addOneOwnItem(itemId, itemNum);
										}
									}

									yield break;
								}

								yield return null;
							}
						}
					}
					
					yield break;
				}
				yield return null;
			}
		}
	}

	public void parse_sendMailToParse(string FromFBUid, string ToFBUid, int MailType, string Param1, string Param2, string Param3, string Param4, string Param5, string Param6, string Param7)
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ) ||
		   !fbIsLogin ||
		   myData == null)
		{
			return;
		}

		if(ToFBUid == null)
		{
			return;
		}

		StartCoroutine (__parse_sendMailToParse (FromFBUid, ToFBUid, MailType, Param1, Param2, Param3, Param4, Param5, Param6, Param7));
	}

	IEnumerator __parse_sendMailToParse(string FromFBUid, string ToFBUid, int MailType, string Param1, string Param2, string Param3, string Param4, string Param5, string Param6, string Param7)
	{
		ParseQuery<ParseObject> query = ParseObject.GetQuery("GameRecord").WhereEqualTo("FacebookID", ToFBUid);
		Task< ParseObject > t =  query.FirstAsync();
		
		while( true )
		{
			if(t.IsCompleted)
			{
				if(t.Exception == null)
				{
					ParseObject obj = t.Result;

					string nowTime = DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss");
					string mailStr = string.Format("{0},{1},{2}", nowTime, FromFBUid, MailType);

					if(Param1 != null)
					{
						mailStr = string.Format("{0},{1}", mailStr, Param1);
					}
					else
					{
						mailStr = string.Format("{0},{1}", mailStr, -1);
					}

					if(Param2 != null)
					{
						mailStr = string.Format("{0},{1}", mailStr, Param2);
					}
					else
					{
						mailStr = string.Format("{0},{1}", mailStr, -1);
					}

					if(Param3 != null)
					{
						mailStr = string.Format("{0},{1}", mailStr, Param3);
					}
					else
					{
						mailStr = string.Format("{0},{1}", mailStr, -1);
					}

					if(Param4 != null)
					{
						mailStr = string.Format("{0},{1}", mailStr, Param4);
					}
					else
					{
						mailStr = string.Format("{0},{1}", mailStr, -1);
					}

					if(Param5 != null)
					{
						mailStr = string.Format("{0},{1}", mailStr, Param5);
					}
					else
					{
						mailStr = string.Format("{0},{1}", mailStr, -1);
					}

					if(Param6 != null)
					{
						mailStr = string.Format("{0},{1}", mailStr, Param6);
					}
					else
					{
						mailStr = string.Format("{0},{1}", mailStr, -1);
					}

					if(Param7 != null)
					{
						mailStr = string.Format("{0},{1}", mailStr, Param7);
					}
					else
					{
						mailStr = string.Format("{0},{1}", mailStr, -1);
					}

					obj.AddToList("mailBox", mailStr);
					obj.SaveAsync();
				}
				yield break;
			}

			yield return null;
		}
	}

	public void startUnlockHelp(string MailOringeStr, string ToFBId, int UnlockType, int UnlockParam)
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ) ||
		   !fbIsLogin ||
		   myData == null)
		{
			return;
		}

		if(MailOringeStr == null ||
		   ToFBId == null)
		{
			return;
		}

		StartCoroutine (__startUnlockHelp(MailOringeStr, ToFBId, UnlockType, UnlockParam));
	}

	IEnumerator __startUnlockHelp(string MailOringeStr, string ToFBId, int UnlockType, int UnlockParam)
	{
		string parseObjectId = LocalDBManager.Instance.GetGameRecordObject();
		
		if(parseObjectId.Length > 0)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("GameRecord");
			
			Task< ParseObject > t =  query.GetAsync(parseObjectId);
			//Task< ParseObject > t =  query.FirstAsync();
			
			while( true )
			{
				if(t.IsCompleted)
				{
					if(t.Exception == null)
					{
						ParseObject gameRecord = t.Result;
						
						if(gameRecord != null)
						{
							if( gameRecord.ContainsKey("mailBox") == false )
							{
								yield break;	
							}
							
							MailData data = getMailDataByOringeStr (MailOringeStr);
							
							if(data == null)
							{
								yield break;
							}
							
							IList<object> handleArray = gameRecord.Get<List<object>>("mailBox");
							removeStringFromList(handleArray, MailOringeStr);
							
							gameRecord["mailBox"] = handleArray;
							
							Task t2 =  gameRecord.SaveAsync();
							
							while( true )
							{
								if(t2.IsCompleted)
								{
									if(t2.Exception == null)
									{
										removeMailData(MailOringeStr);
										parse_sendHelpUnlock(ToFBId, UnlockType, UnlockParam);
									}
									
									yield break;
								}
								
								yield return null;
							}
						}
					}
					
					yield break;
				}
				yield return null;
			}
		}
	}

	public void parse_sendHelpUnlock(string ToFBId, int UnlockType, int UnlockParam)
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ) ||
		   !fbIsLogin ||
		   myData == null)
		{
			return;
		}

		StartCoroutine (__parse_sendHelpUnlock(ToFBId, UnlockType, UnlockParam));
	}

	IEnumerator __parse_sendHelpUnlock(string ToFBId, int UnlockType, int UnlockParam)
	{
		ParseQuery<ParseObject> query = ParseObject.GetQuery("GameScore").WhereEqualTo("FacebookID", ToFBId);
		
		Task< ParseObject > t =  query.FirstAsync();
		
		while( true )
		{
			if(t.IsCompleted)
			{
				if(t.Exception == null)
				{
					ParseObject obj = t.Result;
					
					if(obj != null)
					{
						string sDate = DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss");
						string helpStr = string.Format("{0},{1},{2},{3}", sDate, myData.PD_FB_Uid, UnlockType, UnlockParam);

						obj.AddToList("unlockInfo", helpStr);
						obj.SaveAsync();
					}
				}

				yield break;
			}

			yield return null;
		}
	}
	
	public MailData getMailDataByOringeStr(string OringeStr)
	{
		if(mailData.Count <= 0)
		{
			return null;
		}

		for(int i = 0; i < mailData.Count; i++)
		{
			MailData data = mailData[i];

			if(data == null)
			{
				continue;
			}

			if(data.mailOringeStr == OringeStr)
			{
				return data;
			}
		}

		return null;
	}

	public void removeMailData(string OringeStr)
	{
		for(int i = 0; i < mailData.Count; i++)
		{
			MailData data = mailData[i];
			
			if(data == null)
			{
				continue;
			}
			
			if(data.mailOringeStr == OringeStr)
			{
				mailData.RemoveAt(i);
				break;
			}
		}
	}

	public DailyGiftData getDailyGiftDataByFBUid(string FBUid)
	{
		if(dailyGiftData.Count <= 0)
		{
			return null;
		}

		for(int i = 0; i < dailyGiftData.Count; i++)
		{
			DailyGiftData data = dailyGiftData[i];

			if(data == null)
			{
				continue;
			}

			if(data.dailyGiftTargetFBUid == FBUid)
			{
				return data;
			}
		}

		return null;
	}

	public string getTimeIntervalSinceNow(int Second)
	{
		DateTime dt = DateTime.Now;
		dt = dt.AddSeconds (Second);

		return dt.ToString ("yyyy-MM-dd HH:mm:ss");
	}

	public void transUnlockInfoToDB()
	{

	}

	public bool checkIfChallengTimeUp(string LimmitTime)
	{
		if(LimmitTime.Length <= 0)
		{
			return true;
		}

		DateTime fromDate = Convert.ToDateTime(LimmitTime);
		DateTime nowDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

		TimeSpan ts = fromDate - nowDate;
		double seconds = ts.TotalSeconds;

		if(seconds < 0)
		{
			return true;
		}

		return false;
	}

	public void transOtherChallengDataToMailData()
	{
		if(otherChallengMeData.Count <= 0)
		{
			removeMailDataByType(Config.MESSAGE_TYPE_CHALLENG_FROM_OTHER);
			return;
		}

		removeMailDataByType(Config.MESSAGE_TYPE_CHALLENG_FROM_OTHER);

		for(int i = 0; i < otherChallengMeData.Count; i++)
		{
			ChallengData challengData = otherChallengMeData[i];

			if(challengData == null)
			{
				continue;
			}

			string sChallengComeFrom = challengData.challengFBUid;

			MailData mail = new MailData(Config.MESSAGE_TYPE_CHALLENG_FROM_OTHER, sChallengComeFrom);
			mailData.Add(mail);
		}
	}

	public void removeMailDataByType(int Type)
	{
		for(int i = 0; i < mailData.Count; i++)
		{
			MailData mail = mailData[i];

			if(mail == null)
			{
				continue;
			}

			if(mail.mailType == Type)
			{
				mailData.RemoveAt(i);
			}
		}
	}

	public void checkAndRecoverGameScore(ParseObject Obj)
	{
		int parseBigStage = int.Parse(Obj["BigStageRecord"].ToString());
		int parseSmallStage = int.Parse(Obj["SmallStageRecord"].ToString());

		int DBBigStage = int.Parse(LocalDBManager.Instance.getRecordBigStage ());
		int DBSmallStage = int.Parse(LocalDBManager.Instance.getRecordSmallStage ());

		bool parseScoreBest = true;

		if(parseBigStage > DBBigStage)
		{
			parseScoreBest = true;
		}
		else if(DBBigStage > parseBigStage)
		{
			parseScoreBest = false;
		}
		else
		{
			if(parseSmallStage > DBSmallStage)
			{
				parseScoreBest = true;
			}
			else if(DBSmallStage >= parseSmallStage)
			{
				parseScoreBest = false;
			}
		}

		if(parseScoreBest)
		{
			LocalDBManager.Instance.updateRecordStage(parseBigStage, parseSmallStage);
			
			DataManager.Instance.BigStageNum = parseBigStage;
			DataManager.Instance.SmallStageNum = parseSmallStage;
		}
		else
		{
			Obj[@"BigStageRecord"] = DBBigStage.ToString();
			Obj[@"SmallStageRecord"] = DBSmallStage.ToString();
		}

		List< StageRecordData > dbScoreArray = new List< StageRecordData >();
		LocalDBManager.Instance.getAllStageScoreRecord (dbScoreArray);

		List< StageRecordData > scoreArray = new List< StageRecordData >();

		for(int i = 1; i <= 5; i++)
		{
			// 小關卡數
			for(int j = 1; j <= 23; j++)
			{
				int parse_StarNum = getParseScoreFromArray_StarNum(i, j , Obj);
				int DB_StarNum = getDBScoreFromArray_StarNum(i, j , dbScoreArray);

				if(parse_StarNum == -1 &&
				   DB_StarNum != -1)
				{
					int DB_MoveScore = getDBScoreFromArray_BestMoveScore(i, j , dbScoreArray);
					int DB_BombScore = getDBScoreFromArray_BestBombScore(i, j , dbScoreArray);

					string sColName = string.Format("Stage{0}_{1}", i, j);
					string sRecordScore = string.Format("{0};{1};{2}", DB_StarNum, DB_MoveScore, DB_BombScore);

					Obj[sColName] = sRecordScore;
				}
				else if(parse_StarNum != -1 &&
				        DB_StarNum == -1)
				{
					int Parse_MoveScore = getParseScoreFromArray_BestMoveScore(i, j, Obj);
					int Parse_BombScore = getParseScoreFromArray_BestBombScore(i, j, Obj);

					StageRecordData data = new StageRecordData(i, j, parse_StarNum, Parse_MoveScore, Parse_BombScore);
					scoreArray.Add(data);
				}
				else if(parse_StarNum != -1 &&
				        DB_StarNum != -1)
				{
					int bestStarNum = 0;
					int bestMoveNum = 0;
					int bestBombNum = 0;

					int DB_MoveScore = getDBScoreFromArray_BestMoveScore(i, j, dbScoreArray);
					int DB_BombScore = getDBScoreFromArray_BestBombScore(i, j, dbScoreArray);

					int Parse_MoveScore = getParseScoreFromArray_BestMoveScore(i, j, Obj);
					int Parse_BombScore = getParseScoreFromArray_BestBombScore(i, j, Obj);

					if(parse_StarNum > DB_StarNum)
					{
						bestStarNum = parse_StarNum;
					}
					else
					{
						bestStarNum = DB_StarNum;
					}

					if(Parse_MoveScore > DB_MoveScore)
					{
						bestMoveNum = Parse_MoveScore;
					}
					else
					{
						bestMoveNum = DB_MoveScore;
					}

					if(Parse_BombScore > DB_BombScore)
					{
						bestBombNum = Parse_BombScore;
					}
					else
					{
						bestBombNum = DB_BombScore;
					}

					if(bestStarNum != parse_StarNum ||
					   bestMoveNum != Parse_MoveScore ||
					   bestBombNum != Parse_BombScore)
					{
						string sColName = string.Format("Stage{0}_{1}", i, j);
						string sRecordScore = string.Format("{0};{1};{2}", bestStarNum, bestMoveNum, bestBombNum);
						
						Obj[sColName] = sRecordScore;
					}

					if(bestStarNum != DB_StarNum ||
					   bestMoveNum != DB_MoveScore ||
					   bestBombNum != DB_BombScore)
					{
						StageRecordData data = new StageRecordData(i, j, bestStarNum, bestMoveNum, bestBombNum);
						scoreArray.Add(data);
					}
				}
			}
		}

		if(scoreArray.Count > 0)
		{
			LocalDBManager.Instance.saveAllStageScoreRecord(scoreArray);
		}

		Obj.SaveAsync ();
	}

	public int getParseScoreFromArray_StarNum(int BigStage, int SmallStage, ParseObject Obj)
	{
		if(Obj == null)
		{
			return -1;
		}

		string sColName = string.Format("Stage{0}_{1}", BigStage, SmallStage);
		if(Obj.ContainsKey(sColName))
		{
			string sScoreFormat = Obj[sColName].ToString();
			string [] TmpStr = sScoreFormat.Split(';');

			if(TmpStr.Length != 3)
			{
				return -1;
			}

			string sBestStar = TmpStr[0];
			return int.Parse(sBestStar);
		}

		return -1;
	}

	public int getParseScoreFromArray_BestMoveScore(int BigStage, int SmallStage, ParseObject Obj)
	{
		if(Obj == null)
		{
			return -1;
		}
		
		string sColName = string.Format("Stage{0}_{1}", BigStage, SmallStage);
		
		if(Obj.ContainsKey(sColName))
		{
			string sScoreFormat = Obj[sColName].ToString();
			string [] TmpStr = sScoreFormat.Split(';');
			
			if(TmpStr.Length != 3)
			{
				return -1;
			}
			
			string sBestMoveScore = TmpStr[1];
			return int.Parse(sBestMoveScore);
		}
		
		return -1;
	}

	public int getParseScoreFromArray_BestBombScore(int BigStage, int SmallStage, ParseObject Obj)
	{
		if(Obj == null)
		{
			return -1;
		}
		
		string sColName = string.Format("Stage{0}_{1}", BigStage, SmallStage);
		
		if(Obj.ContainsKey(sColName))
		{
			string sScoreFormat = Obj[sColName].ToString();
			string [] TmpStr = sScoreFormat.Split(';');
			
			if(TmpStr.Length != 3)
			{
				return -1;
			}
			
			string sBestBombScore = TmpStr[1];
			return int.Parse(sBestBombScore);
		}
		
		return -1;
	}

	public int getDBScoreFromArray_StarNum(int BigStage, int SmallStage, List< StageRecordData > array)
	{
		if(array.Count <= 0)
		{
			return -1;
		}

		for(int i = 0; i < array.Count; i++)
		{
			StageRecordData data = array[i];

			if(data == null)
			{
				continue;
			}

			int bigStage = data.BigStage;
			int smallStage = data.SmallStage;

			if(bigStage == BigStage &&
			   smallStage == SmallStage)
			{
				int requestNum = data.BestStar;
				return requestNum;
			}
		}

		return -1;
	}

	public int getDBScoreFromArray_BestMoveScore(int BigStage, int SmallStage, List< StageRecordData > array)
	{
		if(array.Count <= 0)
		{
			return -1;
		}
		
		for(int i = 0; i < array.Count; i++)
		{
			StageRecordData data = array[i];
			
			if(data == null)
			{
				continue;
			}
			
			int bigStage = data.BigStage;
			int smallStage = data.SmallStage;
			
			if(bigStage == BigStage &&
			   smallStage == SmallStage)
			{
				int requestNum = data.BestMoveScore;
				return requestNum;
			}
		}
		
		return -1;
	}

	public int getDBScoreFromArray_BestBombScore(int BigStage, int SmallStage, List< StageRecordData > array)
	{
		if(array.Count <= 0)
		{
			return -1;
		}
		
		for(int i = 0; i < array.Count; i++)
		{
			StageRecordData data = array[i];
			
			if(data == null)
			{
				continue;
			}
			
			int bigStage = data.BigStage;
			int smallStage = data.SmallStage;
			
			if(bigStage == BigStage &&
			   smallStage == SmallStage)
			{
				int requestNum = data.BestBombScore;
				return requestNum;
			}
		}
		
		return -1;
	}

	public void parse_synchronousGameRecordDataFromParse()
	{
		CPDebug.Log ("parse_synchronousGameRecordDataFromParse");
		StartCoroutine( __parse_synchronousGameRecordDataFromParse());
	}

	IEnumerator __parse_synchronousGameRecordDataFromParse()
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ) ||
		   !fbIsLogin ||
		   myData == null)
		{
			yield return null;
		}

		string parseObjectId = LocalDBManager.Instance.GetGameRecordObject ();
		
		if(parseObjectId.Length > 0)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("GameRecord");
			
			Task< ParseObject > t =  query.GetAsync(parseObjectId);
			
			while( true )
			{
				if(t.IsCompleted)
				{
					if(t.Exception == null)
					{
						ParseObject gameRecord = t.Result;
						
						if(gameRecord != null)
						{
							parse_HandleChallengData(gameRecord);

							parse_HandleMailData(gameRecord);

							parse_HandleDailyGift(gameRecord);

							parse_HandleMyInviteHelp(gameRecord);

							gameRecordFinish = true;
							CPDebug.Log("gameRecord2 finish");

							// remove Black
							if(gameScoreFinish)
							{
								PopupManager.Instance.VisibleLoading(false);
								CPDebug.Log("Parse Init Finish");

								initNetWorkManager();
							}
						}
					}
					
					yield break;
				}
				yield return null;
			}
		}
	}

	public ChallengData getChallengDataByFBUid(string FBUid)
	{
		if(otherChallengMeData.Count <= 0)
		{
			return null;
		}

		for(int i = 0; i < otherChallengMeData.Count; i++)
		{
			ChallengData data = otherChallengMeData[i];

			if(data == null)
			{
				continue;
			}

			if(data.challengFBUid == FBUid)
			{
				return data;
			}
		}

		return null;
	}

	public void parse_deleteOtherChallengInfo(string OtherFBUid)
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ) ||
		   !fbIsLogin ||
		   myData == null)
		{
			return;
		}

		ChallengData data = getChallengDataByFBUid (OtherFBUid);
		
		if(data == null)
		{
			return;
		}

		StartCoroutine (__parse_deleteOtherChallengInfo(OtherFBUid));
	}

	IEnumerator __parse_deleteOtherChallengInfo(string OtherFBUid)
	{
		string parseObjectId = LocalDBManager.Instance.GetGameRecordObject ();
		
		if(parseObjectId.Length > 0)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("GameRecord");
			
			Task< ParseObject > t =  query.GetAsync(parseObjectId);
			
			while( true )
			{
				if(t.IsCompleted)
				{
					if(t.Exception == null)
					{
						ParseObject gameRecord = t.Result;
						ChallengData data = getChallengDataByFBUid (OtherFBUid);
						
						if(gameRecord != null &&
						   data != null)
						{
							if( gameRecord.ContainsKey( "otherChalleng" ) == false )
							{
								yield break;
							}
							
							IList<object> handleArray = gameRecord.Get<List<object>>("otherChalleng");
							removeStringFromList(handleArray, data.parse_oringe_string);

							gameRecord["otherChalleng"] = handleArray;
							
							Task t2 =  gameRecord.SaveAsync();

							while( true )
							{
								if(t2.IsCompleted)
								{
									if(t2.Exception == null)
									{
										removeOtherChallengData(OtherFBUid);
										removeOtherChallengDataFromMail(OtherFBUid);
									}

									yield break;
								}

								yield return null;
							}
						}
					}
					
					yield break;
				}
				yield return null;
			}
		}
	}

	public void removeOtherChallengData(string FBUid)
	{
		for(int i = 0; i < otherChallengMeData.Count; i++)
		{
			ChallengData data = otherChallengMeData[i];

			if(data == null)
			{
				continue;
			}

			if(data.challengFBUid == FBUid)
			{
				otherChallengMeData.RemoveAt(i);
				break;
			}
		}
	}

	public void removeOtherChallengDataFromMail(string FBUid)
	{
		for(int i = 0; i < mailData.Count; i++)
		{
			MailData data = mailData[i];
			
			if(data == null)
			{
				continue;
			}
			
			if(data.mailType != Config.MESSAGE_TYPE_CHALLENG_FROM_OTHER)
			{
				continue;
			}

			if(data.mailComFrom == FBUid)
			{
				mailData.RemoveAt(i);
				break;
			}
		}
	}

	public void parse_getFriendScoreInfo(string FBUid)
	{
		if((Application.internetReachability == NetworkReachability.NotReachable ) ||
		   !fbIsLogin ||
		   myData == null)
		{
			return;
		}

		StartCoroutine (__parse_getFriendScoreInfo(FBUid));
	}

	IEnumerator __parse_getFriendScoreInfo(string FBUid)
	{
		ParseQuery<ParseObject> query = ParseObject.GetQuery("GameScore").WhereEqualTo("FacebookID", FBUid);
		
		Task< ParseObject > t =  query.FirstAsync();
		
		while( true )
		{
			if(t.IsCompleted)
			{
				if(t.Exception == null)
				{
					ParseObject obj = t.Result;
					
					if(obj != null)
					{
						PersonalData friend = getFriendData(FBUid);

						if(friend != null)
						{
							friend.PD_ParseScoreData = obj;
						}
					}
				}
				
				yield break;
			}
			yield return null;
		}
	}

	// test parse
	/*  ParseObject testObject = new ParseObject("GameStore");
		testObject["foo"] = "bar";
		testObject.SaveAsync();

		ParseQuery<ParseObject> query = ParseObject.GetQuery("GameScore");
		query.GetAsync("yNHFLgFA93").ContinueWith(t =>
		                                          {
			ParseObject gameScore = t.Result;
			Debug.Log("111111111~~" +  gameScore["FacebookName"] );
		});
   */
}
