using Settings;
using UnityEngine;

public class SettingsObject : MonoBehaviour
{
    public string Setting = "search";

    public bool Default = false;

    public GameObject[] Objects;

    void Start ()
    {
        foreach (var go in Objects)
        {
            go.SetActive(SettingsManager.GetBool(Setting));
        }
	}
}
