using UnityEngine;
using System; 

public class PlayerInventory : MonoBehaviour
{
    public int potionCount { get; private set; }

    public event Action<int> OnPotionCollected;

    public void AddPotion(int amount)
    {
        potionCount += amount;
        
        OnPotionCollected?.Invoke(potionCount);
        
        Debug.Log("The potions have been collected! Total potions in inventory:" + potionCount);
    }
}