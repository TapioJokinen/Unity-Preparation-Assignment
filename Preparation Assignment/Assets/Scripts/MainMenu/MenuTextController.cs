using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MainMenu
{
    public class MenuTextController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public TextMeshProUGUI textObject;

        private readonly Color _defaultColor = Color.white;
        private readonly Color _hoverColor = new Color(7f / 255f, 207f / 255f, 199f / 255f);

        private static string NewGame => "New Game";
        private static string LoadGame => "Load Game";
        private static string Exit => "Exit";

        private void Start()
        {
            if (textObject == null)
            {
                textObject = GetComponent<TextMeshProUGUI>();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (textObject != null)
            {
                textObject.color = _hoverColor;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (textObject != null)
            {
                textObject.color = _defaultColor;
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if(textObject.text == NewGame)
            {
                SceneManagerController.Instance.LoadSceneAsync("Game");
            }
            else if(textObject.text == LoadGame)
            {
                Debug.Log("Load Game");
            }
            else if(textObject.text == Exit)
            {
                Application.Quit();
            }
        }
    }
}