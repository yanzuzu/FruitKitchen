using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SoundManager : ManagerComponent<SoundManager>{

	[SerializeField]
	private string ResourcePrefixPath = "Sound/";

	[SerializeField]
	// max audio resource num
	private int MaxAudioNum = 10;

	[SerializeField]
	// Max Se src num
	private int MaxSENum = 3;

	// mapping the Audio Name to Audio file Path
	private Dictionary<string ,string > mSoundNameMap = new Dictionary<string, string>();

	private Dictionary<string , AudioClip  > mSoundMusicObjMap = new Dictionary<string, AudioClip>();
	private List< string > mSoundMusicList = new List<string>();


	private List<AudioSource> mSeAudioSrc = new List<AudioSource>();
	private int mNowSeIdx = 0;
	private AudioSource mBGMAudioSrc;
	void Awake()
	{
		// read the sound txt
		TextAsset txt = AssetsBundleMgr.Instance.getAssetsPrefab("soundTxt") as TextAsset;
		ValidateHelper.Begin().NotNullObj(txt);
		ByteReader reader = new ByteReader(txt);
		mSoundNameMap = reader.ReadDictionary();

		// create AudioSource
		for( int i = 0 ; i< MaxSENum ; i ++ )
			mSeAudioSrc.Add(this.gameObject.AddComponent<AudioSource>());
		mBGMAudioSrc = this.gameObject.AddComponent<AudioSource>();
		mBGMAudioSrc.loop = true;
		this.gameObject.AddComponent<AudioListener>();

	}
	// Play SE
	public void PlaySE( string p_SoundName , bool IsLoop = false )
	{
		playSound(p_SoundName, false , IsLoop );
	}
	// Play BGM
	public void PlayBGM( string p_BGMName , bool IsLoop = true   )
	{
		playSound(p_BGMName, true , IsLoop );
	}
	// set SE Volume
	public void SetSEVolume( float p_volume)
	{
		for( int i = 0 ; i < mSeAudioSrc.Count ; i ++ )
		{
			if( mSeAudioSrc[i] != null )
				mSeAudioSrc[i].volume = p_volume;
		}
	}

	// set BGM Volume
	public void SetBGMVolume( float p_volume)
	{
		mBGMAudioSrc.volume = p_volume;
	}

	// disable SE 
	public void MuteSE( bool IsMute )
	{
		for( int i = 0 ; i < mSeAudioSrc.Count ; i ++ )
		{
			if( mSeAudioSrc[i] != null )
				mSeAudioSrc[i].mute = IsMute;
		}
	}
	// disable BGM 
	public void MuteBGM( bool IsMute  )
	{
		mBGMAudioSrc.mute = IsMute;
	}

	private void playSound( string p_SoundName , bool IsBGM , bool IsLoop )
	{
		if( mSoundNameMap.ContainsKey( p_SoundName	) == false )
		{
			CPDebug.LogError("no this p_SoundName = " + p_SoundName );
			return;
		}
		string SoundFileName = mSoundNameMap[p_SoundName];
		AudioClip PlayAudioClip;
		if( mSoundMusicObjMap.ContainsKey( SoundFileName ))
		{
			PlayAudioClip = mSoundMusicObjMap[SoundFileName];
		}else
		{
			//PlayAudioClip = AssetsBundleMgr.Instance.getAssetsPrefab("Sound/" +  SoundFileName) as AudioClip;
			// because Instantiate Audio Obj can not play , so directly Resources.load
			PlayAudioClip = Resources.Load( ResourcePrefixPath + SoundFileName) as AudioClip;
			ValidateHelper.Begin().NotNullObj(PlayAudioClip);

			// add to the cache
			if( mSoundMusicList.Count >= MaxAudioNum )
			{
				string TmpSoundTmpName = mSoundMusicList[0];
				mSoundMusicList.RemoveAt(0);
				DestroyImmediate( mSoundMusicObjMap[TmpSoundTmpName] );
				mSoundMusicObjMap.Remove( TmpSoundTmpName );
			}
			mSoundMusicObjMap[SoundFileName] = PlayAudioClip;
		}

		if( IsBGM == true )
		{
			mBGMAudioSrc.clip = PlayAudioClip;
			mBGMAudioSrc.loop = IsLoop;
			mBGMAudioSrc.Play();
		}else
		{
			// get the SE Sound Src
			mSeAudioSrc[mNowSeIdx].clip = PlayAudioClip;
			mSeAudioSrc[mNowSeIdx].loop = IsLoop;
			mSeAudioSrc[mNowSeIdx].Play();

			mNowSeIdx = ( mNowSeIdx + 1 ) % mSeAudioSrc.Count;
		}
	

	}

}
