using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dummy : MonoBehaviour
{
    private Vector3 originalSize;

    [SerializeField]
    private Material currentMaterial;

    [SerializeField]
    private Material finishedMaterial;

    [SerializeField]
    private float speedMultiplier = 1.0f;

    private int state = 0; // 0 - off, 1 - turning on, 3 - going down, 4 - going up

    private int nameID;
    private float progress = 1;

    private float startPosition;
    private float endPosition;

    private float speed = 0.05f;

    private void Start()
    {
        nameID = Shader.PropertyToID("_Progress");
        currentMaterial.SetFloat(nameID, progress);
        state = 1;

        startPosition = transform.position.y;
        endPosition = transform.position.y - 0.1f;
    }

    private void Update()
    {
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        if(state == 0)
        {
            progress -= Time.deltaTime * speedMultiplier;
            currentMaterial.SetFloat(nameID, progress);

            if (progress <= 0f)
            {
                progress = 0;
                state = 5;
                GetComponent<MeshRenderer>().material = finishedMaterial;
            }
        }
        if(state == 1)
        {
            progress -= Time.deltaTime * speedMultiplier;
            currentMaterial.SetFloat(nameID, progress);

            if(progress <= 0f)
            {
                progress = 0;
                state = 3;
                GetComponent<MeshRenderer>().material = finishedMaterial;
            }
        }
        else if(state == 2)
        {
            progress += Time.deltaTime * speedMultiplier;
            currentMaterial.SetFloat(nameID, progress);

            if(progress >= 1f)
            {
                state = 0;
                progress = 1;
                transform.parent = null;
                GetComponent<Rigidbody>().isKinematic = false;
                transform.localScale = originalSize;
            }
        }
        else if(state == 3)
        {
            // Go down by speed * Time.deltaTime
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);

            if(transform.position.y <= endPosition + 0.01f)
            {
                state = 4;
            }
        }
        else if(state == 4)
        {
            // Go up by speed * Time.deltaTime
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);

            if(transform.position.y >= startPosition - 0.01f)
            {
                state = 3;
            }
        }
        else if(state == 5)
        {
            progress += Time.deltaTime * speedMultiplier;
            if (progress >= 3f)
                Destroy(gameObject);
        }
    }

    public void SetOriginalSize(Vector3 size)
    {
        originalSize = size;
    }

    public void Release()
    {
        state = 2;
        GetComponent<MeshRenderer>().material = currentMaterial;
    }

}
