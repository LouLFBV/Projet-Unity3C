using UnityEngine;

public class VineSwing : MonoBehaviour
{
    public float swingForce = 10f; // Force de balancement
    public float jumpForce = 15f;  // Force du saut en quittant la liane

    private Rigidbody2D rb;
    private DistanceJoint2D distanceJoint;
    private LineRenderer lineRenderer;

    public bool isSwinging = false;
    private Transform currentVineAnchor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        distanceJoint = GetComponent<DistanceJoint2D>();
        lineRenderer = GetComponent<LineRenderer>();

        distanceJoint.enabled = false;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (currentVineAnchor != null && !isSwinging)
        {
            AttachToVine();
        }

        if (isSwinging)
        {
            DrawVine();
            HandleSwinging();

            // Sauter pour rel�cher la liane
            if (Input.GetButtonDown("Jump"))
            {
                DetachAndJump();
            }
        }
    }

    private void AttachToVine()
    {
        isSwinging = true;

        // Connecter le joint au point d'ancrage
        distanceJoint.connectedAnchor = currentVineAnchor.position;
        distanceJoint.enabled = true;
        lineRenderer.enabled = true;

        // Optionnel : ajuster la distance de la liane selon la position actuelle
        distanceJoint.distance = Vector2.Distance(transform.position, currentVineAnchor.position);
    }

    private void HandleSwinging()
    {
        // R�cup�rer l'input horizontal (Q/D ou Fl�ches)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Appliquer une force pour cr�er un effet de pendule
        Vector2 force = new Vector2(horizontalInput * swingForce, 0);
        rb.AddForce(force, ForceMode2D.Force);
    }

    private void DrawVine()
    {
        // On vérifie que l'ancrage existe toujours avant de dessiner00:08]
        if (currentVineAnchor != null && lineRenderer != null)
        {
            lineRenderer.SetPosition(0, currentVineAnchor.position); 
        lineRenderer.SetPosition(1, transform.position); 
    }
    }

    private void DetachAndJump()
    {
        isSwinging = false;
        distanceJoint.enabled = false;
        lineRenderer.enabled = false;
        currentVineAnchor = null; // Réinitialise l'ancrage après le saut

        Vector2 jumpVector = new Vector2(rb.linearVelocity.x, jumpForce);
        rb.linearVelocity = jumpVector;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Vine")) 
    {
            // On n'efface l'ancrage que si le joueur n'est PAS en train de se balancer
            if (!isSwinging)
            {
                currentVineAnchor = null; 
        }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Le joueur a touché : " + collision.gameObject.name + " avec le tag : " + collision.tag);

        if (collision.CompareTag("Vine"))
        {
            Debug.Log("Zone de liane détectée ! Prêt à s'accrocher.");
            currentVineAnchor = collision.transform;
        }
    }
}