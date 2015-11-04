using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;
public class JsonParser  {
	public T ParserData<T>( string p_jsonStr )
	{
		return JsonReader.Deserialize<T>(p_jsonStr);
	}
}
