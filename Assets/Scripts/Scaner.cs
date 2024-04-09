using System;
using System.Collections;
using UnityEngine;

public class Scaner : MonoBehaviour
{
    public event Action<Transform> ResourceDetected;

    [SerializeField] private float _frequency = 5.0f;
    [SerializeField] private float _radius = 50.0f;
    [SerializeField] private ParticleSystem _scanFX;
    [SerializeField] private AudioClip _scanSFX;

    private void Start()
    {
        StartCoroutine(Scan());
    }

    private IEnumerator Scan()
    {
        WaitForSeconds wait = new WaitForSeconds(_frequency);

        while(true)
        {
            yield return wait;

            Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

            if (colliders.Length > 0 ) 
            { 
                foreach (Collider collider in colliders) 
                {
                    if (collider.TryGetComponent(out Resource resource) && collider.transform.parent.TryGetComponent<ResourceSpawner>(out _))
                    {
                        ResourceDetected?.Invoke(resource.transform);
                    }
                }
            }

            if (_scanFX)
            {
                Instantiate(_scanFX, transform);
            }

            if (_scanSFX)
            {
                AudioSource.PlayClipAtPoint(_scanSFX,transform.position);
            }            
        }
    }

}
