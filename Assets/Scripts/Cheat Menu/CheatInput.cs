using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CheatInput : MonoBehaviour
{
    public TMP_InputField inputField;
    TMP_Text textComponent;
    Mesh mesh;
    Vector3[] vertices;
    [SerializeField]
    private Color oldColor = new Color(0.4087165f, 0.6698113f, 0.2748754f);

    [SerializeField]
    private int shakeTime = 5;
    [SerializeField]
    private float shakeStrength = 1.5f;
    [SerializeField]
    private float shakeStep = 0.05f;
    [SerializeField]
    private float shakeDamping = 1.5f;

    [SerializeField]
    private AudioClip dudSound, enableSound;
    private AudioSource audioSource;



    // Start is called before the first frame update
    void Start()
    {
        textComponent = inputField.textComponent;
        inputField.onValidateInput +=
         delegate (string s, int i, char c) { return char.IsLetter(c) ? char.ToLower(c) : '\0'; };
        inputField.caretWidth = 1;
        audioSource = GetComponent<AudioSource>();
    }

    public void TestCode(string s)
    {
        if (s.Length < 6)
        {
            // TODO: Display message
            Debug.Log("cheat codes are 6 characters long");
            StartCoroutine(DudAnimation());
            return;
        }

        // TODO: actually implement cheat modes
        string mode;
        switch (s)
        {
            case "flappy":
                mode = "flappy";
                break;
            case "bigmde":
                mode = "big";
                break;
            default:
                StartCoroutine(DudAnimation());
                return;
        }
        StartCoroutine(GoodAnimation());
        return;
    }

    IEnumerator DudAnimation()
    {
        Debug.Log("That's NO GOOD");
        Vector3 curPos = textComponent.rectTransform.position;
        audioSource.PlayOneShot(dudSound);
        for (int i = 0; i < shakeTime; i++)
        {
            // float offset = shakeStrength * (1 - (float) i / shakeTime) * (2 * (i % 2) - 1);
            // First term controls overall offset, second controls damping, third controls oscillating
            // https://www.desmos.com/calculator/zijeg9pe2a
            float offset = shakeStrength * Mathf.Pow(shakeDamping, -i) * (2 * (i % 2) - 1);
            textComponent.rectTransform.SetPositionAndRotation(new Vector3(curPos.x + offset, curPos.y, curPos.z), textComponent.rectTransform.rotation);
            yield return new WaitForSeconds(shakeStep);
        }
        textComponent.rectTransform.SetPositionAndRotation(curPos, textComponent.rectTransform.rotation);
        inputField.text = "";
    }

    IEnumerator GoodAnimation()
    {
        // TODO: Everything
        audioSource.PlayOneShot(enableSound);
        Mesh oldMesh = textComponent.mesh;
        textComponent.color = Color.white;
        for (int i = 0; i < shakeTime; i++)
        {
            // Fade from white back to green
            textComponent.color = Color.Lerp(oldColor, Color.white, 1 - (float)i/shakeTime);

            textComponent.ForceMeshUpdate();
            mesh = textComponent.mesh;
            vertices = mesh.vertices;


            // It's characterCount - 1 because for some reason there's some bullshit empty character at the end that I can't seem to deal with
            for (int charNum = 0; charNum < textComponent.textInfo.characterCount - 1; charNum++)
            {
                // Look at WavyText.cs for a clearer, better documented, and similar animation

                TMP_CharacterInfo c = textComponent.textInfo.characterInfo[charNum];
                if (c.character == ' ' || c.character == '\0') continue;

                // Index of the first vertex of this character
                int idx = c.vertexIndex;

                Vector3 offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * shakeStrength*2 * Mathf.Pow(shakeDamping, -i);

                for (int j = 0; j < 4; j++)
                {
                    vertices[idx + j] += offset;
                }
            }
            mesh.vertices = vertices;
            textComponent.canvasRenderer.SetMesh(mesh);
            yield return new WaitForSeconds(shakeStep);
        }

        textComponent.color = oldColor;
        textComponent.canvasRenderer.SetMesh(oldMesh);

        yield return null;
    }
}
