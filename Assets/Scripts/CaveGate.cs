using UnityEngine;

public class CaveGate : MonoBehaviour
{
    private void OnEnable()
    {
        // Gatekeeper öldüğünde çalışacak fonksiyonu bağla
        GatekeeperAI.OnGatekeeperDeath += OpenGate;
    }

    private void OnDisable()
    {
        // Bellek sızıntısını önlemek için bağlantıyı kopar
        GatekeeperAI.OnGatekeeperDeath -= OpenGate;
    }

    private void OpenGate()
    {
        Debug.Log("<color=green>CAVE ENTRY OPENED:</color> Gatekeeper is gone.");
        // Kapıyı yok et veya animasyon oynat (biz şimdilik yok ediyoruz)
        Destroy(gameObject); 
    }
}