using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckIfHub : MonoBehaviour
{
    string sceneName;
    Button hubButton;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        hubButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneName == "Hub")
        {
            hubButton.interactable = false;
        }
    }
}
