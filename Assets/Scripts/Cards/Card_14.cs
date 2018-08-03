using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_14 : Card 
{
	[SerializeField]
	private int damage = 6;
	[SerializeField]
	private float defendPower = -0.1f;
	//持续回合数
	[SerializeField]
	private int turn = 2;


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
					targetMap.DefendPower += defendPower;
					targetMap.all_buff.Add(new Buff(turn, 0, defendPower));
					GameManager.Instance.buffMap.Add(targetMap);
					playerOP.save_Steps.SaveCardSteps(targetMap, damage);
					foreach (Map m in targetMap.NextMaps)
					{
						if (m.PlayerID == targetMap.PlayerID)
						{
							playerOP.save_Steps.SaveCardSteps(m, (int)(damage / 2.0f + 0.5));
							m.DefendPower += defendPower;
							m.all_buff.Add(new Buff(turn, 0, defendPower));
							GameManager.Instance.buffMap.Add(m);
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
		damage = 9;
		leaderPoint = 5;
		upgrade = true;
	}
}
