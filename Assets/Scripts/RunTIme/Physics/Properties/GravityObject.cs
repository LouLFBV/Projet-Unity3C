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
public class GravityObject
{
    public static float GetGrav(GravityPreset preseptG)
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
