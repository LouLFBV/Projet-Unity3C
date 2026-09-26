using UnityEngine;
/// <summary>
/// Defines predefined gravitational acceleration values for different celestial bodies.
/// </summary>
public enum GravityPreset
{
    /// <summary>
    /// Represents the Sun's gravitational acceleration.
    /// </summary>
    Sun,
    /// <summary>
    /// Represents Jupiter's gravitational acceleration.
    /// </summary>
    Jupiter,
    /// <summary>
    /// Represents Neptune's gravitational acceleration.
    /// </summary>
    Neptune,
    /// <summary>
    /// Represents Saturn's gravitational acceleration.
    /// </summary>
    Saturn,
    /// <summary>
    /// Represents Earth's gravitational acceleration.
    /// </summary>
    Earth,

    /// <summary>
    /// Represents Uranus's gravitational acceleration.
    /// </summary>
    Uranus,
    /// <summary>
    /// Represents Venus's gravitational acceleration.
    /// </summary>
    Venus,
    /// <summary>
    /// Represents Mars's gravitational acceleration.
    /// </summary>
    Mars,
    /// <summary>
    /// Represents Mercury's gravitational acceleration.
    /// </summary>
    Mercury
}

/// <summary>
/// Defines the gravitational acceleration applied to an object.
/// Supports predefined celestial body values or a custom gravity value.
/// </summary>
public class GravityObject : MonoBehaviour
{
    [Header("Properties")]
    /// <summary>
    /// The predefined celestial body used to determine the gravitational acceleration.
    /// </summary>
    [SerializeField] private GravityPreset _gravity = GravityPreset.Earth;
    /// <summary>
    /// A custom gravitational acceleration value.
    /// A value of <c>0.0f</c> causes the selected <see cref="GravityPreset"/> to be used instead.
    /// </summary>
    [SerializeField] private float _customGravity = 0.0f;
    /// <summary>
    /// Gets the effective gravitational acceleration.
    /// Uses the custom value when it is different from zero; otherwise, uses the selected preset.
    /// </summary>
    private float _realGravity => _customGravity == 0.0f ?  GravityObject.GetGrav(_gravity) : _customGravity;

    /// <summary>
    /// Gets the effective gravitational acceleration applied by this object.
    /// </summary>
    public float RealGravity  => _realGravity;
    /// <summary>
    /// Gets the gravitational acceleration associated with the specified preset.
    /// </summary>
    /// <param name="preseptG">The gravitational preset to evaluate.</param>
    /// <returns>The gravitational acceleration associated with the specified preset.</returns>
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
}
