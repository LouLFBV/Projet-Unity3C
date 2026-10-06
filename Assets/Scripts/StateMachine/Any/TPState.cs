using UnityEngine;

class TPState : PlayerState
{
    private float _tpAnimationDuration = 1.483f;
    private float _tpTimer = float.MinValue;

    public TPState(PlayerCharacter character) : base(character) { }

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

    public override void FixedUpdate() { }

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

    private void UpdateGhostPosition()
    {
        if (Character.SpriteGhost == null) return;

        Vector2 origin = Character.Body.Position;
        Vector2 targetPosition = CalculateTargetPosition(origin);
        Vector2 finalPosition = ResolveTeleportPosition(origin, targetPosition);

        // Positionnement du fantôme
        Character.SpriteGhost.transform.position = finalPosition;

        //  Orienter le fantôme vers la position visée par la souris
        if (Character.SpriteGhost.TryGetComponent<SpriteRenderer>(out var ghostSprite))
        {
            float aimDirectionX = targetPosition.x - origin.x;
            if (Mathf.Abs(aimDirectionX) > 0.01f)
            {
                ghostSprite.flipX = aimDirectionX < 0;
            }
        }
    }

    private void UpdateTPZoneTimer()
    {
        if (Character.TPZoneTimer == null) return;

        // Temps écoulé ramené entre 0 et 1
        float elapsedTime = Time.unscaledTime - _tpTimer;
        float progress = Mathf.Clamp01(elapsedTime / _tpAnimationDuration);

        // Inversion pour réduire de 1 (taille normale) jusqu'à 0 (disparition)
        float scaleRatio = 1f - progress;

        // Scale de référence (prend la taille de TPZone ou Vector3.one par défaut)
        Vector3 baseScale = Character.TPZone != null
            ? Character.TPZone.transform.localScale
            : Vector3.one;

        Character.TPZoneTimer.transform.localScale = baseScale * scaleRatio;
    }

    #region --- CALCULS DE TRAJECTOIRE ---

    private Vector2 CalculateTargetPosition(Vector2 origin)
    {
        // Sécurité si Camera.main est introuvable
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

    private Vector2 ResolveTeleportPosition(Vector2 origin, Vector2 target)
    {
        Vector2 direction = target - origin;
        float distance = direction.magnitude;

        if (distance <= 0.05f) return origin;

        direction.Normalize();
        Vector2 boxSize = Character.Collider.size * 0.9f;

        // 1. Zone dégagée
        Collider2D overlap = Physics2D.OverlapBox(target, boxSize, 0f, Character.GroundLayer);
        if (overlap == null) return target;

        // 2. Obstacle rencontré
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