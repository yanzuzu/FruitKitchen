using UnityEngine;
using System.Collections;
using Pathfinding.Serialization.JsonFx;
using System.Collections.Generic;

public class TipRecordData
{
	public int BigStage = 0;
	public int SmallStage = 0;
	public bool IsRecord = true;
}

public class StageRecordData
{
	public int BigStage = 0;
	public int SmallStage = 0;
	public int BestStar = 0;
	public int BestMoveScore = 0;
	public int BestBombScore = 0;

	public StageRecordData(){}
	public StageRecordData(int bigStage, int smallStage, int bestStar, int bestMoveScore, int bestBombScore)
	{
		BigStage = bigStage;
		SmallStage = smallStage;
		BestStar = bestStar;
		BestMoveScore = bestMoveScore;
		BestBombScore = bestBombScore;
	}
	/*
	public StageRecordData(int bigStage, int smallStage, int bestStar, int bestMoveScore, int bestBombScore)
	{
		BigStage = bigStage;
		SmallStage = smallStage;
		BestStar = bestStar;
		BestMoveScore = bestMoveScore;
		BestBombScore = bestBombScore;
	}*/
}

public class LocalDBManager : ManagerComponent< LocalDBManager >{
	//public static LocalDBManager Instance = null;

	public Dictionary< string ,T > GetDictMapData<T>( string p_Data )
	{
		Dictionary< string , T >  TmpMap = new Dictionary<string, T>();
		if( p_Data != string.Empty )
		{
			Dictionary< string , object > TmpObjectMap = JsonReader.Deserialize< Dictionary< string , object > >( p_Data );
			foreach( string MapKey in TmpObjectMap.Keys )
			{
				T  TmpStageData = JsonReader.Deserialize<T>( JsonWriter.Serialize( TmpObjectMap[MapKey] ) ); 
				TmpMap[MapKey] = TmpStageData;
			}
		}
		return TmpMap;
	}

	// Use this for initialization
	void Start () {
		string bigStage = getRecordBigStage ();

		if(bigStage.Length > 0)
		{
			DataManager.Instance.recordBigStageNum = int.Parse(bigStage);
		}
		else
		{
			DataManager.Instance.recordBigStageNum = 1;
		}

		string smallStage = getRecordSmallStage ();
		
		if(smallStage.Length > 0)
		{
			DataManager.Instance.recordSmallStageNum = int.Parse(smallStage);
		}
		else
		{
			DataManager.Instance.recordSmallStageNum = 4;
		}
	}

	public void resetAll()
	{
		PlayerPrefs.DeleteAll ();
	}

	////////////////Facebook////////////////
	public void SetFBUid( string p_uid )
	{
		PlayerPrefs.SetString("FB_Uid",p_uid );
		PlayerPrefs.Save();
	}
	public string GetFBUid()
	{
		return PlayerPrefs.GetString("FB_Uid","" );
	}
	////////////////Facebook////////////////
	////////////////Parse////////////////
	public void SetGameScoreObject( string obj )
	{
		PlayerPrefs.SetString("gameScoreObjectId", obj);
		PlayerPrefs.Save();
	}
	
	public string GetGameScoreObject()
	{
		return PlayerPrefs.GetString ("gameScoreObjectId","" );
	}

	public void SetGameRecordObject( string obj )
	{
		PlayerPrefs.SetString("gameRecordObjectId", obj);
		PlayerPrefs.Save();
	}
	
	public string GetGameRecordObject()
	{
		return PlayerPrefs.GetString ("gameRecordObjectId","" );
	}
	public void clearParseTable()
	{
		SetGameScoreObject ("");
		SetGameRecordObject ("");
	}
	////////////////Parse////////////////
	////////////////LifeNum////////////////
	public bool initLifeRecordTable()
	{
		if(GetLifeNum() == -1)
		{
			SetLifeNum(Config.MAX_PLAY_NUM);
			return true;
		}

		return false;
	}

	public void SetLifeNum( int num )
	{
		PlayerPrefs.SetInt("LifeNum", num);
		PlayerPrefs.Save();
	}
	
