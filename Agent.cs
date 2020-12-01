using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Area51Elevator
{
    class Agent
    {
        int count = 0;

        public enum AgentCurrentFloor { G, S, T1, T2 };

        public AgentCurrentFloor CurrentAgentFloor { get; set; }
        public string AgentLevel { get; set; }

        public string AgentNumber { get; set; }

        public Elevator Elevator { get; set; }

        public bool inTheBase { get; set; }

        static Random rand = new Random();

        ManualResetEvent eventFinishedWork = new ManualResetEvent(false);

        public BaseFloors GetRandomFloor()
        {
            int leaveOrNot = rand.Next(1, 3);
            int floor = rand.Next(1, 5);
            if (floor == 1)
            {

                return BaseFloors.S;
            }
            else if (floor == 2)
            {

                return BaseFloors.T1;
            }
            else if (floor == 3)
            {

                return BaseFloors.T2;
            }
            else
            {
                return BaseFloors.G;
            }

            

        }

        private void EnteredTheBase()
        {
            Console.WriteLine("Agent number " + AgentNumber + " with security level " + AgentLevel + " entered the base and is currently at level G.");
            inTheBase = true;
            Thread.Sleep(400);
        }

        private void ElevatorActions()
        {
            while (inTheBase == true)
            {
                int someAction = rand.Next(1, 7);
                Thread.Sleep(300);
                if(someAction == 5 || someAction == 3)
                {
                    Console.WriteLine(AgentNumber + " waits for the elevator.");
                    Elevator.EnterElevator(this);
                }
                
                 

                if(this.CurrentAgentFloor == AgentCurrentFloor.G && someAction == 1)
                {
                    inTheBase = false;
                    Console.WriteLine("Agent number" + this.AgentNumber + " finished work.");
                    eventFinishedWork.Set();
                }
               
                
            }
        }

        private void StartTheEventsInternal()
        {
            EnteredTheBase();//Initial state for each agent-he enters the base
            while(inTheBase == true)
            {
                int randomNum = rand.Next(20);
                if(randomNum < 4)
                {
                    ElevatorActions();
                    
                }
                else
                {
                    Thread.Sleep(5000);
                    ElevatorActions();
                }
                
            }
            
        }


        public void StartTheEvents()
        {
            Thread t = new Thread(StartTheEventsInternal);
            t.Start();

        }

        public bool FinishWork
        {
            get
            {
                return eventFinishedWork.WaitOne(0);
            }
        }
    }
}
