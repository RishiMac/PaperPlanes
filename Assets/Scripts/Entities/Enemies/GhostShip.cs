using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostShip : Entity
{
    private Camera cam;
    private float animationSpeed = 0.01f;
    private ShipProjectileSpawner spawnerScript;
    public Sprite bigShip;
    private Component[] a;
    // adjust lines 42, 56, 64 for animations

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        this.GetComponent<Renderer>().enabled = false;
        a = this.GetComponentsInChildren(typeof(Renderer));
        foreach (Component b in a)
        {
            Renderer c = (Renderer) b;
            c.enabled = false;
        }
        spawnerScript = GetComponentInChildren<ShipProjectileSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && this.GetComponent<Renderer>().enabled == false)
        {
            Debug.Log("Entered ghost ship region");

            transform.parent = cam.transform;
            transform.position += new Vector3(5, -15, 0);
            this.GetComponent<Renderer>().enabled = true;
            foreach (Component b in a)
            {
                Renderer c = (Renderer) b;
                c.enabled = true;
            }
            StartCoroutine(animateIn());
        }
    }

    IEnumerator animateIn()
    {
        Debug.Log(transform.position.y);
        while (transform.position.y < 10) {
            transform.position += new Vector3(0, 0.1f, 0);
            yield return new WaitForSeconds(animationSpeed);
        }
        cutscene();
    }

    void cutscene() {
        Debug.Log("Cutscene.");
        StartCoroutine(animateToPosition());
    }

    IEnumerator animateToPosition()
    {
        while (transform.position.y < 25) {
            transform.position += new Vector3(0, 0.1f, 0);
            yield return new WaitForSeconds(animationSpeed);
        }

        transform.position += new Vector3(1.25f, 10, 0f);
        yield return new WaitForSeconds(animationSpeed*25);
        this.GetComponent<SpriteRenderer>().sprite = bigShip;
        
        while (transform.position.y > 11.25) {
            transform.position += new Vector3(0, -0.1f, 0);
            yield return new WaitForSeconds(animationSpeed);
        }

        yield return new WaitForSeconds(animationSpeed*25);

        spawnerScript.startPhase1();
        //Can call phase 2 through spawnerScript.startPhase2();

        //TODO Add animation/time between phases
    }
}