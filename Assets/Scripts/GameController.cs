using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject arrow;
    public float angle;
    public Vector3 arrowPos;

    
    private void OnWork()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);


            if (hit.collider != null)
            {
                if (hit.collider.tag == "1")
                {
                    Instantiate(arrow, arrowPos, Quaternion.identity);
                    TestForRotation();
                }
            }
        }
    }
    public void TestForRotation()
    {
        GameObject pointA = GameObject.Find("Land2");
        GameObject pointB = GameObject.Find("Land1");
        Vector3 vecA = pointA.transform.position;
        Vector3 vecB = pointB.GetComponent<Transform>().position;//两种写法都体验一下
        arrowPos = (vecA + vecB) / 2;
        float distance = (vecB - vecA).magnitude;//两点间距离计算
        arrow.transform.localScale = new Vector3(distance / 6, 1, 1f);
        Vector3 direction = vecB - vecA;                                    //终点减去起点  
        angle = Vector3.Angle(direction, Vector3.right);              //计算旋转角度  
        arrow.GetComponent<Transform>().Rotate(0, 0, angle);


    }
}
