using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CSVTable
{
	string m_resPath = "TxtData/";

	public Dictionary< int , Dictionary< string , string > > m_DataMap = new Dictionary<int, Dictionary<string, string>>();
	List<int> m_DataKey = new List<int>();

	public void Setup(string FileName )
	{

		TextAsset  TmpTxtAsset = AssetsBundleMgr.Instance.getAssetsPrefab( m_resPath + FileName) as TextAsset;	
		string [] LineList = TmpTxtAsset.text.Split('\n');

		List< string > TmpColumnList = new List<string>();

		for( int i = 0 ; i < LineList.Length ; i ++ )
		{
			if( i == 0 )
			{
				// header
				string [] TmpHeaderList = LineList[i].Split(',');
				for( int k = 1 ; k < TmpHeaderList.Length ; k ++ )
				{
					// k== 0 is Key , ignore
					TmpColumnList.Add( TmpHeaderList[k] );
				}
			}else{
				if( string.IsNullOrEmpty( LineList[i] ) == true ) continue;
				// Data
				string [] TmpDataList = LineList[i].Split(',');
				int TmpKey = 0;
				for( int k = 0 ; k < TmpDataList.Length ; k ++ )
				{
					if( 0 == k )
					{
						TmpKey = int.Parse( TmpDataList[k] );
						m_DataKey.Add( TmpKey);
						m_DataMap[ TmpKey ] = new Dictionary<string, string>();
					}else
					{
						m_DataMap[ TmpKey ][TmpColumnList[k-1]] = TmpDataList[k];
					}
				}
			}
		}

	}

	public List<int> GetKeys()
	{
		return m_DataKey;
	}

	public string readFieldAsString( int p_key , string p_columnName )
	{
		if( m_DataMap.ContainsKey( p_key ) == false )
		{
			CPDebug.LogError("no this Key = " + p_key);
			return "";
		}
		if( m_DataMap[ p_key ].ContainsKey( p_columnName ) == false )
		{
			CPDebug.LogError("no this p_columnName = " + p_columnName);
			return "";
		}
		return m_DataMap[p_key][p_columnName];
	}

	public int readFieldAsInt( int p_key , string p_columnName )
	{
		if( m_DataMap.ContainsKey( p_key ) == false )
		{
			CPDebug.LogError("no this Key = " + p_key);
			return -1;
		}
		if( m_DataMap[ p_key ].ContainsKey( p_columnName ) == false )
		{
			CPDebug.LogError("no this p_columnName = " + p_columnName);
			return -1;
		}
		return int.Parse( m_DataMap[p_key][p_columnName] );
	}
}
