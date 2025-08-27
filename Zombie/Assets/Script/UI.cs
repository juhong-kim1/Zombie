using UnityEngine;
using UnityEngine.Analytics;

public class UI : MonoBehaviour
{
    public bool IsGameOver { get; private set; }

    public GameObject gameOverUI;

    public void Awake()
    {
        gameOverUI.SetActive(false);
    }

    public void OnPlayerDead()
    {
        IsGameOver = true;
        gameOverUI.SetActive(true);
    }
}
