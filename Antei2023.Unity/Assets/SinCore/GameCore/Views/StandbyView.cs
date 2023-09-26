using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StandbyView : View
{
    [SerializeField]
    private UnityEngine.Video.VideoPlayer video;
    [SerializeField]
    private TMPro.TMP_Text text;
    [SerializeField]
    private KinectManager kinectManager;
    private float timer = 0;
    [SerializeField]
    private GameManager gameManager;

    private void Start()
    {
        OnVideoEnd(video);
    }

    void OnEnable() //Сначала подписываем нашу функцию на событие конца видео
    {
        video.loopPointReached += OnVideoEnd;
    }

    void OnDisable() //Отписываем для предотвращения утечки памяти
    {
        video.loopPointReached -= OnVideoEnd;
    }

    public void OnVideoEnd(UnityEngine.Video.VideoPlayer causedVideoPlayer)
    {
        text.GetComponent<Animator>().SetTrigger("Play");
    }

    private void FixedUpdate()
    {

        if (video.targetCameraAlpha>0)
        {
            if (kinectManager.IsUserDetected() && gameManager.currentState==GameManager.State.standby)
            {
                timer +=Time.deltaTime;
                if (timer > 3)
                {
                    BtnClick();
                }
            }
            else
            {
                timer = 0;
            }
            /*if (Input.anyKey)
            {
                BtnClick();
            }*/
        }
    }

    public void BtnClick()
    {
        print("Btn click");
        GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.State.game);
    }

    public override void EnableView()
    {
        StopAllCoroutines();
        StartCoroutine(videoAnim(true));
    }

    public override void DisableView()
    {
        StopAllCoroutines();
        StartCoroutine(videoAnim(false));
    }

    private IEnumerator videoAnim(bool showOn)
    {
        if (showOn)
        {
            text.enabled = true;
            while (video.targetCameraAlpha < 1)
            {
                yield return new WaitForFixedUpdate();
                video.targetCameraAlpha += Time.deltaTime;
            }
        }
        else
        {
            text.enabled = false;
            while (video.targetCameraAlpha >0)
            {
                yield return new WaitForFixedUpdate();
                video.targetCameraAlpha -= Time.deltaTime;
            }
        }
    }
}
