using Microsoft.Maui.Controls;

namespace HangmanAssignment;

public partial class HangmanGamePage : ContentPage
{
    private readonly string[] _images = { "hang1.png", "hang2.png", "hang3.png", "hang4.png", "hang5.png", "hang6.png", "hang7.png", "hang8.png" };
    private readonly (string Question, string Answer)[] _words =
    {
            ("What is a project management framework?", "SCRUM"),
            ("What outlines project goals?", "CHARTER"),
            ("An iterative methodology?", "AGILE"),
            ("Workflow tool?", "KANBAN"),
            ("Who approves a project?", "SPONSOR"),
            ("Chart for tasks?", "GANTT"),
            ("Plan of costs?", "BUDGET"),
            ("Unexpected changes?", "RISK"),
            ("First project phase?", "INITIATION"),
            ("Ensures goals met?", "QUALITY")
        };

    private int _currentIndex = 0;
    private int _wrongGuesses = 0;
    private string _currentAnswer = "";
    private string _currentDisplay = "";

    public HangmanGamePage()
    {
        InitializeComponent();
        StartGame();
    }

    private void StartGame()
    {
        if (_currentIndex >= _words.Length) _currentIndex = 0; // Restart if all words are used

        var currentWord = _words[_currentIndex];
        _currentAnswer = currentWord.Answer;
        _currentDisplay = new string('_', _currentAnswer.Length);

        QuestionLabel.Text = currentWord.Question;
        WordDisplay.Text = string.Join(" ", _currentDisplay.ToCharArray());
        HangmanImage.Source = _images[0];
        GameStatus.Text = "";
        LetterEntry.IsEnabled = true;
        _wrongGuesses = 0;
    }

    private void OnGuessClicked(object sender, EventArgs e)
    {
        var input = LetterEntry.Text?.ToUpper();
        LetterEntry.Text = "";

        if (string.IsNullOrWhiteSpace(input) || input.Length != 1)
        {
            DisplayAlert("Invalid Input", "Enter a single letter.", "OK");
            return;
        }

        if (_currentAnswer.Contains(input))
        {
            // Reveal correct letters
            char[] displayArray = _currentDisplay.ToCharArray();
            for (int i = 0; i < _currentAnswer.Length; i++)
            {
                if (_currentAnswer[i] == input[0])
                {
                    displayArray[i] = input[0];
                }
            }
            _currentDisplay = new string(displayArray);
            WordDisplay.Text = string.Join(" ", _currentDisplay.ToCharArray());

            if (!_currentDisplay.Contains("_"))
            {
                GameStatus.Text = "You survived";
                LetterEntry.IsEnabled = false;
                _currentIndex++;
                DisplayAlert("Great Job! You survived", "Next question!", "OK");
                StartGame();
            }
        }
        else
        {
            // Incorrect guess
            _wrongGuesses++;
            if (_wrongGuesses < _images.Length)
            {
                HangmanImage.Source = _images[_wrongGuesses];
            }
            else
            {
                GameStatus.Text = $"You died The word was: {_currentAnswer}";
                LetterEntry.IsEnabled = false;
                DisplayAlert("Game Over , you died", $"The correct word was: {_currentAnswer}. Starting a new word.", "OK");
                _currentIndex++;
                StartGame();
            }
        }
    }
}