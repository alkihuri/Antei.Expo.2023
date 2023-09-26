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
    // private Vector2 scorePanelScale;
    // private Vector2 timePanelScale;
    [SerializeField]
    private ParticleSystem[] Particles;
 

     [HideInInspector]
    public int needToWin=100;

    [SerializeField]
    private TMP_Text bubbleTMP;
    [SerializeField]
    private Image[] bubbleImage;
    [SerializeField]
    private Animator bubbleAnim;


    private void Awake()
    {
        Particles[0].Stop();
        Particles[1].Stop();
        bubbleTMP.enabled = false;
        foreach(Image img in bubbleImage)
        {
            img.color = new Color(1, 1, 1, 0);
        }
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
        StartCoroutine(showUI());
    }

    public override void DisableView()
    {
        StartCoroutine(closeUI());
    }

    private IEnumerator showUI()
    {
        Particles[0].Play();
        Particles[1].Play();
        ScorePanel.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        while (LogoImage.color.a < 1)
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

        bubbleAnim.SetTrigger("PassiveBecome");
        bubbleTMP.enabled = true;
        while(bubbleImage[bubbleImage.Length-1].color.a < 1)
        {
            foreach (Image img in bubbleImage)
            {
                yield return new WaitForFixedUpdate();
                img.color = new Color(1, 1, 1, img.color.a+1.4f*Time.deltaTime);
            }
        }
        yield return new WaitForSeconds(3);
        while (bubbleImage[bubbleImage.Length - 1].color.a > 0)
        {
            foreach (Image img in bubbleImage)
            {
                yield return new WaitForFixedUpdate();
                img.color = new Color(1, 1, 1, img.color.a - 1.4f * Time.deltaTime);
            }
        }
        bubbleTMP.enabled = false;
       
        
    }
    private IEnumerator closeUI()
    {
        Particles[0].Stop();
        Particles[1].Stop();
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
