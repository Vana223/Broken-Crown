using UnityEngine;

public class InfiniteScrolling : MonoBehaviour
{
    public Transform player;
    public float scrollSpeed = 2f;
    public Transform[] backgrounds;
    private float backgroundWidth;

    void Start()
    {
        // предполагаем, что все фоны одинаковой ширины
        backgroundWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float direction = Input.GetAxisRaw("Horizontal"); // -1 влево, 1 вправо

        foreach (Transform bg in backgrounds)
        {
            bg.position += Vector3.left * direction * scrollSpeed * Time.deltaTime;

            // если фон ушёл слишком влево
            if (player.position.x - bg.position.x > backgroundWidth)
            {
                bg.position += Vector3.right * backgroundWidth * backgrounds.Length;
            }

            // если фон ушёл слишком вправо
            if (bg.position.x - player.position.x > backgroundWidth)
            {
                bg.position -= Vector3.right * backgroundWidth * backgrounds.Length;
            }
        }
    }
}
