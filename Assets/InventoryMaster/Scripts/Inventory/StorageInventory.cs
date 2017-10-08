using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StorageInventory : MonoBehaviour
{

    [SerializeField]
    public GameObject inventory;

    [SerializeField]
    public List<Item> storageItems = new List<Item>();

    [SerializeField]
    private ItemDataBaseList itemDatabase;

    [SerializeField]
    public int distanceToOpenStorage;

    public float timeToOpenStorage;

    private InputManager inputManagerDatabase;

    float startTimer;
    float endTimer;
    bool showTimer;

    public int itemAmount = 3;

    Tooltip tooltip;
    Inventory inv;

    GameObject player;
    GameObject canvas;
    Transform inventory2;

    static Image timerImage;
    static GameObject timer;

    public int containerQuality = 1;                                                    // Use to set the quality of the storage container : 1-6

    bool closeInv;

    bool showStorage;

    public void addItemToStorage(int id, int value)
    {
        Item item = itemDatabase.getItemByID(id);
        item.itemValue = value;
        storageItems.Add(item);
    }

    void Start()
    {
        canvas = GameObject.Find("Canvas - Inventory");
        inventory2 = canvas.transform.Find("Panel - Storage");
        inventory2.gameObject.SetActive(true);
        inventory = GameObject.FindGameObjectWithTag("Storage");
        //inventory.SetActive(true);
        if (inputManagerDatabase == null)
            inputManagerDatabase = (InputManager)Resources.Load("InputManager");

        player = GameObject.FindGameObjectWithTag("Player");
        inv = inventory.GetComponent<Inventory>();
        //inv = inventory2.GetComponent<Inventory>();
        ItemDataBaseList inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");

        int creatingItemsForChest = 0;

        int randomItemAmount = Random.Range(1, itemAmount);

        while (creatingItemsForChest < randomItemAmount)
        {
            int randomItemNumber = Random.Range(1, inventoryItemList.itemList.Count - 1);
            int raffle;
            if (containerQuality == 2)
            {
                raffle = Random.Range(20, 90);                                                       // Ensures some higher quality items
            }
            else if (containerQuality == 3)
            {
                raffle = Random.Range(10, 50);                                                       // Ensures some higher quality items
            }
            else if (containerQuality == 4)
            {
                raffle = Random.Range(5, 30);                                                       // Ensures some higher quality items
            }
            else if (containerQuality == 5)
            {
                raffle = Random.Range(1, 20);                                                       // Ensures some higher quality items
            }
            else if (containerQuality == 6)
            {
                raffle = Random.Range(1, 10);                                                       // For boss chests or something
            }
            else
            {
                raffle = Random.Range(50, 100);
            }



            if (raffle <= inventoryItemList.itemList[randomItemNumber].rarity)
            {
                int randomValue = Random.Range(1, inventoryItemList.itemList[randomItemNumber].getCopy().maxStack);
                if (randomValue > 5) randomValue /= 2;
                Item item = inventoryItemList.itemList[randomItemNumber].getCopy();
                item.itemValue = randomValue;
                storageItems.Add(item);
                creatingItemsForChest++;
            }
        }

        //if (GameObject.FindGameObjectWithTag("Timer") != null)
        //{
        //    timerImage = GameObject.FindGameObjectWithTag("Timer").GetComponent<Image>();
        //    timer = GameObject.FindGameObjectWithTag("Timer");
        //    timer.SetActive(false);
        //}
        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
            tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
        //inventory.SetActive(false);
    }

    public void setImportantVariables()
    {
        if (itemDatabase == null)
            itemDatabase = (ItemDataBaseList)Resources.Load("ItemDatabase");
    }

    void Update()
    {

        float distance = Vector3.Distance(this.gameObject.transform.position, player.transform.position);

        if (showTimer)
        {
            if (timerImage != null)
            {
                timer.SetActive(true);
                float fillAmount = (Time.time - startTimer) / timeToOpenStorage;
                timerImage.fillAmount = fillAmount;
            }
        }

        if (distance <= distanceToOpenStorage && Input.GetKeyDown(inputManagerDatabase.StorageKeyCode))
        {
            showStorage = !showStorage;
            inventory.transform.position = new Vector3(this.gameObject.transform.position.x + 300, this.gameObject.transform.position.y + 300);
            StartCoroutine(OpenInventoryWithTimer());
        }

        if (distance > distanceToOpenStorage && showStorage)
        {
            showStorage = false;
            if (inventory.activeSelf)
            {
                storageItems.Clear();
                setListofStorage();
                inventory.SetActive(false);
                inv.deleteAllItems();
            }
            tooltip.deactivateTooltip();
            //timerImage.fillAmount = 0;
            timer.SetActive(false);
            showTimer = false;
        }
    }

    IEnumerator OpenInventoryWithTimer()
    {
        if (showStorage)
        {
            startTimer = Time.time;
            showTimer = true;
            yield return new WaitForSeconds(timeToOpenStorage);
            if (showStorage)
            {
                inv.ItemsInInventory.Clear();
                inventory.SetActive(true);
                addItemsToInventory();
                showTimer = false;
                if (timer != null)
                    timer.SetActive(false);
            }
        }
        else
        {
            storageItems.Clear();
            setListofStorage();
            inventory.SetActive(false);
            inv.deleteAllItems();
            tooltip.deactivateTooltip();
        }


    }



    void setListofStorage()
    {
        Inventory inv = inventory.GetComponent<Inventory>();
        storageItems = inv.getItemList();
    }

    void addItemsToInventory()
    {
        Inventory iV = inventory.GetComponent<Inventory>();
        for (int i = 0; i < storageItems.Count; i++)
        {
            iV.addItemToInventory(storageItems[i].itemID, storageItems[i].itemValue);
        }
        iV.stackableSettings();
    }






}
