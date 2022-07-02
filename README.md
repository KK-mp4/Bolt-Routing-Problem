# Minecraft Piton Bolt Network Routing Problem (work in progress)
## Different algorithms for connecting stations in a piston bolt network

With all the new development in [piston bolt tech](https://youtube.com/playlist?list=PLI-RNUGw-AeRkX7MQm9ArljzVCuuSzg0y) old servers might consider rebuilding their piston bolt network in the nether.

When you already know where the stations will be it is an interesting [combinatorial optimization](https://en.wikipedia.org/wiki/Combinatorial_optimization) problem of how to connect all stations with piston bolts in a way that your system is **both easy** to build (has smallest total piston bolt length) **and fast** (has smallest average piston bolt travel time between any set of two stations). I was always sleeping at [graph theory](https://en.wikipedia.org/wiki/Graph_theory) lectures, so I'm not saying that this is good stuff I coded in here.

## Problem Domain
Generalizing and looking outside the Minecraft this problem comes down to finding an optimal interconnect for a given set of points (stations) on a Euclidean 2 dimentional plane. Minecraft restriction adds to this problem the fact that plane is actually a rectilinear lattice graph. So classical euclidean geometry is getting replaced.
Graph itself will be weighted and [undirected](https://en.m.wikipedia.org/wiki/Graph_(discrete_mathematics)#Undirected_graph), since ether people always build bolts both ways, or maybe you managed to get falling end portal, thus allowing you to teleport back to spawn instantly. Weight of a vertex will be calculated using [Chebyshev distance](https://en.wikipedia.org/wiki/Chebyshev_distance) since it takes same amount of time to go 1 block ether diagonally or straight.

For all the calculations I will use coordinates of stations at [Dugged SMP](https://redirect.dugged.net:8443/map/#Survival-Nether-Top/0/7/128/-442/64), but same can be applied to any.<br/>

![image](https://user-images.githubusercontent.com/103208695/176758019-9b6523cc-89e9-464a-837e-a7187b8d20b1.png)

### 1. Merging at coordinates.
First solution doesn’t go far from the existing system. All stations are connected to a single internal node (portal to main storage system in our case), in graph theory this is also called a [star network](https://en.m.wikipedia.org/wiki/Star_(graph_theory)). But it is optimized from what we currently have by going as much as possible diagonally before going straight. That way we shorten travel time (since diagonally we travel sqrt((20^2)+(20^2)) = 28.28 m/s instead of 20 m/s).<br/>
![image](https://user-images.githubusercontent.com/103208695/176998754-63e4d135-e6cb-41ab-8d84-6db799609430.png)
|||
| ------------- | ------------- |
| Bolt count  | 29 |
| Total bolt distance  | 19.2 km  |
| Total travel time  | 16 min  |
| Average travel time  | 66.1 sec  |

### 2. Merging at average coordinates.
Something that sounds like a good idea but is not.<br/>
![image](https://user-images.githubusercontent.com/103208695/176998779-be364226-eeb7-4b0a-9324-64b2aabe47ba.png)
|||
| ------------- | ------------- |
| Bolt count  | 29 |
| Total bolt distance  | 19.6 km  |
| Total travel time  | 16.4 min  |
| Average travel time  | 67.7 sec  |

### 3. All to All
Of course I had to try this, theoretically shortest amount of travel time - 16 sec less than previous results.<br/>
![image](https://user-images.githubusercontent.com/103208695/176998799-48fed7fc-65a2-462a-a2dd-3bad63811843.png)
|||
| ------------- | ------------- |
| Bolt count  | 435 |
| Total bolt distance  | 432 km  |
| Total travel time  | 360 min  |
| Average travel time  | 50 sec  |

### 4. [Nearest Neighbour (NN) algorithm](https://en.wikipedia.org/wiki/Nearest_neighbour_algorithm)
Heuristic solution to [travelling salesman problem](https://en.wikipedia.org/wiki/Travelling_salesman_problem). This should find the shortest rout possible without additional stations (terminals) by going to each station only once. I tried starting from different stations and seems like our downaccel gold farm gives the shortest total bolt distance - 2 times less than what we currenly have. Sadly average travel time got 3 times more than what we currently have.<br/>
![image](https://user-images.githubusercontent.com/103208695/176998817-9e8a011d-32d2-4461-86ba-0584786e0263.png)
|||
| ------------- | ------------- |
| Bolt count  | 29 |
| Total bolt distance  | 11.7 km  |
| Total travel time  | 9.8 min  |
| Average travel time  | 293 sec  |

### 5. Loop
Small improvement to average travel time for NN algorithm we can do is to close the loop, this decreases travel time by 50%.<br/>
![image](https://user-images.githubusercontent.com/103208695/176998841-85e70cb8-453b-42c1-a045-5c47a00e5edd.png)
|||
| ------------- | ------------- |
| Bolt count  | 30 |
| Total bolt distance  | 14.6 km  |
| Total travel time  | 12.2 min  |
| Average travel time  | 182.7 sec  |

### 6. [Rectilinear Steiner tree](https://en.m.wikipedia.org/wiki/Steiner_tree_problem)
Work in progress on a heuristic [algorithm](https://www.textroad.com/pdf/JBASR/J.%20Basic.%20Appl.%20Sci.%20Res.,%203(1s)611-613,%202013.pdf) for constracting Stainer minimal tree<br/>

# Getting started
Eh, it is not really user friendly, find Bolt_List.json and enter your stations by hand.<br/>
![image](https://user-images.githubusercontent.com/103208695/176999826-1f5e58fc-1c66-46b0-b786-b07adbca8225.png)

# License
This program is licensed under the GNU General Public License v3.0. Please read the License file to know about the usage terms and conditions.

