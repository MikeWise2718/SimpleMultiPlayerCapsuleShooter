﻿/*     INFINITY CODE 2013-2018      */
/*   http://www.infinity-code.com   */

using UnityEngine;

/// <summary>
/// Class control the map for the SpriteRenderer.
/// </summary>
[AddComponentMenu("Infinity Code/Online Maps/Controls/SpriteRenderer")]
[RequireComponent(typeof(SpriteRenderer))]
public class OnlineMapsSpriteRendererControl:OnlineMapsControlBase2D
{
    private Collider _cl;
    private Collider2D _cl2D;

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Singleton instance of OnlineMapsSpriteRendererControl control.
    /// </summary>
    public new static OnlineMapsSpriteRendererControl instance
    {
        get { return OnlineMapsControlBase.instance as OnlineMapsSpriteRendererControl; }
    }

    /// <summary>
    /// Collider
    /// </summary>
    public Collider cl
    {
        get
        {
            if (_cl == null)
            {
#if UNITY_4_6 || UNITY_4_7
                _cl = collider;
#else
                _cl = GetComponent<Collider>();
#endif
            }
            return _cl;
        }
    }

    /// <summary>
    /// Collider2D
    /// </summary>
    public Collider2D cl2D
    {
        get
        {
            if (_cl2D == null)
            {
#if UNITY_4_6 || UNITY_4_7
                _cl2D = collider2D;
#else
                _cl2D = GetComponent<Collider2D>();
#endif
            }
            return _cl2D;
        }
    }

    private bool GetCoords2D(Vector2 position, out double lng, out double lat)
    {
        lng = lat = 0;
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(position), Mathf.Infinity);
        if (hit.collider == null || hit.collider.gameObject != gameObject) return false;
        if (cl2D == null) return false;

        HitPointToCoords(hit.point, out lng, out lat);
        return true;
    }

    private bool GetCoords3D(Vector3 position, out double lng, out  double lat)
    {
        lng = lat = 0;
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(position), out hit)) return false;

        if (hit.collider.gameObject != gameObject) return false;

        HitPointToCoords(hit.point, out lng, out lat);
        return true;
    }

    public override bool GetCoords(out double lng, out double lat, Vector2 position)
    {
        if (GetCoords2D(position, out lng, out lat)) return true;
        return GetCoords3D(position, out lng, out lat);
    }

    public override Rect GetRect()
    {
        Vector2 p1 = Camera.main.WorldToScreenPoint(spriteRenderer.bounds.min);
        Vector2 p2 = Camera.main.WorldToScreenPoint(spriteRenderer.bounds.max);
        Vector2 s = p2 - p1;
        return new Rect(p1.x, p1.y, s.x, s.y);
    }

    public override Vector2 GetScreenPosition(double lng, double lat)
    {
        double tlx, tly, brx, bry;
        map.GetTileCorners(out tlx, out tly, out brx, out bry);
        int maxX = 1 << map.zoom;
        if (tlx > brx) brx += maxX;

        double px, py;
        map.projection.CoordinatesToTile(lng, lat, map.zoom, out px, out py);

        if (px + maxX / 2 < tlx) px += maxX;

        double rx = (px - tlx) / (brx - tlx) - 0.5;
        double ry = 0.5 - (py - tly) / (bry - tly);

        Bounds bounds = spriteRenderer.sprite.bounds;
        Vector3 size = bounds.size;

        rx *= size.x;
        ry *= size.y;

        Vector3 worldPoint = transform.localToWorldMatrix.MultiplyPoint(new Vector3((float)rx, (float)ry, bounds.center.z));
        return Camera.main.WorldToScreenPoint(worldPoint);
    }

    private void HitPointToCoords(Vector3 point, out double lng, out double lat)
    {
        double tlx, tly, brx, bry;
        map.GetTileCorners(out tlx, out tly, out brx, out bry);

        if (tlx > brx) brx += 1 << map.zoom;

        Bounds bounds = spriteRenderer.sprite.bounds;
        Vector3 size = bounds.size;
        Vector3 localPoint = transform.worldToLocalMatrix.MultiplyPoint(point);
        double px = localPoint.x / size.x + 0.5;
        double py = localPoint.y / size.y + 0.5;
        px = (brx - tlx) * px + tlx;
        py = bry - (bry - tly) * py;

        map.projection.TileToCoordinates(px, py, map.zoom, out lng, out lat);
    }

    protected override void OnEnableLate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Can not find SpriteRenderer.");
            OnlineMapsUtils.DestroyImmediate(this);
        }
    }

    public override void SetTexture(Texture2D texture)
    {
        base.SetTexture(texture);
        MaterialPropertyBlock props = new MaterialPropertyBlock();
#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
        props.AddTexture("_MainTex", texture);
#else
        props.SetTexture("_MainTex", texture);
#endif
        spriteRenderer.SetPropertyBlock(props);
    }
}