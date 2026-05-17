using UnityEngine;

public class StoryNPC : MonoBehaviour, IInteractable
{
    [TextArea(3, 10)]
    public string[] storyLines; // fill in the story parts in the Inspector

    public void Interact(Transform player)
    {
        if (storyLines.Length > 0)
        {
            DialogueManager.instance.StartStory(storyLines);
        }
        else
        {
            Debug.Log("No story assigned to this NPC!");
        }
    }
}