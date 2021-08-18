using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialLevelManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject Camera;
    public GameObject spawnPoint;

    public GameObject indicatorMiniGame;
    public GameObject stopSignMiniGame;
    public GameObject giveWayMiniGame;
    public GameObject threeSecRuleMiniGame;

    [SerializeField] private GameObject sessionCompletedMenu;
    [SerializeField] private GameObject tutorialCompletedMenu;

    public MiniGame currentGame;

    [SerializeField] private GameObject []playerSpawnPoint;
    [SerializeField] private GameObject []cameraPos;

    private GameState gameState;

    [SerializeField] private GameObject AICar;
    [SerializeField] private GameObject miniGameNameText;

    public enum MiniGame
    {
        indicatorMini,
        stopSignMini,
        giveWayMini,
        threeSecRuleMini
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = FindObjectOfType<GameState>();
        currentGame = MiniGame.indicatorMini;

        StartCoroutine(UpdateSessionText("Indicator Session"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onContinueButtonClick()
    {
        switch (currentGame)
        {
            case MiniGame.indicatorMini:
                currentGame = MiniGame.stopSignMini;
                stopSignMiniGame.SetActive(true);
                indicatorMiniGame.SetActive(false);
                sessionCompletedMenu.SetActive(false);

                Time.timeScale = 1f;
                ResetPlayer();
                Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameState.currentState = GameState.States.Running;
                StartCoroutine(UpdateSessionText("Stop Sign Session"));

                break;

            case MiniGame.stopSignMini:               
                currentGame = MiniGame.giveWayMini;  
                giveWayMiniGame.SetActive(true);
                stopSignMiniGame.SetActive(false);
                sessionCompletedMenu.SetActive(false);


                Time.timeScale = 1f;
                ResetPlayer();
                Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameState.currentState = GameState.States.Running;
                StartCoroutine(UpdateSessionText("Giveway Session"));

                break;

            case MiniGame.giveWayMini:
                currentGame = MiniGame.threeSecRuleMini;
                threeSecRuleMiniGame.SetActive(true);
                giveWayMiniGame.SetActive(false);
                sessionCompletedMenu.SetActive(false);

                Time.timeScale = 1f;
                ResetPlayer();
                Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameState.currentState = GameState.States.Running;
                StartCoroutine(UpdateSessionText("Three Sec Session"));
                AICar.GetComponent<Animator>().SetTrigger("MoveAI");

                break;

            case MiniGame.threeSecRuleMini:
                threeSecRuleMiniGame.SetActive(false);

                Time.timeScale = 1f;
                ResetPlayer();
                Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameState.currentState = GameState.States.Running;

                break;
        }
    }

    public void TutorialLevelCompleted()
    {
        Time.timeScale = 0f;
        gameState.currentState = GameState.States.IsPause;
        //Pausing game

        if (currentGame == MiniGame.threeSecRuleMini)
        {
            tutorialCompletedMenu.SetActive(true);
            //Display Tutorial level completed menu
        }
        else
        {
            sessionCompletedMenu.SetActive(true);
            //Display Session Completed Menu
        }
    }

    public void ResetPlayer()
    {
        switch (currentGame)
        {
            case MiniGame.indicatorMini:
                Player.transform.position = playerSpawnPoint[0].transform.position;
                Player.transform.rotation = playerSpawnPoint[0].transform.rotation;
                Camera.transform.position = cameraPos[0].transform.position;
                Camera.transform.rotation = cameraPos[0].transform.rotation;
                Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                break;

            case MiniGame.stopSignMini:
                Player.transform.position = playerSpawnPoint[1].transform.position;
                Player.transform.rotation = playerSpawnPoint[1].transform.rotation;
                Camera.transform.position = cameraPos[1].transform.position;
                Camera.transform.rotation = cameraPos[1].transform.rotation;
                Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                break;

            case MiniGame.giveWayMini:
                Player.transform.position = playerSpawnPoint[2].transform.position;
                Player.transform.rotation = playerSpawnPoint[2].transform.rotation;
                Camera.transform.position = cameraPos[2].transform.position;
                Camera.transform.rotation = cameraPos[2].transform.rotation;
                Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                break;

            case MiniGame.threeSecRuleMini:
                Player.transform.position = playerSpawnPoint[3].transform.position;
                Player.transform.rotation = playerSpawnPoint[3].transform.rotation;
                Camera.transform.position = cameraPos[3].transform.position;
                Camera.transform.rotation = cameraPos[3].transform.rotation;
                Player.GetComponent<Rigidbody>().velocity = Vector3.zero;

                AICar.GetComponent<Animator>().SetTrigger("MoveAI");
                break;
        }
    }

    private IEnumerator UpdateSessionText(string name)
    {
        miniGameNameText.SetActive(true);
        miniGameNameText.GetComponent<TextMeshProUGUI>().text = name;
        yield return new WaitForSeconds(5f);
        miniGameNameText.GetComponent<TextMeshProUGUI>().text = "";
        miniGameNameText.SetActive(false);
    }
}
