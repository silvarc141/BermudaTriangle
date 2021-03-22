using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public GameObject heldObject;
    public LayerMask dragMask;

    private EyeCenter eye;

    private void Awake()
    {
        eye = FindObjectOfType<EyeCenter>();

    }

    private void Update()
    {
        if (heldObject != null)
        {
            heldObject.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Rect scrRect = AspectUtility.screenRect;

            //Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(scrRect.min);
            //Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(scrRect.max);       

            Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(Vector2.zero);
            Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            heldObject.transform.position = new Vector3(
                Mathf.Clamp(heldObject.transform.position.x, minScreenBounds.x, maxScreenBounds.x),
                Mathf.Clamp(heldObject.transform.position.y, minScreenBounds.y, maxScreenBounds.y),
                heldObject.transform.position.z);
        }


        if (Input.GetMouseButtonUp(0))
        {
            if (heldObject != null)
            {
                AudioManager.instance.Play("PutDown");
                VFXManager.SpawnParticleOneshot(VFXManager.instance.dropVFX, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                heldObject = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!eye.IsInClickDistance())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, dragMask);

                if (hit.collider != null)
                {
                    AudioManager.instance.Play("PickUp");
                    VFXManager.SpawnParticleOneshot(VFXManager.instance.clickEmptyVFX, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    //Debug.Log(hit.collider.gameObject.name);
                    heldObject = hit.collider.gameObject;
                }
                else
                {
                    AudioManager.instance.Play("ClickEmpty");
                    VFXManager.SpawnParticleOneshot(VFXManager.instance.clickEmptyVFX, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                }
            }
        }
    }

}
