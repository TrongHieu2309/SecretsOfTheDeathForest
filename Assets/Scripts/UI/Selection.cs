using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour 
{
    [SerializeField] private RectTransform[] options;
    private RectTransform rect;
    private int currentPosition;
    private Animator anim;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        anim.SetTrigger("select");
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(-1);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(1);
        }
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) // KeypadEnter: enter bàn phím số bên trái
        {
            HandleSelect();
        } 
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (currentPosition < 0)
        {
            currentPosition = options.Length - 1;
        }
        else if (currentPosition > options.Length - 1)
        {
            currentPosition = 0;
        }

        rect.position = new Vector3 (rect.position.x, options[currentPosition].position.y, 0);
    }

    private void HandleSelect()
    {
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}