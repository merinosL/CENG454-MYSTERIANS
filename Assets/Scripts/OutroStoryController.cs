using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class OutroStoryController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI storyText;

    [Header("Characters")]
    public GameObject player;
    public GameObject wolf;

    void Start()
    {
        StartCoroutine(OutroSequence());
    }

    IEnumerator OutroSequence()
    {
        storyText.text = "Little Red Riding Hood braved the deep, dark woods.";
        yield return new WaitForSeconds(4f);

        storyText.text = "With steady hands, she gathered every hidden potion.";
        yield return new WaitForSeconds(4f);

        storyText.text = "At last, she stood tall before the Evil Wizard.";
        yield return new WaitForSeconds(4f);

        storyText.text = "Her courage was finally stronger than her fear.";
        yield return new WaitForSeconds(4f);

        storyText.text = "With the final potion, she broke the dark spell.";
        yield return new WaitForSeconds(4f);

        storyText.text = "The forest began to bloom with life once again.";
        yield return new WaitForSeconds(4f);

        storyText.text = "Little Red Riding Hood was no longer a lost, frightened girl.";
        yield return new WaitForSeconds(4f);

        storyText.text = "She had become the true guardian of the woods.";
        yield return new WaitForSeconds(4f);

        storyText.text = "She rushed back to find her dear friend.";
        yield return new WaitForSeconds(4f);

        storyText.text = "As the wolf drank the potion, its strength returned.";
        yield return new WaitForSeconds(4f);

        storyText.text = "No longer sick, the wolf stood by her side with joy.";
        yield return new WaitForSeconds(4f);

        storyText.text = "The shadows vanished, and sunlight poured through the trees.";
        yield return new WaitForSeconds(4f);

        storyText.text = "Together, they walked into the warm, golden light.";
        yield return new WaitForSeconds(4f);

        storyText.text = "And so, the forest lived happily ever after.";
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("EndScene");
    }
}