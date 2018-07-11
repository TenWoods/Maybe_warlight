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
			addNums[addMaps.IndexOf(map)] += addNum;
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
				index = startMap.MoveDirMap.IndexOf(endMap);
				startMap.BaseSoldierNum += startMap.MoveSoldierNum[index];
				startMap.MoveSoldierNum[index] = moveNum;
				startMap.BaseSoldierNum -= moveNum;
				return;
			}
			startMap.MoveDirMap.Add(endMap);
			startMap.MoveSoldierNum.Add(moveNum);
			startMap.BaseSoldierNum -= moveNum;
			return;
		}
		commandMaps.Add(startMap);
		arrows.Add(arrow);
		startMap.MoveDirMap.Add(endMap);
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
