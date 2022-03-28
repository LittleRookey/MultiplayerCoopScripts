using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] public SlotManager slotManager;
    [SerializeField] private List<GameObject> tabList;

    [SerializeField]
    private ItemManager itemManager;

    private Dictionary<GameObject, Image> tabImgDctn = new Dictionary<GameObject, Image>(); //�� �̹��� ��ųʸ�
    private Dictionary<GameObject, Button> tabBtnDctn = new Dictionary<GameObject, Button>(); //�� ��ư ��ųʸ�

    [SerializeField]
    private InventoryTab equipTab, useTab, etcTab, cashTab;

    [SerializeField]
    private Color normalTabColor, tabSelectedColor;

    //private InventoryTab setupTab;

    private void Awake()
    {
        foreach (var tab in tabList)
        {
            tabImgDctn.Add(tab, tab.GetComponent<Image>());
            tabBtnDctn.Add(tab, tab.GetComponent<Button>());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Create Tabs for inventory
        //equipTab = TabManager.GetTab("EquipTab");
        CreateEquipTab();
        useTab = TabManager.GetTab("UseTab");
        etcTab = TabManager.GetTab("ETCTab");
        cashTab = TabManager.GetTab("CashTab");
        //accessoryTab = TabManager.GetTab("SetUpTab");

        // Refresh slots 
        slotManager.Refresh(TabManager.GetTab("EquipTab"));

        // Item Handler event settings
        foreach (var handler in ItemHandler.HandlerList)
        {
            handler.DragOutEvent = (item) => Debug.Log("Drag Out: " + item.Name);
            handler.SlotMoveFailEvent = (item) => Debug.Log("Slot Move Fail: " + item.Name);
            handler.TypeNotMatchEvent = (item, slot) => Debug.Log("Type doesn't match: " + item.Type);
        }
        slotManager.SlotSort();
    }

    // Update is called once per frame
    void Update()
    {
        ItemHandler.RequestItemHandle(); // Detect event end handler
        
    }

    private void CreateEquipTab()
    {
        //������ ���� �� �ǿ� ��� (Item ��ü)
        InventoryTab equipTab = TabManager.GetTab("EquipTab");

        Equipment longSword = new Equipment("sword_1", "Long Sword", "Sword", "�������� ���� ���� �̳� �� ��.", itemManager.GetItemsOf(Gears.Melee)[0]);
        longSword.UseEvent += (item) => Debug.Log("Item: " + item.Name);
        longSword.SetPrice(200, 200);

        Equipment dagger = new Equipment("dagger_1", "Dagger", "Sword", "�ܵ�, ��ô���̴�.", itemManager.GetItemsOf(Gears.DuelistOffhand)[1]);
        dagger.UseEvent += (item) => Debug.Log("Item: " + item.Name);
        dagger.SetPrice(150, 150);

        //Item shuriken = new Item("", "Shuriken", "Sword", "�̰� ������ �뵵�� �ƴϴ�.", spriteList[2]);
        //shuriken.UseEvent += (item) => Debug.Log("Item: " + item.Name);
        //shuriken.SetPrice(125, 125);

        equipTab.Add(longSword);
        equipTab.Add(dagger);
        //swordTab.Add(shuriken);
    }

    ////������ ���� �̺�Ʈ
    //private void BuyEvent(SlotItem buyItem)
    //{
    //    InventoryTab tab = slotManager.LastRefreshedTab;
    //    ShopHelper.Buy(ref money, buyItem, tab, () => Debug.Log("�� ����"));
    //    moneyText.text = money.ToString();
    //    SlotManager.RefreshAll();
    //}

    //�ð��� �� ��ȯ
    public void TabConvert(GameObject tabItem)
    {
        Debug.Log("Tab converted");
        foreach (var tab in tabList)
        {
            if (tabItem != tab.gameObject)
            {
                tabBtnDctn[tab].interactable = true;
                tabImgDctn[tab].color = normalTabColor;
                tabBtnDctn[tab].transform.Find("TabFocus").gameObject.SetActive(false);
                //Debug.Log(tabItem.transform.Find("TabFocus").parent.name);
                //tabItem.transform.Find("TabFocus").gameObject.SetActive(false);
            }
            else
            {
                tabBtnDctn[tab].interactable = false;
                tabImgDctn[tab].color = tabSelectedColor;
                tabBtnDctn[tab].transform.Find("TabFocus").gameObject.SetActive(true);
                //Debug.Log("selected: " +tabItem.transform.Find("TabFocus").parent.name);
                //tabItem.transform.Find("TabFocus").gameObject.SetActive(true);
            }
        }
        slotManager.Refresh(TabManager.GetTab(tabItem.name));
    }

    //�������� ���õǾ����� ��� Ŭ�� ���� �� ��ȯ
    public void TabConvertWithItem(GameObject tabItem)
    {
        if (ItemHandler.SelectedItem != null)
        {
            TabConvert(tabItem);
        }
    }
}
