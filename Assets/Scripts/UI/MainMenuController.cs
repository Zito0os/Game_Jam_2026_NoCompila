using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Escena_leo"; // <-- pon aquí el nombre exacto

    [Header("New Game Button Sprites")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Image newGameImage;
    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteSelected;

    [Header("Optional")]
    [SerializeField] private bool setSelectedOnStart = true;

    private void Reset()
    {
        // Auto-fill cuando lo agregas en el editor
        newGameButton = GetComponentInChildren<Button>();
        if (newGameButton != null)
            newGameImage = newGameButton.GetComponent<Image>();
    }

    private void Start()
    {
        if (newGameButton == null || newGameImage == null)
        {
            Debug.LogError("MainMenuController: falta asignar newGameButton/newGameImage.");
            return;
        }

        // Sprite inicial
        SetButtonSprite(false);

        // Eventos del botón
        newGameButton.onClick.AddListener(PlayNewGame);

        // Para mouse (hover) y para selección con teclado/control
        AddEventTriggers(newGameButton.gameObject);

        if (setSelectedOnStart)
        {
            // Deja seleccionado por default (para teclado/control)
            EventSystem.current?.SetSelectedGameObject(newGameButton.gameObject);
            SetButtonSprite(true);
        }
    }

    private void AddEventTriggers(GameObject target)
    {
        var triggers = target.GetComponent<EventTrigger>();
        if (triggers == null) triggers = target.AddComponent<EventTrigger>();

        triggers.triggers ??= new System.Collections.Generic.List<EventTrigger.Entry>();

        // Hover/Pointer Enter
        var enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        enter.callback.AddListener(_ => SetButtonSprite(true));
        triggers.triggers.Add(enter);

        // Hover exit
        var exit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        exit.callback.AddListener(_ => SetButtonSprite(false));
        triggers.triggers.Add(exit);

        // Selección (teclado/control)
        var select = new EventTrigger.Entry { eventID = EventTriggerType.Select };
        select.callback.AddListener(_ => SetButtonSprite(true));
        triggers.triggers.Add(select);

        // Deselección
        var deselect = new EventTrigger.Entry { eventID = EventTriggerType.Deselect };
        deselect.callback.AddListener(_ => SetButtonSprite(false));
        triggers.triggers.Add(deselect);
    }

    private void SetButtonSprite(bool selected)
    {
        if (spriteNormal == null || spriteSelected == null) return;
        newGameImage.sprite = selected ? spriteSelected : spriteNormal;
    }

    public void PlayNewGame()
    {
        // Cargar escena del juego
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
