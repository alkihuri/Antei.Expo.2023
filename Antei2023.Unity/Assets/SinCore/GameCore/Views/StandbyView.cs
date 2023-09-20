using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandbyView : View
{
    [SerializeField]
    private GameObject video;

    private void FixedUpdate()
    {
        if (video.activeSelf)
        {
            if (Input.anyKey)
            {
                BtnClick();
            }
        }
    }

    public void BtnClick()
    {
        print("Btn click");
        GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.State.game);
    }

    public override void EnableView()
    {
        video.SetActive(true);
    }

    public override void DisableView()
    {
        video.SetActive(false);
    }
}
