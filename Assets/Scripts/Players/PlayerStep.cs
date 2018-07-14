using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStep
{
	/*增兵地图块集合*/
	private List<Map> addMaps;
	/*增兵数量集合*/
	private List<int> addNums;
	/*指挥地图块集合*/
	private List<Map> commandMaps;
	/*指挥所用的箭头*/
	private List<GameObject> arrows;
	/*卡牌使用集合*/
	private List<Map> cardMaps;

	public PlayerStep()
	{
		addMaps = new List<Map>();
		addNums = new List<int>();
		commandMaps = new List<Map>();
		arrows = new List<GameObject>();
		cardMaps = new List<Map>();
	}


	/// <summary>
	/// 储存玩家增兵行动步骤
	/// </summary>
	public void SaveAddSteps(int addNum, Map map)
	{
		if (addMaps.Contains(map))
		{
			addNums[addMaps.IndexOf(map)] += addNum;//indexof有什么用，这一行什么意思
		}
		else
		{
			addMaps.Add(map);
			addNums.Add(addNum);
		}
	} 

	/// <summary>
	/// 储存玩家指挥操作
	/// </summary>
	public void SaveCommamdSteps(Map startMap, Map endMap, int moveNum, GameObject arrow)
	{
		int index = 0;
		if (commandMaps.Contains(startMap))
		{
			if (startMap.MoveDirMap.Contains(endMap))
			{
<<<<<<< HEAD
				index = startMap.MoveDirMap.IndexOf(endMap);//后面直接点动态数组数组名
=======
				index = startMap.MoveDirMap.IndexOf(endMap);
				startMap.BaseSoldierNum += startMap.MoveSoldierNum[index];
>>>>>>> 5fbded97ccda4bfc261f31dfd925208ab53a024c
				startMap.MoveSoldierNum[index] = moveNum;
				startMap.BaseSoldierNum -= moveNum;
				return;
			}
			startMap.MoveDirMap.Add(endMap);
			arrows.Add(arrow); //保存留在地图上的箭头
			arrow.GetComponent<BoxCollider2D>().enabled = false; //设置为不可点击
			startMap.MoveSoldierNum.Add(moveNum);
			startMap.BaseSoldierNum -= moveNum;
			return;
		}
		commandMaps.Add(startMap);
		startMap.MoveDirMap.Add(endMap);
		arrows.Add(arrow); //保存留在地图上的箭头
		arrow.GetComponent<BoxCollider2D>().enabled = false; //设置为不可点击
		startMap.MoveSoldierNum.Add(moveNum);
		startMap.BaseSoldierNum -= moveNum;
	}

	/// <summary>
	/// 储存卡牌操作阶段
	/// </summary>
	/// <param name="map">作用的卡牌</param>
	public void SaveCardSteps(Map map)
	{
		if (cardMaps.Contains(map))
		{
			return;
		}
		cardMaps.Add(map);
	}

	public void CleanSteps()
	{
		addMaps.Clear();
		addNums.Clear();
		commandMaps.Clear();
		cardMaps.Clear();
		foreach(GameObject arrow in arrows)
		{
			arrow.SetActive(false);
			arrow.GetComponent<BoxCollider2D>().enabled = true;
		}
		arrows.Clear();
	}

	public List<Map> AddMaps 
	{
		get
		{
			return addMaps;
		}
	}

	public List<int> AddNums 
	{
		get
		{
			return addNums;
		}
	}

	public List<Map> CommandMaps
	{
		get
		{
			return commandMaps;
		}
	}

	public List<GameObject> Arrows 
	{
		get
		{
			return arrows;
		}
	}
}
