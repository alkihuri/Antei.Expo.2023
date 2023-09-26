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

    [SerializeField]
    private Animator bubbleAnim;
    [SerializeField]
    private RectTransform crab;
    private void Awake()
    {
        crabsTMP.color = new Color(1,1,1,0);
        crabsValueTMP.color = new Color (crabsValueTMP.color.r,  crabsValueTMP.color.g,  crabsValueTMP.color.b, 0);
        MainTMP.enabled = false;
        LittleTMP.enabled = false;
        crab.localScale = Vector2.zero;
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
        StopAllCoroutines();
        StartCoroutine(closeUI());
    }
    public void EndGame(bool isWin)
    {
        if (isWin)
        {
            MainTMP.text = "Congratulations!";
            LittleTMP.text = "Take a photo with your crab";
        }
        else
        {
            MainTMP.text = "You lost!";
            LittleTMP.text = "Try again";                  
        }
        crabsValueTMP.text = GameObject.FindObjectOfType<GameCore>().collectedCrabs.ToString() + "/" + GameObject.FindObjectOfType<GameCore>().config.allCrabsNeedToWin.ToString();
        StopAllCoroutines();
        //StartCoroutine(TimerLost());
        StartCoroutine(showUI(isWin));
    }

    private IEnumerator showUI(bool isWin)
    {
        crabsTMP.color = new Color(1, 1, 1, 1);
        crabsValueTMP.color = new Color(crabsValueTMP.color.r, crabsValueTMP.color.g, crabsValueTMP.color.b, 1);
        if (isWin)
        {
            bubbleAnim.SetTrigger("ActiveBecome");
        }
        MainTMP.enabled = true;
        LittleTMP.enabled = true;
        crab.localScale = new Vector2(0,0);
        while (allBGImages[allBGImages.Length - 3].color.a < 1)
        {
            yield return new WaitForFixedUpdate();
            foreach (Image img in allBGImages)
            {
                if ( (img == allBGImages[allBGImages.Length - 2] || img == allBGImages[allBGImages.Length - 1]) && !isWin)
                {
                    continue;
                }
                img.color = new Color(1, 1, 1, img.color.a + 1.4f * Time.deltaTime);
            }
        }
        if (isWin)
        {
            while (crab.localScale.x < 1)
            {
                yield return new WaitForFixedUpdate();
                crab.localScale = new Vector2(crab.localScale.x + 0.8f * Time.deltaTime, crab.localScale.y + 0.8f * Time.deltaTime);
            }
        }
        yield return new WaitForSeconds(2.2f);
        if (isWin)
        {
            while (crab.localScale.x > 0)
            {
                yield return new WaitForFixedUpdate();
                crab.localScale = new Vector2(crab.localScale.x - 1.2f * Time.deltaTime, crab.localScale.y - 1.2f * Time.deltaTime);
            }
        }
        yield return new WaitForSeconds(2);

        if (!isWin)
        {
            GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.State.standby);
        }
        else
        {
            GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.State.photo);
        }
        
    }

    private IEnumerator closeUI()
    {
        crabsTMP.color = new Color(1, 1, 1, 0);
        crabsValueTMP.color = new Color(crabsValueTMP.color.r, crabsValueTMP.color.g, crabsValueTMP.color.b, 0);
        MainTMP.enabled = false;
        LittleTMP.enabled = false;
        while (allBGImages[allBGImages.Length-3].color.a > 0)
        {
            yield return new WaitForFixedUpdate();
            foreach (Image img in allBGImages)
            {
                img.color = new Color(1, 1, 1, img.color.a - 1.4f * Time.deltaTime);
            }
        }      
    }
}
