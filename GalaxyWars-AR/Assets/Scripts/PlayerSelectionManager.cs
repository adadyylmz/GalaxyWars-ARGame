using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    public Transform playerSwitcherTransform;
    public GameObject[] spinnerTopModels;


    public Button next_Button;
    public Button prev_Button;

    public int playerSelectionNumber;

    [Header("UI")]
    public TextMeshProUGUI playerModelType_Text;

    public GameObject UI_Selection;
    public GameObject UI_AfterSelection;

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        UI_Selection.SetActive(true);
        UI_AfterSelection.SetActive(false);
        playerSelectionNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region UI Callback Methods
    public void NextPlayer()
    {
        playerSelectionNumber += 1;

        if(playerSelectionNumber >= spinnerTopModels.Length)
        {
            playerSelectionNumber = 0;
        }
        Debug.Log(playerSelectionNumber);

        next_Button.enabled = false;
        prev_Button.enabled = false;
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1.0f));
        if(playerSelectionNumber == 0 || playerSelectionNumber ==1)
        {
            //This means the player model type is ATTACK
            playerModelType_Text.text = "Attack";
        }
        else
        {
            //This means the player model type is DEFEND
            playerModelType_Text.text = "Defend";

        }
    }

    public void PreviousPlayer()
    {
        if(playerSelectionNumber <= 0)
        {
            playerSelectionNumber = spinnerTopModels.Length - 1;
        }
        playerSelectionNumber -= 1;
        Debug.Log(playerSelectionNumber);


        next_Button.enabled = false;
        prev_Button.enabled = false;
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1.0f));

        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            //This means the player model type is ATTACK
            playerModelType_Text.text = "Attack";
        }
        else
        {
            //This means the player model type is DEFEND
            playerModelType_Text.text = "Defend";

        }

    }

    public void OnSelectButtonClicked()
    {
        UI_Selection.SetActive(false);
        UI_AfterSelection.SetActive(true);

        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable { { MultiplayerARGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }

    public void OnReselectButtonClicked()
    {
        UI_Selection.SetActive(true);
        UI_AfterSelection.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }

    public void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    #endregion

    #region Private Methods

    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f) 
    
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation*Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;

        next_Button.enabled = true;
        prev_Button.enabled = true;
    }

    #endregion
}
