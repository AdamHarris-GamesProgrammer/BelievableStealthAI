using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
    public BehaviorTree tree;
    


    PlayerController _player;

    // Start is called before the first frame update
    void Awake()
    {
        tree = tree.Clone();
        tree.Bind();
        _player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.Won) return;

        

        tree.Update();
    }
}
