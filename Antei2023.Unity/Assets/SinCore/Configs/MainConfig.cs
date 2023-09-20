using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MainConfig", order = 1)]
public class MainConfig : ScriptableObject
{
    [Header("Main options")]
    public float fallSpeed;
    public int allTimePlaying;
    public int allCrabsNeedToWin;
    [Header("Extra options")]
    public float speedOfGenerationStart;
    public float speedOfGenerationMiddle;
    public float speedOfGenerationEnd;
    public float speedVariations;
    public float startTimeTo;
    public float middleTimeTo;
}
