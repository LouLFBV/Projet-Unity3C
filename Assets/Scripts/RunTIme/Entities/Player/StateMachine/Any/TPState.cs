using UnityEngine;

/// <summary>
/// Represents the state in which the player prepares and executes a teleport.
/// Handles teleport timing, ghost positioning, teleport zone visualization,
/// mana consumption, and teleport destination collision resolution.
/// </summary>
class TPState : PlayerState
{
    /// <summary>
    /// Duration of the teleport animation in seconds.
    /// </summary>
    private float _tpAnimationDuration = 1.483f;
    /// <summary>
    /// Timestamp at which the teleport animation started, using unscaled time.
    /// </summary>
    private float _tpTimer = float.MinValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="TPState"/> class.
    /// </summary>
    /// <param name="character">Player character associated with this state.</param>
    public TPState(PlayerCharacter character) : base(character) { }
    /// <summary>
    /// Initializes the teleport state, checks mana availability,
    /// freezes the game time scale, displays the teleport ghost,
    /// and triggers the teleport start animation.
    /// </summary>
    public override void Enter()
    {
        Debug.Log("<color=yellow>[TPState]</color> Enter");

        if (!Character.ManaSystem.HasEnoughMana(Character.CostTP))
        {
            SetPopState(1);
            return;
        }

        Time.timeScale = Character.TimeScaleInTP;
        _tpTimer = Time.unscaledTime;

        if (Character.SpriteGhost != null)
        {
            Character.SpriteGhost.SetActive(true);
        }

        Character.TriggerStartTP();
    }

    /// <summary>
    /// Updates the teleport state, including the ghost position,
    /// teleport zone timer, cancellation checks, and teleport execution.
    /// </summary>
    public override void Update()
    {
        if (!Character.ManaSystem.HasEnoughMana(Character.CostTP) || Character.IsCancelTP)
        {
            if (Character.IsCancelTP)
            {
                Character.TriggerCancelTP();
            }
            SetPopState(1);
            return;
        }

        UpdateGhostPosition();
        UpdateTPZoneTimer();

        bool timeOut = (Time.unscaledTime - _tpTimer) >= _tpAnimationDuration;
        bool keyReleased = !Character.IsInTP;

        if (timeOut || keyReleased)
        {
            ExecuteTP();
        }
    }

    /// <summary>
    /// Cleans up the teleport state by hiding the teleport visuals
    /// and restoring the normal time scale.
    /// </summary>
    public override void Exit()
    {
        if (Character.SpriteGhost != null)
        {
            Character.SpriteGhost.SetActive(false);
        }
        if (Character.TPZoneTimer != null)
        {
            Character.TPZoneTimer.transform.localScale = Character.TPZone.transform.localScale;
            Character.TPZoneTimer.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    /// <summary>
    /// Executes the teleport by calculating the target position,
    /// resolving collisions, moving the player, consuming mana,
    /// and triggering the teleport event.
    /// </summary>
    private void ExecuteTP()
    {
        Vector2 origin = Character.Body.Position;
        Vector2 targetPosition = CalculateTargetPosition(origin);
        Vector2 finalPosition = ResolveTeleportPosition(origin, targetPosition);

        Character.Body.SetPosition(finalPosition);

        if (Character.ManaSystem != null)
        {
            Character.ManaSystem.ConsumeMana(Character.CostTP);
        }

        Character.TriggerTP();
        SetPopState(1);
    }

    /// <summary>
    /// Updates the teleport ghost position and orientation based on the current mouse position.
    /// </summary>
    private void UpdateGhostPosition()
    {
        if (Character.SpriteGhost == null) return;

        Vector2 origin = Character.Body.Position;
        Vector2 targetPosition = CalculateTargetPosition(origin);
        Vector2 finalPosition = ResolveTeleportPosition(origin, targetPosition);

        // Position the teleport ghost at the resolved destination.
        Character.SpriteGhost.transform.position = finalPosition;

        // Orient the ghost toward the position targeted by the mouse.
        if (Character.SpriteGhost.TryGetComponent<SpriteRenderer>(out var ghostSprite))
        {
            float aimDirectionX = targetPosition.x - origin.x;
            if (Mathf.Abs(aimDirectionX) > 0.01f)
            {
                ghostSprite.flipX = aimDirectionX < 0;
            }
        }
    }
    /// <summary>
    /// Updates the teleport zone timer scale based on the elapsed teleport animation time.
    /// </summary>
    private void UpdateTPZoneTimer()
    {
        if (Character.TPZoneTimer == null) return;

        // Normalize the elapsed time between 0 and 1.
        float elapsedTime = Time.unscaledTime - _tpTimer;
        float progress = Mathf.Clamp01(elapsedTime / _tpAnimationDuration);

        // Invert the progress to reduce the scale from 1 (normal size) to 0 (disappearance).
        float scaleRatio = 1f - progress;

        // Use the teleport zone scale as the reference, or Vector3.one by default.
        Vector3 baseScale = Character.TPZone != null
            ? Character.TPZone.transform.localScale
            : Vector3.one;

        Character.TPZoneTimer.transform.localScale = baseScale * scaleRatio;
    }

    #region --- TRAJECTORY CALCULATIONS ---

    /// <summary>
    /// Calculates the desired teleport destination based on the current mouse position,
    /// constrained by the maximum teleport distance.
    /// </summary>
    /// <param name="origin">Starting position of the teleport.</param>
    /// <returns>The desired teleport destination.</returns>
    private Vector2 CalculateTargetPosition(Vector2 origin)
    {
        // Fallback in case the main camera cannot be found.
        Camera cam = Camera.main ?? Object.FindFirstObjectByType<Camera>();
        if (cam == null) return origin;

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(cam.transform.position.z - Character.transform.position.z);

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 cursorDirection = (Vector2)mouseWorldPos - origin;

        if (cursorDirection.magnitude > Character.DistanceTP)
        {
            cursorDirection = cursorDirection.normalized * Character.DistanceTP;
        }

        return origin + cursorDirection;
    }
    /// <summary>
    /// Resolves the final teleport position by checking for obstacles along the trajectory
    /// and preventing the player from teleporting inside a collider.
    /// </summary>
    /// <param name="origin">Starting position of the teleport.</param>
    /// <param name="target">Desired teleport destination.</param>
    /// <returns>A safe teleport destination that does not intersect with the ground layer.</returns>
    private Vector2 ResolveTeleportPosition(Vector2 origin, Vector2 target)
    {
        Vector2 direction = target - origin;
        float distance = direction.magnitude;

        if (distance <= 0.05f) return origin;

        direction.Normalize();
        Vector2 boxSize = Character.Collider.size * 0.9f;

        // 1. Check whether the target position is clear.
        Collider2D overlap = Physics2D.OverlapBox(target, boxSize, 0f, Character.GroundLayer);
        if (overlap == null) return target;

        // 2. Check for an obstacle encountered along the teleport trajectory.
        RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, direction, distance, Character.GroundLayer);
        if (hit)
        {
            float safeDistance = Mathf.Max(0f, hit.distance - Character.SkinWidth);
            return origin + direction * safeDistance;
        }

        return target;
    }
    #endregion
}