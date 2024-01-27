using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField playerNameInputField;
    public GameObject UI_LoginGameobject;

    [Header("Lobby UI")]
    public GameObject UI_LobbyGameobject;
    public GameObject UI_3DGameobject;

    [Header("Connection Status UI")]
    public GameObject UI_ConnectionStatusGameobject;
    public Text connectionStatusText;
    public bool showConnectionStatus = false;

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnected) 
        {
            //Activating only Lobby UI
            UI_LobbyGameobject.SetActive(true);
            UI_3DGameobject.SetActive(true);
            UI_ConnectionStatusGameobject.SetActive(false);
            UI_LoginGameobject.SetActive(false);
        }
        else
        {
            //Activating only login UI since we did not connect to Photon yet
            UI_LobbyGameobject.SetActive(false);
            UI_3DGameobject.SetActive(false);
            UI_ConnectionStatusGameobject.SetActive(true);
            UI_LoginGameobject.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(showConnectionStatus) 
        {
            connectionStatusText.text = "Connection status: " + PhotonNetwork.NetworkClientState;

        }
    }
    #endregion

    #region UI Callback Methods
    public void OnEnterGameButtonClicked()
    {

        string playerName = playerNameInputField.text;

        if (!string.IsNullOrEmpty(playerName) )
        {
            UI_LobbyGameobject.SetActive(false);
            UI_3DGameobject.SetActive(false);
            UI_ConnectionStatusGameobject.SetActive(true);
            UI_LoginGameobject.SetActive(false);

            showConnectionStatus = true;

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("Player name is invalid");
        }
    }

    public void OnQuickMatchButtonClicked()
    {
        //SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }

    #endregion

    #region Photon Callback Methods

    public override void OnConnected()
    {
        Debug.Log("We connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon Server");
        
        UI_LobbyGameobject.SetActive(true);
        UI_3DGameobject.SetActive(true);
        UI_ConnectionStatusGameobject.SetActive(false);
        UI_LoginGameobject.SetActive(false);
    }
    #endregion


}
