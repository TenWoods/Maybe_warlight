﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_27 : Card 
{

	[SerializeField]
	private int condition = 4;
	[SerializeField]
	private int id = -1;

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
					if (targetMap.PlayerID == playerID)
					{
						Debug.Log("对象错误(卡牌)");
						return;
					}
					playerOP.save_Steps.SaveCardSteps(targetMap, condition, id);
					if (upgrade)
					{
						foreach(Map m in targetMap.NextMaps)
						{
							if (m.PlayerID != playerID)
							{
								playerOP.save_Steps.SaveCardSteps(m, condition, id);
							}
						}
					}
					playerOP.LeaderPoint -= leaderPoint;
					playerOP.UpdateLeaderPointUI();
					hasUsed = true;
				}
			}
		}
	}

	public override void UpGrade()
	{
		leaderPoint = 5;
		upgrade = true;
	}
}
