using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EmailManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private TMP_InputField emailTMP;

    [SerializeField]
    private EmailTest emailTest;

    private void Start()
    {
        emailTMP.text = "";
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Return) && gameManager.currentState==GameManager.State.email)
        {
            EmailCompleted();
        }
        emailTMP.ActivateInputField();
    }

    public void StartEmail()
    {
        emailTMP.text = "";       
    }

    public void EmailCompleted()
    {
        emailTest.Send(emailTMP.text);
        emailTMP.text = "";
        gameManager.ChangeState(GameManager.State.end);

    }
}
