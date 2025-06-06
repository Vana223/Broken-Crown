using UnityEngine;

public class InfiniteScrolling : MonoBehaviour
{
    public Transform player;
    public float scrollSpeed = 2f;
    public Transform[] backgrounds;
    private float backgroundWidth;

    void Start()
    {
        backgroundWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float direction = Input.GetAxisRaw("Horizontal");

        foreach (Transform bg in backgrounds)
        {
            bg.position += Vector3.left * direction * scrollSpeed * Time.deltaTime;

            if (player.position.x - bg.position.x > backgroundWidth)
            {
                bg.position += Vector3.right * backgroundWidth * backgrounds.Length;
            }

            if (bg.position.x - player.position.x > backgroundWidth)
            {
                bg.position -= Vector3.right * backgroundWidth * backgrounds.Length;
            }
        }
    }
}