	public int GetLifeNum()
	{
		return PlayerPrefs.GetInt ("LifeNum", Config.MAX_PLAY_NUM);
	}

	public void addLife(int Num)
	{
		int nowLife = GetLifeNum();

		nowLife += Num;

		if(nowLife > Config.MAX_PLAY_NUM)
		{
			nowLife = Config.MAX_PLAY_NUM;
		}
	
		if(nowLife < 0)
		{
			nowLife = 0;
		}

		SetLifeNum(nowLife);
	}

	public bool decreaseOneLife()
	{
		int nowLife = GetLifeNum ();

		if( nowLife <= 0 ) return false;
		nowLife--;
		/*
		if(nowLife < 0)
		{
			nowLife = 0;
		}*/

		SetLifeNum (nowLife);
		return true;
	}
	public void updateNextrecoverTime( string time )
	{
		PlayerPrefs.SetString("NextrecoverTime", time);
		PlayerPrefs.Save();
	}
	
	public string getNextrecoverTime()
	{
		return PlayerPrefs.GetString ("NextrecoverTime", "");
	}
	////////////////LifeNum////////////////
	////////////////Tip////////////////
	public void addRecord( int BigStage , int SmallStage)
	{
		TipRecordData pData = new TipRecordData ();
		pData.BigStage = BigStage;
		pData.SmallStage = SmallStage;
		pData.IsRecord = true;

		string Key = string.Format("{0}:{1}" , BigStage, SmallStage);
		string TmpStrMapData = PlayerPrefs.GetString("TipRecordData" , "");
		
		Dictionary< string , TipRecordData >  TmpMap = GetDictMapData< TipRecordData >(TmpStrMapData);
		TmpMap[Key] = pData;
		PlayerPrefs.SetString("TipRecordData" , JsonWriter.Serialize(TmpMap) );
		PlayerPrefs.Save();
	}
	public bool isRecordExist( int p_BigStage , int p_SmallStage )
	{
		string Key = string.Format("{0}:{1}" , p_BigStage,p_SmallStage );
		string TmpStrMapData = PlayerPrefs.GetString("TipRecordData" , "");
		
		Dictionary< string , TipRecordData >  TmpMap = GetDictMapData< TipRecordData >(TmpStrMapData);

		if(TmpMap.ContainsKey( Key ) == false)
		{
			return false;
		}

		return true;
	}
	////////////////Tip////////////////
	////////////////Option////////////////
	public void updateOptionSet(bool MusicClose, float MusicNum, bool SoundClose, float SoundNum, int Language)
	{
		if(MusicClose)
		{
			string text = "1";
			PlayerPrefs.SetString("musicClose", text);
		}
		else
		{
			string text = "0";
			PlayerPrefs.SetString("musicClose", text);
		}

		PlayerPrefs.SetString("musicPower", MusicNum.ToString());

		if(SoundClose)
		{
			string text = "1";
			PlayerPrefs.SetString("soundClose", text);
		}
		else
		{
			string text = "0";
			PlayerPrefs.SetString("soundClose", text);
		}
		
		PlayerPrefs.SetString("soundPower", SoundNum.ToString());
		PlayerPrefs.SetString("language", Language.ToString());
		PlayerPrefs.Save();
	}

	public string getMusicClose()
	{
		return PlayerPrefs.GetString ("musicClose");
	}

	public void setMusicMute(bool Mute)
	{
		if(Mute)
		{
			string text = "1";
			PlayerPrefs.SetString("musicClose", text);
			PlayerPrefs.Save();
		}
		else
		{
			string text = "0";
			PlayerPrefs.SetString("musicClose", text);
			PlayerPrefs.Save();
		}
	}

	public string getMusicNum()
	{
		return PlayerPrefs.GetString ("musicPower");
	}

	public void setMusicPower(float Power)
	{
		PlayerPrefs.SetString ("musicPower", Power.ToString());
		PlayerPrefs.Save();
	}

	public string getSoundClose()
	{
		return PlayerPrefs.GetString ("soundClose");
	}

