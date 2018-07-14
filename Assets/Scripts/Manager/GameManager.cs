using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	/*游戏玩家*/
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
	private bool startCaculate = false;
	/*所有玩家的行为步骤*/
	private PlayerStep[] all_Steps; 
	
	private void Start() 
	{
		mapManagerNum = mapManagers.Length;
		all_Steps = new PlayerStep[players.Length];//？？
		InitMapBlocks();
		InitPlayers();
	}

	private void Update()
	{
		if (!gameStart)
		{
			return;
		}
		//CheckCaculation();
		//if (!startCaculate)
		//{
		//	return;
		//}

	}

	/// <summary>
	/// 初始化所有地图方块
	/// </summary>
	private void InitMapBlocks()
	{
		for (int i = 0; i < mapManagerNum; i++)
		{
			mapManagers[i].InitBlocksData(this);//?
		}
	}

	/// <summary>
	/// 初始化所有玩家
	/// </summary>
	private void InitPlayers()
	{
		for (int i = 0; i < players.Length; i++)
		{
			players[i].PlayerID = i + 1;//?不该就是i吗
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
		}//感觉不够吧
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
