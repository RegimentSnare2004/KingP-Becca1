using UnityEngine;

public class Menu : MonoBehaviour
{
    public void gotoGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    public void gotoMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void gotoCharacterSelection()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterSelection");
    }
}
