using UnityEngine;

public class CaveGate : MonoBehaviour
{
    public GameObject gatekeeper;

    private void OnEnable()
    {
        if (gatekeeper != null)
        {
            var healthComp = gatekeeper.GetComponent<EnemyHealth>();
            if (healthComp != null) healthComp.OnDeath += OpenGate;
        }
    }

    private void OnDisable()
    {
        if (gatekeeper != null)
        {
            var healthComp = gatekeeper.GetComponent<EnemyHealth>();
            if (healthComp != null) healthComp.OnDeath -= OpenGate;
        }
    }

    private void OpenGate()
    {
        Destroy(gameObject);
    }
}