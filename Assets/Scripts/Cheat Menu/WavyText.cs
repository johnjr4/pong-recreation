using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WavyText : MonoBehaviour
{
    [SerializeField]
    float amplitude = 0.5f;
    [SerializeField]
    float period = 1f;
    [SerializeField]
    float speed = 3f;

    private TMP_Text textMesh;
    Mesh mesh;
    Vector3[] vertices;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        textMesh.ForceMeshUpdate();
        mesh = textMesh.mesh;
        vertices = mesh.vertices;

        for (int i = 0; i < textMesh.textInfo.characterCount; i++)
        {
            TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];

            // For some absolutely bizarre reason, spaces are placed at index 0
            // If you don't skip them, you end up offsetting the first char extra
            if (c.character == ' ') continue;

            // Index of the first vertex corresponding to this character
            int idx = c.vertexIndex;

            Vector3 offset = new Vector3(0, Mathf.Sin(period * idx + speed * Time.time) * amplitude, 0);
            
            // Debug stationary offset
            // Vector3 offset = new Vector3(0, amplitude, 0);

            // Adds offset to all 4 vertices for this chararcter
            for (int j = 0; j < 4; j++)
            {
                vertices[idx + j] += offset;
            }

        }
        mesh.vertices = vertices;
        textMesh.canvasRenderer.SetMesh(mesh);
    }
}
