using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCharacterCollision : MonoBehaviour
{
    [SerializeField]
    CapsuleCollider2D characterCollider, characterBlockCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        //characterCollider = GetComponent<CapsuleCollider2D>();
        Physics2D.IgnoreCollision(characterCollider, characterBlockCollider, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
