using System.Windows;
using System.Windows.Media.Animation;
class AnimLib
{
    public static readonly IEasingFunction Quad = new QuadraticEase { EasingMode = EasingMode.EaseInOut };
    public static readonly IEasingFunction Sine = new SineEase { EasingMode = EasingMode.EaseInOut };
    public static readonly IEasingFunction Back = new BackEase { EasingMode = EasingMode.EaseInOut };
    public static readonly IEasingFunction Bounce = new BounceEase { EasingMode = EasingMode.EaseInOut };
    public static readonly IEasingFunction Circle = new CircleEase { EasingMode = EasingMode.EaseInOut };
    public static readonly IEasingFunction Cubic = new CubicEase { EasingMode = EasingMode.EaseInOut };
    public static readonly IEasingFunction Elastic = new ElasticEase { EasingMode = EasingMode.EaseInOut };
    public static readonly IEasingFunction Exponential = new ExponentialEase { EasingMode = EasingMode.EaseInOut };
    public static readonly IEasingFunction Power = new PowerEase { EasingMode = EasingMode.EaseInOut };
    public static readonly IEasingFunction Quartic = new QuarticEase { EasingMode = EasingMode.EaseInOut };
    public static readonly IEasingFunction Quintic = new QuinticEase { EasingMode = EasingMode.EaseInOut };

    public static void ObjectMove(DependencyObject Object, Thickness Get, Thickness Set, int MilliSeconds = 750)
    {
        Storyboard Board = new Storyboard();
        ThicknessAnimation Animation = new ThicknessAnimation
        {
            EasingFunction = Quad,
            From = Get,
            To = Set,
            AutoReverse = false,
            Duration = new Duration(TimeSpan.FromMilliseconds(MilliSeconds))
        };

        Storyboard.SetTarget(Animation, Object);
        Storyboard.SetTargetProperty(Animation, new PropertyPath(FrameworkElement.MarginProperty));

        Board.Children.Add(Animation);
        Board.Begin();
    }

    public static void Fade(DependencyObject Object, double From = 0, double To = 1, int MilliSeconds = 500)
    {
        Storyboard Board = new Storyboard();
        DoubleAnimation Animation = new DoubleAnimation()
        {
            From = From,
            To = To,
            AutoReverse = false,
            Duration = new Duration(TimeSpan.FromMilliseconds(MilliSeconds))
        };

        Storyboard.SetTarget(Animation, Object);
        Storyboard.SetTargetProperty(Animation, new PropertyPath("Opacity", 1));

        Board.Children.Add(Animation);
        Board.Begin();
    }

    public static void DoubleAnimation(DependencyObject Object, string Property, double From = 0, double To = 1, int MilliSeconds = 500)
    {
        Storyboard Board = new Storyboard();
        DoubleAnimation Animation = new DoubleAnimation()
        {
            From = From,
            To = To,
            AutoReverse = false,
            Duration = new Duration(TimeSpan.FromMilliseconds(MilliSeconds))
        };

        Storyboard.SetTarget(Animation, Object);
        Storyboard.SetTargetProperty(Animation, new PropertyPath(Property, 1));

        Board.Children.Add(Animation);
        Board.Begin();
    }
}