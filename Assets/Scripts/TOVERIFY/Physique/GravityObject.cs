using UnityEngine;

public enum GravityPreset
{
    Sun,
    Jupiter,
    Neptune,
    Saturn,
    Earth,
    Uranus,
    Venus,
    Mars,
    Mercury
}


public class GravityObject : MonoBehaviour
{
    [SerializeField] private ForceObject force = null;
    [SerializeField] private GravityPreset gravity = GravityPreset.Earth;
    [SerializeField] private float customGravity = 0.0f;
    private float RealGravity => customGravity == 0.0f ?  GravityObject.GetGrav(gravity) : customGravity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (force == null)
            force = GetComponent<ForceObject>();
        if(force == null)
            Debug.LogError("no Component found set it manually");
    }

    static float GetGrav(GravityPreset preseptG)
    {
        switch (preseptG)
        {
            case GravityPreset.Sun:
                return 274.0f;
         
            case GravityPreset.Jupiter:
                return 24.79f;

            case GravityPreset.Neptune:
                return 11.15f;
            
            case GravityPreset.Saturn:
                return 10.44f;
             
            case GravityPreset.Earth:
                return 9.81f;
             
            case GravityPreset.Uranus:
                return 8.69f;
             
            case GravityPreset.Venus:
                return 8.87f;
               
            case GravityPreset.Mars:
                return 3.71f;
               
            case GravityPreset.Mercury:
                return 3.7f;
                
        }

        return 0.0f;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
            force.AddForce(Vector2.down * RealGravity, ForceType.Force);
    }
}
