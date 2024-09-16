using UnityEditor;
using GMI_Technical_Assessment.Code;
using UnityEngine;

public class GridTestWindow : EditorWindow
{
    private readonly Color DEFAULT_DIGIT_COLOR = Color.white;

    private GridAnalyzer gridAnalyzer;
    private MatchTest[] matchTests;

    private SerializedObject serializedObject;
    private SerializedProperty textFilesProperty;

    [SerializeField] private TextAsset[] testFiles;

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
        InitMatchTest();

        serializedObject = new SerializedObject(this);
        textFilesProperty = serializedObject.FindProperty(nameof(testFiles));
    }

    private void OnGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(textFilesProperty);

        if (GUILayout.Button("Check Next Test"))
        {
            
        }

        if (GUILayout.Button("Run till Test Failed"))
        {
            
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    private void InitMatchTest()
    {
        matchTests = new MatchTest[]
        {
            new MatchTest("M"),
            new MatchTest("P"),
            new MatchTest("G"),
            new MatchTest("U"),
        };
    }

    private void InitGridAnalyzer()
    {
        MatchFormations multicolorFormation = new MatchFormations
        (
            "Multicolor",
            Color.blue,
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
            Color.red,
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
            Color.green,
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
            Color.yellow,
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