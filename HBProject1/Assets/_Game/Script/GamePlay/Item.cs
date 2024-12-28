using System;
using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemType type;
    public ItemType Type => type;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 2;

    public event Action<Item> CollectEvent;

    public bool IsArrive(Vector3 target)
    {
        return Vector3.Distance(rb.position, target) < 0.1f;
    }

    public void OnMove(Vector3 targetPoint)
    {
        rb.position = Vector3.MoveTowards(rb.position, targetPoint, Time.deltaTime * speed);
    }

    public void OnMove(Vector3 targetPoint, Quaternion targetRot, float time)
    {
        StartCoroutine(IEOnMove(targetPoint, targetRot, time));
    }

    private IEnumerator IEOnMove(Vector3 targetPoint, Quaternion targetRot, float time)
    {
        float timeCount = 0;
        Vector3 startPoint = rb.position;
        Quaternion startRot = rb.rotation;

        while (timeCount < time)
        {
            timeCount += Time.deltaTime;
            rb.position = Vector3.Lerp(startPoint, targetPoint, timeCount / time);
            rb.rotation = Quaternion.Lerp(startRot, targetRot, timeCount / time);
            yield return null;
        }
    }

    public void OnSelect()
    {
        rb.useGravity = false;
    }

    public void OnDrop()
    {
        rb.useGravity = true;
    }

    public void Force(Vector3 force)
    {
        OnDrop();
        rb.velocity = Vector3.zero;
        rb.AddForce(force);
    }

    internal void SetKinematic(bool v)
    {
        rb.isKinematic = v;
    }

    internal void Collect()
    {
        CollectEvent?.Invoke(this);
        gameObject.SetActive(false);
    }
}
