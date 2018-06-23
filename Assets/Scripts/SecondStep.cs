using UnityEngine;
using System.Collections;
using UnityEngine.U2D;

public class SecondStep : MonoBehaviour
{
    public float angle;
    public Vector3 arrowPos;

    public GameObject arrow;
    // Use this for initialization  
    void Start()
    {
        TestForRotation();
        

    }

    // Update is called once per frame  
    void Update()
    {

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