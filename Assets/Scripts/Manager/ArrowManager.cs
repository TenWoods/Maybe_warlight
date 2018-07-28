using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour 
{
	private static ArrowManager _instance;
	/*红箭头预制体*/
	public GameObject red_Prefab;
	/*绿箭头预制体*/
	public GameObject green_Prefab;
	/*场景中显示的箭头*/
	private List<GameObject> arrows_Able;
	/*场景中保留的箭头*/
	private List<GameObject> arrows_Remain;

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		arrows_Able = new List<GameObject>();
		arrows_Remain = new List<GameObject>();
	}

	public static ArrowManager Instance 
	{
		get
		{
			return _instance;
		}
	}

	public GameObject InitArrow(Vector3 start, Vector3 end, int color)
	{
		GameObject arrow = null;
		Vector3 dir;   //箭头方向
		Vector3 pos;   //箭头的位置
		float angle;   //旋转角度
		dir = end - start;
		angle = Vector3.Angle(Vector3.left, dir);
		//修正
		if (Vector3.Cross(Vector3.left, dir).z < 0)
		{
			angle *= -1;
		}
		pos = (end + start) / 2;
		//箭头颜色
		switch(color)
		{
			case 0 : arrow = GameObject.Instantiate(red_Prefab, pos, Quaternion.Euler(0, 0, angle));
					 break;
			case 1 : arrow = GameObject.Instantiate(green_Prefab, pos, Quaternion.Euler(0, 0, angle));
					 break;
		}
		if (arrow != null)
		{
			//控制缩放
			arrow.transform.localScale = new Vector3(dir.magnitude / 2, 1, 1);
		}
		return arrow;
	}

	/// <summary>
	/// 隐藏显示的箭头
	/// </summary>
	public void DisabledArrows()
	{
		int i;
		for (i = 0; i < arrows_Able.Count; i++)
		{
			if (arrows_Remain.Contains(arrows_Able[i]))
			{
				continue;
			}
			arrows_Able[i].SetActive(false);
		}
	}

	public void CleanRemainArrow()
	{
		foreach (GameObject a in arrows_Remain)
		{
			a.SetActive(false);
		}
		arrows_Remain.Clear();
	}

	public List<GameObject> Arrows_Able 
	{
		get
		{
			return arrows_Able;
		}
	}

	public List<GameObject> Arrows_Remain 
	{
		get
		{
			return arrows_Remain;
		}
	}
}
