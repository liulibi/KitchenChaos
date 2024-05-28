using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerIamge;

    private void Update()
    {
        timerIamge.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
