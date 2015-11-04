using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FaceBookData
{
	public string id = string.Empty;
	public string name = string.Empty;
	public bool installed = false;
	public FaceBookPicData picture;
}

public class FaceBookPicData
{
	public FaceBookPicDetail data;
}

public class FaceBookPicDetail
{

	public bool is_silhouette;
	public string url = string.Empty;
}


public class FaceBookFriendData
{
	public List<FaceBookData> data;
}




