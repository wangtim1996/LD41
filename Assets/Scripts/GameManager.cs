using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    enum State { MENU, GAME, INSTRUCTIONS, WINNER };
    private State currState;
    public GameObject menuScreen;
    public GameObject instructionScreen;
    public GameObject winnerScreen;
    public Transform playerSpawn;
    public Transform bossSpawn;

    public GameObject playerPrefab;

    public GameObject bossPrefab;
    public GameObject airStrikePrefab;
    public GameObject basicPrefab;
    public GameObject laserPrefab;

    public static GameManager Instance;

    private const float SPAWNOFFSET = 2.0f;

    private int numShipsAlive = 0;

    private GameObject currBoss;
    public  List<GameObject> ships;

    public AudioClip blip;
    

    // Use this for initialization
    void Awake()
    {
        ships = new List<GameObject>();
        currState = State.MENU;
        if (Instance != null)
        {
            Debug.Log("GameManger singleton error");
        }
        Instance = this;
        menuScreen.SetActive(true);
        instructionScreen.SetActive(false);
        winnerScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currState)
            {
                case State.MENU:
                    Application.Quit();
                    break;
                case State.GAME:
                case State.INSTRUCTIONS:
                case State.WINNER:
                    foreach (GameObject s in ships)
                    {
                        if (s != null)
                        {
                            Destroy(s);
                        }
                    }
                    AudioManager.Instance.PlayClip(blip);
                    menuScreen.SetActive(true);
                    instructionScreen.SetActive(false);
                    winnerScreen.SetActive(false);

                    currState = State.MENU;
                    break;
                default:
                    break;
            }
        }
    }

    public void StartGameEasy()
    {
        AudioManager.Instance.PlayClip(blip);
        menuScreen.SetActive(false);
        instructionScreen.SetActive(false);
        winnerScreen.SetActive(false);


        numShipsAlive = CleanUp();
        Camera.main.transform.position = new Vector3(0, 0, -10);
        int numships = 2;
        for (int i = 0; i < numships; i++)
        {
            Vector3 pos = new Vector3(i - (numships / 2) * SPAWNOFFSET, 0, 0) + playerSpawn.position;
            GameObject s =Instantiate(playerPrefab, pos, Quaternion.identity);
            ships.Add(s);
            numShipsAlive++;
        }

        currBoss = Instantiate(bossPrefab, bossSpawn.position, Quaternion.identity);
        Transform t;
        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(laserPrefab, t);

        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(basicPrefab, t);

        currState = State.GAME;
        currBoss.GetComponent<Boss>().StartGame();

    }

    public void StartGameMedium()
    {
        AudioManager.Instance.PlayClip(blip);
        menuScreen.SetActive(false);
        instructionScreen.SetActive(false);
        winnerScreen.SetActive(false);

        numShipsAlive = CleanUp();
        Camera.main.transform.position = new Vector3(0, 0, -10);
        int numships = 3;
        for (int i = 0; i < numships; i++)
        {
            Vector3 pos = new Vector3((i - (numships / 2)) * SPAWNOFFSET, 0, 0) + playerSpawn.position;
            GameObject s = Instantiate(playerPrefab, pos, Quaternion.identity);
            ships.Add(s);
            numShipsAlive++;
        }

        currBoss = Instantiate(bossPrefab, bossSpawn.position, Quaternion.identity);
        Transform t;
        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(laserPrefab, t);
        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(laserPrefab, t);
        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(airStrikePrefab, t);
        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(basicPrefab, t);
        currState = State.GAME;
        currBoss.GetComponent<Boss>().StartGame();

    }

    public void StartGameHard()
    {
        AudioManager.Instance.PlayClip(blip);
        menuScreen.SetActive(false);
        instructionScreen.SetActive(false);
        winnerScreen.SetActive(false);

        numShipsAlive = CleanUp();
        Camera.main.transform.position = new Vector3(0, 0, -10);
        int numships = 5;
        for (int i = 0; i < numships; i++)
        {
            Vector3 pos = new Vector3(i - (numships / 2) * SPAWNOFFSET, 0, 0) + playerSpawn.position;
            GameObject s = Instantiate(playerPrefab, pos, Quaternion.identity);
            ships.Add(s);
            numShipsAlive++;
        }

        currBoss = Instantiate(bossPrefab, bossSpawn.position, Quaternion.identity);
        Transform t;
        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(laserPrefab, t);
        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(laserPrefab, t);
        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(basicPrefab, t);
        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(laserPrefab, t);
        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(laserPrefab, t);
        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(airStrikePrefab, t);
        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(airStrikePrefab, t);
        t = currBoss.GetComponent<Boss>().GetWeaponSlot();
        Instantiate(laserPrefab, t);
        currState = State.GAME;
        currBoss.GetComponent<Boss>().StartGame();

    }

    public void Win(bool win)
    {
        if(currState == State.GAME)
        {
            menuScreen.SetActive(false);
            instructionScreen.SetActive(false);
            winnerScreen.SetActive(true);
            if (win)
            {
                Transform goText = winnerScreen.transform.Find("goText");
                if(goText != null)
                {
                    goText.GetComponentInChildren<Text>().text = "You Win";

                }
                Debug.Log("WINNER");

            }
            else
            {
                Transform goText = winnerScreen.transform.Find("goText");
                if (goText != null)
                {
                    goText.GetComponentInChildren<Text>().text = "You Lose";

                }
                Debug.Log("LOSER");

            }

            currState = State.WINNER;

        }
    }

    public void CloseWin()
    {
        AudioManager.Instance.PlayClip(blip);
        menuScreen.SetActive(true);
        instructionScreen.SetActive(false);
        winnerScreen.SetActive(false);

        currState = State.MENU;

        //KILL GAMEOBJECTS
    }

    public void OpenInstr()
    {
        AudioManager.Instance.PlayClip(blip);
        menuScreen.SetActive(false);
        instructionScreen.SetActive(true);
        winnerScreen.SetActive(false);

        currState = State.INSTRUCTIONS;
    }

    public void CloseInstr()
    {
        AudioManager.Instance.PlayClip(blip);
        menuScreen.SetActive(true);
        instructionScreen.SetActive(false);
        winnerScreen.SetActive(false);

        currState = State.MENU;
    }

    public void ShipDestroyed()
    {

        numShipsAlive--;

        if(numShipsAlive <= 0 && currState == State.GAME)
        {
            Win(false);
        }
    }

    private int CleanUp()
    {
        ClearBullets();
        if(currBoss != null)
        {
            Destroy(currBoss);
        }
        int numDestroyed = 0;
        foreach(GameObject s in ships)
        {
            if(s != null)
            {
                numDestroyed++;
                Destroy(s);
            }
        }
        currBoss = null;
        ships.Clear();
        return numDestroyed;
    }

    public void ClearBullets()
    {
        BulletPoolManager.Instance.playerPool.ClearAll();
        BulletPoolManager.Instance.enemyPool.ClearAll();
    }

}
