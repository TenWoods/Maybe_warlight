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

	public override void CardEffect()
	{
		Debug.Log("Check");
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hitInfo = Physics2D.Raycast(mousPos, Vector2.zero);
			if (hitInfo.collider != null)
			{
				if (hitInfo.collider.tag == "Map")
				{
					Map targetMap = hitInfo.collider.GetComponent<Map>();
					if (targetMap.PlayerID != playerID)
					{
						Debug.Log("对象错误(卡牌)");
						return;
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
					hasUsed = true;
				}
			}
		}
	}

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
