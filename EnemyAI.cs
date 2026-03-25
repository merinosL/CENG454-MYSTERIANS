using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase,
        Dead
    }

    public State currentState = State.Patrol;

    void Start()
    {
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                break;
            case State.Chase:
                break;
            case State.Dead:
                break;
        }
    }
}