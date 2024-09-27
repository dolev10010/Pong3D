using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject startButton;
    public GameObject playAgainButton;
    public Text winMessageText;
    public GameObject ball;
    public GameObject[] paddles;

    public Text player1ScoreText;
    public Text player2ScoreText;

    private void Start()
    {
        if (startButton == null || playAgainButton == null || winMessageText == null)
        {
            Debug.LogError("Start Button, Play Again Button, or Win Message Text is not assigned in the Inspector.");
            return;
        }

        ShowStartButton();
    }

    private void ShowStartButton()
    {
        startButton.SetActive(true);
        playAgainButton.SetActive(false);
        winMessageText.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        startButton.SetActive(false);
        playAgainButton.SetActive(false);
        winMessageText.gameObject.SetActive(false);
        Time.timeScale = 1f;
        ResetBallAndPaddles();
    }

    public void ResetGame()
    {
        GameManager.Instance.ResetGame();
        playAgainButton.SetActive(false);
        winMessageText.text = "";
        winMessageText.gameObject.SetActive(false);
        UpdateScoreUI(1, 0);
        UpdateScoreUI(2, 0);
        ResetBallAndPaddles();
        Time.timeScale = 1f;
    }

    private void ResetBallAndPaddles()
    {
        if (ball != null)
        {
            ball.transform.position = Vector3.zero;
            ball.GetComponent<BallController>().StartBall();
        }

        if (paddles != null)
        {
            foreach (GameObject paddle in paddles)
            {
                paddle.transform.position = new Vector3(paddle.transform.position.x, 0f, paddle.transform.position.z);
            }
        }
    }

    public void UpdateScoreUI(int playerIndex, int score)
    {
        if (playerIndex == 1)
        {
            player1ScoreText.text = score.ToString();
        }
        else if (playerIndex == 2)
        {
            player2ScoreText.text = score.ToString();
        }
    }

    public void GameOver(string winnerMessage)
    {
        playAgainButton.SetActive(true);
        winMessageText.gameObject.SetActive(true);
        winMessageText.text = winnerMessage;
        Time.timeScale = 0f;
    }
}
