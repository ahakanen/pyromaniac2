using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[Header("References")]
	[SerializeField] Rigidbody2D rigidbody;
	[SerializeField] ParticleSystem flamethrower;
	[Header("Attributes")]
	[SerializeField] float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
		if (rigidbody == null)
		{
			rigidbody = GetComponent<Rigidbody2D>();
		}
    }

    // Update is called once per frame
    void Update()
    {
		Vector2 direction;
		direction = Vector2.zero;
		direction.x = Input.GetAxis("Horizontal");
		direction.y = Input.GetAxis("Vertical");
		rigidbody.velocity = direction.normalized * speed * Time.deltaTime;
		// fire flamethrower
		if (Input.GetMouseButtonDown(0))
		{
			flamethrower.Play();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			flamethrower.Stop();
		}
	}
}
