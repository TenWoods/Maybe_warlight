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
			players[i].PlayerID = i + 1;
			//TODO:生成玩家之后对玩家进行初始化
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

	/// <summary>
	/// 改变玩家至增兵状态
	/// </summary>
	public void ChangeToAdd()
	{
		foreach(Player p in players)
		{
			p.ChangerOperateState(OperateState.ADD_SOLDIER);
		}
	}

	/// <summary>
	/// 改变玩家至命令状态
	/// </summary>
	public void ChangeToCommand()
	{
		foreach(Player p in players)
		{
			p.ChangerOperateState(OperateState.COMMAND_SOLDIER);
		}
	}

	/// <summary>
	/// 改变玩家至使用卡牌状态
	/// </summary>
	public void ChangeToUseCard()
	{
		foreach(Player p in players)
		{
			p.ChangerOperateState(OperateState.USE_CARDS);
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
