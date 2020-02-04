# Hashcode 2020
This solution will produce a *perfect* score, collectively testing all cases in under a second (~0.81 seconds average). 
It is a **recursive** solution written in **C#** and does not implement memoization. 

##Score
![Score](https://github.com/trevorlao95/hashcode2020/blob/master/hashcode2020qualifier/img/score.png)

##Input
This is the root of the program. It's goal is to immediately locate the inputs and convert them for the Main method. 

![Read Input](https://github.com/trevorlao95/hashcode2020/blob/master/hashcode2020qualifier/img/input1.png)
For this solution, only two variables need to be extracted from the input: **the target number of slices** and an **enumerable structure** of given pizzas. The helper function *ReadInput* locates the file while *Pizzafy* takes in the converted inputs and the name of the output ("a_output.txt"). 

![Watch](https://github.com/trevorlao95/hashcode2020/blob/master/hashcode2020qualifier/img/input2.png)
A watch is added to time the solution in milliseconds. This is not needed to solve the solution.

![Thread](https://github.com/trevorlao95/hashcode2020/blob/master/hashcode2020qualifier/img/input3.png)
As *Pizzafy* will go very deep recursively, the maximum stack size needs to be increased prior to calling the function. This is required for **e_also_big.in** and other larger custom inputs.

##Main
This is the Main method. It's goal is to call the recursive function and then produce the respective output file, optionally logging the results along the way. 

![Main](https://github.com/trevorlao95/hashcode2020/blob/master/hashcode2020qualifier/img/main1.png)
**Repizzafy** will repeatedly call upon itself until the satisfactory stack of Pizza's is returned. The next paragraph converts and logs the results into Slices, Pizzas needed and the order of Pizzas needed. The last two lines create the required output file. 

##Recursion
This is the recursion method. It's goal is to construct and return the correct Pizza Stack structure all the way back to the main method. The chosen return types are a Tuple of int (Slices Eaten) and a Stack<string> (Pizzas Eaten). The int is used as a comparator of slices in each branch, while the stack is used to track the Pizzas, manipulating it in *O(1)* time (Pop, Push).  

![Inputs](https://github.com/trevorlao95/hashcode2020/blob/master/hashcode2020qualifier/img/recursion1.png)
* **i**: This is the current index of recursion. Each index represents a layer of the recursive tree and a respective pizza from the stack originally given.
* **n**: This is the target number of slices, a given input.
* **se**: This is the amount of slices eaten at each iteration.
* **he**: This is the highest slices eaten in any iteration encountered so far.
* **s**: This is the Int Array that contains each pizza and their slices.
* **pe**: This is the order of pizzas eaten in the current iteration.
* **hep**: This is the order of Pizzas eaten in the highest iteration encountered so far.

###Base Cases
Base Cases, according to (https://web.mit.edu/6.005/www/fa16/classes/14-recursion/#structure_of_recursive_implementations) are the simplest, smallest instance of the problem, that can’t be decomposed any further. Base cases often correspond to emptiness – the empty string, the empty list, the empty set, the empty tree, zero, etc.

![First Base Case](https://github.com/trevorlao95/hashcode2020/blob/master/hashcode2020qualifier/img/recursion2.png)
This is the first base case. It's designed to exit the current recursion stack when **i** (Index) reaches below 0. **i** will continue to decrease as long as *it can*, but keep in mind that this scenario only pertains to *one* recursive branch and many more of these branches will likely be traversed. In other words, this base case will be hit many times before you reach the right answer. 

Everytime **i** reaches below 0, all the possible values for that particular branch have been explored. Therefore, if the amount of slices eaten is greater than the highest recorded *so far*, then set **he** (Highest Eaten) to **se** (Slices eaten), and **hep** (Highest Eaten Pizzas) to **pe** (Current Pizzas Eaten). 

In both scenarios, the value is returned by a layer without recursing further.

![Second Base Case](https://github.com/trevorlao95/hashcode2020/blob/master/hashcode2020qualifier/img/recursion3.png)
This is the second base case. It's designed to exit the entire recursive function when the current slices eaten plus the next pizza, equal *exactly* our target. In other words, there is no need to continue recursing when the solution is already found. 

Note: This method currently only exits one branch of recursion, there are additional checks below to ensure that the tree collapses at every branch and eventually returns to the root. 

##Recursive Steps
The recursive steps represent the usual case steps for the method. They are continually called upon to eventually construct a recursive tree. 

![First Recursive Step](https://github.com/trevorlao95/hashcode2020/blob/master/hashcode2020qualifier/img/recursion4.png)
This is the first recursive step. It's designed as an adaptation to a tree traversal to ensure that all values for that current iteration are looked at. If **se** (Slices eaten) plus **s[i]** (Pizza specified by the Index) is greater than our target, then our current branch needs to continue by skipping the current index. 
In larger input scenarios, this is unlikely to result in a value closer to the target until many indexes away (Think n = 500, se = 450, s[i, i-1...] = 300, 250...). In cases where the lower bound is relatively high, it's not unusual for this branch to continually wash away until exiting through the first base case. 

![Second Recursive Step](https://github.com/trevorlao95/hashcode2020/blob/master/hashcode2020qualifier/img/recursion5.png)
This is the second recursive step. It's designed to traverse the tree many times, creating the bulk of the *legal* (se < n) nodes. 
The method recurses the right-hand side of the tree first, as this is the side more likely to be closer to the target value (A larger target value, will likely have higher bounds of Pizza's, meaning we are less likely to fill the target by starting with the smaller indexes).  

The method also contains exit conditions for Base Case 2, where the entire tree is required to immediately collapse. The two fall out conditions ensure that any returning branches will immediately exit or else be trapped in any more recursive layers (Which is the normal route for any value that is not exactly **n**). This is a comparatively unusual exit condition, as Base Case 1 only exits the current iteration and not the entire tree. 

Each time *Repizzafy* is called, a branch is created and the ones that do not exit early reach the last few comparison lines. This will always grab and return the larger value without fear of it being an illegal node (the value was created by pizzas in the stack and any value greater than **n** was weeded out prior). 


