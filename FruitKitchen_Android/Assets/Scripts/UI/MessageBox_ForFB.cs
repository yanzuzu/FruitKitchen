using UnityEngine;
using System.Collections;

public class MessageBox_ForFB : MonoBehaviour {

	public bool isAverageScore = true;
	public UILabel textLabel;
	public UILabel moveLabel;
	public UILabel bombLabel;
	public UILabel cancelBtnLabel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Setup( params object [] p_param )
	{
		isAverageScore = (bool)p_param [0];

		textLabel.text = Localization.Localize ("82");
		moveLabel.text = Localization.Localize ("63");
		bombLabel.text = Localization.Localize ("62");
		cancelBtnLabel.text = Localization.Localize ("47");
	}

	public void OnMoveBtnClick()
	{
		if(isAverageScore)
		{
			int averageScore = LocalDBManager.Instance.getAverageMoveScore(DataManager.Instance.BigStageNum);

			string Msg = Localization.Localize("83");
			string text = string.Format(Msg, DataManager.Instance.BigStageNum, averageScore);

			FaceBookManager.Instance.postTextOnMyFB (text);
		}
		else
		{
			int nowBigStage = DataManager.Instance.BigStageNum;
			int nowSmallStage = DataManager.Instance.SmallStageNum;
			
			int maxScore = LocalDBManager.Instance.getBestMoveScore(nowBigStage, nowSmallStage);
			
			if(nowBigStage == 1)
			{
				nowSmallStage -= 3;
			}
			
			string stage = string.Format("{0}-{1}", nowBigStage, nowSmallStage);
			string Msg = Localization.Localize("85");
			string text = string.Format(Msg, stage, maxScore);
			
			FaceBookManager.Instance.postTextOnMyFB (text);
		}

		OnCancelBtnClick();
	}

	public void OnBombBtnClick()
	{
		if(isAverageScore)
		{
			int averageScore = LocalDBManager.Instance.getAverageBombScore(DataManager.Instance.BigStageNum);
			
			string Msg = Localization.Localize("84");
			string text = string.Format(Msg, DataManager.Instance.BigStageNum, averageScore);
			
			FaceBookManager.Instance.postTextOnMyFB (text);
		}
		else
		{
			int nowBigStage = DataManager.Instance.BigStageNum;
			int nowSmallStage = DataManager.Instance.SmallStageNum;

			int maxScore = LocalDBManager.Instance.getBestBombScore(nowBigStage, nowSmallStage);
			
			if(nowBigStage == 1)
			{
				nowSmallStage -= 3;
			}
			
			string stage = string.Format("{0}-{1}", nowBigStage, nowSmallStage);
			string Msg = Localization.Localize("86");
			string text = string.Format(Msg, stage, maxScore);
			
			FaceBookManager.Instance.postTextOnMyFB (text);
		}

		OnCancelBtnClick();
	}
	
	public void OnCancelBtnClick()
	{
		PopupManager.Instance.CloseCurrentPopup();
	}
}
