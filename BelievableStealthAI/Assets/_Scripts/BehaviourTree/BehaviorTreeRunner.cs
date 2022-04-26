using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
    public BehaviorTree tree;
    
    PlayerController _player;

    Health _health;

    // Start is called before the first frame update
    void Awake()
    {
        tree = tree.Clone();
        tree.Bind();
        _player = FindObjectOfType<PlayerController>();

        _health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_health.IsDead) return;
        if (_player.Won) return;

        tree.Update();
    }
}
