using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody _rigidbody;

    [SerializeField] private float _speed;
    [SerializeField] private float _stopRate;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private GameObject _gameWinPanel;
    [SerializeField] private GameObject _enemy;

    private Vector2 _inputVector;
    private int _count;

    private bool _isGameEnd;
    public string CountText => $"Count : {_count}";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _count = 0;
        _countText.SetText(CountText);
        _isGameEnd = false;
        _gameWinPanel.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 input = new Vector3(_inputVector.x, 0, _inputVector.y);
        // 입력이 있냐 없냐를 기준으로 이동 처리, 감소
        if (input == Vector3.zero || _isGameEnd)
        {
            // 이동 입력이 없다
            Vector3 currentVelocity = _rigidbody.linearVelocity;
            _rigidbody.linearVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, _stopRate);
        }
        else
        {
            // 이동 입력이 들어 왔다.
            Vector3 movement = input * _speed;
            _rigidbody.AddForce(movement);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            _count++;
            _countText.SetText(CountText);
        }

        if (_count >= 10)
        {
            _gameWinPanel.SetActive(true);
            _isGameEnd = true;
            Destroy(_enemy.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Destroy(gameObject);

            _gameWinPanel.SetActive(true);
            var text = _gameWinPanel.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "You Lose!";
        }
    }
}
