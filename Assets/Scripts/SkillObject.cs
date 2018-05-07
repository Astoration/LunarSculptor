using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject : MonoBehaviour {
    public float speed = 5f;
    public int destricbleLayer = 9;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, 3f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.up * speed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != destricbleLayer) return;
        var districble = collision.gameObject.GetComponent<Destructible>();
        districble.Damage(999);
    }


}
