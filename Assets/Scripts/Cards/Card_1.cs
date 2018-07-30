using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 援护
/// </summary>
public class Card_1 : Card 
{
	//增援士兵数
	[SerializeField]//调试用
	private int addSoldierNum = 10;

	public override bool CardEffect(int playerID, GameObject target)
	{
		Map targetMap = target.GetComponent<Map>();
		if (targetMap.PlayerID != playerID)
		{
			Debug.Log("对象错误(卡牌)");
			return false;
		}
		if (!upgrade)
		{
			targetMap.BaseSoldierNum += addSoldierNum;
		}
		else
		{
			targetMap.BaseSoldierNum += addSoldierNum;
		}
		targetMap.UpdateMapUI();
		GameManager.Instance.Players[playerID].LeaderPoint -= leaderPoint;
		return true;
	}

	public override void UpGrade()
	{
		addSoldierNum = 20;
		leaderPoint = 4;
	}
}
