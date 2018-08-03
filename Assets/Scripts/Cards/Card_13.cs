using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_13 : Card 
{
	[SerializeField]
	private int damage = 3;

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
					playerOP.save_Steps.SaveCardSteps(targetMap, playerOP.LeaderPoint * damage);
					playerOP.LeaderPoint = 0;
					playerOP.UpdateLeaderPointUI();
					hasUsed = true;
				}
			}
		}
	}

	public override void UpGrade()
	{
		damage = 4;
		upgrade = true;
	}
}
