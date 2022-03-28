using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    [SerializeField] private GameObject shopSlotsParent;

    Dictionary<Gears, List<Sprite>> GearsAndSprites = new Dictionary<Gears, List<Sprite>>();


    //// Start is called before the first frame update
    void Start()
    {

        //for (int i = 0; i < 13; i++)
        //{
        //    if ((Gears)i == Gears.DuelistOffhand)
        //    {
        //        GearsAndSprites.Add((Gears)i, Resources.LoadAll<Sprite>("GearsIcons/Melee").ToList().OrderBy(o => o.name.Length).ThenBy(c => c.name).ToList());
        //    }
        //    else
        //    {
        //        GearsAndSprites.Add((Gears)i, Resources.LoadAll<Sprite>("GearsIcons/" + ((Gears)i).ToString()).ToList().OrderBy(o => o.name.Length).ThenBy(c => c.name).ToList());
        //    }
        //    CurrentChosenGears.Add((Gears)i, 0);
        //}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