	public void setSoundMute(bool Mute)
	{
		if(Mute)
		{
			string text = "1";
			PlayerPrefs.SetString("soundClose", text);
			PlayerPrefs.Save();
		}
		else
		{
			string text = "0";
			PlayerPrefs.SetString("soundClose", text);
			PlayerPrefs.Save();
		}
	}

	public string getSoundNum()
	{
		return PlayerPrefs.GetString ("soundPower");
	}

	public void setSoundPower(float Power)
	{
		PlayerPrefs.SetString ("soundPower", Power.ToString());
		PlayerPrefs.Save();
	}

	public void setLanguage(int Language)
	{
		PlayerPrefs.SetString ("language", Language.ToString());
		PlayerPrefs.Save();
	}

	public string GetLanguage()
	{
		int TmpEn = PlayerPrefs.GetInt("languageEn" , 0 );
		if( TmpEn == 1 ) return "English";
		int TmpTw = PlayerPrefs.GetInt("languageTw" , 1 );
		if( TmpTw == 1 ) return "tw";
		return "tw";
	}	
	// get Music and Sound Setting
	public float GetSound()
	{
		return PlayerPrefs.GetFloat("Sound", 0.5f );
		
	}
	public void SetSound( float p_sound)
	{
		PlayerPrefs.SetFloat("Sound" , p_sound );
		PlayerPrefs.Save();
	}
	public float GetMusic()
	{
		return PlayerPrefs.GetFloat("Music", 0.5f );
	}
	public void SetMusic( float p_music)
	{
		PlayerPrefs.SetFloat("Music" , p_music );
		PlayerPrefs.Save();
	}
	
	public bool GetMusicMute()
	{
		int TmpMute = PlayerPrefs.GetInt("MusicMute" , 0 );
		bool Result = TmpMute == 0 ? false : true; 
		return Result;
	}
	
	public bool GetSoundMute()
	{
		int TmpMute = PlayerPrefs.GetInt("SoundMute" , 0 );
		bool Result = TmpMute == 0 ? false : true; 
		return Result;
	}
	////////////////Option////////////////
	////////////////Item////////////////
	public void addOneOwnItem(int p_ItemID , int p_ItemNum )
	{
		Dictionary< int , int > TmpMap = GetBackPack();
		if( TmpMap.ContainsKey( p_ItemID ) == false )
			TmpMap[ p_ItemID ] = 0;
		TmpMap[ p_ItemID ] += p_ItemNum;
		if( TmpMap[ p_ItemID ] > Config.ItemMaxNum ) TmpMap[ p_ItemID ] = Config.ItemMaxNum;
		PlayerPrefs.SetString("BackPack",JsonWriter.Serialize(TmpMap));
		PlayerPrefs.Save();
	}
	
	public int getOwnItemNum( int p_ItemID )
	{
		Dictionary< int , int > Result = GetBackPack();
		if( Result.ContainsKey( p_ItemID ) == false ) return 0;
		return Result[ p_ItemID ];
	}

	public bool decreaseItemNum( int p_ItemID , int p_ItemNum )
	{
		Dictionary< int , int > TmpMap = GetBackPack();
		if( TmpMap.ContainsKey( p_ItemID ) == false )
			return false;
		TmpMap[ p_ItemID ] -= p_ItemNum;
		if( TmpMap[ p_ItemID ] < 0 ) 
			return false;
		if(  TmpMap[ p_ItemID ] <= 0 )
			TmpMap.Remove(p_ItemID );
		PlayerPrefs.SetString("BackPack",JsonWriter.Serialize(TmpMap));
		PlayerPrefs.Save();

		return true;
	}

	public Dictionary< int , int > GetBackPack()
	{
		Dictionary< int , int > Result = new Dictionary< int , int >();
		string TmpJsonStr = PlayerPrefs.GetString("BackPack" , ""  );
		if( TmpJsonStr == "" )
			return Result;
		Dictionary< string , int > TmpResult = JsonReader.Deserialize<Dictionary<string,int>>(TmpJsonStr);
		foreach( string Key in TmpResult.Keys )
		{
			Result[ int.Parse( Key ) ] = TmpResult[ Key ];
		}
		return Result;
	}

