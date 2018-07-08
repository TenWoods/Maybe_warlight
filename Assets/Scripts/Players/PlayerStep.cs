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
	/*卡牌使用集合*/
	private List<Map> cardMaps;
	/*读取步骤指针*/
	private int step_Pointer = 0;

	public PlayerStep()
	{
		addMaps = new List<Map>();
		addNums = new List<int>();
		commandMaps = new List<Map>();
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
	public void SaveCommamdSteps(Map startMap, Map endMap, int moveNum)
	{
		int index = 0;
		if (commandMaps.Contains(startMap))
		{
			if (startMap.MoveDirMap.Contains(endMap))
			{
				index = startMap.MoveDirMap.IndexOf(endMap);
				startMap.MoveSoldierNum[index] = moveNum;
				return;
			}
			startMap.MoveDirMap.Add(endMap);
			startMap.MoveSoldierNum.Add(moveNum);
			return;
		}
		commandMaps.Add(startMap);
		startMap.MoveDirMap.Add(endMap);
		startMap.MoveSoldierNum.Add(moveNum);
	}

	public void SaveCardSteps(Map map)
	{
		if (cardMaps.Contains(map))
		{
			return;
		}
		cardMaps.Add(map);
	}

	/// <summary>
	/// 读取储存的步骤并播放效果
	/// </summary>
	public void LoadSteps()
	{
		//TODO:还没想好怎么做
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

}
