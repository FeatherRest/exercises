The project below requires integration between Unity (C#) and a python application.
The functionality itself is kept as simple as possible.

Python:

Develop a python application that hosts a REST endpoint:
    /color
and can be called with a POST of a hex color (e.g. 000000 for black or FF0000 for red) to set the desired ball color.
The default is white (FFFFFF).
You may change the exact message format as you see fit, as long as it includes the desired color code.

Develop a unity application that displays a single ball that will be colored according to the color selected via the python color selector app. 
It's acceptable if it takes up to 2 seconds for the color to update - but the faster the better.
