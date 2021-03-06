﻿// <author>Jeremy Brown</author>
// <date>24-Sep-2018</date>
// <summary>Starter Code for Copads Project 2 (2171)</summary>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CommandLine;

//using CommandLine;

namespace Project2
{
    class Project2
    {
        /// <summary>
        ///     
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
               var parser = new Parser(with => with.HelpWriter = TextWriter.Null);
               parser.ParseArguments<TestOptions, Options>(args)
                   .WithParsed(opts =>
                   {
                       var testParams = (TestOptions) opts;
                       var stopWatch = new Stopwatch();
                       // run your tests
                       // YOUR CODE GOES HERE
                       switch (testParams.type)
                       {
                           case TestMethods.both:
                               var bList1 = GenerateTest(int.Parse(testParams.TestCount));
                               var bList2 = bList1;
                               Console.WriteLine("Detected {0} Processors", Environment.ProcessorCount);
                               stopWatch.Start();
                               bList1.QuicksortSequential();
                               stopWatch.Stop();
                               Console.WriteLine("Sequential");
                               Console.WriteLine(stopWatch.Elapsed.ToString("s\\.fffffff"));
                               stopWatch.Restart();
                               bList2.QuicksortParallel();
                               stopWatch.Stop();
                               Console.WriteLine("Parallel");
                               Console.WriteLine(stopWatch.Elapsed.ToString("s\\.fffffff"));
                               break;
                           case TestMethods.par:
                               var pList = GenerateTest(int.Parse(testParams.TestCount));
                               Console.WriteLine("Detected {0} Processors", Environment.ProcessorCount);
                               stopWatch.Start();
                               pList.QuicksortParallel();
                               stopWatch.Stop();
                               Console.WriteLine("Parallel");
                               Console.WriteLine(stopWatch.Elapsed.ToString("s\\.fffffff"));
                               break;
                           case TestMethods.seq:
                               var sList = GenerateTest(int.Parse(testParams.TestCount));
                               Console.WriteLine("Detected {0} Processors", Environment.ProcessorCount);
                               stopWatch.Start();
                               sList.QuicksortSequential();
                               stopWatch.Stop();
                               Console.WriteLine("Sequential");
                               Console.WriteLine(stopWatch.Elapsed.ToString("s\\.fffffff"));
                               break;
                       }

                   })
                   .WithNotParsed(err =>
                   {
                       parser.ParseArguments<Options>(args)
                           .WithParsed(opts => {
                               // sort the file and output to stdout
                               // YOUR CODE GOES HERE
                               try
                               {
                                   var fileContent = File.ReadAllLines(opts.FileName);
                                   if (opts.parseint)
                                   {
                                       var fileList = new List<int>(Array.ConvertAll(fileContent, e => int.Parse(e)));
                                       SortAndPrint(fileList);
                                   }
                                   else
                                   {
                                       var fileList = new List<string>(fileContent);
                                       SortAndPrint(fileList);
                                   }
                               }
                               catch (FileNotFoundException)
                               {
                                   Console.WriteLine();
                                   Console.WriteLine("The file specified was not found.");
                                   Console.WriteLine("Please try again.");
                                   Console.WriteLine();
                               }
                               catch (FormatException)
                               {
                                   Console.WriteLine();
                                   Console.WriteLine("The file specified contained an incorrect value.");
                                   Console.WriteLine("Please fix the file and try again.");
                                   Console.WriteLine();
                               }
                           })
                           .WithNotParsed(errs =>
                           {
                               // show the help
                               Console.WriteLine("Project2 [<command>|filename]");
                               Console.WriteLine();
                               Console.WriteLine("Commands are listed below:");
                               Console.WriteLine();
                               Console.WriteLine("testwith -t <type> <count>    Runs a test scenario for quicksort");
                               Console.WriteLine("                              type can be:");
                               Console.WriteLine("                                 seq - sequential quicksort");
                               Console.WriteLine("                                 par - parallel quicksort");
                               Console.WriteLine("                                 both - both sequential and parallel");
                               Console.WriteLine();
                               Console.WriteLine("when no command is given, you can specify a filename, and the ");
                               Console.WriteLine("optional -g.  -g will force the data to be integer types");
                               Console.WriteLine();
                               Console.WriteLine("Example:");
                               Console.WriteLine("Project2 -g test.txt");
                               Console.WriteLine("Project2 testwith -t par 10000");
                               Console.WriteLine("Project2 test.txt");
                           });
                   });
        }

        /// <summary>
        /// Helper method to sort and print our file reads
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private static void SortAndPrint<T>(List<T> list ) where T: IComparable
        {
            Console.WriteLine();
            list.QuicksortParallel();
            list.ForEach(e => {
                Console.WriteLine(e);
            });
        }

        /// <summary>
        /// Generates a test list of ints for quicksort
        /// </summary>
        private static List<int> GenerateTest(int count)
        {
            var rand = new Random();
            var sortList = new List<int>();

            for (int i = 0; i < count; i++)
            {
                sortList.Add(rand.Next());
            }

            return sortList;
        }
    }
}