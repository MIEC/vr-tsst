using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour {

    [SerializeField]
    private Transform lowLidR;

    [SerializeField]
    private Transform lowLidL;

    [SerializeField]
    private Transform upLidR;

    [SerializeField]
    private Transform upLidL;

    /// <summary>
    /// Begin process of Blinking
    /// </summary>
    private void Awake()
    {
        StartCoroutine("EyesClosed");
    }

    /// <summary>
    /// Close the eyelids by changing their position for a short amount of time
    /// </summary>
    /// <returns></returns>
    private IEnumerator EyesClosed()
    {
        upLidL.transform.position = new Vector3(upLidL.transform.position.x, upLidL.transform.position.y - 0.007f, upLidL.transform.position.z);
        upLidR.transform.position = new Vector3(upLidR.transform.position.x, upLidR.transform.position.y - 0.007f, upLidR.transform.position.z);

        lowLidL.transform.position = new Vector3(lowLidL.transform.position.x, lowLidL.transform.position.y + 0.002f, lowLidL.transform.position.z);
        lowLidR.transform.position = new Vector3(lowLidR.transform.position.x, lowLidR.transform.position.y + 0.002f, lowLidR.transform.position.z);

        yield return new WaitForSecondsRealtime(0.15f);
        StartCoroutine("EyesOpen");
    }

    /// <summary>
    /// Open the eyelids again by undoing the changes done when closing the eyes, ando hold the eyes open for a random amount of time betwen one and eight seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator EyesOpen()
    {
        upLidL.transform.position = new Vector3(upLidL.transform.position.x, upLidL.transform.position.y + 0.007f, upLidL.transform.position.z);
        upLidR.transform.position = new Vector3(upLidR.transform.position.x, upLidR.transform.position.y + 0.007f, upLidR.transform.position.z);

        lowLidL.transform.position = new Vector3(lowLidL.transform.position.x, lowLidL.transform.position.y - 0.002f, lowLidL.transform.position.z);
        lowLidR.transform.position = new Vector3(lowLidR.transform.position.x, lowLidR.transform.position.y - 0.002f, lowLidR.transform.position.z);
        int random = UnityEngine.Random.Range(1, 9);
        yield return new WaitForSecondsRealtime(random);
        StartCoroutine("EyesClosed");
    }
}
