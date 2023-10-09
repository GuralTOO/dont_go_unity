ApiMaster (aka API Controller script) handles user coordinates, lists of cities, and initiates ITP activation

Each ITP (of InterpreterBase type) does the actual requests and compiles a list of API retrieved vehicles in its company.
If a vehicle is new, ITP sends it to MapSpawner to instantiate and add to the ActiveVehciles Dictionary.
If a vehicle is old, then its timestamp (cnt) in the dictionary gets updated to cntBoss (retrieved from APIMaster)

The MapSpanwer spanws given vehicles on the map/adds to the dictionary.
It also updates the screen locations of existing vehicles when the map moves.
Additionally, it removes old vehicles if their timeStamp is not up to date.