using UnityEngine;
using TMPro;
using System.Collections;

public class IntroStoryController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI storyText;

    [Header("Characters")]
    public IntroWalk player;
    public WolfIntro wolf;

    void Start()
    {
        StartCoroutine(StorySequence());
    }

    IEnumerator StorySequence()
    {
        // 1
        storyText.text = "Little Red Riding Hood was walking alone through the dark forest...";
        yield return new WaitForSeconds(4f);

        // 2
        storyText.text = "The wind whispered between the trees, as if the forest was alive...";
        yield return new WaitForSeconds(4f);

        // 3
        storyText.text = "She had always been told not to go too deep... but she had no choice...";
        yield return new WaitForSeconds(4f);

        // 4
        storyText.text = "Suddenly, she heard a soft sound behind the bushes...";
        yield return new WaitForSeconds(3f);

        // 5
        storyText.text = "A small wounded wolf appeared from the shadows...";
        wolf.Growl();
        yield return new WaitForSeconds(4f);

        // 6
        storyText.text = "They stared at each other... neither of them moved...";
        yield return new WaitForSeconds(3f);

        // 7
        storyText.text = "But something was wrong... the wolf was sick...";
        yield return new WaitForSeconds(4f);

        // 8
        storyText.text = "Only a special potion hidden deep in the forest could save it...";
        yield return new WaitForSeconds(4f);

        // 9
        storyText.text = "And so, their journey began...";
        yield return new WaitForSeconds(4f);

        // END
        storyText.text = "";

        // Burada gameplay scene'e geçebilirsin
        // SceneManager.LoadScene("Level1");
    }
}