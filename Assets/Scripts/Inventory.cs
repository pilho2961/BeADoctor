using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Image[] images;
    public ItemsSO[] itemData;

    private void Awake()
    {
        // Get all child objects of the Inventory GameObject with an Image component
        Image[] childImages = GetComponentsInChildren<Image>();

        if (childImages.Length > 0 && childImages[0].transform == transform)
        {
            // Exclude the first Image component if it is attached to this GameObject
            images = new Image[childImages.Length - 1];
            for (int i = 1; i < childImages.Length; i++)
            {
                images[i - 1] = childImages[i];
            }
        }
        else
        {
            // If the first Image component is not attached to this GameObject, assign all Image components
            images = childImages;
        }
    }

    private void OnEnable()
    {
        // 얻은 아이템 정보를 아이템 창에 순서대로 띄우기
    }
}
