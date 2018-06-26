using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Operation
{
	/*执行操作的玩家ID*/
	private int playerID;
	/*目前玩家处于的操作阶段*/
	private OperateState state;
	/*分配阶段的第一个地图块*/
	private GameObject firstMap;
	/*分配阶段的第二个地图块*/
	private GameObject secondMap;

    private void Start()
	{
		//对数据初始化
		state = OperateState.OP_END;
		firstMap = null;
		secondMap = null;
	}

	public void Operate(Player player, OperateState state)
	{
		this.state = state;
		//TODO:玩家所能进行的所有操作
		ClickMapBlock();
	}

	/*对鼠标点击事件进行处理*/
	private void ClickMapBlock()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("鼠标点击");
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			Physics.Raycast(ray, out hitInfo);
			if (hitInfo.collider != null)
			{
				Debug.Log("击中物体");
				//TODO:每个case需要做的事
				switch(state)
				{
					case OperateState.COMMAND_SOLDIER:
					{
						if (hitInfo.collider.tag == "Map")
						{
							CommandOperate(hitInfo.collider.gameObject);	
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

	private void CommandOperate(GameObject mapBlock)
	{
		if (firstMap == null)
		{
			firstMap = mapBlock;
			Debug.Log("选择起始地图块");
			return;
		}
		secondMap = mapBlock;
		Debug.Log("选择第二块地图块");
		Debug.Log("画了个箭头");
		firstMap = null;
		secondMap = null;
		//TODO:画箭头，计算第一地图块上的有效人数

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
