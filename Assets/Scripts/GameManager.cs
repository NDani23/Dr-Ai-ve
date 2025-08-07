using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{

    [SerializeField] Player player;
    [SerializeField] Player agent;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject menuUI;
    [SerializeField] Text timeText;
    [SerializeField] Text playerLastTimeText;
    [SerializeField] Text agentLastTimeText;
    [SerializeField] Text playerBestTimeText;
    [SerializeField] Text agentBestTimeText;
    [SerializeField] private GameObject app;
    public static GameManager Instance { get; private set; }

    private TrackScript[] tracks;
    private int currentTrackIdx;

    private float time = 0.0f;

    private float playerLastLapTime = 0.0f;
    private float agentLastLapTime = 0.0f;
    private float playerBestLapTime = 0.0f;
    private float agentBestLapTime = 0.0f;

    private bool drive = false;
    private bool game = false;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tracks = app.GetComponentsInChildren<TrackScript>();
        currentTrackIdx = 0;


        for (int i = 0; i < tracks.Length; i++)
        {
            tracks[i].gameObject.SetActive(i == currentTrackIdx ? true : false);
        }

        player.setTrack(ref tracks[currentTrackIdx]);
        agent.setTrack(ref tracks[currentTrackIdx]);
    }

    public void setNextTrack()
    {
        tracks[currentTrackIdx].gameObject.SetActive(false);
        currentTrackIdx = (currentTrackIdx + 1) % (tracks.Length);
        tracks[currentTrackIdx].gameObject.SetActive(true);

        player.setTrack(ref tracks[currentTrackIdx]);
        agent.setTrack(ref tracks[currentTrackIdx]);
    }

    public void setPrevTrack()
    {
        tracks[currentTrackIdx].gameObject.SetActive(false);
        currentTrackIdx = currentTrackIdx - 1 < 0 ? tracks.Length-1 : currentTrackIdx - 1;
        tracks[currentTrackIdx].gameObject.SetActive(true);

        player.setTrack(ref tracks[currentTrackIdx]);
        agent.setTrack(ref tracks[currentTrackIdx]);
    }

    public void setLapTime(float lapTime, int driverType)
    {
        if(driverType == 1)
        {
            playerLastLapTime = lapTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(playerLastLapTime);
            playerLastTimeText.text = timeSpan.ToString(@"mm\:ss\:ff");

            if (playerLastLapTime < playerBestLapTime || playerBestLapTime == 0.0f)
            {
                playerBestLapTime = playerLastLapTime;
                timeSpan = TimeSpan.FromSeconds(playerBestLapTime);
                playerBestTimeText.text = timeSpan.ToString(@"mm\:ss\:ff");
            }

            time = 0.0f;
        }
        else
        {
            agentLastLapTime = lapTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(agentLastLapTime);
            agentLastTimeText.text = timeSpan.ToString(@"mm\:ss\:ff");

            if (agentLastLapTime < agentBestLapTime || agentBestLapTime == 0.0f)
            {
                agentBestLapTime = agentLastLapTime;
                timeSpan = TimeSpan.FromSeconds(agentBestLapTime);
                agentBestTimeText.text = timeSpan.ToString(@"mm\:ss\:ff");
            }

        }


        if(playerBestLapTime < agentBestLapTime && playerBestLapTime != 0.0f)
        {
            agentBestTimeText.color = Color.black;
            playerBestTimeText.color = Color.green;
        }
        else if(playerBestLapTime > agentBestLapTime && agentBestLapTime != 0.0f)
        {
            agentBestTimeText.color = Color.green;
            playerBestTimeText.color = Color.black;
        }
    }

    public bool IsGameTime()
    {
        return game;
    }

    public void StartGame()
    {
        time = 0.0f;
        menuUI.SetActive(false);
        gameUI.SetActive(true);
        game = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !drive && game)
        {
            player.StartDrive();
            agent.StartDrive();
            drive = true;           
        }

        if (Input.GetKey(KeyCode.Escape) && game)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //EndGame();
        }

        if (!drive) return;
        time += Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        timeText.text = timeSpan.ToString(@"mm\:ss\:ff");
    }
}
