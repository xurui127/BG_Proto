using UnityEngine;

public class FruitAnimation : MonoBehaviour
{

    [SerializeField]float rotationSpeed = 60f;
    [SerializeField] float floatAmplitude = 0.2f;
    [SerializeField] float floatFrequency = 1f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPos + new Vector3(0, offsetY, 0);
    }
}
