using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WinLoseView : View
{
    [SerializeField]
    private Image[] allBGImages;
    [SerializeField]
    private TMP_Text MainTMP;
    [SerializeField]
    private TMP_Text LittleTMP;

    [SerializeField]
    private TMP_Text crabsTMP;
    [SerializeField]
    private TMP_Text crabsValueTMP;


    private void Awake()
    {
        MainTMP.enabled = false;
        LittleTMP.enabled = false;
        foreach (Image img in allBGImages)
        {
            img.color = new Color(1, 1, 1, 0);
        }
    }
    public override void EnableView()
    {
        EndGame(GameObject.FindObjectOfType<GameCore>().collectedCrabs >= GameObject.FindObjectOfType<GameCore>().config.allCrabsNeedToWin);
    }
    public override void DisableView()
    {
        StartCoroutine(closeUI());
    }
    public void EndGame(bool isWin)
    {
        if (isWin)
        {
            MainTMP.text = "Congratulations!";
            LittleTMP.text = "Take a photo with your crab";
            StartCoroutine(TimerLost());
        }
        else
        {
            MainTMP.text = "You lost!";
            LittleTMP.text = "Try again";
            StartCoroutine(TimerLost());
        }
        MainTMP.enabled = true;
        LittleTMP.enabled = true;
    }

    private IEnumerator showUI(bool isWin)
    {
        while (allBGImages[0].color.a != 1)
        {
            yield return new WaitForFixedUpdate();
            foreach (Image img in allBGImages)
            {
                img.color = new Color(1, 1, 1, img.color.a + 1.4f * Time.deltaTime);
            }
        }
    }

    private IEnumerator closeUI()
    {
        MainTMP.enabled = false;
        LittleTMP.enabled = false;
        while (allBGImages[0].color.a != 0)
        {
            yield return new WaitForFixedUpdate();
            foreach (Image img in allBGImages)
            {
                img.color = new Color(1, 1, 1, img.color.a - 1.4f * Time.deltaTime);
            }
        }
    }

    private IEnumerator TimerLost()
    {
        yield return new WaitForSeconds(5);
        GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.State.standby);
    }
}
