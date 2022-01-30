using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
	[Header("References")]
	[SerializeField] Rigidbody2D rigidbody;
	[SerializeField] ParticleSystem flamethrower;
	[SerializeField] GameObject target;
	[SerializeField] LayerMask tileLayer;
	[Header("Attributes")]
	[SerializeField] float speed = 1000f;
	public Camera cam;
	Vector2 mousePos;
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
		// player rotation
		cam.ScreenToWorldPoint(Input.mousePosition);
		mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
		Vector2 lookDir = mousePos - rigidbody.position;
		float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
		rigidbody.rotation = angle;
		// fire flamethrower
		if (Input.GetMouseButtonDown(0))
		{
			flamethrower.Play();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			flamethrower.Stop();
		}
		if (Input.GetMouseButton(0))
		{
			FireFlamethrower();
		}
	}

	void FireFlamethrower()
	{
		Vector3Int gridPosition = GameManager.Instance.mapManager.map.WorldToCell(target.transform.position);
		GameManager.Instance.burnManager.ChangeHitpoints(gridPosition, -1);
	}
}
