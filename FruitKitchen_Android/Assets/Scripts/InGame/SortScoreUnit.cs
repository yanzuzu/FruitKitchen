using UnityEngine;
using System.Collections;

public class SortScoreUnit : MonoBehaviour {
	[SerializeField]
	private UITexture m_img;
	[SerializeField]
	private UILabel m_score;

	public void Setup(string url , int score)
	{
		FaceBookManager.Instance.SetFbImg( m_img , url );
		m_score.text = score.ToString();
	}
}
