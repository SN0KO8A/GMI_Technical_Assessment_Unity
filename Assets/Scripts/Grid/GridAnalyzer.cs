using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GMI_Technical_Assessment.Code
{
    internal class GridAnalyzer
    {
        private MatchFormations[] formations;
        private MatchTest[] tests;

        public MatchTest[] MatchTests { get { return tests; } set { tests = value; } }

        public GridAnalyzer(MatchFormations[] formations)
        {
            this.formations = formations;
            this.tests = new MatchTest[0];
        }

        public GridAnalyzer(MatchFormations[] formations, MatchTest[] tests)
        {
            this.formations = formations;
            this.tests = tests;
        }

        public void Analyze(Grid grid)
        {
            foreach (MatchFormations rule in formations)
            {
                rule.FindMatches(grid);
            }
        }

        public bool TestAnalyze(Grid grid)
        {
            bool isAllTestPassed = true;

            for (int i = 0; i < formations.Length; i++)
            {
                int currentMatches = formations[i].FindMatches(grid);

                if(i < tests.Length)
                {
                    bool isTestPassed = tests[i].TestMatches(currentMatches);
                    isAllTestPassed = isAllTestPassed && isTestPassed;
                }
            }

            return isAllTestPassed;
        }

        public void DisplayResult()
        {
            foreach (MatchFormations rule in formations)
            {
                rule.DisplayResult();
            }
        }

        public void DisplayTestResult()
        {
            for (int i = 0; i < formations.Length; i++)
            {
                formations[i].DisplayResult();

                if (i < tests.Length)
                {
                    tests[i].DisplayTestResult();
                }
            }
        }
    }

    public class MatchTest
    {
        private string matchID = "E";
        private int requiredMatches = 0;

        private bool isTestPassed;

        public string ID => matchID;

        public MatchTest(string matchID)
        {
            this.matchID = matchID;
        }

        public void SetRequiredMatches(int requiredMatches)
        {
            this.requiredMatches = requiredMatches;
        }

        public bool TestMatches(int currentMatches)
        {
            if (requiredMatches <= 0)
                isTestPassed = true;
            else
                isTestPassed = currentMatches == requiredMatches;

            return isTestPassed;
        }

        public void DisplayTestResult()
        {
            if (requiredMatches > 0)
            {
                ConsoleColor consoleColor = isTestPassed ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
                string resultText = isTestPassed ? "PASSED" : "FAILED";

                Console.Write($"Test result - ");
                Console.ForegroundColor = consoleColor;
                Console.Write(resultText);
                Console.ResetColor();
                Console.Write($" ID: {matchID}, required: {requiredMatches}\n");
            }
            else
            {
                Console.WriteLine("NO TEST");
            }
        }
    }
}
