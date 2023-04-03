using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man_Animation : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator _animator;
    public Rigidbody[] _all_rbodys;

    private void Awake()
    {

    }

    void Start()
    {
        for (int i = 0; i < _all_rbodys.Length; i++)
        {
            _all_rbodys[i].isKinematic = true;
        }
        _animator.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
