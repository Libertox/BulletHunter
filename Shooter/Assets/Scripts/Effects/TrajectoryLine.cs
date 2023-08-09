using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter
{
    public class TrajectoryLine: MonoBehaviour
    {
        [SerializeField] private int linePoints = 25;
        [SerializeField] private float timeBetweenPoints = 0.1f;
        [SerializeField] private LineRenderer lineRenderer;

        public void DrawLine(Vector3 startPosition, Vector3 startVelocity)
        {
            Show();
            lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
       
            int i = 0;
            lineRenderer.SetPosition(i, startPosition);
            for (float time = 0; time < linePoints; time += timeBetweenPoints)
            {
                i++;
                Vector3 point = startPosition + time * startVelocity;
                point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y * 0.5f * time * time);

                lineRenderer.SetPosition(i, point);

                Vector3 lastPosition = lineRenderer.GetPosition(i - 1);
                if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude))
                {
                    lineRenderer.SetPosition(i, hit.point);
                    lineRenderer.positionCount = i + 1;
                    return;
                }

            }
        }


        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}
