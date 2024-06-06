using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{

    [SerializeField] Sprite[] bubbleColors = new Sprite[] { };

    public int ColorIndex = 0;

    public GameObject GridReference;
    public enum State { 
        Move,
        Stop,
        Die
    }
    private float _speed = 20f;

    [HideInInspector] public Vector2 BubbleDirection;

    [HideInInspector] public Vector3 AimPoint;

    public Vector3Int GridPos { get; set; }

    public string Color { get; private set; }

    public State CurrentState;

    private SpriteRenderer _bubbleRenderer;

    
    private Rigidbody2D _rb;

    void Start()
    {
        BubbleDirection = (AimPoint - transform.position).normalized;

        _rb = GetComponent<Rigidbody2D>();

        ColorIndex = Random.Range(0, bubbleColors.Length);

        _bubbleRenderer = gameObject.GetComponent<SpriteRenderer>();
        _bubbleRenderer.sprite = bubbleColors[ColorIndex];

        CurrentState = State.Move;
    }


    void Update()
    {
        
        switch (CurrentState)
        {
            case State.Move:
                BubbleMove();
                break;
            case State.Stop:
                BubbleStop();
                break;
            case State.Die:
                BubbleDie();
                break;
        }
        
    }

    void BubbleMove()
    {
        Vector2 moveVelocity = BubbleDirection * _speed * Time.deltaTime;

        _rb.MovePosition(_rb.position + moveVelocity);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        if (collision.collider.CompareTag("Wall"))
        {
            if (collision.gameObject.name == "top")
                CurrentState = State.Stop;
        }
        if (collision.collider.CompareTag("Bubble"))
        {
            CurrentState = State.Stop;
            
        }
        BubbleDirection = Vector2.Reflect(BubbleDirection, contact.normal);

        BubbleDirection = BubbleDirection.normalized;

        _rb.MovePosition(_rb.position + BubbleDirection * 0.1f);
    }

    void BubbleStop()
    {
        _rb.velocity = Vector2.zero;

        GridManager grid = GridReference.GetComponent<GridManager>();
        Vector3Int cellPosition = grid.WorldToCell(transform.position);
        transform.position = grid.GetCellCenterWorld(cellPosition);

        grid.AddBubbleToGrid(this, cellPosition);
        grid.FindAndHandleMatches(this);

    }
    void BubbleDie()
    {

    }
}
