using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using TrueFalseQuiz.Resources.Strings;


namespace TrueFalseQuiz
{
    public partial class MainPage : ContentPage
    {
        private string[] _questionKeys = { "QuestionOne", "QuestionTwo", "QuestionThree", "QuestionFour", "QuestionFive" }; // Use keys instead of hardcoded strings
        private bool[] _answers = { false, true, false, false, true };
        private string[] _imageFilenames = { "talk.jpg", "himno.jpg", "tacos.jpg", "america.jpg", "democracy.jpg" };
        private int _currentQuestionIndex = 0;
        private int _score = 0;
        private double _totalPanX = 0; // To track the total horizontal pan

        public MainPage()
        {
            InitializeComponent();
            LoadQuestion();
        }

        private void LoadQuestion()
        {
            if (_currentQuestionIndex < _questionKeys.Length)
            {
                QuestionLabel.Text = AppResources.ResourceManager.GetString(_questionKeys[_currentQuestionIndex]); // Load from resources
                QuestionImage.Source = ImageSource.FromFile(_imageFilenames[_currentQuestionIndex]);
                AnswerLabel.Text = "";

                TrueButton.Text = AppResources.TrueButtonText; // Localize button text
                FalseButton.Text = AppResources.FalseButtonText; // Localize button text

                TrueButton.IsVisible = true;
                FalseButton.IsVisible = true;
            }
            else
            {
                QuestionLabel.Text = AppResources.QuizFinishedText; // Localize "Quiz finished!"
                QuestionImage.Source = null;
                AnswerLabel.Text = string.Format(AppResources.FinalScore, _score, _questionKeys.Length); // Localize and format the final score

                TrueButton.IsVisible = false;
                FalseButton.IsVisible = false;
            }
        }

        private async void OnTrueButtonClicked(object sender, EventArgs e)
        {
            await CheckAnswer(true);
        }

        private async void OnFalseButtonClicked(object sender, EventArgs e)
        {
            await CheckAnswer(false);
        }

        private async Task CheckAnswer(bool userAnswer)
        {
            bool correctAnswer = _answers[_currentQuestionIndex];
            if (_currentQuestionIndex < _answers.Length)
            {
                string correctText = correctAnswer ? AppResources.TrueButtonText : AppResources.FalseButtonText; // Localize the correct answer text
                string userAnswerText = userAnswer ? AppResources.TrueButtonText : AppResources.FalseButtonText;   // Localize the user's answer text

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
                await Task.Delay(2000);

                LoadQuestion();
            }
        }

        private async void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (_currentQuestionIndex < _questionKeys.Length && TrueButton.IsVisible) // Only allow swipe if a question is active and buttons are visible
            {
                _totalPanX += e.TotalX;

                if (e.StatusType == GestureStatus.Completed) // Corrected from 'e.State' to 'e.StatusType' and replaced 'GestureRecognizerState.Ended' with 'GestureStatus.Completed'
                {
                    if (_totalPanX > 50) // Swiped right (adjust threshold as needed)
                    {
                        await CheckAnswer(true);
                        ResetPan();
                    }
                    else if (_totalPanX < -50) // Swiped left (adjust threshold as needed)
                    {
                        await CheckAnswer(false);
                        ResetPan();
                    }
                    else
                    {
                        // Optional: Add some visual feedback if the swipe wasn't significant enough
                        ResetPan();
                    }
                }
            }
        }

        private void ResetPan()
        {
            _totalPanX = 0;
        }
    }
}
