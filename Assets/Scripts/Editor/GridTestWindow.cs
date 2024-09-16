using System;
using System.Collections.Generic;
using UnityEditor;
using GMI_Technical_Assessment.Code;
using UnityEngine;
using Grid = GMI_Technical_Assessment.Code.Grid;

public class GridTestWindow : EditorWindow
{
    private readonly Color DEFAULT_DIGIT_COLOR = Color.white;

    private GridAnalyzer gridAnalyzer;

    private int currentTestIndex = 0;
    private Vector2 scrollPos = Vector2.zero;

    private SerializedObject serializedObject;
    private SerializedProperty textFilesProperty;

    [SerializeField] private TextAsset[] testFiles;

    private bool isFullRun = false;
    private TextAsset currentTestFile;
    private Grid currentGrid = null;
    
    [MenuItem("Window/Grid Test")]
    private static void ShowWindow()
    {
        GridTestWindow window = (GridTestWindow)EditorWindow.GetWindow(typeof(GridTestWindow));
        window.titleContent = new GUIContent("Grid Test");
        window.Show();
    }

    private void OnEnable()
    {
        InitGridAnalyzer();

        serializedObject = new SerializedObject(this);
        textFilesProperty = serializedObject.FindProperty(nameof(testFiles));
    }

    private void OnGUI()
    {
        serializedObject.Update();
        DrawCenterLayout();
        DrawBottomLayout();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawCenterLayout()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.PropertyField(textFilesProperty);
        EditorGUILayout.Space(10f);
        
        DrawCurrentFilename();
        EditorGUILayout.LabelField("OUTPUT:", EditorStyles.boldLabel);

        if (currentGrid != null)
        {
            DrawGrid(currentGrid);
        }
        
        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("TEST:", EditorStyles.boldLabel);
        DrawTestResult();
        
        EditorGUILayout.EndScrollView();
    }

    private void DrawBottomLayout()
    {
        GUILayout.FlexibleSpace();
        EditorGUI.BeginDisabledGroup(testFiles == null || testFiles.Length <= 0);
        
        if (GUILayout.Button("Check Next Test"))
        {
            ReadNextTestFile();
        }

        if (GUILayout.Button("Run Test"))
        {
            isFullRun = true;
        }

        if (GUILayout.Button("Clear"))
        {
            currentTestIndex = -1;
            currentTestFile = null;
            currentGrid = null;
        }
        
        EditorGUI.EndDisabledGroup();
    }

