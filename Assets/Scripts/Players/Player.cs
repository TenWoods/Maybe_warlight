using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Operator 
{
	/*卡牌生成的位置*/
	public Transform CardSpawnPoint;
	private void Start() 
	{
		maps = new List<Map>();
		cards_in_hand = new List<int>();
		steps = new PlayerStep();
		opState = OperateState.OP_START;
		selfOperate = new Operation(this, steps);
	}

	private void Update() 
	{
		if (!gameStart)
		{
			return;
		}
		if (!operateEnd)
		{
			if (!hasUpdated)
			{
				selfOperate.UpdateData(this);//可能是更新数据
				hasUpdated = true;
			}
			selfOperate.Operate(this, opState);//得看Operation脚本
		}
	}

	#region 玩家操作状态改变
	
	/// <summary>
	/// 变为开始状态
	/// </summary>
	public void ChangeOperateStateStart()
	{
		opState = OperateState.OP_START;
	}

	/// <summary>
	/// 变为增兵状态
	/// </summary>
	public void ChangeOperateStateAdd()
	{
		opState = OperateState.ADD_SOLDIER;
		if (opState == OperateState.COMMAND_SOLDIER)
		{
			DisabledArrows();
			commandUI.SetActive(false);
		}
	}

	/// <summary>
	/// 变为指挥状态
	/// </summary>
	public void ChangeOperateStateCommand()
	{
		opState = OperateState.COMMAND_SOLDIER;
	}

	/// <summary>
	/// 变为卡片使用状态
	/// </summary>
	public void ChangeOperateStateCard()
	{
		opState = OperateState.USE_CARDS;
		if (opState == OperateState.COMMAND_SOLDIER)
		{
			DisabledArrows();
			commandUI.SetActive(false);
		}
	}
	
	/// <summary>
	/// 变为回合结束
	/// </summary>
	public void ChangeOperateStateEnd()
	{
		opState = OperateState.OP_END;
		if (opState == OperateState.COMMAND_SOLDIER)
		{
			DisabledArrows();
			commandUI.SetActive(false);
		}
	}

	#endregion

	/// <summary>
	/// 新的回合开始更新统帅值
	/// </summary>
	public void UpdateLeaderPoint()
	{
<<<<<<< HEAD
		if (cards_in_hand.Count >= cards_Num_Max)
		{
			//TODO:抽完牌后弃牌
			return;
		}
		cards_in_hand.Add(cards[cards_index]);//add里面加的是什么类型的
=======
		soldierNum = maps.Count;
		hasUpdated = false;
>>>>>>> 5fbded97ccda4bfc261f31dfd925208ab53a024c
	}

	/// <summary>
	/// 清除上个回合的步骤
	/// </summary>
	public void CleanSteps()
	{
		steps.CleanSteps();
	}

	/// <summary>
	/// 玩家抽卡(由GameManager调用)
	/// </summary>
	public void GetCard(int num)
	{
		//初始化数组
		if (cards_in_hand == null)
		{
			cards_in_hand = new List<int>();
		}
		//判断是否抽完牌
		if ((cards_index + 1) > cards.Length)
		{
			Debug.Log("牌库空了");
			return;
		}
		if (cards_in_hand.Count == cards_Num_Max)
		{
			cards_index++;
			return;
		}
		//根据抽牌次数抽牌
		for (int i = 0; i < num; i++)
		{
			cards_in_hand.Add(cards[cards_index]);
			cards_index++;
			Debug.Log("抽牌");
			Instantiate(Resources.Load("Card"), transform.position, transform.rotation);
		}
	}

	/// <summary>
	/// 让操作类隐藏箭头
	/// </summary>
	public void DisabledArrows()
	{
		selfOperate.DisabledArrows();
	}
}
