using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ItemManager : MonoBehaviour
{
    Dictionary<Jobs, Gears> JobsAndWeapons = new Dictionary<Jobs, Gears>();
    Dictionary<Jobs, Gears> JobsAndOffhands = new Dictionary<Jobs, Gears>();

    public Dictionary<Gears, List<Sprite>> GearsAndSprites = new Dictionary<Gears, List<Sprite>>();

    //Dictionary<Jobs, Dictionary<Gears, List<Sprite>>> all_items;

    public List<Sprite> GetItemsOf(Gears gear)
    {
        for (int k = 0; k < GearsAndSprites.Count; k++)
        {
            //Debug.Log(((Gears)k).ToString() + ": "+ GearsAndSprites[(Gears)k].Count);
        }
        
        return GearsAndSprites[gear];
    }
    void Init()
    {
        // iterate over jobs
        //for (int i = 0; i < 4; i++)
        //{
        // iterate over Gears type
        for (int k = 0; k < 13; k++)
        {
            if ((Gears)k == Gears.DuelistOffhand)
            {
                GearsAndSprites.Add((Gears)k, Resources.LoadAll<Sprite>("GearsIcons/Melee").ToList().OrderBy(o => o.name.Length).ThenBy(c => c.name).ToList());
            }
            else
            {
                GearsAndSprites.Add((Gears)k, Resources.LoadAll<Sprite>("GearsIcons/" + ((Gears)k).ToString()).ToList().OrderBy(o => o.name.Length).ThenBy(c => c.name).ToList());
            }
        }

        // iterate over jobs
        //for (int i = 0; i < 4; i++)
        //{
        //    // iterate over Gears type
        //    for (int k = 0; k < 13; k++)
        //    {
        //        // iterate over number of gears
        //        for (int j = 0; j < GearsAndSprites[k].Count; j++)
        //        {
        //            all_items[i][k][j] = GearsAndSprites[k][j];
        //        }
        //    }
        //}

    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
        JobsAndWeapons.Add(Jobs.Warrior, Gears.Melee);
        JobsAndOffhands.Add(Jobs.Warrior, Gears.Shield);

        JobsAndWeapons.Add(Jobs.Archer, Gears.Bow);

        JobsAndOffhands.Add(Jobs.Archer, Gears.Quiver);
        JobsAndWeapons.Add(Jobs.Elementalist, Gears.Staff);

        JobsAndWeapons.Add(Jobs.Duelist, Gears.Melee);
        JobsAndOffhands.Add(Jobs.Duelist, Gears.DuelistOffhand);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
