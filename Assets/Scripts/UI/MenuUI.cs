using System;
using UnityEngine;
using UnityEngine.UI;


[DefaultExecutionOrder(100)]
public class MenuUI : MonoBehaviour
{
    private bool gameStarted = false;

    private void OnEnable()
    {
        gameStarted = false;
        AudioManager.Instance.PlayBGM(SoundType.BGM_MENU);
    }

    private void OnDisable()
    {
        AudioManager.Instance.StopBGM();
    }

    private void Update()
    {
        if (Input.GetAxis("Submit") > 0f)
        {
            if (gameStarted)
            {
                return;
            }
            gameStarted = true;
            GameManager.Instance.BeginGame();
        }
    }
}
