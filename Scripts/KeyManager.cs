using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    
    [SerializeField] private GameObject mostParentInventory;
    [SerializeField] private KeyCode inventoryOpenKey = KeyCode.I;


    private bool isInventoryOpen;
    // Start is called before the first frame update
    private void Awake()
    {
        if (inventoryOpenKey == KeyCode.None)
        {
            inventoryOpenKey = KeyCode.I;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(inventoryOpenKey))
        {
            if (isInventoryOpen) // if inventory is open
            {
                isInventoryOpen = false;
                mostParentInventory.gameObject.SetActive(false);
            }
            else
            {
                isInventoryOpen = true;
                mostParentInventory.gameObject.SetActive(true);
            }

        }
    }
}
