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
					targetMap.BaseSoldierNum += addSoldierNum;
					targetMap.UpdateMapUI();
					playerOP.LeaderPoint -= leaderPoint;
					playerOP.UpdateLeaderPointUI();
					hasUsed = true;
				}
			}
		}
	}

	public override void UpGrade()
	{
		addSoldierNum = 20;
		leaderPoint = 4;
		upgrade = true;
	}
}
