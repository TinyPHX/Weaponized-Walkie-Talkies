using ExtensionsMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [SerializeField] private new Camera camera;

    public int shakeCount;
    public float shakeSpacingMin = .1f;
    public float shakeSpacingMax = .2f;
    public Vector3 positionShake = Vector3.zero;
    public Vector3 rotationShake = Vector3.zero;
    public Vector3 scaleShake = Vector3.zero;
    public bool reset = true;

    private Vector3 startPosition;
    private Vector3 startRotation;
    private Vector3 startScale;

    private void Awake()
    {
        if (camera == null)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }
    
    public void Shake()
    {
        startPosition = camera.transform.localPosition;
        startRotation = camera.transform.localRotation.eulerAngles;
        startScale = camera.transform.localScale;

        float totalDealy = 0;

        for (int i = 0; i < shakeCount; i++)
        {
            float delay = Random.Range(shakeSpacingMin, shakeSpacingMax);
            totalDealy += delay;

            StartCoroutine(ShakeDelayed(totalDealy));
        }

        if (reset)
        {
            StartCoroutine(Reset(totalDealy + .1f));
        }
    }

    IEnumerator ShakeDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (positionShake != Vector3.zero)
        {
            camera.transform.localPosition += new Vector3(
                Random.Range(-positionShake.x, positionShake.x),
                Random.Range(-positionShake.y, positionShake.y),
                Random.Range(-positionShake.z, positionShake.z)
                );
        }

        if (rotationShake != Vector3.zero)
        {
            camera.transform.localRotation *= Quaternion.Euler(new Vector3(
                Random.Range(-rotationShake.x, rotationShake.x),
                Random.Range(-rotationShake.y, rotationShake.y),
                Random.Range(-rotationShake.z, rotationShake.z)
                ));
        }

        if (scaleShake != Vector3.zero)
        {
            camera.transform.localScale += new Vector3(
                Random.Range(-scaleShake.x, scaleShake.x),
                Random.Range(-scaleShake.y, scaleShake.y),
                Random.Range(-scaleShake.z, scaleShake.z)
                );
        }
    }

    IEnumerator Reset(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        camera.transform.localPosition = startPosition;
        camera.transform.rotation = Quaternion.Euler(startRotation);
        camera.transform.localScale = startScale;
    }
}
