using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(0.25f, 2);
    public float lifeTime = 6f;
    public float fadeTime = 4f;

    [Header("Set Dynamically")]
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;

    private Rigidbody _rigid;
    private BoundsCheck _boundCheck;
    private Renderer _cubeRend;


    void Awake()
    {
        cube = transform.Find("Cube").gameObject; // find the cube
        letter = GetComponent<TextMesh>();
        _rigid = GetComponent<Rigidbody>();
        _boundCheck = GetComponent<BoundsCheck>();
        _cubeRend = cube.GetComponent<Renderer>();

        Vector3 vel = Random.onUnitSphere; // randomize the powerup's velocity
        vel.z = 0;
        vel.Normalize();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        _rigid.velocity = vel;

        transform.rotation = Quaternion.identity;

        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y));
        birthTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;

        // Destroy powerup if u >= 1
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        if (u > 0)
        {
            Color c = _cubeRend.material.color;
            c.a = 1f - u;
            _cubeRend.material.color = c;
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if (!_boundCheck.isOnScreen)
            Destroy(gameObject);
    }

    // Used to set the type of powerup
    public void SetType(int rand)
    {
        // Create a powerup to increase the ship's speed
       if (rand < Main.powerUpDropChanceStatic/3) {
            letter.text = "S";
        }
        
        // Create a powerup to increase the damage done by the projectile
        else if (rand < Main.powerUpDropChanceStatic / 1.5)
        {
            letter.text = "D";
            _cubeRend.material.color = Color.yellow;
        }

        // Create an 'anti-powerup' that resets the ship's stats to their default values and decreases its shield level by 1
        else
        {
            letter.text = "X";
            _cubeRend.material.color = Color.red;
        }
    }
    // Called by Hero class to destroy game object
    public void AbsorbedBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
