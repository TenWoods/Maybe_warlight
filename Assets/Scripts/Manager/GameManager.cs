using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	/*游戏玩家(目前只有一个玩家)*/
	[SerializeField]
	private Player[] players;
	/*地图管理者数量*/
	[SerializeField]
	private int mapManagerNum;
	[SerializeField]
	/*地图管理者*/
	private MapManager[] mapManagers;
	/*游戏是否开始*/
	[SerializeField]
	private bool gameStart = false;
	/*是否开始结算*/
	[SerializeField]
	private bool startCaculate = false;
	/*所有玩家的行为步骤*/
	private PlayerStep[] all_Steps; 
	
	private void Start() 
	{
		mapManagerNum = mapManagers.Length;
		all_Steps = new PlayerStep[players.Length];
		InitMapBlocks();
		InitPlayers();
	}

	private void Update()
	{
		if (!gameStart)
		{
			return;
		}
		CheckCaculation();
		if (!startCaculate)
		{
			return;
		}
		Caculation();
	}

	/// <summary>
	/// 初始化所有地图方块
	/// </summary>
	private void InitMapBlocks()
	{
		for (int i = 0; i < mapManagerNum; i++)
		{
			mapManagers[i].InitBlocksData(this);
		}
	}

	/// <summary>
	/// 初始化所有玩家
	/// </summary>
	private void InitPlayers()
	{
		for (int i = 0; i < players.Length; i++)
		{
			players[i].PlayerID = i;
			players[i].GetCard(players[i].GetCardNum);
			//TODO:生成玩家之后对玩家进行初始化
		}
	}

	/// <summary>
	/// 检测是否开始结算
	/// </summary>
	private void CheckCaculation()
	{
		foreach(Player p in players)
		{
			if (p.OpState != OperateState.OP_END)
			{
				startCaculate = false;
				return;
			}
		}
		startCaculate = true;
	}

	/// <summary>
	/// 回合结束开始结算
	/// </summary>
	private void Caculation()
	{
		int i;
		//还原地图状态
		foreach(Player p in players)
		{
			for (i = 0; i < p.Steps.AddMaps.Count; i++)
			{
				p.Steps.AddMaps[i].BaseSoldierNum -= p.Steps.AddNums[i];
				p.Steps.AddMaps[i].UpadteMapUI();
			}
			for (i = 0; i < p.Steps.CommandMaps.Count; i++)
			{
				foreach (int n in p.Steps.CommandMaps[i].MoveSoldierNum)
				{
					p.Steps.CommandMaps[i].BaseSoldierNum += n;
				}
			}
		}
		//增兵过程
		foreach(Player p in players)
		{
			for (i = 0; i < p.Steps.AddMaps.Count; i++)
			{
				p.Steps.AddMaps[i].BaseSoldierNum += p.Steps.AddNums[i];
				p.Steps.AddMaps[i].UpadteMapUI();
			}
		}
		//指挥过程
		foreach(Player p in players)
		{
			foreach(Map m in p.Steps.CommandMaps)
			{
				AttackCaculation(m);
			}
		}
		//还原操作状态
		foreach(Player p in players)
		{
			p.CleanSteps();
			p.ChangeOperateStateStart();
			p.UpdateLeaderPoint();
		}
		startCaculate = false;
	}

	/// <summary>
	/// 攻击计算
	/// </summary>
	/// <param name="startMap">攻击方地图块</param>
	private void AttackCaculation(Map startMap)
	{
		List<Map> targetMaps = startMap.MoveDirMap;
		List<int> moveNums = startMap.MoveSoldierNum;
		float result;
		Debug.Log(targetMaps.Count);
		for(int i = 0; i < targetMaps.Count; i++)
		{
			if (startMap.BaseSoldierNum < moveNums[i])
			{
				//如果地图块上的人数不足，调增出兵人数
				moveNums[i] = startMap.BaseSoldierNum - 1;
			}
			if (startMap.PlayerID == targetMaps[i].PlayerID)
			{
				targetMaps[i].BaseSoldierNum += moveNums[i];
			}
			if (targetMaps[i].Terrain != Terrain.DESERT)
			{
				startMap.DefendPower += 0.1f;
			}
			//攻击力根据地形改变
			//高度:高地>平地=森林=荒漠>谷地
			switch (startMap.Terrain)
			{
				case Terrain.HIGHLAND :
				{
					if (targetMaps[i].Terrain != Terrain.HIGHLAND)
					{
						startMap.AttackPower += 0.2f;
					}
					break;
				}
				case Terrain.FOREST :
				{
					if (targetMaps[i].Terrain == Terrain.VALLY)
					{
						startMap.AttackPower += 0.2f;
					}
					else if (targetMaps[i].Terrain == Terrain.HIGHLAND)
					{
						startMap.AttackPower -= 0.2f;
					}
					startMap.AttackPower += 0.1f; 
					break;
				} 
				case Terrain.DESERT : 
				{
					if (targetMaps[i].Terrain == Terrain.VALLY)
					{
						startMap.AttackPower += 0.2f;
					}
					else if (targetMaps[i].Terrain == Terrain.HIGHLAND)
					{
						startMap.AttackPower -= 0.2f;
					}
					startMap.AttackPower -= 0.1f; 
					break;
				}
				case Terrain.FLATLAND :
				{
					if (targetMaps[i].Terrain == Terrain.VALLY)
					{
						startMap.AttackPower += 0.2f;
					}
					else if (targetMaps[i].Terrain == Terrain.HIGHLAND)
					{
						startMap.AttackPower -= 0.2f;
					}
					break;
				}
				case Terrain.VALLY : 
				{
					if (targetMaps[i].Terrain != Terrain.VALLY)
					{
						startMap.AttackPower -= 0.2f;
					}
					break;
				}
				default : break;
			}
			//结算结果
			//攻击人数*攻击力 - 防御人数*防御力
			result = moveNums[i] * startMap.AttackPower - targetMaps[i].BaseSoldierNum * startMap.DefendPower;
			// Debug.Log("攻击力" + power);
			// Debug.Log("防御力" + defend);
			Debug.Log("结算结果" + result);
			if (result > 0)
			{
				targetMaps[i].BaseSoldierNum = (int)(result + 0.9f);
				if (targetMaps[i].PlayerID != -1)
				{
					players[targetMaps[i].PlayerID].Maps.Remove(targetMaps[i]);
				}
				targetMaps[i].PlayerID = startMap.PlayerID;
				players[startMap.PlayerID].Maps.Add(targetMaps[i]);
			}
			Debug.Log(moveNums[i]);
			startMap.BaseSoldierNum -= moveNums[i];
			//更新UI显示
			startMap.UpadteMapUI();
			targetMaps[i].UpadteMapUI();
		}
	}

	/// <summary>
	/// 开始游戏
	/// </summary>
	public void StartGame()
	{
		Debug.Log("开始");
		gameStart = true;
		foreach (Player p in players)
		{
			p.GameStart = gameStart;
			p.OperateEnd = false;
		}
	}

	/// <summary>
	/// 结束游戏
	/// </summary>
	public void EndGame()
	{
		Debug.Log("结束");
		gameStart = false;
		foreach (Player p in players)
		{
			p.GameStart = gameStart;
		}
	}

	public Player[] Players 
	{
		get
		{
			return players;
		}
	}

	public MapManager[] MapManagers
	{
		get
		{
			return mapManagers;
		}
	}
}
