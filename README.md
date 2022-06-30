# Piton Bolt Network Routing Problem (work in progress)
## Different algorithms for connecting stations in a piston bolt network

With all the new development in [piston bolt tech](https://youtube.com/playlist?list=PLI-RNUGw-AeRkX7MQm9ArljzVCuuSzg0y) old servers might consider rebuilding their piston bolt network in the nether.

When you already know where the stations will be it is an interesting [combinatorial optimization](https://en.wikipedia.org/wiki/Combinatorial_optimization) problem of how to connect all stations with piston bolts in a way that your system is **both easy** to build (has least piston bolt length) **and fast** (smallest piston bolt travel time). I was always sleeping at [graph theory](https://en.wikipedia.org/wiki/Graph_theory) lectures, so I'm not saying that this is good stuff I coded in here

For all the calculations I will use coordinates of stations at [Dugged SMP](https://redirect.dugged.net:8443/map/#Survival-Nether-Top/0/7/128/-442/64), but same can be applied to any.<br/>
![image](https://user-images.githubusercontent.com/103208695/176758019-9b6523cc-89e9-464a-837e-a7187b8d20b1.png)

### 1. Merging at our storage.
First solution doesn’t go far from the existing system, just optimizes it. By going as much as possible diagonally before going straight we shorten travel time (since diagonally we travel sqrt((20^2)+(20^2)) = 28.28 m/s instead of 20 m/s).<br/>
![image](https://user-images.githubusercontent.com/103208695/176758216-699fc23a-e4bd-4764-9873-b6ea1fdb21b6.png)

### 2. Merging at average coordinates.
Saves us one second of average travel time!<br/>
![image](https://user-images.githubusercontent.com/103208695/176758358-d4546ed2-9a82-4924-9e9b-a8835912940b.png)

### 3. All to All
Of course I had to try this, theoretically shortest amount of travel time.<br/>
![image](https://user-images.githubusercontent.com/103208695/176761921-fd75888c-84c7-4321-b673-a35a9a239e09.png)

### 4. [Travelling salesman problem. #1](https://en.wikipedia.org/wiki/Travelling_salesman_problem)
This should find the shortest rout possible by going to each station only once. Problem is that actual solution is amount of stations factorial, so for us it is **27!**...
So first thing I tried is to just go from farthest stations from 0, 0 to closest, as you can see this is a very bad approach.<br/>
![image](https://user-images.githubusercontent.com/103208695/176759044-f2f7bd42-7bf7-42d9-8f1b-27cd0244ced0.png)
