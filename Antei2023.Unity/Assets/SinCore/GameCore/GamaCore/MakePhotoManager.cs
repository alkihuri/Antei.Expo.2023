using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePhotoManager : MonoBehaviour
{
    [SerializeField]
    private PhotoMaker photoMaker;

    private void Start()
    {
        photoMaker = GameObject.FindObjectOfType<PhotoMaker>();
    }


    public void MakePhoto()
    {
       
    }


}
