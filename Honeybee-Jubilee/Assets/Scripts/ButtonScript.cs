using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void ReturnToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }


}
