using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    [SerializeField]
    private float MinXPos;
    [SerializeField]
    private float MaxXPos;
    [SerializeField]
    private float YStartPos;
    [SerializeField]
    private GameObject CrabPrefab;
    public MainConfig config;
    [SerializeField]
    private Transform allCrabsParent;

    private bool IsGameStarted = false;
    private int currentTime  = 0;

    public List<Crab> allCrabs = new();
    public int collectedCrabs = 0;

    private GameView view;
    private Bucket bucket;
    private void Awake()
    {
        bucket = GameObject.FindObjectOfType<Bucket>();
        collectedCrabs = 0;
        view = GameObject.FindObjectOfType<GameView>();
       // InitGame();
    }
    public void InitGame()
    {
        StartCoroutine(InitGameTimer());

    }

    private IEnumerator InitGameTimer()
    {
        currentTime = 0;
        collectedCrabs = 0;
        view.ChangeCrabs(0);
        view.ChangeTime(config.allTimePlaying, config.allTimePlaying);
        view.EnableView();
        while (!bucket.canUse)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.2f);      
        IsGameStarted = true;
        StartCoroutine(GameTimer());
        yield return new WaitForSeconds(0);
        StartCoroutine(CrabGenerator());
    }
    
    private IEnumerator GameTimer()
    {
        while (IsGameStarted)
        {
            yield return new WaitForSeconds(1);
            currentTime++;
            view.ChangeTime(config.allTimePlaying-currentTime, config.allTimePlaying);
            if (currentTime >= config.allTimePlaying)
            {
                IsGameStarted = false;
                List<Crab> all = allCrabs;
                for(int i=0; i<all.Count; i++)
                {
                    if (all[i] != null) {
                        all[i].TakeCrab();
                    }
                }
                GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.State.lose);
            }
        }
        
        
    }

    private IEnumerator CrabGenerator()
    {
        if(currentTime < config.startTimeTo)
        {
            yield return new WaitForSeconds(config.speedOfGenerationStart);
        }
        else if (currentTime >= config.startTimeTo && currentTime <= config.middleTimeTo)
        {
            yield return new WaitForSeconds(config.speedOfGenerationMiddle);
        }
        else
        {
            yield return new WaitForSeconds(config.speedOfGenerationEnd);
        }
        CreateCrab();
        if (IsGameStarted)
        {
            StartCoroutine(CrabGenerator());
        }
    }
    private void CreateCrab()
    {
        if (!IsGameStarted)
        {
            return;
        }
        allCrabs.Add(Instantiate(CrabPrefab, new Vector3(Random.Range(MinXPos, MaxXPos), YStartPos, 0),Quaternion.identity, allCrabsParent).GetComponent<Crab>());
        allCrabs[allCrabs.Count - 1].InitCrab(this, config.fallSpeed);
    }

    public void PlusCrab(Crab crab)
    {
        if (!IsGameStarted) {
            return; 
        }
        
        allCrabs.Remove(crab);
        collectedCrabs++;
        view.ChangeCrabs(collectedCrabs);

        if (collectedCrabs>=config.allCrabsNeedToWin) {
            IsGameStarted = false;
            List<Crab> all = allCrabs;
            for (int i = 0; i < all.Count; i++)
            {
                if (all[i] != null)
                {
                    all[i].TakeCrab();
                }
            }
            StartCoroutine(winTimer());

        }
    }

    private IEnumerator winTimer()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject.FindObjectOfType<GameManager>().ChangeState(GameManager.State.win);
    }
}
