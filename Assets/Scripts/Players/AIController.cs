using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Operator
{
	private void Start() 
	{
		base.Start();
	}

	private void Update() 
	{
		if (!gameStart)
		{
			return;
		}
		
	}

	private void Operation()
	{
		switch (opState)
		{
			case OperateState.OP_START : break;
			case OperateState.ADD_SOLDIER :
			{
				break;
			}
			case OperateState.COMMAND_SOLDIER :
			{
				break;
			}
			case OperateState.USE_CARDS : 
			{
				break;
			}
			case OperateState.OP_END : break;
		}
	}

	/// <summary>
	/// 获取增兵目标地图
	/// </summary>
	/// <returns>增兵目标地图</returns>
	private List<Map> GetAddMap() 
	{
		List<Map> addMaps = new List<Map>();
		return null;
	}

	/// <summary>
	/// 获取进攻目标地图
	/// </summary>
	/// <returns>进攻目标地图</returns>
	private List<Map> GetTargetMap()
	{
		List<Map> targetMaps = new List<Map>();
		//TODO
		return null;
	}
}
