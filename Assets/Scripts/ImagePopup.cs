using UnityEngine;

public class ImagePopup : MonoBehaviour
{
    public GameObject popupPanel; // Drag ImagePopup here

    public void OpenPopup()
    {
        popupPanel.SetActive(true);
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}