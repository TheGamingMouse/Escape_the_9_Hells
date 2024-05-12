using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string promt { get; }
    public string npcName { get; }
    public bool InteractE(Interactor interactor);
    public bool InteractQ(Interactor interactor);
}