	public void ClearBackPack()
	{
		PlayerPrefs.SetString("BackPack" , ""  );
		PlayerPrefs.Save();
	}
	////////////////Item////////////////
	////////////////StageRecord////////////////
	public string getRecordBigStage()
	{
		return PlayerPrefs.GetString ("recordBigScore", "1");
	}

	public string getRecordSmallStage()
	{
		return PlayerPrefs.GetString ("recordSmallScore", "4");;
	}

	public void updateRecordStage(int BigStage, int SmallStage)
	{
		PlayerPrefs.SetString("recordBigScore", BigStage.ToString());
		PlayerPrefs.SetString("recordSmallScore", SmallStage.ToString());
		PlayerPrefs.Save();
	}

	public void updateRecordBigStage(int BigStage)
	{
		PlayerPrefs.SetString("recordBigScore", BigStage.ToString());
		PlayerPrefs.Save();
	}

	public void updateRecordSmallStage(int SmallStage)
	{
		PlayerPrefs.SetString("recordSmallScore", SmallStage.ToString());
		PlayerPrefs.Save();
	}

	public void getAllStageScoreRecord(List< StageRecordData > array)
	{
		string TmpStrMapData = PlayerPrefs.GetString("StageScoreDataMap" , "");
		Dictionary< string , StageRecordData >  TmpMap = GetDictMapData< StageRecordData >(TmpStrMapData);

		foreach( string Key in TmpMap.Keys )
		{
			StageRecordData data = TmpMap[Key];	
			array.Add(data);
		}
	}

	public void saveAllStageScoreRecord(List< StageRecordData > array)
	{
		for(int i = 0; i < array.Count; i++)
		{
			StageRecordData data = array[i];

			if(data == null)
			{
				continue;
			}

			int bigStage = data.BigStage;
			int smallStage = data.SmallStage;

			SetStageRecord(bigStage, smallStage, data);
		}
	}
	////////////////StageRecord////////////////
	////////////////StageScoreRecord////////////////
	public void saveStageStarNum(int BigStage, int SmallStage, int StarNum, int MoveNum, int BombNum)
	{
		CPDebug.Log ("saveStageStarNum111111");
		int bestMoveRecord = getBestMoveScore (BigStage, SmallStage);

		if(MoveNum > bestMoveRecord)
		{
			bestMoveRecord = MoveNum;
		}

		int bestBombRecord = getBestBombScore (BigStage, SmallStage);

		if(BombNum > bestBombRecord)
		{
			bestBombRecord = BombNum;
		}

		int bestStarNum = getStageStarNum (BigStage, SmallStage);

		if(StarNum > bestStarNum)
		{
			bestStarNum = StarNum;
		}

		if(BigStage == 1 &&
		   SmallStage <= 3)
		{
			bestMoveRecord = 0;
			bestBombRecord = 0;
			bestStarNum= 0;
		}

		StageRecordData data = GetStageRecord (BigStage, SmallStage);

		CPDebug.Log ("saveStageStarNum222222");
		if(data.BigStage == 0 &&
		   data.SmallStage == 0)
		{
			CPDebug.Log ("saveStageStarNum333333");
			data = new StageRecordData(BigStage, SmallStage, StarNum, MoveNum, BombNum);
			SetStageRecord(BigStage, SmallStage, data);
			NetWorkManager.Instance.parse_UpdateOneStage(BigStage, SmallStage, bestStarNum, bestMoveRecord, bestBombRecord);

			int smallStageMaxNum = DataManager.Instance.getSmallStageNum(BigStage);

			if(SmallStage == smallStageMaxNum)
			{
				CPDebug.Log ("saveStageStarNum444444");
				int totalStarNum = getStageTotalStarNum(BigStage);

				if(totalStarNum >= Config.UNLOCK_BIG_STAGE_STAR_NUM)
				{
					unlockNextBigStage();
				}
				else
				{
					DataManager.Instance.recordSmallStageNum = SmallStage;
					updateRecordSmallStage(SmallStage);
				}
			}
			else
			{
				CPDebug.Log ("saveStageStarNum555555");
				DataManager.Instance.recordSmallStageNum = SmallStage + 1;
				updateRecordSmallStage(DataManager.Instance.recordSmallStageNum);
			}

			NetWorkManager.Instance.parse_UpdateStage(DataManager.Instance.recordBigStageNum, DataManager.Instance.recordSmallStageNum);
		}
		else
		{
			CPDebug.Log ("saveStageStarNum666666");
			data.BestStar = bestStarNum;
			data.BestMoveScore = bestMoveRecord;
			data.BestBombScore = bestBombRecord;

			SetStageRecord(BigStage, SmallStage, data);
			NetWorkManager.Instance.parse_UpdateOneStage(BigStage, SmallStage, bestStarNum, bestMoveRecord, bestBombRecord);

			CPDebug.Log ("recordBigStageNum = " + DataManager.Instance.recordBigStageNum);
			if(DataManager.Instance.recordBigStageNum < Config.MAX_BIG_STAGE_NUM)
			{
				if(DataManager.Instance.recordBigStageNum == DataManager.Instance.BigStageNum)
				{
					CPDebug.Log ("saveStageStarNum777777");
					int totalStar = getStageTotalStarNum(DataManager.Instance.BigStageNum);
					CPDebug.Log ("totalStar = " + totalStar);
					if(totalStar >= Config.UNLOCK_BIG_STAGE_STAR_NUM)
					{
						CPDebug.Log ("saveStageStarNum888888");
						unlockNextBigStage();
					}
				}
			}
		}
	}

