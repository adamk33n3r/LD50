using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public TMPro.TextMeshProUGUI titleText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retry() {
        SceneManager.LoadScene("Game");
    }
    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void FinishGame(bool won) {
        if (won) {
            this.titleText.text = "You Win!";
        } else {
            this.titleText.text = "You Lose!";
        }
    }
}
