using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class DropShadow : MonoBehaviour
{

    public float ShadowOffset;
    public Material ShadowMaterial;

    SpriteRenderer spriteRenderer;
    GameObject shadowGameObject;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // create new gameobject to be used as drop shadow
        shadowGameObject = new GameObject("Shadow");

        // create a new SpriteRenderer for Shadow gameobject
        SpriteRenderer shadowSpriteRenderer = shadowGameObject.AddComponent<SpriteRenderer>();

        // set the shadow gameobject's sprite to the original sprite
        shadowSpriteRenderer.sprite = spriteRenderer.sprite;
        shadowSpriteRenderer.transform.localScale = spriteRenderer.transform.localScale;
        // set the shadow gameobject's material to the shadow material we created
        shadowSpriteRenderer.material = ShadowMaterial;

        // update the sorting layer of the shadow to always lie behidn the sprite
        shadowSpriteRenderer.sortingLayerID = spriteRenderer.sortingLayerID;
        shadowSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // update the position of the sprite's shadow with moving sprite
        shadowGameObject.transform.localPosition = transform.localPosition + new Vector3(ShadowOffset, -ShadowOffset, 0f);
    }

    public void DestroyShadow()
    {
        Destroy(shadowGameObject);
    }

    public void ShadowEnabled(bool enableShadow)
    {
        if (shadowGameObject != null)
        {
           shadowGameObject.SetActive(enableShadow);
        } else
        {
            Debug.Log("Things went wrong (null reference shadow)");
            Application.Quit();
        }
    }
}
