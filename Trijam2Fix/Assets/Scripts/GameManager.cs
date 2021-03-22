using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score = 0;
    public int hiScore = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiScoreText;

    public MovingTarget movingTargetPrefab;

    private float midPointMaxOffset = 10;
    private float baseMovingTargetSpeed = 3f;
    private float targetStartDelay = 2f;
    private float spawnCooldown = 5f;
    private int simultaneousSpawnCount = 1;
    private float lastSpawnTime = -Mathf.Infinity;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        UpdateScoreDisplay();
    }

    private void Start()
    {
        AudioManager.instance.Play("Chain1");
    }

    private void Update()
    {
        UpdateScoreDisplay();

        if (Time.time - lastSpawnTime > spawnCooldown / (1 + score / 10))
        {
            for (int i = 0; i < simultaneousSpawnCount; i++)
            {
                CreateNewTargetPath();
            }
            lastSpawnTime = Time.time;
        }

    }

    public void UpdateScoreDisplay()
    {
        scoreText.text = score.ToString();
        hiScoreText.text = hiScore.ToString();
    }

    public void AddPoints(int amountAdded)
    {
        if (amountAdded > 0)
        {
            score += amountAdded;

            if (score > hiScore)
            {
                hiScore = score;
            }
        }
        else if (amountAdded < 0)
        {
            score = Mathf.Clamp(score + amountAdded, 0, int.MaxValue);

        }

        UpdateScoreDisplay();
    }

    private void CreateNewTargetPath()
    {
        Vector2[] linePos = CalculateTargetLine();
        Vector2 start = linePos[0];
        Vector2 end = linePos[1];

        MovingTarget target = Instantiate(movingTargetPrefab, start, Quaternion.identity);

        target.Init(start, end, targetStartDelay / (1 + score / 10), baseMovingTargetSpeed * (1 + score / 10));

    }

    private Vector2[] CalculateTargetLine()
    {
        float startOffset = 2;

        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        Vector2 camPos = Camera.main.transform.position;
        Rect camRect = new Rect(camPos, new Vector2(cameraWidth, cameraHeight));

        Vector2 originPoint = camPos;

        Vector2 midPoint = new Vector2(originPoint.x + Random.Range(-1f, 1f) * midPointMaxOffset, originPoint.y);

        Vector2 screenEdgePoint = GetRandomEdgePointFromRect(camRect);
        Vector2 edgeToMid = midPoint - screenEdgePoint;

        float thisShouldBeLongEnough = 2 * Mathf.Sqrt((Mathf.Pow(Camera.main.orthographicSize * 2, 2) + Mathf.Pow(Camera.main.orthographicSize * 2 * Camera.main.aspect, 2)));

        Vector2 endPoint = edgeToMid * thisShouldBeLongEnough;
        Vector2 screenEdgePointOffsetted = screenEdgePoint - startOffset * edgeToMid.normalized;

        Debug.DrawLine(screenEdgePointOffsetted, endPoint, Color.red);

        Vector2[] ret = { screenEdgePointOffsetted, endPoint };
        return ret;
    }

    public static Vector2 GetRandomEdgePointFromRect(Rect rect)
    {
        Vector2 edgePoint = Vector2.zero;

        if (Random.value > 0.5f)
        {
            edgePoint.x = Random.Range(-rect.width / 2, rect.width / 2);
            edgePoint.y = ((Random.value > 0.5f) ? rect.height : -rect.height) / 2;
        }
        else
        {
            edgePoint.x = ((Random.value > 0.5f) ? rect.width : -rect.width) / 2;
            edgePoint.y = Random.Range(-rect.height / 2, rect.height / 2);
        }

        return edgePoint;
    }

    /*
    public static Vector2[] GetLineSegmentAndRectIntersectionPoint(Vector2 line, Rect rect)
    {
        Vector2 intersectionPoint1 = Vector2.zero;
        Vector2 intersectionPoint2 = Vector2.zero;




        Vector2[] ret = { intersectionPoint1, intersectionPoint2 };
        return ret;
    }
    */


}