	public void SetStageRecord( int p_BigStage , int p_SmallStage , StageRecordData p_Data )
	{
		string Key = string.Format("{0}:{1}" , p_BigStage,p_SmallStage );
		string TmpStrMapData = PlayerPrefs.GetString("StageScoreDataMap" , "");
		
		Dictionary< string , StageRecordData >  TmpMap = GetDictMapData< StageRecordData >(TmpStrMapData);
		TmpMap[Key] = p_Data;
		CPDebug.Log ("SetStageRecord Key = " + Key);
		PlayerPrefs.SetString("StageScoreDataMap" , JsonWriter.Serialize(TmpMap) );
		PlayerPrefs.Save();
	}
	public StageRecordData GetStageRecord( int p_BigStage , int p_SmallStage )
	{
		string Key = string.Format("{0}:{1}" , p_BigStage,p_SmallStage );
		string TmpStrMapData = PlayerPrefs.GetString("StageScoreDataMap" , "");
		
		Dictionary< string , StageRecordData >  TmpMap = GetDictMapData< StageRecordData >(TmpStrMapData);
		if(  false == TmpMap.ContainsKey( Key ) ) return new StageRecordData(); 
		return TmpMap[Key];
	}
	public void clearStageScoreRecordTable()
	{
		PlayerPrefs.SetString("StageScoreDataMap" , "" );
		PlayerPrefs.Save();
	}

	public int getStageStarNum(int BigStage, int SmallStage)
	{
		StageRecordData data = GetStageRecord (BigStage, SmallStage);

		if(data != null)
		{
			return data.BestStar;
		}

		return 0;
	}

	public int getBestMoveScore(int BigStage, int SmallStage)
	{
		StageRecordData data = GetStageRecord (BigStage, SmallStage);
		
		if(data != null)
		{
			return data.BestMoveScore;
		}

		return 0;
	}

	public int getBestBombScore(int BigStage, int SmallStage)
	{
		StageRecordData data = GetStageRecord (BigStage, SmallStage);
		
		if(data != null)
		{
			return data.BestBombScore;
		}

		return 0;
	}

