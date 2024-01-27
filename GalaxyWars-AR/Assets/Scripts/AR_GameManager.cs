using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class AR_GameManager : MonoBehaviourPunCallbacks
{

    [Header("UI")]
    public GameObject UI_InformPanelGameObject;
    public TextMeshProUGUI UI_InformText;
    public GameObject searchForGamesButtonGameObject;
    public GameObject adjust_Button;
    public GameObject raycastCenter_Image;


    // Start is called before the first frame update
    void Start()
    {
        UI_InformPanelGameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region UI Callback Methods
    public void JoinRandomRoom()
    {
        UI_InformText.text = "Searching for available rooms...";
        PhotonNetwork.JoinRandomRoom();
        searchForGamesButtonGameObject.SetActive(false);
    }

    public void OnQuitMatchButtonClicked()
    {
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();

        }
        else
        {
            SceneLoader.Instance.LoadScene("Scene_Lobby");
        }
    }

    #endregion

    #region PHOTON Callback Methods

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode.ToString() + message);
        UI_InformText.text = message;
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    { 
        adjust_Button.SetActive(false);
        raycastCenter_Image.SetActive(false);

        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            UI_InformText.text = "Joined to" + PhotonNetwork.CurrentRoom.Name + ". Waiting for other players...";
        }
        else
        {
            UI_InformText.text = "Joined to" + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(UI_InformPanelGameObject.gameObject, 2.0f));
        }
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        UI_InformText.text = newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count: " + PhotonNetwork.CurrentRoom.PlayerCount;

        StartCoroutine(DeactivateAfterSeconds(UI_InformPanelGameObject.gameObject, 2.0f));
    }

    public override void OnLeftRoom()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    #endregion

    #region PRIVATE Methods
    private void CreateAndJoinRoom()
    {
        string randomRoomName = "Room" + Random.Range(0, 1000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        //Creating the room
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _gameObject.SetActive(false);
    }

    #endregion
}
