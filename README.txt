Team Members:
	Alexander Jackson 587332
	David Toulmin 638541
	
Terrain Implementation:
	The terrain is generated using the DiamondSquare algorithm acting on a 2D array of points. The algorithm generates a value at each point
	that is used to represent the height of the terrain at that point.
	Once this step is completed, the coordinate array is passed to another function which iterates over the coordinates twice to construct a list
	of VertexPositionNormalColor objects, with the position data for each being represented by it's indices in the array and the value stored
	at those indices. Using the coordinate system, the map is laid out as a grid. The first iteration through the coordinates constructs the
	'upperleft' triangle of each square on the grid and the second iteration constructs the 'bottomright' triangle.
	These are then passed into a SharpDX vertex buffer and used to construct the terrain model.
	
	The lighting is done by using the sun model to generate a vector representing the direction the sunlight is coming from relative to the XZ plane.
	Other objects can then access this data using a function to read it from the sun, and this is used in their draw calls to calculate how they're lit.