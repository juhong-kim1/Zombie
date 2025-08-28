using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    private int score;

    public bool IsGameOver { get; private set; }

    public void Start()
    {
        var findGo = GameObject.FindWithTag("Player");
        var playerHealth = findGo.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            //playerHealth.onDeath += EndGame;
        }
    }

    public void AddScore(int add)
    {
        score += add;
        uiManager.SetUpdateScore(score);
    }

    public void EndGame()
    {
        IsGameOver = true;
        uiManager.GameOverUpdate(true);
    }

}



