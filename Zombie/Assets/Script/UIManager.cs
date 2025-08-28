using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public Text ammoText;
    public Text scoreText;
    public Text waveText;

    public GameObject gameOverUi;

    //private Gun gun;

    public void OnEnable()
    {
        SetAmmoText(0, 0);
        SetUpdateScore(0);
        SetWaveText(0, 0);
        GameOverUpdate(false);

    }

    public void SetAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = $"{magAmmo} /{remainAmmo}";
    }

    public void SetUpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void SetWaveText(int wave, int count)
    {
        waveText.text = $"Wave: {wave}\nEnemy Left: {count}";
    }

    public void GameOverUpdate(bool active)
    {
        gameOverUi.SetActive(active);
    }

    public void OnClickReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}