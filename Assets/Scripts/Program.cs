// using GMI_Technical_Assessment.Code;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
//
// namespace GMI_Technical_Assessment
// {
//     internal class Program
//     {
//         private const ConsoleColor DEFAULT_DIGIT_COLOR = ConsoleColor.White;
//
//         static void Main(string[] args)
//         {
//             string path = "E:\\ProgramProjects\\GMI_Technical_Assessment\\GMI_Technical_Assessment\\Tests\\";
//             TestRun(path);
//         }
//
//         private static GridAnalyzer GetCurrentGridAnalyzer()
//         {
//             MatchFormations multicolorFormation = new MatchFormations
//                 (
//                     "Multicolor",
//                     ConsoleColor.Blue,
//
//                     new MatchRule[]
//                     {
//                         new CrossShape(DEFAULT_DIGIT_COLOR),
//                         new TShape(DEFAULT_DIGIT_COLOR),
//                         new ShapeRule(
//                             DEFAULT_DIGIT_COLOR,
//                             "Straight Line of 5",
//                             new int[,]
//                             {
//                                 { 1, 1, 1, 1, 1},
//                             }),
//                     }
//                 );
//
//             MatchFormations propellerFormation = new MatchFormations
//                 (
//                     "Propeller",
//                     ConsoleColor.Red,
//
//                     new MatchRule[]
//                     {
//                         new ShapeRule(
//                             DEFAULT_DIGIT_COLOR,
//                             "Double Layer",
//                             new int[,]
//                             {
//                                 { 1, 1, 1},
//                                 { 1, 1, 1},
//                             }),
//                         new ShapeRule(
//                             DEFAULT_DIGIT_COLOR,
//                             "T Shape (alternative)",
//                             new int[,]
//                             {
//                                 { 1, 1, 1},
//                                 { 1, 1, 0},
//                             }),
//                         new ShapeRule(
//                             DEFAULT_DIGIT_COLOR,
//                             "2x2 Block",
//                             new int[,]
//                             {
//                                 { 1, 1},
//                                 { 1, 1},
//                             }),
//                     }
//                 );
//
//             MatchFormations greenThreeFormation = new MatchFormations
//                 (
//                     "Match 3",
//                     ConsoleColor.Green,
//
//                     new MatchRule[]
//                     {                        
//                         new ShapeRule(
//                             DEFAULT_DIGIT_COLOR,
//                             "Straight Line of 3",
//                             new int[,]
//                             {
//                                 { 1, 1, 1},
//                             })
//                     }
//                 );
//
//             MatchFormations unmatchedFormation = new MatchFormations
//                 (
//                     "Unmatched",
//                     ConsoleColor.Yellow,
//
//                     new MatchRule[]
//                     {
//                         new UnmatchedRule(DEFAULT_DIGIT_COLOR),
//                     }
//                 );
//
//             GridAnalyzer gridAnalyzer = new GridAnalyzer(
//                 new MatchFormations[]
//                     {
//                         multicolorFormation,
//                         propellerFormation,
//                         greenThreeFormation,
//                         unmatchedFormation,
//                     }
//             );
//
//             return gridAnalyzer;
//         }
//
//         static MatchTest[] GetTests()
//         {
//             return new MatchTest[]
//                 {
//                     new MatchTest("M"),
//                     new MatchTest("P"),
//                     new MatchTest("G"),
//                     new MatchTest("U"),
//                 };
//         }
//
//         static void TestRun(string testFolderPath)
//         {
//             string[] allTestFiles = Directory.GetFiles(testFolderPath, "*.txt", SearchOption.AllDirectories);
//
//             int currentFileIndex = 0;
//
//             Console.WriteLine("Esc - Exit");
//             Console.WriteLine("Enter - Next File");
//             Console.WriteLine("R - Run till the test fail");
//             Console.WriteLine("");
//
//             bool fullRun = false;
//
//             while (true)
//             {
//                 ConsoleKeyInfo keyInfo = default;
//
//                 if (!fullRun)
//                     keyInfo = Console.ReadKey(true);
//
//                 if (keyInfo.Key == ConsoleKey.Escape || currentFileIndex >= allTestFiles.Length)
//                     break;
//
//                 if (keyInfo.Key == ConsoleKey.R)
//                     fullRun = true;
//
//                 Console.Clear();
//                 Console.WriteLine("\x1b[3J");
//                 Console.WriteLine("Esc - Exit");
//                 Console.WriteLine("Enter - Next File");
//                 Console.WriteLine("R - Run till the test fail");
//                 Console.WriteLine("");
//                 Console.WriteLine($"FILES {currentFileIndex + 1}/{allTestFiles.Length}");
//
//                 if (keyInfo.Key == ConsoleKey.Enter || fullRun)
//                 {
//                     string fileName = allTestFiles[currentFileIndex];
//
//                     Grid grid = GridLoader.LoadFromFile(fileName);
//                     GridAnalyzer gridAnalyzer = GetCurrentGridAnalyzer();
//                     MatchTest[] tests = GetTests();
//
//                     grid.SetColor(DEFAULT_DIGIT_COLOR);
//
//                     tests = GridLoader.FillTests(tests, fileName);
//                     gridAnalyzer.MatchTests = tests;
//
//                     if (grid != null)
//                     {
//                         Console.WriteLine("FILE NAME: " + fileName);
//
//                         if (!gridAnalyzer.TestAnalyze(grid))
//                             fullRun = false;
//
//                         if(!fullRun)
//                             DisplayGrid(grid, gridAnalyzer);
//                     }
//
//                     currentFileIndex++;
//                 }
//             }
//
//             Console.WriteLine("Finished!");
//         }
//
//         private static void DisplayGrid(Grid grid, GridAnalyzer gridAnalyzer)
//         {
//             Console.WriteLine("");
//             grid.DisplayMatrix();
//             Console.WriteLine("");
//             gridAnalyzer.DisplayTestResult();
//             Console.WriteLine("");
//         }
//     }
// }
