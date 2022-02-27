using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    public float _turnSpeed = 15.0f;
    public float _aimDuration = 0.3f;

    Camera _mainCamera;
    PlayerController _player;

    [SerializeField] GameObject _followCam;
    [SerializeField] GameObject _aimCam;
    // Start is called before the first frame update
    void Awake()
    {
        _mainCamera = Camera.main;
        //Debug.Log("Character Aiming cursor");
        _player = FindObjectOfType<PlayerController>();
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_player.Started || _player.Won) return;

        //Gets the cameras Y euler angle
        float yawCamera = _mainCamera.transform.rotation.eulerAngles.y;

        //Slerps towards that angle
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), _turnSpeed * Time.fixedDeltaTime);

        //if the right mouse button is down, set the aim camera to be active, and disable the follow camera
        if (Input.GetMouseButton(1))
        {
            _aimCam.SetActive(true);
            _followCam.SetActive(false);
        }
        //Do the opposite as above
        else
        {
            _aimCam.SetActive(false);
            _followCam.SetActive(true);
        }
    }
}
