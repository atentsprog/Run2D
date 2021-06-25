using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float speed = 20;
    void Update()
    {
        if (RunGameManager.instance.gameState != RunGameManager.GameStateType.Playing)
            return;

        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
