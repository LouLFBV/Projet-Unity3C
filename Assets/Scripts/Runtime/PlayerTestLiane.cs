using UnityEngine;

public class PlayerTestLiane : MonoBehaviour
{
    private VineSwing _vineSwing;
    void Start()
    {
        if (_vineSwing == null )
        {
            _vineSwing = GetComponent<VineSwing>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_vineSwing.isSwinging)return;
        if(Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * 10f, 0, 0);
        }
        
        if(Input.GetButtonDown("Jump"))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }
    }
}
