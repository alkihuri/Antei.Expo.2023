using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum State
    {
        standby=1,
        tutorial=2,
        game=3,
        photo=4,
        lose=5,
        win=6
    }

    private State currentState = State.standby;


    [SerializeField]
    private StandbyView standbyView;
    [SerializeField]
    private GameView gameView;
    [SerializeField]
    private WinLoseView winLoseView;
    [SerializeField]
    private MakePhotoView makePhotoView;

    [SerializeField]

    private GameCore gameCore;
    private void Start()
    {
        ChangeState(State.standby);
    }

    public void ChangeState(State newState)
    {
        print(newState.ToString());
        if (newState == State.standby)
        {
            winLoseView.DisableView();
            standbyView.EnableView();
            
        }
        else if(newState == State.game)
        {
            standbyView.DisableView();
            //gameView.EnableView();
            gameCore.InitGame();
        }
        else if (newState == State.lose)
        {
            gameView.DisableView();
            winLoseView.EnableView();
        }
        else if (newState == State.win)
        {
            gameView.DisableView();
            winLoseView.EnableView();
        }
        else if (newState == State.photo)
        {
            
        }
        else if (newState == State.tutorial)
        {

        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("escape"))  // если нажата клавиша Esc (Escape)
        {
            Application.Quit();    // закрыть приложение
        }
    }
}
