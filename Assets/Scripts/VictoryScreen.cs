using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public static void PlayAgain() {
        SceneManager.LoadScene("_Scene_0");
    }

    public static void Quit() {
        Application.Quit();
    }
}
