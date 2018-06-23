using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject arrow;
    private SecondStep Sec;
    void Start()
    {
        Sec = GameObject.FindWithTag("arrow").GetComponent<SecondStep>();
    }

    void onWork()
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
                    Instantiate(Sec.arrow, Sec.arrowPos, Quaternion.identity);
                    Sec.TestForRotation();
                }
            }
        }
    }
}
