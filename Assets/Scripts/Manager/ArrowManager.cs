using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour 
{
	private static ArrowManager _instance;
	/*红箭头预制体*/
	public GameObject red_head_Prefab;
	/*红箭尾预制体*/
	public GameObject red_tail_Prefab;
	/*绿箭头预制体*/
	public GameObject green_head_Prefab;
	/*绿箭尾预制体*/
	public GameObject green_tail_Prefab;

	private void Awake()
	{
		_instance = this;
	}

	public ArrowManager Instance 
	{
		get
		{
			return _instance;
		}
	}

	public void InitArrow(Vector3 start, Vector3 end, int color)
	{
		Vector3 dir;   //箭头方向
		Vector3 pos;   //箭头的位置
	}
}
