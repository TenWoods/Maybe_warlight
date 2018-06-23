using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoidlerClick : MonoBehaviour
{
    public Text text1;
    public Text text2;
    public Text textSum;
    public int clickNumber1 = 0;
    public int clickNumber2 = 0;
    public int clickNumberSum = 0;
    void Update()
    {

        if (Input.GetMouseButtonDown(0)&&clickNumberSum<5)
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);


            if (hit.collider != null) { 
            if (hit.collider.tag =="1")
            {
                clickNumber1++;
                clickNumberSum++;
                    
                text1.text = clickNumber1.ToString();
                

            }
            else if (hit.collider.tag == "2")
            {
                clickNumber2++;
                clickNumberSum++;
                text2.text = clickNumber2.ToString();

               
            }

            }
           

            textSum.text = "本轮可补充兵力:"+clickNumberSum.ToString()+"/5";
        }
    }
}
