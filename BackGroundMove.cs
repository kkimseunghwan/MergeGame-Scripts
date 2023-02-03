using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{

    public static bool MapEnd;
    public static bool MapMoving;
    public bool isFight;

    // Start is called before the first frame update
    void Start()
    {
        MapEnd = false;
        MapMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        MapMove();
    }

    public void MapMove()
    {
        Vector2 pos = transform.position;

        
        for (int i = 0; i < GameManagerV02.instance._HeroUnitList.Count ; i++)
        {
            if (GameManagerV02.instance._HeroUnitList[i].GetComponent<Character>().isFight)
            {
                isFight = true;
                MapMoving = true;
            }
        }
        

        if (transform.position.x >= -12 && (!isFight || !MapMoving) )
        {
            pos.x -= 1.5f * Time.deltaTime;
            MapEnd = false;
        }
        else if (transform.position.x <= -12)
        {
            MapEnd = true;
        }

        transform.position = pos;

        Debug.Log("isFight>" + isFight + "     MapMoving>" + MapMoving + "     MapEnd>" + MapEnd);
    }





}
