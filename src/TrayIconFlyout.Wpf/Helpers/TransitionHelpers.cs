// Copyright (c) 0x5BFA. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace U5BFA.Libraries
{
    internal static class TransitionHelpers
    {
        internal static Storyboard GetWindows11BottomToTopTransitionStoryboard(DependencyObject target, int from, int to)
        {
            var storyboard = new Storyboard();

            var keyFrames = new DoubleAnimationUsingKeyFrames();
            keyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)),
                Value = from,
            });

            keyFrames.KeyFrames.Add(new SplineDoubleKeyFrame()
            {
                KeySpline = new() { ControlPoint1 = new(0.1, 0.9), ControlPoint2 = new(0.4, 1.0) },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(267)),
                Value = to,
            });

            Storyboard.SetTarget(keyFrames, target);
            Storyboard.SetTargetProperty(keyFrames, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            storyboard.Children.Add(keyFrames);

            return storyboard;
        }

        internal static Storyboard GetWindows11TopToBottomTransitionStoryboard(DependencyObject target, int from, int to)
        {
            var storyboard = new Storyboard();

            var keyFrames = new DoubleAnimationUsingKeyFrames();
            keyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)),
                Value = from,
            });

            keyFrames.KeyFrames.Add(new SplineDoubleKeyFrame()
            {
                KeySpline = new() { ControlPoint1 = new(0.2, 0.0), ControlPoint2 = new(0.9, 0.0) },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200)),
                Value = to,
            });

            Storyboard.SetTarget(keyFrames, target);
            Storyboard.SetTargetProperty(keyFrames, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            storyboard.Children.Add(keyFrames);

            return storyboard;
        }

        internal static Storyboard GetWindows11RightToLeftTransitionStoryboard(DependencyObject target, int from, int to)
        {
            var storyboard = new Storyboard();

            var keyFrames = new DoubleAnimationUsingKeyFrames();
            keyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)),
                Value = from,
            });

            keyFrames.KeyFrames.Add(new SplineDoubleKeyFrame()
            {
                KeySpline = new() { ControlPoint1 = new(0.1, 0.9), ControlPoint2 = new(0.4, 1.0) },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(167)),
                Value = to,
            });

            Storyboard.SetTarget(keyFrames, target);
            Storyboard.SetTargetProperty(keyFrames, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            storyboard.Children.Add(keyFrames);

            return storyboard;
        }

        internal static Storyboard GetWindows11LeftToRightTransitionStoryboard(DependencyObject target, int from, int to)
        {
            var storyboard = new Storyboard();

            var keyFrames = new DoubleAnimationUsingKeyFrames();
            keyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)),
                Value = from,
            });

            keyFrames.KeyFrames.Add(new SplineDoubleKeyFrame()
            {
                KeySpline = new() { ControlPoint1 = new(0.2, 0.0), ControlPoint2 = new(0.9, 0.0) },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(167)),
                Value = to,
            });

            Storyboard.SetTarget(keyFrames, target);
            Storyboard.SetTargetProperty(keyFrames, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            storyboard.Children.Add(keyFrames);

            return storyboard;
        }
    }
}
