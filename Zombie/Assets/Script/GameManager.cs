using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool IsGameOver { get; private set; }

    public GameObject gameOverUI;
    public Text ammoText;
    public Text score;
    public Gun gun;

    public void Awake()
    {
        gameOverUI.SetActive(false);
    }

    private void Update()
    {
        ammoText.text = gun.magAmmo + "/" + gun.ammoRemain;
    }

    public void OnPlayerDead()
    {
        IsGameOver = true;
        gameOverUI.SetActive(true);
    }
}
