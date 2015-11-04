using UnityEngine;
using System.Collections;

public class ManagerComponent<T> : MonoBehaviour where T : ManagerComponent<T>
{
	private static T m_instance = null;
	
	public static T Instance
	{
		get
		{
			if(m_instance == null)
			{
                GameObject[] tempGos = GameObject.FindGameObjectsWithTag("Manager");
                foreach (GameObject tempGo in tempGos)
                {
                    m_instance = tempGo.GetComponent<T>();
                    if (m_instance != null)
                    {
                        CPDebug.Log(m_instance.GetType() + " Instance Found");
                        break;
                    }
                }
			}
            if (m_instance == null)
            {
                CPDebug.LogWarning(typeof(T) + " Instance Not Found");
            }
			return m_instance;
		}
	}
	
	void OnDestroy()
	{
		m_instance = null;
	}

	public void OnApplicationQuit()
	{
		m_instance = null;
	}
}