using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        InGame,
        GameOver
    }

    public GameState ActiveState { get; set; }
    
    public static GameManagerController Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        ActiveState = GameState.MainMenu;
    }
    
}
