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

    private Dictionary<GameObject, Image> tabImgDctn = new Dictionary<GameObject, Image>(); //탭 이미지 딕셔너리
    private Dictionary<GameObject, Button> tabBtnDctn = new Dictionary<GameObject, Button>(); //탭 버튼 딕셔너리

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
        //아이템 생성 및 탭에 등록 (Item 객체)
        InventoryTab equipTab = TabManager.GetTab("EquipTab");

        Equipment longSword = new Equipment("sword_1", "Long Sword", "Sword", "누군가의 한이 맺힌 겁나 긴 검.", itemManager.GetItemsOf(Gears.Melee)[0]);
        longSword.UseEvent += (item) => Debug.Log("Item: " + item.Name);
        longSword.SetPrice(200, 200);

        Equipment dagger = new Equipment("dagger_1", "Dagger", "Sword", "단도, 투척용이다.", itemManager.GetItemsOf(Gears.DuelistOffhand)[1]);
        dagger.UseEvent += (item) => Debug.Log("Item: " + item.Name);
        dagger.SetPrice(150, 150);

        //Item shuriken = new Item("", "Shuriken", "Sword", "이건 던지는 용도가 아니다.", spriteList[2]);
        //shuriken.UseEvent += (item) => Debug.Log("Item: " + item.Name);
        //shuriken.SetPrice(125, 125);

        equipTab.Add(longSword);
        equipTab.Add(dagger);
        //swordTab.Add(shuriken);
    }

    ////아이템 구매 이벤트
    //private void BuyEvent(SlotItem buyItem)
    //{
    //    InventoryTab tab = slotManager.LastRefreshedTab;
    //    ShopHelper.Buy(ref money, buyItem, tab, () => Debug.Log("돈 부족"));
    //    moneyText.text = money.ToString();
    //    SlotManager.RefreshAll();
    //}

    //시각적 탭 변환
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

    //아이템이 선택되어있을 경우 클릭 없이 탭 변환
    public void TabConvertWithItem(GameObject tabItem)
    {
        if (ItemHandler.SelectedItem != null)
        {
            TabConvert(tabItem);
        }
    }
}
