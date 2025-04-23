using Microsoft.Maui.Controls;
using System.Threading.Tasks;


namespace TrueFalseQuiz
{
    public partial class MainPage : ContentPage
    {
        private string[] _questions = {
        "Spain has one official language.",
        "Spain’s national anthem has no words.",
        "Spaniards eat TACOS.",
        "Spain is below of USA.",
        "Spain is a democracy"

        };
        private bool[] _answers = { false, true, false, false, true };
        private string[] _imageFilenames = { "talk.jpg", "himno.jpg", "tacos.jpg", "america.jpg", "democracy.jpg" }; // Add your image filenames here
        private int _currentQuestionIndex = 0;
        private int _score = 0;

        public MainPage()
        {
            InitializeComponent();
            LoadQuestion();
        }

        private void LoadQuestion()
        {
            if (_currentQuestionIndex < _questions.Length)
            {
                QuestionLabel.Text = _questions[_currentQuestionIndex];
                QuestionImage.Source = ImageSource.FromFile(_imageFilenames[_currentQuestionIndex]);
                AnswerLabel.Text = "";
                

                TrueButton.IsVisible = true;
                FalseButton.IsVisible = true;
            }
            else
            {
                QuestionLabel.Text = "Quiz finished!";
                QuestionImage.Source = null;
                AnswerLabel.Text = $"Your final score is: {_score} out of {_questions.Length}";

                TrueButton.IsVisible = false;
                FalseButton.IsVisible = false;
            }
        }

        private async void OnTrueButtonClicked(object sender, EventArgs e) // Note the 'async' keyword
        {
            await CheckAnswer(true); // Note the 'await' keyword
        }

        private async void OnFalseButtonClicked(object sender, EventArgs e) // Note the 'async' keyword
        {
            await CheckAnswer(false); // Note the 'await' keyword
        }

        private async Task CheckAnswer(bool userAnswer) // Note the 'async Task' return type
        {
            bool correctAnswer = _answers[_currentQuestionIndex];
            if (_currentQuestionIndex < _answers.Length)
            {
                if (userAnswer == _answers[_currentQuestionIndex])
                {
                    AnswerLabel.Text = $"Correct! The answer is: {correctAnswer}";
                    _score++;
                }
                else
                {
                    AnswerLabel.Text = $"Incorrect. The correct answer is: {correctAnswer}";
                }
                TrueButton.IsVisible = false;
                FalseButton.IsVisible = false;

                _currentQuestionIndex++;
                // Introduce a delay of 2 seconds (2000 milliseconds)
                await Task.Delay(2000);

                LoadQuestion();
            }
        }
    }
}
