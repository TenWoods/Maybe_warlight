using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*操作进行的阶段(枚举)定义*/
public enum OperateState{ADD_SOLDIER, COMMAND_SOLDIER, USE_CARDS, OP_END};
public class Player : MonoBehaviour 
{
	/*玩家所拥有的地图格*/
	private List<Map> maps;
	/*玩家在GameManager处的索引*/
	private int playerID = 0;
	/*玩家当前操作*/
	[SerializeField]//调试用
	private OperateState opState;
	/*玩家当前操作结束*/
	[SerializeField]//调试用
	private bool operateEnd = true;
	/*玩家操作类*/
	private Operation selfOperate;

	private void Start() 
	{
		maps = new List<Map>();
		selfOperate = new Operation();
		opState = OperateState.OP_END;
	}

	private void Update() 
	{
		if (!operateEnd)
		{
			selfOperate.Operate(this, opState);
		}
	}

	public List<Map> Maps 
	{
		get
		{
			return maps;
		}
	}

	public bool OperateEnd 
	{
		set 
		{
			operateEnd = value;
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
}
