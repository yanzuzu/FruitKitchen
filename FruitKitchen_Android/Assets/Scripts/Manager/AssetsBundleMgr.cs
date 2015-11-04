using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
/*
 * 2013-01-06 小豬 
 * AssetsBundleMgr 負責將Bundle資料下載並存起來提供給外部取得
*/

public class AssetsBundleMgr : ManagerComponent<AssetsBundleMgr> {

	// 一次下載的量
	int MaxDownloadNum = 4;
	//int NowDownloadNum = 0;
	//  cache altas num
	int CacheAtlasMaxNum = 30;
	// 下載完成的callBack
	public delegate void OnDownLoadFinishCallBack();
	// 下載過程的callBack
	public delegate void OnDownLoadProcessCallBack();
	// playPref的Key Name
	private string PlayPrefVersionKey = "APversion";
	// AssetPrefab Map
	private Dictionary<string , UnityEngine.Object > AssetsMap = new Dictionary<string, UnityEngine.Object>();
	private List<string> AssetsQueue = new List<string>();
	private string m_baseUrl = "";
	// 要下載的物件
	private OnDownLoadFinishCallBack FinishCB = null;
	private OnDownLoadProcessCallBack ProcessCB = null;
	private List<string> DownLoadDataList = new List<string>();
	private List<string> CurrentDownLoadList = new List<string>();
	
	// Use this for initialization
	void Start () {
		EventManager.Instance.registerEvent(EventManager.EventName.LoadScene , onLoadScene );
	}

	void onLoadScene(params object [] param)
	{
		// clear the Map
		AssetsMap.Clear();
		AssetsQueue.Clear();
	}

	/*
	 * 取得 Bundle 的 prefab 資料
	 * params: Bundle路徑
	 * return: prefab Object( 若沒有,回傳Null )
	 */
	public UnityEngine.Object getAssetsPrefab( string PrefabPath )
	{

//#if UNITY_EDITOR
		// input: AIWN00011310240001\/wep_01_001
		if( AssetsMap.ContainsKey( PrefabPath ) ) return AssetsMap[PrefabPath];

		UnityEngine.Object tempObj = Resources.Load(PrefabPath);
        if (null == tempObj) {
			CPDebug.LogError("Read from resources failed, path : " + PrefabPath);
        }
        else {
            UnityEngine.Object tempObj2 = GameObject.Instantiate(tempObj);
			if( AssetsQueue.Count >= CacheAtlasMaxNum )
			{
				string RemoveAssetPath = AssetsQueue[0];
				AssetsQueue.RemoveAt(0);
				DestroyImmediate( AssetsMap[RemoveAssetPath] );
				AssetsMap.Remove(RemoveAssetPath);

			}

			AssetsMap[PrefabPath] = tempObj2;
			AssetsQueue.Add( PrefabPath );

			return AssetsMap[PrefabPath];
        }		
		return null;
//#else
//		if( AssetsMap.ContainsKey( PrefabRootName ) == false ) 
//		{
//			CPDebug.LogError(string.Format( "this PrefabPath = {0} is not exist!!",PrefabPath));
//			return null;
//		}
//		return AssetsMap[PrefabRootName];

//#endif

	}
	// Update is called once per frame
	void Update () {
		// to do: check Download Failed!!
		// 判斷是否有東西要下載
		if( DownLoadDataList.Count == 0 ) return;

		if( CurrentDownLoadList.Count < MaxDownloadNum )
		{
			if( DownLoadDataList.Count > 0)
			{
				string Path = DownLoadDataList[0];
				DownLoadDataList.Remove(Path);
				CurrentDownLoadList.Add(Path);
				StartCoroutine(StartDownLoad(Path));
			}
		}

	}
	// 開始下載
	private IEnumerator StartDownLoad(string FilePath )
	{
		// wait fot Caching System
		while( !Caching.ready )
			yield return null;
		//int Version = -1;

		if(  FilePath == "" ) yield break;

		// check sum if need to download
		string CheckSumValue = "";
		string OldCheckSumValue = "";

		if( PlayerPrefs.HasKey( FilePath ) )
		{
			CheckSumValue = PlayerPrefs.GetString(FilePath);
		}

		if( CheckSumValue == OldCheckSumValue )
		{

			checkDownLoadResult(FilePath);
			yield break;
		}

		PlayerPrefs.SetString(FilePath,OldCheckSumValue);

		using( WWW wwwData = WWW.LoadFromCacheOrDownload( m_baseUrl + FilePath , -1 ) )
		{
			yield return wwwData;
			if( wwwData.error != null )
			{
				// to do: show the MessageBox to notify need the Web
				CPDebug.LogError(string.Format("Download error = {0}",wwwData.error));
				yield break;
			}

			// DownLoad Finish
			AssetBundle bundle = wwwData.assetBundle;
			if( bundle != null )
			{
				UnityEngine.Object DataObj =  Instantiate(bundle.mainAsset);
				AssetsMap[ FilePath ] = DataObj;
			}
			// Unload the AssetBundles compressed contents to conserve memory
			bundle.Unload(false);
			checkDownLoadResult(FilePath);
		}
	}
	void checkDownLoadResult( string FilePath )
	{
		CurrentDownLoadList.Remove( FilePath);
		// callBack process
		if( ProcessCB != null )
			ProcessCB();
		// 判斷是否已經全部下載完成
		if(  DownLoadDataList.Count == 0 && CurrentDownLoadList.Count == 0)
		{
			FinishCB();
		}
	}
}
