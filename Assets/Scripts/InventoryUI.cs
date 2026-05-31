using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel; 
    public PlayerInventory playerInventory;
    public TextMeshProUGUI potionText;

    private void Update()
    {
     
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    private void OnEnable()
    {
        if (playerInventory != null)
            playerInventory.OnPotionCollected += UpdateUI;
    }

    private void OnDisable()
    {
        if (playerInventory != null)
            playerInventory.OnPotionCollected -= UpdateUI;
    }

    private void UpdateUI(int count)
    {
        potionText.text = "x " + count.ToString();
    }
}