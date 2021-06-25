using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RunGameManager : MonoBehaviour
{
    public static RunGameManager instance;
    TextMeshProUGUI startText;
    Animator startTextAnimator;

    internal void StageEnd()
    {
        gameState = GameStateType.End;
        startText.text = "Clear!";
        startTextAnimator.Play("ShowText");
        SetEnableMoveCameras(false);
        player.OnStageEnd();
    }
    public float cameraSpeed = 20;
    void SetEnableMoveCameras(bool state)
    {
        float speed = state ? cameraSpeed : 0;
        moveCameras.ForEach(x => x.speed = speed);
        player.speed = speed;
    }

    TextMeshProUGUI pointText;

    public int waitSeconds = 2;
    public float delayHideText = 1;


    int coin;
    public int Coin
    {
        get
        {
            return coin;
        }
        set { coin = value;
            UpdateCoinTest(coin);
        }
    }


    private void UpdateCoinTest(int coin)
    {
        pointText.text = coin.ToNumber();
    }

    private void Awake()
    {
        instance = this;
    }
    List<MoveCamera> moveCameras;
    Player player;
    IEnumerator Start()
    {
        gameState = GameStateType.Ready;
        moveCameras = new List<MoveCamera>(FindObjectsOfType<MoveCamera>());
        player = FindObjectOfType<Player>();
        
        startText = transform.Find("StartText").GetComponent<TextMeshProUGUI>();
        pointText = transform.Find("PointText").GetComponent<TextMeshProUGUI>();
        startTextAnimator = startText.GetComponent<Animator>();

        for (int i = waitSeconds; i > 0; i--)
        {
            startText.text = i.ToString();
            startTextAnimator.Play("ShowText");
            yield return new WaitForSeconds(1);
        }
        startTextAnimator.Play("ShowText");
        startText.text = "Start!";

        gameState = GameStateType.Playing;
        yield return new WaitForSeconds(delayHideText);
        startTextAnimator.Play("HideText");
    }
    public GameStateType gameState = GameStateType.NotInit;
    public enum GameStateType
    {
        NotInit,
        Ready,
        Playing,
        End,
    }
}
