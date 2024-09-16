using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMI_Technical_Assessment.Code
{
    public class MatchFormations
    {
        protected string name;
        protected int matchesCount;

        protected MatchRule[] rules;

        protected ConsoleColor formationColor;

        public MatchFormations(string name, ConsoleColor formationColor, MatchRule[] rules)
        {
            matchesCount = 0;

            this.name = name;
            this.rules = rules;

            this.formationColor = formationColor;

            SetRulesRequiredColor(formationColor);
        }

        public virtual int FindMatches(Grid grid)
        {
            matchesCount = 0;

            foreach (var rule in rules)
            {
                matchesCount += rule.FindMatches(grid);
            }

            return matchesCount;
        }

        public virtual void DisplayResult()
        {
            Console.ForegroundColor = formationColor;
            Console.Write(name);
            Console.ResetColor();

            Console.Write($" Count: {matchesCount}\n");
        }

        private void SetRulesRequiredColor(ConsoleColor formationColor)
        {
            foreach (var rule in rules)
            {
                rule.RequiredColor = formationColor;
            }    
        }
    }
}
