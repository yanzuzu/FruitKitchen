using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LifeManager : ManagerComponent< LifeManager >  {

	/// <summary>
	/// The left play number.
	/// </summary>
	public int leftPlayNum = 0;
	/// <summary>
	/// The pause counter.
	/// </summary>
	public bool pauseCounter = false;
	/// <summary>
	/// The has change life number.
	/// </summary>
	public bool hasChangeLifeNum = false;
	/// <summary>
	/// The next recover time.
	/// </summary>
	public DateTime nextRecoverTime = DateTime.MinValue;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Inits the life manager.
	/// </summary>
	public void initLifeManager()
	{
		if (LocalDBManager.Instance.initLifeRecordTable ()) 
		{
			pauseCounter = true;
			nextRecoverTime = DateTime.MinValue;
		}

		handleLife ();
	}

	/// <summary>
	/// Increases the life.
	/// </summary>
	/// <param name="Num">Number.</param>
	public void increaseLife(int Num)
	{
		LocalDBManager.Instance.addLife (Num);
		leftPlayNum = LocalDBManager.Instance.GetLifeNum();
	}

	/// <summary>
	/// Uses the life.
	/// </summary>
	/// <returns><c>true</c>, if life was used, <c>false</c> otherwise.</returns>
	public bool useLife()
	{
		leftPlayNum = LocalDBManager.Instance.GetLifeNum();

		if(leftPlayNum <= 0)
		{
			return false;
		}

		if(LocalDBManager.Instance.decreaseOneLife())
		{
			if(pauseCounter &&
			   nextRecoverTime == DateTime.MinValue)
			{
				DateTime nowDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				nextRecoverTime = nowDate.AddSeconds (Config.RECOVER_TIME);

				string sTimeStr = nextRecoverTime.ToString("yyyy-MM-dd HH:mm:ss");
				LocalDBManager.Instance.updateNextrecoverTime(sTimeStr);

				pauseCounter = false;
			}

			leftPlayNum--;
			return true;
		}

		return false;
	}

	/// <summary>
	/// Handles the life.
	/// </summary>
	public void handleLife()
	{
		updateLifeNumByDB ();

		StartCoroutine(countLife());
	}

	IEnumerator countLife()
	{
		while(true)
		{
			if(!pauseCounter)
			{
				if(hasChangeLifeNum)
				{
					hasChangeLifeNum = false;
					leftPlayNum = LocalDBManager.Instance.GetLifeNum();
					
					if(leftPlayNum >= Config.MAX_PLAY_NUM)
					{
						pauseCounter = true;
						nextRecoverTime = DateTime.MinValue;
						yield return null;
					}
				}

				if(nextRecoverTime != DateTime.MinValue)
				{
					DateTime nowDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					
					TimeSpan ts = nextRecoverTime - nowDate;
					int seconds = (int)ts.TotalSeconds;
					
					if(seconds < 0)
					{
						seconds = Math.Abs(seconds);

						int recoverSecond = Config.RECOVER_TIME;					
						int recoverNum = (seconds / recoverSecond) + 1;
						
						increaseLife(recoverNum);
						leftPlayNum = LocalDBManager.Instance.GetLifeNum();
						
						if(leftPlayNum >= Config.MAX_PLAY_NUM)
						{
							pauseCounter = true;
							nextRecoverTime = DateTime.MinValue;
						}
						else
						{
							int passSecond = Config.RECOVER_TIME - (seconds % recoverSecond);
							nextRecoverTime = nowDate.AddSeconds (passSecond);
							
							string sTimeStr = nextRecoverTime.ToString("yyyy-MM-dd HH:mm:ss");
							LocalDBManager.Instance.updateNextrecoverTime(sTimeStr);
						}
					}
				}
			}

			yield return new WaitForSeconds(1.0f);
		}
	}

	/// <summary>
	/// Updates the life number by D.
	/// </summary>
	public void updateLifeNumByDB()
	{
		leftPlayNum = LocalDBManager.Instance.GetLifeNum();

		if(leftPlayNum >= Config.MAX_PLAY_NUM)
		{
			pauseCounter = true;
			nextRecoverTime = DateTime.MinValue;
			return;
		}

		DateTime nowDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

		string sNextRecoverTime = LocalDBManager.Instance.getNextrecoverTime ();

		if(sNextRecoverTime.Length > 0)
		{
			nextRecoverTime = Convert.ToDateTime(sNextRecoverTime);
		}

		TimeSpan ts = nextRecoverTime - nowDate;

		int seconds = (int)ts.TotalSeconds;

		if(seconds < 0)
		{
			seconds = Math.Abs(seconds);
			int recoverSecond = Config.RECOVER_TIME;
			
			int recoverNum = (seconds / recoverSecond) + 1;
			increaseLife(recoverNum);
			
			leftPlayNum = LocalDBManager.Instance.GetLifeNum();

			if(leftPlayNum >= Config.MAX_PLAY_NUM)
			{
				pauseCounter = true;
				nextRecoverTime = DateTime.MinValue;
			}
			else
			{
				int passSecond = Config.RECOVER_TIME - (seconds % recoverSecond);
				nextRecoverTime = nowDate.AddSeconds (passSecond);

				string sTimeStr = nextRecoverTime.ToString("yyyy-MM-dd HH:mm:ss");
				LocalDBManager.Instance.updateNextrecoverTime(sTimeStr);
			}
		}
		else
		{
			pauseCounter = false;
		}
	}

	/// <summary>
	/// Gets the next recover second.
	/// </summary>
	/// <returns>The next recover second.</returns>
	public int getNextRecoverSecond()
	{
		if(pauseCounter ||
		   nextRecoverTime == DateTime.MinValue)
		{
			return -1;
		}

		DateTime nowDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

		string sNextRecoverTime = LocalDBManager.Instance.getNextrecoverTime ();
		nextRecoverTime = Convert.ToDateTime(sNextRecoverTime);
		
		TimeSpan ts = nextRecoverTime - nowDate;
		
		int seconds = Math.Abs((int)ts.TotalSeconds);

		return seconds;
	}

	public string getNextRecoverSecondStr()
	{
		int leftTime = getNextRecoverSecond();
		return transSecondFormat (leftTime);
	}
	
	public string transSecondFormat(int Second)
	{
		if(Second <= 0)
		{
			return "00:00";
		}
		else if(Second >= 3600)
		{
			return "59:59";
		}

		int min = Second / 60;
		int second = Second % 60;

		string sMin = null;

		if(min < 10)
		{
			sMin = string.Format("0{0}", min);
		}
		else
		{
			sMin = min.ToString();
		}

		string sSecond = null;
		
		if(second < 10)
		{
			sSecond = string.Format("0{0}", second);
		}
		else
		{
			sSecond = second.ToString();
		}

		string sTime = string.Format ("{0}:{1}", sMin, sSecond);
		return sTime;
	}
}
