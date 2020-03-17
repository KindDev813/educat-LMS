﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduCATS.Data.Models.Testing.Passing;
using EduCATS.Data.User;
using EduCATS.Helpers.Date;
using EduCATS.Helpers.Lists;
using EduCATS.Networking.Models.Testing;
using EduCATS.Pages.Testing.Passing.Models;

namespace EduCATS.Pages.Testing.Passing.ViewModels
{
	public partial class TestPassingPageViewModel
	{
		async Task answerQuestion()
		{
			setLoading(true);

			TestingCommonAnswerPostModel commonPostModel = null;

			switch (_questionType) {
				case 0:
				case 1:
					commonPostModel = getSelectedAnswer();
					break;
				case 2:
					commonPostModel = getEditableAnswer();
					break;
				case 3:
					commonPostModel = getMovableAnswer();
					break;
			}

			await answerCommonQuestion(commonPostModel);

			setLoading(false);
		}

		TestingCommonAnswerPostModel getSelectedAnswer()
		{
			var answersList = Answers?.Select(
				a => new TestingAnswerPostModel(a.Id, Convert.ToInt32(a.IsSelected)));

			return composeAnswer(answersList.ToList());
		}

		TestingCommonAnswerPostModel getMovableAnswer()
		{
			var answersList = Answers?.Select(
				(a, index) => new TestingAnswerPostModel(Answers[index].Id, index));
			return composeAnswer(answersList.ToList());
		}

		TestingCommonAnswerPostModel getEditableAnswer()
		{
			var answersList = Answers?.Select(
				a => new TestingAnswerPostModel(Answers[0].Id, Answers[0].ContentToAnswer));
			return composeAnswer(answersList.ToList());
		}

		TestingCommonAnswerPostModel composeAnswer(List<TestingAnswerPostModel> answersList)
		{
			return new TestingCommonAnswerPostModel {
				Answers = answersList,
				QuestionNumber = _questionNumber,
				TestId = _testIdString,
				UserId = AppUserData.UserId
			};
		}

		void setTestData(TestDetailsModel test)
		{
			_isTimeForEntireTest = test.SetTimeForAllTest;
			_timeForCompletion = test.TimeForCompleting;
			_questionCount = test.CountOfQuestions;
			_testIdString = test.Id.ToString();
		}

		void setTimer()
		{
			if (_timeForCompletion == 0)
				return;

			if (_isTimeForEntireTest) {
				setTimerForEntireTest();
			} else {
				setTimerForQuestion();
			}
		}

		void setTimerForEntireTest()
		{
			_testStarted = DateTime.Now;

			_device.SetTimer(TimeSpan.FromSeconds(1), () => {
				if (checkTimerCancellation()) {
					return false;
				}

				var timePassed = DateHelper.CheckDatesDifference(_testStarted, DateTime.Now);
				var timeLeft = new TimeSpan(0, _timeForCompletion, 0).Subtract(timePassed);
				setTitle(timeLeft);

				if (timePassed.TotalMinutes >= _timeForCompletion) {
					completeTest();
					return false;
				}

				return true;
			});
		}

		void setTimerForQuestion()
		{
			_questionStarted = DateTime.Now;

			_device.SetTimer(TimeSpan.FromSeconds(1), () => {
				if (checkTimerCancellation()) {
					return false;
				}

				var timePassed = DateHelper.CheckDatesDifference(_questionStarted, DateTime.Now);
				var timeLeft = new TimeSpan(0, _timeForCompletion, 0).Subtract(timePassed);
				setTitle(timeLeft);

				if (timePassed.TotalSeconds >= _timeForCompletion) {
					completeQuestion();
					return false;
				}

				return true;
			});
		}

		bool checkTimerCancellation()
		{
			if (_timerCancellation) {
				_timerCancellation = false;
				return true;
			}

			return false;
		}

		void moveAnswer(object obj, bool down)
		{
			if (obj == null || obj.GetType() != typeof(int)) {
				return;
			}

			var id = (int)obj;
			var answers = Answers;
			var answer = answers.SingleOrDefault(a => a.Id == id);

			if (answer == null) {
				return;
			}

			var index = answers.IndexOf(answer);

			if (down) {
				answers.Swap(
					index,
					index == answers.Count - 1 ? 0 : index + 1);
			} else {
				answers.Swap(
					index == 0 ? 0 : index,
					index == 0 ? answers.Count - 1 : index - 1);
			}

			Answers = new List<TestPassingAnswerModel>(answers);
		}

		int getNextQuestion()
		{
			return _questionNumber + 1 <= _questionCount ? _questionNumber + 1 : 1;
		}
	}
}
