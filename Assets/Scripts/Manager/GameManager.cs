using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	/*游戏玩家(目前只有一个玩家)*/
	[SerializeField]
	private Operator[] players;
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

	private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

	private void Awake()
	{
		_instance = this;
	}
	
	private void Start() 
	{
		mapManagerNum = mapManagers.Length;
		all_Steps = new PlayerStep[players.Length];
		InitPlayers();
		InitMapBlocks();
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
			mapManagers[i].InitBlocksData();
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
			//TODO:生成玩家之后对玩家进行初始化
		}
	}

	/// <summary>
	/// 检测是否开始结算
	/// </summary>
	private void CheckCaculation()
	{
		foreach(Operator p in players)
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
		foreach(Operator p in players)
		{
			if (p.gameObject.gameObject.tag != "Player")
			{
				continue;
			}
			for (i = 0; i < p.Steps.AddMaps.Count; i++)
			{
				p.Steps.AddMaps[i].BaseSoldierNum -= p.Steps.AddNums[i];
				p.Steps.AddMaps[i].UpdateMapUI();
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
		foreach(Operator p in players)
		{
			for (i = 0; i < p.Steps.AddMaps.Count; i++)
			{
				p.Steps.AddMaps[i].BaseSoldierNum += p.Steps.AddNums[i];
				p.Steps.AddMaps[i].UpdateMapUI();
			}
		}
		//指挥过程
		foreach(Operator p in players)
		{
			Debug.Log(p.gameObject.name + ":" + p.Steps.CommandMaps.Count);
			foreach(Map m in p.Steps.CommandMaps)
			{
				AttackCaculation(m);
			}
		}
		//每个地图管理检查士兵数的增加
		foreach(MapManager mm in mapManagers)
		{
			mm.CheckUpdateAdd();
		}
		//还原操作状态
		foreach(Operator p in players)
		{
			p.CleanSteps();
			p.ChangeOperateStateStart();
			p.UpdateSoldierNum();
		}
		startCaculate = false;
		//游戏结束检测
		foreach (Operator p in players)
		{
			if (p.Maps.Count == 0)
			{
				gameStart = false;
				Debug.Log("游戏结束");
			}
		}
	}

	/// <summary>
	/// 攻击计算
	/// </summary>
	/// <param name="startMap">攻击方地图块</param>
	private void AttackCaculation(Map startMap)
	{
		Debug.Log("进攻结算:" + startMap.gameObject.name);
		List<Map> targetMaps = startMap.MoveDirMap;
		List<int> moveNums = startMap.MoveSoldierNum;
		float result;
		float attackPower = 0;
		float defendPower = 0;
		for(int i = 0; i < targetMaps.Count; i++)
		{
			attackPower = startMap.AttackPower;
			defendPower = targetMaps[i].DefendPower;
			if (startMap.BaseSoldierNum < moveNums[i])
			{
				Debug.Log(startMap.gameObject.name);
				//如果地图块上的人数不足，调增出兵人数
				moveNums[i] = startMap.BaseSoldierNum - 1;
			}
			if (startMap.PlayerID == targetMaps[i].PlayerID)
			{
				startMap.BaseSoldierNum -= moveNums[i];
				targetMaps[i].BaseSoldierNum += moveNums[i];
				startMap.UpdateMapUI();
				targetMaps[i].UpdateMapUI();
				return;
			}
			if (targetMaps[i].Terrain != Terrain.DESERT)
			{
				defendPower += 0.1f;
			}
			//攻击力根据地形改变
			//高度:高地>平地=森林=荒漠>谷地
			switch (startMap.Terrain)
			{
				case Terrain.HIGHLAND :
				{
					if (targetMaps[i].Terrain != Terrain.HIGHLAND)
					{
						attackPower += 0.2f;
					}
					break;
				}
				case Terrain.FOREST :
				{
					if (targetMaps[i].Terrain == Terrain.VALLY)
					{
						attackPower += 0.2f;
					}
					else if (targetMaps[i].Terrain == Terrain.HIGHLAND)
					{
						attackPower -= 0.2f;
					}
					attackPower += 0.1f; 
					break;
				} 
				case Terrain.DESERT : 
				{
					if (targetMaps[i].Terrain == Terrain.VALLY)
					{
						attackPower += 0.2f;
					}
					else if (targetMaps[i].Terrain == Terrain.HIGHLAND)
					{
						attackPower -= 0.2f;
					}
					attackPower -= 0.1f; 
					break;
				}
				case Terrain.FLATLAND :
				{
					if (targetMaps[i].Terrain == Terrain.VALLY)
					{
						attackPower += 0.2f;
					}
					else if (targetMaps[i].Terrain == Terrain.HIGHLAND)
					{
						attackPower -= 0.2f;
					}
					break;
				}
				case Terrain.VALLY : 
				{
					if (targetMaps[i].Terrain != Terrain.VALLY)
					{
						attackPower -= 0.2f;
					}
					break;
				}
				default : break;
			}
			result = moveNums[i] * attackPower - targetMaps[i].BaseSoldierNum * defendPower;
			Debug.Log("结算结果" + result);
			//结算后占领土地
			if (result > 0)
			{
				targetMaps[i].BaseSoldierNum = (int)(result + 0.9f);
				if (targetMaps[i].BaseSoldierNum == 0)
				{
					targetMaps[i].BaseSoldierNum = 1;
				}
				if (targetMaps[i].PlayerID != -1)
				{
					players[targetMaps[i].PlayerID].Maps.Remove(targetMaps[i]);
				}
				targetMaps[i].PlayerID = startMap.PlayerID;
				targetMaps[i].UpdateFlagUI();
				players[startMap.PlayerID].Maps.Add(targetMaps[i]);
			}
			Debug.Log(moveNums[i]);
			startMap.BaseSoldierNum -= moveNums[i];
			//更新UI显示
			startMap.UpdateMapUI();
			targetMaps[i].UpdateMapUI();
		}
	}

	/// <summary>
	/// 开始游戏
	/// </summary>
	public void StartGame()
	{
		Debug.Log("开始");
		gameStart = true;
		foreach (Operator p in players)
		{
			p.ChangeOperateStateStart();
			p.GameStart = true;
		}
	}

	/// <summary>
	/// 结束游戏
	/// </summary>
	public void EndGame()
	{
		Debug.Log("结束");
		gameStart = false;
		foreach (Operator p in players)
		{
			p.GameStart = gameStart;
		}
	}

	public Operator[] Players 
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
