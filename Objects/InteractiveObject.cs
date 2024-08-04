using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InteractiveObject
{
    public void OnPlayerInteracting(PlayerAnimator _playerAnimator, InteractiveObject _interactObject);
}