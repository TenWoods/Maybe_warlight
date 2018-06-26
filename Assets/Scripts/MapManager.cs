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

	public void InitBlocks(GameManager gm)
	{
		for(int i = mapBlockIDStart; i < mapBlocksNum + mapBlockIDStart; i++)
		{
			gm.Maps[i].Terrain = terrain;
			//TODO:可能会有其他的数据初始化
		}
	}
}
