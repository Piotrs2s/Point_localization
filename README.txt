The sense of point localization program is implementation of method that 
determines whether a point is in or outside of a given geometrical figure.

What it does:
Program generates 10000 randomly distributed points and two random polygons.
Then it counts point inside polygons and also estimates field of all figures 
together basing on quantity of points in it.

How it works:
For each point, program checks if it isn't a part of the side of figure (IsPart method).
If not, it creates at the same height an auxiliary line extended beyond the area 
and counts the number of intersections with the sides of each polygon (IsCrossing method).
An odd quantity of intersections means that the point is outside the figure.

Application:
Determining if the point is in figure finds application e.g in online maps, localization or geofencing.