    private void DrawGrid(Grid grid)
    {
        Grid.GridCell[,] gridMatrix = grid.Matrix;
        
        GUILayout.Space(10f);
        Rect rect = GUILayoutUtility.GetLastRect();
        
        rect.y += 10f;
        rect.x += 10f;

        int fontSize = 14;
        int fontSizeFactor = 2;
        
        int width = fontSize * fontSizeFactor;
        int height = fontSize * fontSizeFactor;
        int space = 5;
        
        GUILayout.Box(GUIContent.none,
            EditorStyles.textArea, 
            GUILayout.Width(width * gridMatrix.GetLength(1) + 10f), 
            GUILayout.Height(height * gridMatrix.GetLength(0) + 10f));

        for (int i = 0; i < gridMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < gridMatrix.GetLength(1); j++)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = gridMatrix[i, j].color;
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleCenter;
                style.fontSize = fontSize;

                GUI.Label(new Rect(rect.x + width * j,
                        rect.y + height * i,
                        width, height),
                    gridMatrix[i, j].value.ToString(),
                    style);
            }
        }
    }

    private void DrawTestResult()
    {
        if (currentGrid == null)
            return;
        
        MatchFormations[] formations = gridAnalyzer.Formations;
        MatchTest[] matchTests = gridAnalyzer.MatchTests;
        
        GUIStyle richTextStyle = new GUIStyle(EditorStyles.label);
        richTextStyle.richText = true;
        richTextStyle.fontSize = 14;
        
        for (int i = 0; i < formations.Length; i++)
        {
            string hexColor = "#" + ColorUtility.ToHtmlStringRGB(formations[i].FormationColor);
            string testResult = $"<color={hexColor}>{formations[i].Name}</color> Count: {formations[i].MatchesCount}";
            
            if (i >= matchTests.Length)
                testResult += " - NO TEST";
            else if(matchTests[i].RequiredMatches <= 0)
                testResult += " - NO TEST";
            else if(matchTests[i].IsTestPassed)
                testResult += $"<color=green> - Passed</color> ({matchTests[i].RequiredMatches})";
            else if(!matchTests[i].IsTestPassed)
                testResult += $"<color=red> - Failed</color> ({matchTests[i].RequiredMatches})";
            
            GUILayout.Label(testResult, richTextStyle);
        }
    }

    private void DrawCurrentFilename()
    {
        if (currentTestFile == null)
            return;
        
        GUILayout.Space(10f);
        GUILayout.Label($"({currentTestIndex + 1} / {testFiles.Length}) - {currentTestFile.name}", 
            EditorStyles.boldLabel);
        GUILayout.Space(10f);
    }

    private void ReadNextTestFile()
    {
        currentTestIndex++;
        currentTestIndex = currentTestIndex % testFiles.Length;
        
        currentTestFile = testFiles[currentTestIndex];
        
        currentGrid = GridLoader.LoadFromFile(currentTestFile);
        currentGrid.SetColor(DEFAULT_DIGIT_COLOR);
        
        MatchTest[] matchTests = GridLoader.FillTests(currentTestFile, GetMatchTest());
        gridAnalyzer.MatchTests = matchTests;

        bool allTestPassed = gridAnalyzer.TestAnalyze(currentGrid);
        LogTestResults(allTestPassed);
    }

    private void LogTestResults(bool isSuccess)
    {
        string log = $" FILE NAME: {currentTestFile.name} ";

        if (isSuccess)
        {
            Debug.Log($"ALL TEST PASSED! " + log);
        }
        
        else
        {
            string failedTest = String.Empty;

            MatchFormations[] formations = gridAnalyzer.Formations;
            MatchTest[] matchTests = gridAnalyzer.MatchTests;
            
            for (int i = 0; i < formations.Length; i++)
            {
                if(i >= matchTests.Length)
                    continue;
                
                if (matchTests[i].IsTestPassed || matchTests[i].RequiredMatches <= 0)
                    continue;
                
                string hexColor = "#" + ColorUtility.ToHtmlStringRGB(formations[i].FormationColor);
                string formationText = $"<color={hexColor}>{formations[i].Name}</color> Count: {formations[i].MatchesCount}";
                string testResult = $"<color=red> - Failed</color> ({matchTests[i].RequiredMatches})";
                
                failedTest += $"FORMATION: {formationText} == {testResult}\n";
            }
            
            Debug.LogError($"TEST FAILED! -> " + log + failedTest);
        }
    }
    
    private MatchTest[] GetMatchTest()
    {
        return new MatchTest[]
        {
            new MatchTest("M"),
            new MatchTest("P"),
            new MatchTest("G"),
            new MatchTest("U"),
        };
    }

    private void InitGridAnalyzer()
    {
        Color lightBlue;
        Color lightGreen;
        Color lightRed;
        Color lightYellow;
        
        ColorUtility.TryParseHtmlString("#3d86fc", out lightBlue);
        ColorUtility.TryParseHtmlString("#3dfc63", out lightGreen);
        ColorUtility.TryParseHtmlString("#fc3d3d", out lightRed);
        ColorUtility.TryParseHtmlString("#fcf63d", out lightYellow);
        
        MatchFormations multicolorFormation = new MatchFormations
        (
            "Multicolor",
            lightBlue,
            new MatchRule[]
            {
                new CrossShape(DEFAULT_DIGIT_COLOR),
                new TShape(DEFAULT_DIGIT_COLOR),
                new ShapeRule(
                    DEFAULT_DIGIT_COLOR,
                    "Straight Line of 5",
                    new int[,]
                    {
                        { 1, 1, 1, 1, 1 },
                    }),
            }
        );

        MatchFormations propellerFormation = new MatchFormations
        (
            "Propeller",
            lightRed,
            new MatchRule[]
            {
                new ShapeRule(
                    DEFAULT_DIGIT_COLOR,
                    "Double Layer",
                    new int[,]
                    {
                        { 1, 1, 1 },
                        { 1, 1, 1 },
                    }),
                new ShapeRule(
                    DEFAULT_DIGIT_COLOR,
                    "T Shape (alternative)",
                    new int[,]
                    {
                        { 1, 1, 1 },
                        { 1, 1, 0 },
                    }),
                new ShapeRule(
                    DEFAULT_DIGIT_COLOR,
                    "2x2 Block",
                    new int[,]
                    {
                        { 1, 1 },
                        { 1, 1 },
                    }),
            }
        );

        MatchFormations greenThreeFormation = new MatchFormations
        (
            "Match 3",
            lightGreen,
            new MatchRule[]
            {
                new ShapeRule(
                    DEFAULT_DIGIT_COLOR,
                    "Straight Line of 3",
                    new int[,]
                    {
                        { 1, 1, 1 },
                    })
            }
        );

        MatchFormations unmatchedFormation = new MatchFormations
        (
            "Unmatched",
            lightYellow,
            new MatchRule[]
            {
                new UnmatchedRule(DEFAULT_DIGIT_COLOR),
            }
        );

        gridAnalyzer = new GridAnalyzer(
            new MatchFormations[]
            {
                multicolorFormation,
                propellerFormation,
                greenThreeFormation,
                unmatchedFormation,
            }
        );
    }
}