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
	[SerializeField]
	private int playerID = 0;
	/*玩家当前操作*/
	[SerializeField]//调试用
	private OperateState opState;
	/*玩家当前操作结束*/
	[SerializeField]//调试用
	private bool operateEnd = true;
	/*玩家操作类*/
	private Operation selfOperate;
	/*玩家统帅值(每回合实时更新)*/
	[SerializeField] //设定初始值
	private int leaderPoint;
	/*手牌最大的数量*/
	[SerializeField]
	private int cards_Num_Max = 0;
	/*手中的卡牌,-1为空*/
	private List<int> cards_in_hand;
	/*牌库*/
	private int[] cards = {0};
	/*抽牌指针在牌库位置*/
	private int cards_index = 0;
	/*每回合开始是否为操作类更新数据*/
	private bool hasUpdated = false;

	private void Start() 
	{
		maps = new List<Map>();
		cards_in_hand = new List<int>();
		opState = OperateState.OP_END;
		selfOperate = new Operation(this);
	}

	private void Update() 
	{
		if (!operateEnd)
		{
			if (!hasUpdated)
			{
				selfOperate.UpdateData(this);
				hasUpdated = true;
			}
			selfOperate.Operate(this, opState);
		}
	}

	/// <summary>
	/// 改变玩家操作状态
	/// </summary>
	/// <param name="op">操作状态枚举变量</param>
	public void ChangerOperateState(OperateState op)
	{
		//TODO:改变玩家操作状态
	}

	/// <summary>
	/// 玩家抽卡(由GameManager调用)
	/// </summary>
	public void GetCard()
	{
		if (cards_in_hand.Count >= cards_Num_Max)
		{
			//TODO:抽完牌后弃牌
			return;
		}
		cards_in_hand.Add(cards[cards_index]);
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

	public OperateState OpState 
	{
		get
		{
			return opState;
		}
		set
		{
			opState = value;
		}
	}

	public int LeaderPoint
	{
		get
		{
			return leaderPoint;
		}
	}
}
