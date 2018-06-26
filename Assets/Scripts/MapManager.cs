using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour 
{
	/*管理的地图方块的地形*/
	private int terrain;
	/*管理的地图块的数量*/
	private int mapBlocksNum;
	/*管理的地图块的起始编号*/
	[SerializeField]
	private int mapBlockIDStart;
	/*初始时所属玩家ID*/
	private int playerID;

	public void InitBlocks(GameManager gm)
	{
		int i;
		for(i = mapBlockIDStart; i < mapBlocksNum + mapBlockIDStart; i++)
		{
			gm.Maps[i].Terrain = terrain;
			gm.Maps[i].MapID_GameManager = i;
			gm.Players[playerID].Maps.Add(gm.Maps[i]);
			//TODO:可能会有其他的数据初始化
		}
		//TODO:目前设定为最后一个为本区域未占领，之后会修改
		gm.Players[playerID].Maps.RemoveAt(i - 1);
	}
}
