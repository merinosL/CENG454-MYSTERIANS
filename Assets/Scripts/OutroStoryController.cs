using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class OutroStoryController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI storyText;

    void Start()
    {
        StartCoroutine(OutroSequence());
    }

    IEnumerator OutroSequence()
    {
        // 1
        storyText.text = "Little Red Riding Hood braved the deep, dark woods.";
        yield return new WaitForSeconds(4f);

        // 2
        storyText.text = "With steady hands, she gathered every hidden potion.";
        yield return new WaitForSeconds(4f);

        // 3
        storyText.text = "At last, she stood tall before the Evil Wizard.";
        yield return new WaitForSeconds(4f);

        // 4
        storyText.text = "Her courage was finally stronger than her fear.";
        yield return new WaitForSeconds(4f);

        // 5
        storyText.text = "With the final potion, she broke the dark spell.";
        yield return new WaitForSeconds(4f);

        // 6
        storyText.text = "The forest began to bloom with life once again.";
        yield return new WaitForSeconds(4f);

        // 7
        storyText.text = "Little Red Riding Hood was no longer a lost, frightened girl.";
        yield return new WaitForSeconds(4f);

        // 8
        storyText.text = "She had become the true guardian of the woods.";
        yield return new WaitForSeconds(4f);

        // 9
        storyText.text = "She rushed back to find her dear friend.";
        yield return new WaitForSeconds(4f);

        // 10
        storyText.text = "As the wolf drank the potion, its strength returned.";
        yield return new WaitForSeconds(4f);

        // 11
        storyText.text = "No longer sick, the wolf stood by her side with joy.";
        yield return new WaitForSeconds(4f);

        // 12
        storyText.text = "The shadows vanished, and sunlight poured through the trees.";
        yield return new WaitForSeconds(4f);

        // 13
        storyText.text = "Together, they walked into the warm, golden light.";
        yield return new WaitForSeconds(4f);

        // 14
        storyText.text = "And so, the forest lived happily ever after.";
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("EndScene");
    }
}