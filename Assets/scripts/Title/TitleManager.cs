using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    private void Start()
    {
        SoundManager.Inst.PlayBGM(0);

    }
    public void Exitgame()
    {
        Application.Quit();
    }
    public void NewGame()
    {
        GameManager.Inst.gameState = GameState.NewGame;
        SceneManager.LoadScene("main");
    }
    public void LoadGame()
    {
        GameManager.Inst.gameState = GameState.LoadData;
        SceneManager.LoadScene("main");
    }
}
