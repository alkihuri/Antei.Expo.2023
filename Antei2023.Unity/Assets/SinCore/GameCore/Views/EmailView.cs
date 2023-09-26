using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
public class EmailView : View
{
    [SerializeField]
    private Image[] allBGImages;
    [SerializeField]
    private GameObject[] allGameObjects;
    [SerializeField]
    private PhotoMaker photoMaker;
    [SerializeField]
    private Image photoImage;

    [SerializeField]
    private GameObject BubbleEnd;
    private void Start()
    {
        BubbleEnd.transform.localScale = Vector2.zero;
        foreach (GameObject go in allGameObjects)
        {
            go.SetActive(false);
        }
        foreach (Image img in allBGImages)
        {
            img.color = new Color(1, 1, 1, 0);
        }
    }
    public override void EnableView()
    {
        StopAllCoroutines();
        StartCoroutine(showUI());
    }
    public override void DisableView()
    {
        StopAllCoroutines();
        StartCoroutine(closeUI());
    }
    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
    private IEnumerator showUI()
    {
        photoImage.sprite = Sprite.Create(LoadPNG(photoMaker.PhotoPath[photoMaker.PhotoPath.Count - 1]), new Rect(0,0,768,768),Vector2.zero,100);
        while (allBGImages[allBGImages.Length - 1].color.a < 1)
        {
            yield return new WaitForFixedUpdate();
            foreach (Image img in allBGImages)
            {
                img.color = new Color(1, 1, 1, img.color.a + 1.4f * Time.deltaTime);
            }
        }
        foreach(GameObject go in allGameObjects)
        {
            go.SetActive(true);
        }
    }

    private IEnumerator closeUI()
    {

        foreach (GameObject go in allGameObjects)
        {
            go.SetActive(false);
        }
        while (allBGImages[allBGImages.Length - 1].color.a > 0)
        {
            yield return new WaitForFixedUpdate();
            foreach (Image img in allBGImages)
            {
                img.color = new Color(1, 1, 1, img.color.a - 1.4f * Time.deltaTime);
            }
        }       
    }


    public void End()
    {
        StopAllCoroutines();
        StartCoroutine(endAnim());
    }


    private IEnumerator endAnim()
    {
        while (allBGImages[0].transform.localScale.x > 0)
        {
            yield return new WaitForFixedUpdate();
            allBGImages[0].transform.localScale = new Vector2(allBGImages[0].transform.localScale.x - 1.4f * Time.deltaTime, allBGImages[0].transform.localScale.y - 1.4f * Time.deltaTime);
        }
        while (BubbleEnd.transform.localScale.x < 1)
        {
            yield return new WaitForFixedUpdate();
            BubbleEnd.transform.localScale = new Vector2(BubbleEnd.transform.localScale.x+1.4f*Time.deltaTime, BubbleEnd.transform.localScale.y + 1.4f * Time.deltaTime);
        }
        yield return new WaitForSeconds(2);
        while (BubbleEnd.transform.localScale.x > 0)
        {
            yield return new WaitForFixedUpdate();
            BubbleEnd.transform.localScale = new Vector2(BubbleEnd.transform.localScale.x - 1.4f * Time.deltaTime, BubbleEnd.transform.localScale.y - 1.4f * Time.deltaTime);
        }
        GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.State.standby);
    }
}
