using System;
using UnityEngine;

namespace GMI_Technical_Assessment.Code
{
    public class MatchFormations
    {
        protected string name;
        protected int matchesCount;

        protected MatchRule[] rules;

        protected Color formationColor;
        
        public string Name => name;
        public int MatchesCount => matchesCount;
        public Color FormationColor => formationColor;

        public MatchFormations(string name, Color formationColor, MatchRule[] rules)
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

        private void SetRulesRequiredColor(Color formationColor)
        {
            foreach (var rule in rules)
            {
                rule.RequiredColor = formationColor;
            }    
        }
    }
}
