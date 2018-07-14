using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YanHu : Card {

	public void CardEffect(int playerID,GameObject target)
    {
        for(int count = 0; count < 10; count++)
        {
            target.GetComponent<Map>().AddSoldier();
        }
        target.GetComponent<Map>().UpadteMapUI();
        
    }
   

}
