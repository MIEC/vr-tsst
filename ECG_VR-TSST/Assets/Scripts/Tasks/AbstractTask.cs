using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractTask : MonoBehaviour
{

    [SerializeField]
    protected GenericButton[] _optionalButtons;
    protected GenericButton[] OptionalButtons
    {
        get { return _optionalButtons; }
    }

    [SerializeField]
    protected UnityEvent _finished;
    public UnityEvent Finished
    {
        get { return _finished; }
    }

    /// <summary>
    /// Sets up the object and its references
    /// </summary>
    protected abstract void InactiveSetup();

    /// <summary>
    /// Call this right before the first Next call
    /// </summary>
    [ContextMenu("Init")]
    protected virtual void Init()
    {
        OptionalButtonSwitcher.Instance.EnableButtons(_optionalButtons);
    }

    /// <summary>
    /// Changes object state
    /// </summary>
    /// <returns>true if Next() Operation was successful, false if Finish() should be called</returns>
    [ContextMenu("Next")]
    public abstract bool Next();

    /// <summary>
    /// Changes object state
    /// Overloaded to check the place this task is at to choose the character actions accordingly
    /// </summary>
    /// <returns>true if Next() Operation was successful, false if Finish() should be called</returns>
    [ContextMenu("Next")]
    public abstract bool Next(Round round ,bool secondRoundActive, bool secondRoundAvailable);

    /// <summary>
    /// Finishes an object
    /// </summary>
    [ContextMenu("Finish")]
    protected virtual void Finish()
    {
	    InactiveSetup();
	    OptionalButtonSwitcher.Instance.DisableAll();
        _finished.Invoke();
    }

    /// <summary>
    ///Restarts the object
    /// </summary>
    [ContextMenu("Restart")]
    public virtual void Restart()
    {
        InactiveSetup();
        Init();
        Next();
    }

    /// <summary>
    ///Restarts the object
    /// </summary>
    [ContextMenu("Restart")]
    public virtual void Restart(Round round, bool secondRoundActive, bool secondRoundAvailable)
    {
        InactiveSetup();
        Init();
        Next(round, secondRoundActive, secondRoundAvailable);
    }

    protected void OnEnable()
    {
        InactiveSetup();
    }

    protected void OnDisable()
    {
        InactiveSetup();
    }

}
