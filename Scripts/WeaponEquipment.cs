using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eWeaponType
{
    one_hand_attack,
    two_hand_attack,
    archer,
    guns
};
public class WeaponEquipment : MonoBehaviour
{

    [SerializeField]
    private Sprite[] weapons;

    // Start is called before the first frame update
    void Start()
    {
        weapons = Resources.LoadAll<Sprite>("Weapons");
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
