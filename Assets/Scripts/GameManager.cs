using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	/*玩家数量*/
	[SerializeField]
	private int playerNum = 1;
	/*游戏玩家*/
	private Player[] players;
	/*地图块的数量*/
	private int mapBlocksNum = 100;
	/*游戏地图*/
	private Map[] maps;
	/*地图管理者数量*/
	[SerializeField]
	private int mapManagerNum;
	[SerializeField]
	/*地图管理者*/
	private MapManager[] mapManagers;
	/*游戏是否开始*/
	[SerializeField]
	private bool gameStart = false;
	
	private void Start() 
	{
		players = new Player[playerNum];
		maps = new Map[mapBlocksNum];
		mapManagerNum = mapManagers.Length;
		//InitPlayers();
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
			mapManagers[i].InitBlocks(this);
		}
	}

	/// <summary>
	/// 初始化所有玩家
	/// </summary>
	private void InitPlayers()
	{
		for (int i = 0; i < playerNum; i++)
		{
			players[i] = new Player();
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

	public Map[] Maps
	{
		get
		{
			return maps;
		}
	}

	public Player[] Players 
	{
		get
		{
			return players;
		}
	}
}
