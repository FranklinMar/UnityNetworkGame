using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // Current version of the game
    [SerializeField] private string VERSION = "v0.0.1";
    // Input field for changing username
    [SerializeField] private TMP_InputField nameInputField;
    // Input field for connecting the room field in "Connect" menu
    [SerializeField] private TMP_InputField connectGameInput;
    // IP address text field for displaying room name in "Host" field
    [SerializeField] private TMP_Text IPaddress;
    // Max player amount constant
    private const int maxPlayerAmount = 2;

    // Default username and key for user preferences data
    private const string DefaultUsername = "Username";
    // Current Username
    public static string Username { get; private set; }

    public void Start()
    {
        // Connect to current version network
        PhotonNetwork.ConnectUsingSettings(VERSION);
        // If username wasn't set
        if (!PlayerPrefs.HasKey(DefaultUsername)) {
            // Set default username
            PlayerPrefs.SetString(DefaultUsername, DefaultUsername); 
        }
        // Get name from user prefernces
        string defaultName = PlayerPrefs.GetString(DefaultUsername);
        // Set name for username input field
        nameInputField.text = defaultName;
        // Set username for current user in network
        PhotonNetwork.playerName = defaultName;
        IPaddress.text = defaultName;
    }

    public void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(VERSION);
    }

    // Change name event function
    public void setName()
    {
        // If is empty
        if (string.IsNullOrEmpty(nameInputField.text) || nameInputField.text.Length == 0)
        {
            // Change to default
            nameInputField.text = DefaultUsername;
        }
        PlayerPrefs.SetString(DefaultUsername, nameInputField.text);
        PhotonNetwork.playerName = nameInputField.text;
        IPaddress.text = nameInputField.text;
    }

    // Exit application function
    public void QuitGame()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Quit");
        Application.Quit();
    }

    // When connnected to master network event function
    public void OnConnectedToMaster()
    {
        // Join default lobby
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    // Create room based on username
    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(PlayerPrefs.GetString(DefaultUsername), new RoomOptions() { MaxPlayers = maxPlayerAmount, IsVisible=true, IsOpen=true }, TypedLobby.Default);
    }

    // Connect to the room if exists, else create
    public void ConnectGame()
    {
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = maxPlayerAmount, IsVisible = true, IsOpen = true };
        PhotonNetwork.JoinOrCreateRoom(connectGameInput.text, roomOptions, TypedLobby.Default);
    }

    // When joined room from lobby event function
    public void OnJoinedRoom()
    {
        // Load game scene in build settings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // Load multiplayer scene
        PhotonNetwork.LoadLevel("FoxGameScene");
    }
}
