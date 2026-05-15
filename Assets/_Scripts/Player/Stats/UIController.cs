using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider expLvlSlider;
    public TMP_Text expLvlText;
    public GameObject levelUpPanel;
    public Button option1Button;
    public Button option2Button;
    public Button option3Button;
    public Button option4Button;
    public TMP_Text option1Text;
    public TMP_Text option2Text;
    public TMP_Text option3Text;
    public TMP_Text option4Text;
    public GameObject losingPanel;
    public GameObject winningPanel;

    private UnlockableWeapons unlockableWeapons;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        unlockableWeapons = FindObjectOfType<UnlockableWeapons>();
    }

    void Update()
    {
        if (unlockableWeapons.LevelUp)
        {
            // Llamar a la función ShowUpgradeChoices con las opciones de mejora
            ShowUpgradeChoices(unlockableWeapons.currentChoices[0], unlockableWeapons.currentChoices[1],
                                unlockableWeapons.currentChoices[2], unlockableWeapons.currentChoices[3]);
        }
    }

    public void UpdateExperience(int currentExp, int levelExp, int currentLvl)
    {
        expLvlSlider.maxValue = levelExp;
        expLvlSlider.value = currentExp;
        expLvlText.text = "Level: " + currentLvl;
    }

    // Mostrar las opciones de mejora cuando se sube de nivel
    public void ShowUpgradeChoices(GameObject choice1, GameObject choice2, GameObject choice3, GameObject choice4)
    {
        levelUpPanel.SetActive(true);  // Mostrar el panel de opciones

        option1Text.text = choice1.name;
        option2Text.text = choice2.name;
        option3Text.text = choice3.name;
        option4Text.text = choice4.name;

        // Asignar las funciones de los botones
        option1Button.onClick.RemoveAllListeners();
        option1Button.onClick.AddListener(() => ApplyUpgrade(choice1));

        option2Button.onClick.RemoveAllListeners();
        option2Button.onClick.AddListener(() => ApplyUpgrade(choice2));

        option3Button.onClick.RemoveAllListeners();
        option3Button.onClick.AddListener(() => ApplyUpgrade(choice3));

        option4Button.onClick.RemoveAllListeners();
        option4Button.onClick.AddListener(() => ApplyUpgrade(choice4));
    }

    // Aplicar la mejora seleccionada
    public void ApplyUpgrade(GameObject upgrade)
    {
        unlockableWeapons.ApplyUpgrade(upgrade);  // Llamar a la función de `UnlockableWeapons`
        UIController.instance.levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
