using UnityEngine;

public class Cloude : MonoBehaviour
{
    private new MeshRenderer renderer;

    public float speed;
    private float offset;

    // Start is called before the first frame update
    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        offset += Time.deltaTime * speed;
        renderer.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
