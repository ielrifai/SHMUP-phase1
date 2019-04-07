using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private BoundsCheck _bndCheck;
    private Renderer _rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField] 
    private WeaponType _type;

    // This public property masks the field _type and takes action when it is set
    public WeaponType type
    { 
        get
        {
            return (_type);
        }
        set
        {
            SetType(value); 
        }
    }

    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
        _rend = GetComponent<Renderer>(); 
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (_bndCheck.offUp || _bndCheck.offRight || _bndCheck.offLeft)
        { 
            Destroy(gameObject);
        }
    }

    public void SetType(WeaponType eType)
    { 
      // Set the _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        _rend.material.color = def.projectileColor;
    }
}