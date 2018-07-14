using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WuZhuang : Card {
    public void CardEffect(int playerID, GameObject[] targets)
    {
        
        //TODO:卡牌的操作
        foreach(GameObject target in targets)
        {
            target.GetComponent<Map>().AddSoldier();
        }
    }
}
