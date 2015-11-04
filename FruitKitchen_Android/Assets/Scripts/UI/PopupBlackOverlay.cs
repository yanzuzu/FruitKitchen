using UnityEngine;
using System.Collections;

public class PopupBlackOverlay : MonoBehaviour {

    UISprite m_sprite;
    void Awake() {
        m_sprite = GetComponent<UISprite>();
    }
    public void Show() {
        gameObject.SetActive(true);
		if( m_sprite != null )
        	m_sprite.color = new Color(0, 0, 0, 0.8f);
    }
    public void Hide() {
        gameObject.SetActive(false);
		if( m_sprite != null )
        	m_sprite.color = new Color(0, 0, 0, 0f);
    }
    public void OnOverlayClick() {
        //PopupManager.Instance.CloseCurrentPopup();        
    }
}
