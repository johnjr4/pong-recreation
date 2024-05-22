using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    private bool player1Turn = true;
    private bool canPause = false;
    public Rigidbody2D ball;
    [SerializeField]
    private bool debugMode = false;
    [SerializeField]
    private Vector2 debugVector = new Vector2(0, 1);
    private Rigidbody2D ballInstance;
    public GameObject player;
    public CameraShake cameraShake;
    public float startSpeed = 0f;
    private int score1, score2 = 0;
    public TextMeshProUGUI scoreText1, scoreText2, countdownText;
    public Button replayButton, quitButton;
    public GameObject particles;
    public EventSystem eventSystem;
    public GameObject arrow;
    public GameObject pauseMenu;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip ballDeath, scoreSound, lowBeep, highBeep;
    [SerializeField]
    private AudioClip[] victorySounds;

    private PlayerInput p1, p2;
    private float p2Pos;

    // Start is called before the first frame update
    void Start()
    {
        // DrawDebugRays()

        audioSource = GetComponent<AudioSource>();

        StartGame();
    }

    void DrawDebugRays()
    {
        Debug.DrawRay(Vector3.zero, new Vector2(1f, .8f) * 8f, Color.white, 200f, false);
        Debug.DrawRay(Vector3.zero, new Vector2(1f, -.8f) * 8f, Color.white, 200f, false);
        Debug.DrawRay(Vector3.zero, new Vector2(1f, .1f) * 8f, Color.white, 200f, false);
        Debug.DrawRay(Vector3.zero, new Vector2(1f, -.1f) * 8f, Color.white, 200f, false);
        Debug.DrawRay(Vector3.zero, new Vector2(-1f, .8f) * 8f, Color.white, 200f, false);
        Debug.DrawRay(Vector3.zero, new Vector2(-1f, -.8f) * 8f, Color.white, 200f, false);
        Debug.DrawRay(Vector3.zero, new Vector2(-1f, .1f) * 8f, Color.white, 200f, false);
        Debug.DrawRay(Vector3.zero, new Vector2(-1f, -.1f) * 8f, Color.white, 200f, false);
    }


    public void StartGame()
    {
        GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>().enabled = true;
        pauseMenu.SetActive(false); // Disables the pause menu
        arrow.SetActive(false); // Disables the arrow
        eventSystem.SetSelectedGameObject(null); // Deselects button
        eventSystem.enabled = false; // Disables UI EventSystem. Not sure if necessary
        countdownText.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        score1 = 0;
        score2 = 0;
        scoreText1.text = score1.ToString();
        scoreText2.text = score2.ToString();
        scoreText1.gameObject.SetActive(true);
        scoreText2.gameObject.SetActive(true);

        // Instantiate two players using a split-keyboard setup. Not totally sure how it works
        p1 = PlayerInput.Instantiate(player,
            controlScheme: "WASD",
            pairWithDevice: Keyboard.current);
        p1.gameObject.transform.position = new Vector3(-5.5f, 0, 0);
        p2 = PlayerInput.Instantiate(player,
            controlScheme: "ArrowKeys",
            pairWithDevice: Keyboard.current);
        p2.gameObject.transform.position = new Vector3(5.5f, 0, 0);

        StartCoroutine(SpawnBall());
    }

    public void endRound(GameObject ball)
    {
        audioSource.PlayOneShot(ballDeath, 0.5f);
        StartCoroutine(cameraShake.Shake());
        ball.GetComponent<DropShadow>().DestroyShadow(); // Destroys shadow using helper method
        Vector2 ballPos = ball.transform.position;
        Destroy(ball);
        Instantiate(particles, ballPos, Quaternion.identity);
        canPause = false;

        StartCoroutine(UpdateScore(ballPos));
    }

    IEnumerator SpawnBall()
    {
        yield return new WaitForSeconds(0.5f);
        countdownText.text = 3.ToString();
        countdownText.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            audioSource.PlayOneShot(lowBeep);
            yield return new WaitForSeconds(1f);

        }

        // Rigidbody2D ballInstance;
        ballInstance = Instantiate(ball);
        ballInstance.velocity = GenerateStartVect(debugMode);
        player1Turn = !player1Turn; // Toggles turn
        countdownText.gameObject.SetActive(false);
        canPause = true;
        audioSource.PlayOneShot(highBeep);

    }
    IEnumerator UpdateScore(Vector2 ballPos)
    {
        yield return new WaitForSeconds(0.5f);
        if (ballPos.x > 0)
        {
            score1++;
            scoreText1.text = score1.ToString();
        } else
        {
            score2++;
            scoreText2.text = score2.ToString();
        }
        audioSource.PlayOneShot(scoreSound);

        // Score check for end game state or not
        if (score1 >= SettingsManager.winScore )
        {
            StartCoroutine(EndGame(true));
        } else if (score2 >= SettingsManager.winScore)
        {
            StartCoroutine(EndGame(false));
        } else
        {
            StartCoroutine(SpawnBall());
        }

    }

    Vector2 GenerateStartVect(bool debug)
    {
        // Debug vector: straight up
        if (debug)
        {
            return debugVector * startSpeed;
        }

        float xComp = 1f;
        if (player1Turn) { xComp = -1f; }
        float yComp = Random.Range(0.1f, 0.8f); // Picks a y value such that 0.1 <= y <= 0.8
        if (Random.value < 0.5f) { yComp = -yComp; } // Randomly determines if y value should be negative
        return new Vector2(xComp, yComp).normalized * startSpeed; // Returns vector from components
    }

    IEnumerator EndGame(bool player1Win)
    {
        yield return new WaitForSeconds(1f);
        if (player1Win)
        {
            countdownText.text = "Player 1 wins!";
        } else
        {
            countdownText.text = "Player 2 wins!";
        }

        DisableScene();

        StartCoroutine(cameraShake.Shake());
        countdownText.gameObject.SetActive(true);
        audioSource.clip = victorySounds[Random.Range(0, victorySounds.Length)];
        audioSource.Play();

        yield return new WaitWhile(() => audioSource.isPlaying);

        eventSystem.enabled = true;
        replayButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        replayButton.Select();
        arrow.SetActive(true);
    }

    private void Update()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame && canPause) PauseGame();
    }

    private void PauseGame()
    {
        // Timescale freezes game
        Time.timeScale = 0;
        canPause = false;
        p1.gameObject.SetActive(false);
        p1.gameObject.GetComponent<DropShadow>().ShadowEnabled(false);

        // p2 has to be destroyed and reinstantiated in resume() or input system breaks
        p2Pos = p2.gameObject.transform.position.y;
        p2.gameObject.GetComponent<DropShadow>().DestroyShadow();
        Destroy(p2.gameObject);

        // Disable renderer for ball so that it keeps going once you unpause
        ballInstance.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        ballInstance.gameObject.GetComponent<DropShadow>().ShadowEnabled(false);

        scoreText1.enabled = false;
        scoreText2.enabled = false;
        GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>().enabled = false;

        eventSystem.enabled = true;
        pauseMenu.SetActive(true);
        pauseMenu.GetComponentInChildren<Button>().Select();
        arrow.SetActive(true);
    }

    public void Resume()
    {
        eventSystem.enabled = false;
        pauseMenu.SetActive(false);
        arrow.SetActive(false);

        eventSystem.SetSelectedGameObject(null);

        p1.gameObject.SetActive(true);
        p1.gameObject.GetComponent<DropShadow>().ShadowEnabled(true);

        // Reinstantiates p2 due to afforementioned input system issue
        p2 = PlayerInput.Instantiate(player,
            controlScheme: "ArrowKeys",
            pairWithDevice: Keyboard.current);
        p2.gameObject.transform.position = new Vector3(5.5f, p2Pos, 0);

        ballInstance.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        ballInstance.gameObject.GetComponent<DropShadow>().ShadowEnabled(true);

        scoreText1.enabled = true;
        scoreText2.enabled = true;
        GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>().enabled = true;


        Time.timeScale = 1;
        canPause = true;
    }

    public void ExitGame()
    {
        StartCoroutine(ICantCode(0.2f));
    }

    private IEnumerator ICantCode(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void DisableScene()
    {
        scoreText1.gameObject.SetActive(false);
        scoreText2.gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>().enabled = false;
        p1.gameObject.GetComponent<DropShadow>().DestroyShadow();
        Destroy(p1.gameObject);
        p2.gameObject.GetComponent<DropShadow>().DestroyShadow();
        Destroy(p2.gameObject);
    }

}
