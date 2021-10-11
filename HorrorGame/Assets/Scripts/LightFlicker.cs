using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float maxInterval;
    private bool isFlick = false;
    private float timeDelay;
    

    // Update is called once per frame
    void Update()
    {
        if (isFlick == false)
        {
            StartCoroutine(FlickerLight());
        }
    }

    IEnumerator FlickerLight()
    {
        isFlick = true;
        this.gameObject.GetComponent<Light2D>().enabled = false;
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
        this.gameObject.GetComponent<Light2D>().enabled = true;
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
        isFlick = false;
    }
}
