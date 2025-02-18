using UnityEngine;

namespace Game
{
    public class InitializeGame : MonoBehaviour
    {
        private void Awake()
        {
            GameManagerController.Instance.ActiveState = GameManagerController.GameState.InGame;
        }
    }
}
