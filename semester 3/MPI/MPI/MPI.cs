using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MPI;

namespace MPI
{
    class MPI
    {
        static void Main(string[] args)
        {
            using (Environment env = new Environment(ref args))
            {
                Intracommunicator world = Communicator.world;
                if (args.Length != 2)
                {
                    if (world.Rank == 0)
                    {
                        Console.WriteLine("Please, enter two arguments: input file and output file pathes");
                    }
                    return;
                }
                int rank = world.Rank;
                int processors = world.Size;
                List<int> array = new List<int>();
                if (world.Rank == 0)
                {
                    try
                    {
                        FileStream file = new FileStream(args[0], FileMode.Open);
                        StreamReader readFile = new StreamReader(file);
                        while (!readFile.EndOfStream)
                        {
                            string[] lines = readFile.ReadLine().Split(' ');
                            foreach (var line in lines)
                            {
                                array.Add(int.Parse(line));
                            }
                        }
                        readFile.Close();
                    }
                    catch (Exception el)
                    {
                        Console.WriteLine(el.Message);
                        Console.WriteLine("MPI programm stopped");
                        return;
                    }
                    for (int i = 1; i < processors; i++)
                    {
                        world.Send(array, i, 0);
                    }
                }
                else
                {
                    array = world.Receive<List<int>>(0, 0);
                }

                List<int> nodeArray = new List<int>();
                int blockSize = array.Count / processors;
                for (int i = blockSize * rank; i < blockSize * (rank + 1); i++)
                {
                    nodeArray.Add(array[i]);
                }
                if (rank == (processors - 1))
                {
                    for (int i = blockSize * (rank + 1); i < array.Count; i++)
                    {
                        nodeArray.Add(array[i]);
                    }
                }

                QSort.Quicksort<int>(nodeArray, 0, nodeArray.Count - 1);
                int m = array.Count / (processors * processors);
                List<int> masterArray = new List<int>();
                
                if (rank != 0)
                {
                    for (int i = 0; i < processors * m; i += m)
                    {
                        world.Send(nodeArray[i], 0, 0);
                    }
                }
                else
                {
                    for (int i = 0; i < processors * m; i += m)
                    {
                        for (int j = 1; j < processors; j++)
                        {

                            int value = world.Receive<int>(j, 0);
                            masterArray.Add(value);
                        }
                        masterArray.Add(nodeArray[i]);
                    }
                }         
                List<int> pivotList = new List<int>();
                if (rank == 0)
                {
                    QSort.Quicksort(masterArray, 0, masterArray.Count - 1);
                    for (int i = processors - 1 + processors / 2; i < processors * processors + processors / 2; i += processors)
                    {
                        if(i < masterArray.Count)
                        {
                            pivotList.Add(masterArray[i]);
                        }                    
                    }                
                }
               
                List<int>[] blockArrays = new List<int>[processors];
                if (rank == 0)
                {
                    for (int i = 0; i < processors; i++)
                    {
                        blockArrays[i] = new List<int>();
                        foreach (int el in array)
                        {
                            if ((i != 0) && (i != processors - 1) && (el >= pivotList[i - 1]) && (el < pivotList[i]))
                            {
                                if ((el >= pivotList[i - 1]) && (el < pivotList[i]))
                                {
                                    blockArrays[i].Add(el);
                                }
                            }
                            else if ((i == 0) && (el < pivotList[i]))
                            {
                                blockArrays[i].Add(el);
                            }
                            else if (i == 0 && processors == 1)
                            {
                                blockArrays[i].Add(el);
                            }
                            else if ((i == processors - 1) && (el >= pivotList[i - 1]))
                            {
                                blockArrays[i].Add(el);
                            }                         
                        }
                    }
                    world.Scatter(blockArrays);
                }
                if (rank != 0)
                {
                    blockArrays[rank] = world.Scatter<List<int>>(0);
                }               
                List<int> newArr = blockArrays[rank];
                QSort.Quicksort(newArr, 0, newArr.Count - 1);
                List<int>[] finalArrays = world.Gather(newArr, 0);
                if (rank == 0)
                {
                    List<int> finalList = new List<int>();
                    foreach(var list in finalArrays)
                    {
                        finalList = finalList.Concat(list).ToList();
                    }
                    StreamWriter streamWriter = new StreamWriter(args[1]);
                    for (int i = 0; i < finalList.Count; i++)
                    {
                        streamWriter.Write(finalList[i]);
                        if (i != finalList.Count - 1)
                        {
                            streamWriter.Write(' ');
                        }
                    }
                    
                    streamWriter.Close();
                    finalList.Clear();
                    foreach (var t in finalArrays)
                    {
                        t.Clear();
                    }
                    foreach (var t in blockArrays)
                    {
                        t.Clear();
                    }
                }
                array.Clear();
                nodeArray.Clear();
                masterArray.Clear();
                pivotList.Clear();
                newArr.Clear();       
            }
        }
    }
}
