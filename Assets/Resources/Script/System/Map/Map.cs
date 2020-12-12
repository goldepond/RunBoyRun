using UnityEngine;

public class Map : MonoBehaviour
{
    public float speed = 5.0f;
    public float startPosition;
    public float endPosition;

    private void Update()
    {
        // x포지션을 조금씩 이동
        transform.Translate(-1 * speed * Time.deltaTime, 0, 0);

        // 목표 지점에 도달했다면
        if (transform.position.x <= endPosition)
        {
            transform.Translate(-1 * (endPosition - startPosition), 0, 0);
            ScrollEnd();
        }
    }

    private void ScrollEnd()
    {
        // 원래 위치로 초기화 시킨다.
       
    }
}