using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARPlacementAndPlaneDetectionController : MonoBehaviour
{
    ARPlaneManager m_ARPlaneManager;
    ARPlacementManager m_ARplacementManager;

    public GameObject placeButton;
    public GameObject AdjustButton;
    public GameObject searchForGameButton;
    public GameObject scaleSlider;
    public TextMeshProUGUI informUIPanel_Text;

    private void Awake()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        m_ARplacementManager = GetComponent<ARPlacementManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        placeButton.SetActive(true);
        scaleSlider.SetActive(true);
        AdjustButton.SetActive(false);
        searchForGameButton.SetActive(false);

        informUIPanel_Text.text = "Move phone to detect planes and place the Battle Arena!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableARPlacementAndPlaneDetection()
    {
        m_ARplacementManager.enabled = false;
        m_ARPlaneManager.enabled = false;
        SetAllPlanesActiveOrDeactive(false);
       
        scaleSlider.SetActive(false);
        placeButton.SetActive(false);
        AdjustButton.SetActive(true);
        searchForGameButton.SetActive(true);

        informUIPanel_Text.text = "Great! You placed the Arena.. Now search for games to Battle!";

    }

    public void EnableARPlacementAndPlaneDetection()
    {
        m_ARPlaneManager.enabled = true;
        m_ARplacementManager.enabled = true;
        SetAllPlanesActiveOrDeactive(true);

        scaleSlider.SetActive(true);
        placeButton.SetActive(true);
        AdjustButton.SetActive(false);
        searchForGameButton.SetActive(false);

        informUIPanel_Text.text = "Move phone to detect planes and place the Battle Arena!";

    }

    private void SetAllPlanesActiveOrDeactive(bool value)
    {
        foreach (var plane in m_ARPlaneManager.trackables) 
        {
            plane.gameObject.SetActive(value);
        }
    }
}
