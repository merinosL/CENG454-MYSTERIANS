using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public WinLoseManager winManager;

    void Start()
    {
        if (winManager == null)
        {
            winManager = FindObjectOfType<WinLoseManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (winManager != null)
            {
                winManager.OpenWinPanel();
            }
        }
    }
}