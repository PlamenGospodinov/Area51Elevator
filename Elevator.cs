using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Area51Elevator
{
   
    enum BaseFloors { G, S, T1, T2 };
    class Elevator
    {
        //EVENT FOR THE END OF THE WORKDAY
        ManualResetEvent eventFinishedWork = new ManualResetEvent(false);


        const int maxPeopleInElevator = 1;

        Semaphore semaphore;

        Random rand = new Random();

        List<Agent> agentsInElevator;
        Agent.AgentCurrentFloor currentFloor;
        public Elevator()
        {
            semaphore = new Semaphore(1, maxPeopleInElevator);
            agentsInElevator = new List<Agent>();
        }

        static int currentFloorTime;
        static int lastFloorTime = 0;

        public void EnterElevator(Agent agents)
        {
            semaphore.WaitOne();
            lock (agentsInElevator)
            {
                agentsInElevator.Add(agents);
                Console.WriteLine(agents.AgentNumber + " entered the elevator.");
                InsideElevator(agents);
            }
        }

        public void InsideElevator(Agent agents)
        {
            //1s per floor wait time
            switch (agents.CurrentAgentFloor)
            {
                case Agent.AgentCurrentFloor.T2:
                    currentFloorTime = 3000;
                    Thread.Sleep(Math.Abs(currentFloorTime - lastFloorTime));
                    lastFloorTime = currentFloorTime;
                    break;
                case Agent.AgentCurrentFloor.T1:
                    currentFloorTime = 2000;
                    Thread.Sleep(Math.Abs(currentFloorTime - lastFloorTime));
                    lastFloorTime = currentFloorTime;
                    break;
                case Agent.AgentCurrentFloor.S:
                    currentFloorTime = 1000;
                    Thread.Sleep(Math.Abs(currentFloorTime - lastFloorTime));
                    lastFloorTime = currentFloorTime;
                    break;
                case Agent.AgentCurrentFloor.G:
                    currentFloorTime = 0;
                    Thread.Sleep(Math.Abs(currentFloorTime - lastFloorTime));
                    lastFloorTime = currentFloorTime;
                    break;
            }
            bool inTheElevator = true;
            var elevatorAction = agents.GetRandomFloor();
            while (inTheElevator == true)
            {
                int randomAction = rand.Next(1, 5);

                switch (elevatorAction)
                {
                    case BaseFloors.T2:
                        currentFloorTime = 3000;
                        Thread.Sleep(Math.Abs(currentFloorTime - lastFloorTime));
                        lastFloorTime = currentFloorTime;
                        Console.WriteLine(agents.AgentNumber + " arrived at level T2");
                        if (agents.AgentLevel == "Top-Secret")
                        {
                            inTheElevator = false;
                            agents.CurrentAgentFloor = Agent.AgentCurrentFloor.T2;
                        }
                        else
                        {
                            elevatorAction = GenerateNewFloor(randomAction-1);
                        }
                        break;
                    case BaseFloors.T1:
                        currentFloorTime = 2000;
                        Thread.Sleep(Math.Abs(currentFloorTime - lastFloorTime));
                        lastFloorTime = currentFloorTime;
                        Console.WriteLine(agents.AgentNumber + " arrived at level T1");
                        if (agents.AgentLevel == "Top-Secret")
                        {
                            inTheElevator = false;
                            agents.CurrentAgentFloor = Agent.AgentCurrentFloor.T1;
                        }
                        else
                        {
                            elevatorAction = GenerateNewFloor(randomAction - 2);
                        }
                        break;
                    case BaseFloors.S:
                        currentFloorTime = 1000;
                        Thread.Sleep(Math.Abs(currentFloorTime - lastFloorTime));
                        lastFloorTime = currentFloorTime;
                        Console.WriteLine(agents.AgentNumber + " arrived at level S");
                        if (agents.AgentLevel == "Secret" || agents.AgentLevel == "Top-Secret")
                        {
                            inTheElevator = false;
                            agents.CurrentAgentFloor = Agent.AgentCurrentFloor.S;
                        }
                        else
                        {
                            elevatorAction = BaseFloors.G;
                        }
                        break;
                    
                   case BaseFloors.G:
                    default:
                        currentFloorTime = 0;
                        Thread.Sleep(Math.Abs(currentFloorTime - lastFloorTime));
                        lastFloorTime = currentFloorTime;
                        Console.WriteLine(agents.AgentNumber + " arrived at ground level G.");
                        agents.CurrentAgentFloor = Agent.AgentCurrentFloor.G;
                        inTheElevator = false;
                        break;
                }
                
            }
            
            LeaveElevator(agents);
            currentFloor = agents.CurrentAgentFloor;
        }
        public void LeaveElevator(Agent agents)
        {
            Console.WriteLine("The door opens and the agent gets of the elevator.");
            
            lock (agentsInElevator)
            {
                agentsInElevator.Remove(agents);
            }
            semaphore.Release();

        }

        public BaseFloors GenerateNewFloor(int randNum)
        {
            BaseFloors elevatorAction;
            switch (randNum)
            {
                case 1:
                    elevatorAction = BaseFloors.G;
                    break;
                case 2:
                    elevatorAction = BaseFloors.S;
                    break;
                case 3:
                    elevatorAction = BaseFloors.T1;
                    break;
                case 4:
                default:
                    elevatorAction = BaseFloors.T2;
                    break;
            }

            return elevatorAction;
        }
        


    }
}
