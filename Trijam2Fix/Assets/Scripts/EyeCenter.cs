using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCenter : MonoBehaviour
{
    private List<MovingTarget> targetList = new List<MovingTarget>();

    public LayerMask eyeMask;

    private float clickDist = 1f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, eyeMask);

            if (IsInClickDistance())
            {
                //targetList.RemoveAll(item => item == null);

                if (targetList.Count > 0)
                {
                    GameManager.instance.AddPoints(1);
                    AudioManager.instance.Play("Hit");
                    VFXManager.SpawnParticleOneshot(VFXManager.instance.hitVFX, transform.position);

                    List<MovingTarget> toDestroy = new List<MovingTarget>();
                    for (int i = 0; i < targetList.Count; i++)
                    {
                        toDestroy.Add(targetList[i]);

                        //Debug.Log($"Destroying: {targetList[i].gameObject}");
                        //Destroy(targetList[i].gameObject);
                    }

                    for (int i = 0; i < toDestroy.Count; i++)
                    {
                        Destroy(toDestroy[i].gameObject);
                    }

                }
                else
                {
                    AudioManager.instance.Play("Miss");
                    VFXManager.SpawnParticleOneshot(VFXManager.instance.missVFX, transform.position);
                }
            }
        }
    }

    public bool IsInClickDistance()
    {
        return Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < clickDist;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        targetList.Add(collision.gameObject.GetComponent<MovingTarget>());

        //Debug.Log($"Target acquired: {collision.gameObject.name} Target count: {targetList.Count}");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        MovingTarget tar = collision.gameObject.GetComponent<MovingTarget>();
        targetList.RemoveAll(item => item == tar);

        //Debug.Log($"Target lost: {collision.gameObject.name} Target count: {targetList.Count}");
    }

}
