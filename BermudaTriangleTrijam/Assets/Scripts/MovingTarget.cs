using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    private SpriteRenderer sr;

    private int scoreSubtractionOnEscape = 3;
    private float destroyDelay = 2f;

    private float creationTime = -Mathf.Infinity;
    private bool startedMoving = false;
    private bool hasBeenVisible = false;
    private bool destroying = false;

    private Vector2 direction;
    private float movementSpeed;
    private float startDelay = Mathf.Infinity;

    public LineRenderer line;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Init(Vector2 startPoint, Vector2 endPoint, float delay, float speed)
    {
        line.SetPositions(new Vector3[2] { startPoint, endPoint });
        creationTime = Time.time;
        startDelay = delay;
        direction = (endPoint - startPoint).normalized;
        movementSpeed = speed;

        transform.eulerAngles = new Vector3(0, 0, -Vector2.SignedAngle(direction, Vector2.up));
    }

    private void Update()
    {

        if (Time.time - creationTime >= startDelay)
        {
            if (!startedMoving)
            {
                startedMoving = true;

            }
            transform.position += (Vector3)(direction.normalized * movementSpeed * Time.deltaTime);

            if (sr.IsVisibleFrom(Camera.main))
            {
                if (!hasBeenVisible) hasBeenVisible = true;
            }
            else
            {
                if (hasBeenVisible && !destroying) StartCoroutine(DelayedDestroy(destroyDelay));
            }
        }

    }

    private IEnumerator DelayedDestroy(float delay)
    {
        GameManager.instance.AddPoints(-scoreSubtractionOnEscape);

        destroying = true;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }


    public void OnDestroy()
    {
        StopAllCoroutines();
        if (line != null) Destroy(line);
    }
}
