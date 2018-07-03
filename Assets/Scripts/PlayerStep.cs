using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStep
{
	/*操作步骤集合*/
	private List<int> operations = new List<int>();
	/*操作地图块集合*/
	private List<Map> changedMaps = new List<Map>();
	/*操作数据集合*/
	private List<int> operations_Data = new List<int>();
	/*读取步骤指针*/
	private int step_Pointer = 0;


	/// <summary>
	/// 储存玩家的行动步骤
	/// </summary>
	public void SaveSteps(int operate_ID, Map map, int changeData)
	{
		operations.Add(operate_ID);
		changedMaps.Add(map);
		operations_Data.Add(changeData);
	} 

	/// <summary>
	/// 读取储存的步骤并播放效果
	/// </summary>
	public void LoadSteps()
	{
		//TODO:还没想好怎么做
	}
	
}