	public int getAverageMoveScore(int BigStage)
	{
		string TmpStrMapData = PlayerPrefs.GetString("StageScoreDataMap" , "");
		Dictionary< string , StageRecordData >  TmpMap = GetDictMapData< StageRecordData >(TmpStrMapData);

		int score = 0;
		int stageCount = 0;

		foreach( string Key in TmpMap.Keys )
		{
			StageRecordData data = TmpMap[Key];	

			if(data.BigStage != BigStage)
			{
				continue;
			}

			if(data.BigStage == 1 &&
			   data.SmallStage <= 3)
			{
				continue;
			}

			int recordBestMoveScore = data.BestMoveScore;
			score += recordBestMoveScore;
			stageCount++;
		}

		if(score != 0 &&
		   stageCount != 0)
		{
			int average = score / stageCount;
			return average;
		}

		return 0;
	}

	public int getAverageBombScore(int BigStage)
	{
		string TmpStrMapData = PlayerPrefs.GetString("StageScoreDataMap" , "");
		Dictionary< string , StageRecordData >  TmpMap = GetDictMapData< StageRecordData >(TmpStrMapData);
		
		int score = 0;
		int stageCount = 0;
		
		foreach( string Key in TmpMap.Keys )
		{
			StageRecordData data = TmpMap[Key];	

			if(data.BigStage != BigStage)
			{
				continue;
			}
			
			if(data.BigStage == 1 &&
			   data.SmallStage <= 3)
			{
				continue;
			}
			
			int recordBestBombScore = data.BestBombScore;
			score += recordBestBombScore;
			stageCount++;
		}
		
		if(score != 0 &&
		   stageCount != 0)
		{
			int average = score / stageCount;
			return average;
		}
		
		return 0;
	}

	public int getStageTotalStarNum(int BigStage)
	{
		string TmpStrMapData = PlayerPrefs.GetString("StageScoreDataMap" , "");
	
		Dictionary< string , StageRecordData >  TmpMap = GetDictMapData< StageRecordData >(TmpStrMapData);

		int totalStar = 0;

		foreach (string Key in TmpMap.Keys)
		{
			StageRecordData data = TmpMap[Key];	

			if(data.BigStage == BigStage)
			{
				totalStar += data.BestStar;
			}
		}

		return totalStar;
	}

	public bool isAllSmallStagePass(int BigStage)
	{
		int bestBigStage = int.Parse(getRecordBigStage ());

		if(bestBigStage > BigStage)
		{
			return true;
		}
		else if(bestBigStage < BigStage)
		{
			return false;
		}

		int bestSmallStage = int.Parse(getRecordSmallStage ());
		int thisSmallStageNum = DataManager.Instance.getSmallStageNum (BigStage);

		if(bestSmallStage >= thisSmallStageNum)
		{
			return true;
		}

		return false;
	}
	////////////////StageScoreRecord////////////////
	////////////////UnlockInfo////////////////
	public void insertUnlockInfo(int UnlockType, int UnlockParam)
	{
		Dictionary< int , int > TmpMap = getUnlockInfo();
		TmpMap[ UnlockType ] = UnlockParam;

		PlayerPrefs.SetString("unlockInfo",JsonWriter.Serialize(TmpMap));
		PlayerPrefs.Save();
	}

	Dictionary< int , int > getUnlockInfo()
	{
		Dictionary< int , int > Result = new Dictionary< int , int >();
		string TmpJsonStr = PlayerPrefs.GetString("unlockInfo" , JsonWriter.Serialize( Result )  );
		Result = JsonReader.Deserialize<Dictionary<int,int>>(TmpJsonStr);
		return Result;
	}

	public bool isUnlock(int UnlockType, int UnlockParam)
	{
		Dictionary< int , int > Result = getUnlockInfo();
		if( Result.ContainsKey( UnlockType ) == false )
			return false;

		return true;
	}

	public bool unlockNextBigStage()
	{
		if(DataManager.Instance.recordBigStageNum >= Config.MAX_BIG_STAGE_NUM)
		{
			return false;
		}

		DataManager.Instance.recordBigStageNum++;
		DataManager.Instance.recordSmallStageNum = 1;

		updateRecordStage (DataManager.Instance.recordBigStageNum, DataManager.Instance.recordSmallStageNum);

		return true;
	}

	public void clearUnlockInfoTable()
	{
		
	}
	////////////////UnlockInfo////////////////
}