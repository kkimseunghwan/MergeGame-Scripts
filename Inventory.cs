using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    public List<Slotdata> slots = new List<Slotdata>();
    public int maxSlot = 5;

    public GameObject slotPrefebs;

    //인벤토리 생성
    // Start is called before the first frame update
    void Start()
    {
        GameObject slotPanel = GameObject.Find("HeroSelect");

        for(int i = 0; i < maxSlot; i++)
        {
            GameObject go = Instantiate(slotPrefebs, slotPanel.transform, false);
            go.name = "SelectPanel" + i;
            Slotdata slot = new Slotdata();
            slot.isEmpty = true;
            slot.slotObj = go;
            slots.Add(slot);     
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
