using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroManager : MonoBehaviour
{
    public List<Character> PringHero = new List<Character>();

    public Sprite[] PoringImg;
    public Inventory inven;
    GameManagerV02 _GameManagerV02;
    bool isStart = false;
    // Start is called before the first frame update
    void Start()
    {
        _GameManagerV02 = GameObject.Find("GameManager").GetComponent<GameManagerV02>();
    }



    // Update is called once per frame
    void Update()
    {
        if (!isStart)
        {
            PoringSelect(0);
            isStart = true;
        }

    }

    public void PoringSelect(int num)
    {
        for(int i = 0; i < inven.slots.Count -2; i++)
        {
            if (inven.slots[i].isEmpty)
            {
                if( !_GameManagerV02._HeroUnitList[num].GetComponent<Character>().isUsing )
                {
                    inven.slots[i].slotObj.GetComponent<Image>().sprite = PoringImg[num];
                    inven.slots[i].isEmpty = false;
                    _GameManagerV02._HeroUnitList[num].GetComponent<Character>().isUsing = true;
                }
                break;
            }
        }

    }






}
