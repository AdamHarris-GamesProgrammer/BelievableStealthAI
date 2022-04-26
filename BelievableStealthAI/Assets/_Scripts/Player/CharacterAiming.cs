using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    public float _turnSpeed = 15.0f;
    public float _aimDuration = 0.3f;

    Camera _mainCamera;
    PlayerController _player;

    [SerializeField] GameObject _followCam;
    // Start is called before the first frame update
    void Awake()
    {
        _mainCamera = Camera.main;
        _player = FindObjectOfType<PlayerController>();
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        //Stops the camera from rotating when the game is not started or the game is won
        if (!_player.Started || _player.Won) return;

        //Gets the cameras Y euler angle
        float yawCamera = _mainCamera.transform.rotation.eulerAngles.y;

        //Slerps towards that angle
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), _turnSpeed * Time.fixedDeltaTime);
    }
}
