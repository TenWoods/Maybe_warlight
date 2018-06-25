using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	/*游戏玩家*/
	private Player[] players;
	
	private void Start() 
	{
		players = new Player[4];
		InitPlayers();
	}

	/*初始化所有玩家*/
	private void InitPlayers()
	{
		for (int i = 0; i < players.Length; i++)
		{
			players[i] = new Player();
			players[i].PlayerID = i + 1;
			//TODO:生成玩家之后对玩家进行初始化
		}
	}
}
