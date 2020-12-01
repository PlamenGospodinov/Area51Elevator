using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Area51Elevator
{
    class Program
    {
        static Random rand = new Random();

        static void Main(string[] args)
        {
            Elevator elevator = new Elevator();

            //You can change the number of agents you want to generate
            int numberOfAgents = 4;
            //Creates a list of agents
            List<Agent> agentsList = new List<Agent>(numberOfAgents);
            var agents = Enumerable.Range(1, numberOfAgents).Select(i => new Agent
            {
                AgentNumber = i.ToString(),
                Elevator = elevator
            }).ToList();

            //Randomly creates a security level for each agent
            for (int k = 0; k < numberOfAgents; k++)
            {
                string agentSecurityLevel = "";
                int randomNum = rand.Next(1, 4);
                switch (randomNum)
                {
                    case 1:
                        agentSecurityLevel = "Confidential";
                        break;
                    case 2:
                        agentSecurityLevel = "Secret";
                        break;
                    case 3:
                        agentSecurityLevel = "Top-Secret";
                        break;
                    default:
                        agentSecurityLevel = "Confidential";
                        break;
                }
                agents[k].AgentLevel = agentSecurityLevel;
                agents[k].CurrentAgentFloor = Agent.AgentCurrentFloor.G;
                

            }

            //Starts the simulation
            foreach (var agent in agents)
            {
                agent.StartTheEvents();
                agent.inTheBase = true;
                Thread.Sleep(5000);
            }

            //The program ends when all the agents finish work.
            while (agents.Any(agent => !agent.FinishWork))
            {

            }
            Console.WriteLine("WORKDAY AT BASE \"Area 51\" IS OVER.");
            Console.ReadLine();

        }
    }
}
