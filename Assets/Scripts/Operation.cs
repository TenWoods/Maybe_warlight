using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Operation
{
	/*操作进行的阶段(枚举)定义*/
	public enum OperateState{ADD_SOLDIER, COMMAND_SOLDIER, USE_CARDS, OP_END};
	/*执行操作的玩家ID*/
	private int playerID;
	/*目前玩家处于的操作阶段*/
	private OperateState state;
	/*分配阶段的第一个地图块*/
	private Map firstMap;
	/*分配阶段的第二个地图块*/
	private Map secondMap;

    private void Start()
	{
		//对数据初始化
		state = OperateState.OP_END;
		firstMap = null;
		secondMap = null;
	}

	public void Operate(Player player)
	{
		//TODO:玩家所能进行的所有操作
		ClickMapBlock();
	}

	private void ClickMapBlock()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			Physics.Raycast(ray, out hitInfo);
			if (hitInfo.collider != null)
			{
				//TODO:每个case需要做的事
				switch(state)
				{
					case OperateState.COMMAND_SOLDIER:
					{
						if (hitInfo.collider.tag == "Map")
						{
							
						}
						break;
					}
					case OperateState.ADD_SOLDIER: break;
					case OperateState.OP_END:break;
					case OperateState.USE_CARDS:break;
					default: break;
				}
			}
		}
	}

	public int PlayerID
	{
		get
		{
			return playerID;
		}
		set
		{
			playerID = value;
		}
	}

	public OperateState State
	{
		get
		{
			return state;
		}
		set
		{
			state = value;
		}
	}
}
