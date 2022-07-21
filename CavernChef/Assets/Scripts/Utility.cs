using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For Math stuff
using System;

public class Utility
{
    // Utility function
    // Returns the distance of a point P to a segment (not line) AB
    public static float distToSegment(Vector2 P, Vector2 A, Vector2 B) {
        if (A.x == B.x) {   // If AB is vertical
            if (P.y >= Math.Min(A.y,B.y) && P.y <= Math.Max(A.y,B.y)) {   // If P is within the """width""" of AB
                return Math.Abs(P.x - A.x);
            } else {   // If P is not within the """width of AB"
                return Math.Min((P-A).magnitude, (P-B).magnitude);
            }
        
        } else if (A.y == B.y) {   // If AB is horizontal
            if (P.x >= Math.Min(A.x,B.x) && P.x <= Math.Max(A.x,B.x)) {   // If P is within the """width""" of AB
                return Math.Abs(P.y - A.y);
            } else {
                return Math.Min((P-A).magnitude, (P-B).magnitude);
            }
        
        // If AB is slanted
        } else {
            /*
            Let m = (B.y-A.y)/(B.x-A.x) be the slope of line AB
                l be the line through P perp to AB
            Line AB: y = m(x-A.x) + A.y
                 l:  y = -1/m (x-P.x) + P.y
            AB \cap l:    m(x-A.x) + A.y = -1/m (x-P.x) + P.y
                       m^2(x-A.x) + mA.y = P.x - x + mP.y
                                (m^2+1)x = m^2A.x - mA.y + P.x + mP.y
                                       x = (m^2A.x - mA.y + P.x + mP.y)/(m^2+1)
                Check if this x is between A.x and B.x
            */
            float m = (B.y - A.y)/(B.x - A.x);
            float x_int = (m*m*A.x - m*A.y + P.x + m*P.y)/(m*m + 1);

            if (x_int >= Math.Min(A.x,B.x) && x_int <= Math.Max(A.x,B.x)) {   // If P is within the """width""" of AB
                return distToLine(P, A, B);
            } else {
                return Math.Min((P-A).magnitude, (P-B).magnitude);
            }
        }

        return 0;
    }

    // Utility function
    // Returns the distance of a point P to a NON-VERTICAL line (not segment) AB
    // Actually maybe I should just make this apply to vertical lines too
    public static float distToLine(Vector2 P, Vector2 A, Vector2 B) {
        if (A.x == B.x) {
            return Math.Abs(P.x-A.x);
        }
        // Ax+By+C = 0 to (X,Y) is |AX+BY+C|/sqrt(A^2+B^2)
        // y-y_0 = m(x-x_0)  <=>  y = mx -mx_0+y_0  <=>  mx - y + y_0-mx_0 = 0
        float m = (B.y-A.y)/(B.x-A.x);
        float nume = m*P.x - P.y + A.y - m*A.x;
        float denom = (float) Math.Sqrt(m*m + 1);
        return nume/denom;
    }

    /*
    // Utility function
    // Returns the distance of (X,Y) to Ax+By+C = 0
    public float dist(Vector2 pos, float A, float B, float C) {
        return (float) (   Math.Abs(A*pos.x + B*pos.y + C) / Math.Sqrt(A*A + B*B)   );
    }
    */
}
