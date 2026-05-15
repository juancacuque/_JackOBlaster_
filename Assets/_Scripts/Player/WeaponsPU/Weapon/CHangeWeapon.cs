using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CHangeWeapon : MonoBehaviour
{
    public GameObject gun1;
    public GameObject gun2;
    public GameObject gun3;

    public GameObject[] guns;
    public int activeWeapon = 0;
    private WeaponBase currentWeapon;

    private bool weaponSelected = false;

    public Button buttonPistol; // Referencia al botón para la pistola
    public Button buttonShotgun; // Referencia al botón para la escopeta
    public Button buttonKnife; // Referencia al botón para el cuchillo

    void Start()
    {
        guns = new GameObject[] { gun1, gun2, gun3 };

        foreach (var gun in guns)
        {
            gun.SetActive(false);
        }

        ShowWeaponSelection();

        // Iniciar el temporizador antes de pausar
        StartCoroutine(StartGameWithDelay(2f));

        // Asignar funciones a los botones
        buttonPistol.onClick.AddListener(() => SelectWeapon(0));
        buttonShotgun.onClick.AddListener(() => SelectWeapon(1));
        buttonKnife.onClick.AddListener(() => SelectWeapon(2));
    }

    void Update()
    {
        // El método de selección ahora solo se maneja a través de la UI
    }

    private void ShowWeaponSelection()
    {
        Debug.Log("Select your weapon using the UI buttons.");
    }

    public void SelectWeapon(int number)
    {
        weaponSelected = true;

        foreach (var gun in guns)
        {
            gun.SetActive(false);
        }

        guns[number].SetActive(true);
        activeWeapon = number;
        currentWeapon = guns[number].GetComponent<WeaponBase>();

        if (currentWeapon != null)
        {
            Transform spawnPoint = guns[number].transform.Find("SpawnPoint");
            if (spawnPoint != null)
            {
                currentWeapon.Initialize(spawnPoint);
            }
            else
            {
                Debug.LogError($"No SpawnPoint found for weapon: {guns[number].name}");
            }

            Debug.Log($"You selected the weapon: {guns[number].name}");
        }

        // Reanudar el juego después de seleccionar un arma
        ResumeGame();

        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("The game has started! You can now use your weapon.");
    }

    public WeaponBase GetCurrentWeapon()
    {
        return currentWeapon;
    }

    private IEnumerator StartGameWithDelay(float delay)
    {
        Debug.Log("Game starting in 2 seconds...");
        yield return new WaitForSeconds(delay); // Esperar 2 segundos
        PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0; // Pausar el tiempo
        Debug.Log("Game paused. Select a weapon to start.");
    }

    private void ResumeGame()
    {
        Time.timeScale = 1; // Reanudar el tiempo
        Debug.Log("Game resumed.");
    }
}
