using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndSceneController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI storyText;

    [Header("Characters")]
    public GameObject player;
    public GameObject wolf;

    void Start()
    {
        StartCoroutine(EndSequence());
    }

    IEnumerator EndSequence()
    {
        storyText.text = "Little Red Riding Hood's strength faltered in the heart of the woods.";
        yield return new WaitForSeconds(4f);

        storyText.text = "The potions were too few, the path too long, and her courage not enough.";
        yield return new WaitForSeconds(4f);

        storyText.text = "She reached the wizard, but without the final cure, the spell held fast.";
        yield return new WaitForSeconds(4f);

        storyText.text = "Darkness deepened, swallowing the hope of a blooming forest.";
        yield return new WaitForSeconds(4f);

        storyText.text = "She returned to the wolf, its breaths shallow and fading.";
        yield return new WaitForSeconds(4f);

        storyText.text = "With no potion left to heal, the wolf’s eyes slowly closed forever.";
        yield return new WaitForSeconds(4f);

        storyText.text = "The forest fell silent, draped in an eternal, cold shadow.";
        yield return new WaitForSeconds(4f);

        storyText.text = "Little Red Riding Hood remained in the woods, a guardian of nothing but memories.";
        yield return new WaitForSeconds(4f);

        storyText.text = "The light never returned, and the woods remained lost in the dark.";
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("MainMenu");
    }
}