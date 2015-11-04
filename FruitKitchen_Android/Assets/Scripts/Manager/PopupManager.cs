using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void PopupActionCallback(params object [] p_param);
public partial class PopupManager : ManagerComponent<PopupManager> {
    [SerializeField]
    private Camera m_camera;
    [SerializeField]
    private UIPanel m_popupUiPanel;
    [SerializeField]
    private PopupBlackOverlay m_blackOverlay;
    [SerializeField]
    private Vector3 m_showupPunchScale = new Vector3(0.6f, 0.6f, 0.6f);
    [SerializeField]
    private float m_showupPeriod = 0.6f;
	[SerializeField]
	private GameObject m_loadingImg;

    public List<UICamera> m_otherUiCameras = new List<UICamera>();

    private List<GameObject> m_popups = new List<GameObject>();
    private GameObject m_currentPopup;
    public GameObject m_CurrentPopup {
        get { return m_currentPopup; }
    }
    public bool IsPopupShowing {
        get { return null != m_currentPopup; }
    }

    private void Awake() {
        m_camera.orthographicSize = ((float)(Screen.height) / (float)(Screen.width)) / (1136f / 640f);
    }
	#region PopUI
	public void ShowGameStarOptionUI(StartOptionPopUI.OptionUIType p_UIType , GamePlayLayer playLayer = null)
	{
		m_blackOverlay.Show();
		GameObject TmpObj = CreatePopupGo("popui/StartOptionPopUI");
		StartOptionPopUI TmpGo = TmpObj.GetComponent< StartOptionPopUI >();
		TmpGo.Setup(p_UIType, playLayer);
	}

	public void ShowTutorialUI()
	{
		m_blackOverlay.Show();
		/*GameObject TmpObj = */CreatePopupGo("popui/TurtorialUI");
	}

	public void ShowStoreShopUI()
	{
		m_blackOverlay.Show();
		GameObject TmpObj = CreatePopupGo("popui/StoreShopUI");
		TmpObj.GetComponent< StoreShopUI >().Setup();
	}

	public void ShowMailUI()
	{
		m_blackOverlay.Show();
		GameObject TmpObj = CreatePopupGo("popui/MailUI");
		TmpObj.GetComponent< MailUI >().Setup();
	}

	public void ShowChallengeUI()
	{
		m_blackOverlay.Show();
		GameObject TmpObj = CreatePopupGo("popui/ChallengeUI");
		TmpObj.GetComponent< ChallengeUI >().Setup();

	}

	public void ShowGiftUI()
	{
		m_blackOverlay.Show();
		GameObject TmpObj = CreatePopupGo("popui/GiftUI");
		TmpObj.GetComponent< GiftUI >().Setup();
	}

	public void ShowMessageBox(int MessageType)
	{
		m_blackOverlay.Show();
		GameObject TmpObj = CreatePopupGo("popui/MessageBox");

		TmpObj.GetComponent< MessageBox >().Setup(MessageType);
	}

	public void ShowMessageBox_Tip(string mainText, string okText)
	{
		m_blackOverlay.Show();
		GameObject TmpObj = CreatePopupGo("popui/MessageBox_Tip");
		
		TmpObj.GetComponent< MessageBox_Tip >().Setup(mainText, okText);
	}

	public void ShowMessageBox_ForFB(bool IsAverageScore)
	{
		m_blackOverlay.Show();
		GameObject TmpObj = CreatePopupGo("popui/MessageBox_ForFB");

		TmpObj.GetComponent< MessageBox_ForFB >().Setup(IsAverageScore);
	}

	#endregion

	public void VisibleLoading( bool p_visible )
	{
		m_loadingImg.SetActive( p_visible );
	}

    public void CloseCurrentPopup() {
        if (null != m_currentPopup) {
            Destroy(m_currentPopup.gameObject);
            m_currentPopup = null;
			m_blackOverlay.Hide();
        }
        if (null != m_popups && m_popups.Count > 0) {
            if (null != m_popups[0]) {
                m_currentPopup = m_popups[0];
                m_currentPopup.transform.localPosition = Vector3.zero;
                m_popups.RemoveAt(0);
				m_blackOverlay.Show();
            }
        }
       

        //EnableOtherUiCameras(true);
    }

    #region UTILS
    public Camera GetPopupCamera() {
        return m_camera;
    }
    public void EnableOtherUiCameras(bool p_enable) {
        if (null != m_otherUiCameras) {
            foreach (UICamera tempUiCam in m_otherUiCameras) {
                tempUiCam.enabled = p_enable;
            }
        }
    }
    private GameObject CreatePopupGo(string p_popupPrefabLocation) {
        UnityEngine.Object tempObj = Resources.Load(p_popupPrefabLocation);
        GameObject tempGo = GameObject.Instantiate(tempObj, Vector3.zero, Quaternion.identity) as GameObject;
        tempGo.transform.parent = m_popupUiPanel.transform;
        tempGo.transform.localScale = Vector3.one;
        m_popups.Add(tempGo);

        //if current popup is showing, hide upcoming popup.
        if (null != m_currentPopup) {
            //hide it
            tempGo.transform.localPosition = new Vector3(10000, 0, 0);
        }
        else {
            if (null != m_popups[0]) {
                m_currentPopup = m_popups[0];
                m_popups.RemoveAt(0);
            }
            tempGo.transform.localPosition = Vector3.zero;
            //iTween.PunchScale(tempGo, m_showupPunchScale, m_showupPeriod);
        }
        return tempGo;
    }
    #endregion
}
