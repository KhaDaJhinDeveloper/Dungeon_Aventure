using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : Singleton<GameControl>
{
    #region Input Density
    private int maxDanger = 10;
    private int levelOfDanger = 1;
    public int LevelOfDanger { get => this.levelOfDanger;  
                               set => this.levelOfDanger = Mathf.Clamp(value, 1, this.maxDanger); }
    private float scaleDamage = 0.0f;
    public float ScaleDamage { get => this.scaleDamage;
                               set => this.scaleDamage = Mathf.Clamp01(value); }
    private float density  = 0.4f;
    public float Density { get => this.density;
                           set => this.density = Mathf.Clamp01(value); }
    #endregion
    #region StateGame
    public void NewGame()
    {
        TransitionScene.Instance.PlayTransition
        (
            () => 
            {
                SceneManager.LoadScene("Level1");
                SoundManager.Instance?.PlaySceneMusicBG(NameScene.Level1);
            }
        );
    } 
    public void Continue()
    {
        TransitionScene.Instance.PlayTransition
        (() =>
        {
            string nameScene = GameSceneStateManager.Instance.GetSceneNameData();
            GameSaveManager.Instance.RequestLoadOnNextScene();
            SceneManager.LoadScene(nameScene);
        }
        );
    }    
    public void Restart()
    {
        TransitionScene.Instance.PlayTransition
        (() =>
        {
            GameSaveManager.Instance.DeleteAllDataLocal();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        );
    }
    public void ReturnMainMenu()
    {
        TransitionScene.Instance.PlayTransition
        (() =>
        {
            SceneManager.LoadScene("MainMenu");
            SoundManager.Instance?.PlayMusicBG(SoundID.BG_MainMenu);
        }
        );
    }    
    #endregion
}
