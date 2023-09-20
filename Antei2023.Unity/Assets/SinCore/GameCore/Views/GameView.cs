using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameView : View
{
    [SerializeField]
    private TMP_Text collectedCrabs;
    [SerializeField]
    private TMP_Text currentTime;

    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Image LogoImage;
    [SerializeField]
    private Image[] allBGImages;
    [SerializeField]
    private RectTransform ScorePanel;
    [SerializeField]
    private RectTransform TimePanel;

    [SerializeField]
    private Image timePanelOrange;

    [SerializeField]
    private SpriteRenderer bucket;
    // private Vector2 scorePanelScale;
    // private Vector2 timePanelScale;


    [HideInInspector]
    public int needToWin=100;

    private void Awake()
    {
        bucket.color = new Color(1, 1, 1, 0);
        //scorePanelScale = ScorePanel.localScale;
        //timePanelScale = TimePanel.localScale;
        backgroundImage.color = new Color(1, 1, 1, 0);
        LogoImage.color = new Color(1, 1, 1, 0);
        foreach(Image img in allBGImages)
        {
            img.color = new Color(1, 1, 1, 0);
        }
        ScorePanel.localScale = new Vector2(0,0);
        TimePanel.localScale = new Vector2(0, 0);
        this.needToWin = GameObject.FindObjectOfType<GameCore>().config.allCrabsNeedToWin;
        ChangeCrabs(0);
    }

    public void ChangeCrabs(int value)
    {
        collectedCrabs.text = value.ToString() + "/" + needToWin;
    }
    public void ChangeTime(int value, int allValue)
    {
        timePanelOrange.fillAmount = (float) value / allValue;
        currentTime.text = value.ToString();
    }

    public override void EnableView()
    {
        bucket.color = new Color(1, 1, 1, 1);
        StartCoroutine(showUI());
    }

    public override void DisableView()
    {
        bucket.color = new Color(1, 1, 1, 0);
        StartCoroutine(closeUI());
    }

    private IEnumerator showUI()
    {
        ScorePanel.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        while (LogoImage.color.a<1)
        {
            yield return new WaitForFixedUpdate();           
            LogoImage.color = new Color(1, 1, 1, LogoImage.color.a + 1.4f * Time.deltaTime);
            foreach (Image img in allBGImages)
            {
                img.color = new Color(1, 1, 1, img.color.a + 1.4f * Time.deltaTime);
            }
            ScorePanel.localScale = new Vector2(ScorePanel.localScale.x + 1.4f * Time.deltaTime, ScorePanel.localScale.y + 1.4f * Time.deltaTime);
            TimePanel.localScale = new Vector2(TimePanel.localScale.x + 1.4f * Time.deltaTime, TimePanel.localScale.y + 1.4f * Time.deltaTime);
            backgroundImage.color = new Color(1, 1, 1, backgroundImage.color.a + 1.4f * Time.deltaTime);
        }
    }
    private IEnumerator closeUI()
    {
        //Image scoreImage = ScorePanel.transform.GetChild(0).GetComponent<Image>();
        while (LogoImage.color.a >0)
        {
            yield return new WaitForFixedUpdate();           
            LogoImage.color = new Color(1, 1, 1, LogoImage.color.a - 1.4f * Time.deltaTime);
            foreach (Image img in allBGImages)
            {
                img.color = new Color(1, 1, 1, img.color.a - 1.4f * Time.deltaTime);
            }
            ScorePanel.localScale = new Vector2(ScorePanel.localScale.x - 1.4f * Time.deltaTime, ScorePanel.localScale.y - 1.4f * Time.deltaTime);
            //scoreImage.color = new Color(1, 1, 1, scoreImage.color.a - 1.4f * Time.deltaTime);
            TimePanel.localScale = new Vector2(TimePanel.localScale.x - 1.4f * Time.deltaTime, TimePanel.localScale.y - 1.4f * Time.deltaTime);
            backgroundImage.color = new Color(1, 1, 1, backgroundImage.color.a - 1.4f * Time.deltaTime);
        }
        ScorePanel.localScale = Vector2.zero;
        TimePanel.localScale = Vector2.zero;
    }
}
