using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Transform Player;
    public Action OnUpdate;
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        DistanceController.Init(Player);
    }
    private void Update()
    {
        OnUpdate?.Invoke();
    }
    public void GameOver(GameOverCause cause = GameOverCause.None, int delay = 1)
    {
        var title = "";
        switch (cause)
        {
            case GameOverCause.None:
                title = "Game Over!";
                break;
            case GameOverCause.HeadCrack:
                title = "Head cracked!";
                break;
            case GameOverCause.FuelEnd:
                title = "Fuel finished!";
                break;
        }
        StartCoroutine(GameOverActions());
        IEnumerator GameOverActions()
        {
            yield return new WaitForSeconds(delay);
            UIController.instance.OpenGameOverMenu(title);
            Time.timeScale = 0f;
        }
    }
    public void PauseGame()
    {
        UIController.instance.OpenPausePopup(() =>
        {
            Time.timeScale = 1f;
        });
        Time.timeScale = 0f;
    }
}

public enum GameOverCause
{
    None,
    HeadCrack,
    FuelEnd
}
