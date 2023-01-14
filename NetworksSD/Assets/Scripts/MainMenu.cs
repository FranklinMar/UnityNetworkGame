using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string VERSION = "v0.0.1";
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField connectGameInput;
    [SerializeField] private TMP_Text IPaddress;
    private const int maxPlayerAmount = 2;

    private const string DefaultUsername = "Username";
    public static string Username { get; private set; } = DefaultUsername;

    public void Start()
    {
        PhotonNetwork.ConnectUsingSettings(VERSION);
        if (!PlayerPrefs.HasKey(DefaultUsername)) { 
            PlayerPrefs.SetString(DefaultUsername, DefaultUsername); 
        }

        string defaultName = PlayerPrefs.GetString(DefaultUsername);
        nameInputField.text = defaultName;
        PhotonNetwork.playerName = defaultName;
        IPaddress.text = defaultName;
    }

    public void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(VERSION);
    }

    public void setName()
    {
        if (string.IsNullOrEmpty(nameInputField.text) || nameInputField.text.Length == 0)
        {
            nameInputField.text = DefaultUsername;
            //PhotonNetwork.playerName = nameInputField.text;
            //IPaddress.text = nameInputField.text;
            //return;
        }
        PlayerPrefs.SetString(DefaultUsername, nameInputField.text);
        PhotonNetwork.playerName = nameInputField.text;
        IPaddress.text = nameInputField.text;

    }

    public void PlayGame()
    {
        CreateGame();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(PlayerPrefs.GetString(DefaultUsername), new RoomOptions() { MaxPlayers = maxPlayerAmount, IsVisible=true, IsOpen=true }, TypedLobby.Default);
        //PhotonNetwork.JoinRoom(PlayerPrefs.GetString(DefaultUsername));
        //RoomOptions roomOptions = new RoomOptions() { MaxPlayers = maxPlayerAmount };
        //PhotonNetwork.JoinRoom(connectGameInput.text);
        //PhotonNetwork.JoinOrCreateRoom(connectGameInput.text, roomOptions, TypedLobby.Default);
    }

    public void ConnectGame()
    {
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = maxPlayerAmount, IsVisible = true, IsOpen = true };
        //PhotonNetwork.JoinRoom(connectGameInput.text);
        PhotonNetwork.JoinOrCreateRoom(connectGameInput.text, roomOptions, TypedLobby.Default);
    }

    public void OnJoinedRoom()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PhotonNetwork.LoadLevel("FoxGameScene");
    }
}
